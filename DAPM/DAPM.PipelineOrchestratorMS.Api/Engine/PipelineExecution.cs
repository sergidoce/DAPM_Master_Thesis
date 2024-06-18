using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Engine
{

    public enum PipelineExecutionState
    {
        NotStarted,
        Running,
        Completed,
        Faulted
    }


    public class PipelineExecution : IPipelineExecution
    {
        private Guid _id;
        private ILogger<PipelineExecution> _logger;
        private IServiceProvider _serviceProvider;
        private Pipeline _pipeline;
        private Dictionary<Guid, EngineNode> _nodes;
        private Dictionary<Guid, List<Guid>> _successorDictionary;
        private Dictionary<Guid, List<Guid>> _predecessorDictionary;

        private List<Guid> _dataSinkNodes;

        private List<Step> _steps;
        private Dictionary<Guid, Step> _stepsDictionary;


        private PipelineExecutionState _state;

        public PipelineExecution(Guid id, Pipeline pipelineDto, IServiceProvider serviceProvider) 
        {
            _id = id;
            _nodes = new Dictionary<Guid, EngineNode>();
            _successorDictionary = new Dictionary<Guid, List<Guid>>();
            _predecessorDictionary = new Dictionary<Guid, List<Guid>>();
            _dataSinkNodes = new List<Guid>();
            _stepsDictionary = new Dictionary<Guid, Step>();
            _steps = new List<Step>();

            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetService<ILogger<PipelineExecution>>();

            _pipeline = pipelineDto;
            _state = PipelineExecutionState.NotStarted;

            InitGraph();

        }

        #region Pipeline Execution Public Methods

        public void StartExecution()
        {
            _state = PipelineExecutionState.Running;
            ExecuteAvailableSteps();
        }

        public void ProcessActionResult(ActionResultDTO actionResult)
        {
            var result = actionResult.ActionResult;

            if(result == ActionResult.Completed)
            {
                _stepsDictionary[actionResult.StepId].Status = StepStatus.Completed;
                ExecuteAvailableSteps();
            }
            else
            {
                _logger.LogInformation($"There was an error in step {actionResult.StepId}");
            }
        }

        #endregion

        #region Pipeline Execution Private Methods

        private void ExecuteAvailableSteps()
        {
            var availableSteps = GetAvailableSteps();

            if(availableSteps.Count() == 0)
            {
                _state = PipelineExecutionState.Completed;
                return;
            }

            foreach (var step in availableSteps)
            {
                _stepsDictionary[step].Execute();
            }
        }

        private List<Guid> GetAvailableSteps()
        {
            var result = new List<Guid>();

            foreach (var step in _stepsDictionary.Values)
            {
                if (step.Status == StepStatus.Completed)
                    continue;

                var available = true;
                var prerequisites = step.PrerequisiteSteps;
                foreach (var prerequisite in prerequisites)
                {
                    var prerequisiteStep = _stepsDictionary[prerequisite];
                    if (prerequisiteStep.Status != StepStatus.Completed)
                    {
                        available = false;
                        break;
                    }
                }

                if(available)
                    result.Add(step.Id);
            }

            return result;
        }
        #endregion

        #region Graph Generation


        private void InitGraph()
        {
            GenerateEngineNodes();
            GenerateEngineEdges();
            _steps = GenerateSteps();

            foreach (var step in _steps)
            {
                _stepsDictionary[step.Id] = step;
            }
        }

        private void GenerateEngineNodes()
        {
            var nodes = _pipeline.Nodes;
            foreach (var node in nodes)
            {
                var engineNode = new EngineNode(node);
                _nodes[engineNode.Id] = engineNode;

                if(node.Type == "dataSink")
                {
                    _dataSinkNodes.Add(engineNode.Id);
                }

            }
        }

        private void GenerateEngineEdges()
        {
            var edges = _pipeline.Edges;

            foreach(var edge in edges)
            {
                var sourceHandle = edge.SourceHandle;
                var targetHandle = edge.TargetHandle;

                var sourceNode = GetNodeFromHandle(sourceHandle);
                var targetNode = GetNodeFromHandle(targetHandle);

                if (!_successorDictionary.ContainsKey(sourceNode.Id))
                {
                    _successorDictionary[sourceNode.Id] = new List<Guid>();
                }

                if (!_predecessorDictionary.ContainsKey(targetNode.Id))
                {
                    _predecessorDictionary[targetNode.Id] = new List<Guid>();
                }

                _successorDictionary[sourceNode.Id].Add(targetNode.Id);
                _predecessorDictionary[targetNode.Id].Add(sourceNode.Id);
            }
        }

        private EngineNode GetNodeFromHandle(string handleId)
        {
            foreach (var node in _nodes.Values)
            {
                foreach(var handle in node.SourceHandles)
                {
                    if (handle == handleId)
                        return node;
                }


                foreach (var handle in node.TargetHandles)
                {
                    if (handle == handleId)
                        return node;
                }
            }

            return null;
        }

        private List<Step> CreateStepListRecursive(EngineNode currentNode, List<Guid> visitedNodes)
        {
            var result = new List<Step>();
            // If the path has already been visited or the node is a data source, we trigger base case
            if (visitedNodes.Contains(currentNode.Id) || currentNode.NodeType == "dataSource")
            {
                return result;
            }

            ExecuteOperatorStep executeOperatorStep = new ExecuteOperatorStep(_id ,_serviceProvider);

            var predecessorNodesIds = _predecessorDictionary[currentNode.Id];

            foreach (var predecessorId in predecessorNodesIds)
            {
                
                result.AddRange(CreateStepListRecursive(_nodes[predecessorId], visitedNodes));

                var transferDataStep = GenerateTransferDataStep(predecessorId, currentNode.Id);
                var predecessorLastAssociatedStepId = _nodes[predecessorId].GetLastAssociatedStep();

                if(predecessorLastAssociatedStepId != Guid.Empty)
                    transferDataStep.PrerequisiteSteps.Add(predecessorLastAssociatedStepId);

                result.Add(transferDataStep);

                AssociateNodeWithStep(currentNode.Id, transferDataStep.Id);

                if (currentNode.NodeType == "operator")
                {
                    executeOperatorStep.PrerequisiteSteps.Add(transferDataStep.Id);
                    executeOperatorStep.InputResources.Add(transferDataStep.GetResourceToTransfer());
                }
            }
            if (currentNode.NodeType == "operator")
            {
                var operatorResource = new EngineResource()
                {
                    OrganizationId = currentNode.OrganizationId,
                    RepositoryId = currentNode.RepositoryId,
                    ResourceId = (Guid)currentNode.ResourceId,
                };

                executeOperatorStep.OperatorResource = operatorResource;
                executeOperatorStep.TargetOrganization = currentNode.OrganizationId;

                result.Add(executeOperatorStep);
                AssociateNodeWithStep(currentNode.Id, executeOperatorStep.Id);
            }

            visitedNodes.Add(currentNode.Id);
            return result;

        }

        private List<Step> GenerateSteps()
        {
            var visitedNodes = new List<Guid>();
            var result = new List<Step>();  

            foreach (Guid nodeId in _dataSinkNodes)
            {
                var node = _nodes[nodeId];
                result.AddRange(CreateStepListRecursive(node, visitedNodes));
                visitedNodes.Add(node.Id);
            }

            return result;
        }

        private TransferDataStep GenerateTransferDataStep(Guid sourceNodeId, Guid targetNodeId)
        {
            var sourceNode = _nodes[sourceNodeId];
            var targetNode = _nodes[targetNodeId];

            var resourceToTransfer = new EngineResource()
            {
                OrganizationId = sourceNode.OrganizationId,
                RepositoryId = sourceNode.RepositoryId,
                ResourceId = (Guid)sourceNode.ResourceId,
            };

            Guid? destinationRepository = null;
            
            if(targetNode.NodeType == "dataSink")
                destinationRepository = targetNode.RepositoryId;

            var sourceStorageMode = GetStorageModeFromNode(sourceNode);
            var targetStorageMode = GetStorageModeFromNode(targetNode);

            var step = new TransferDataStep(resourceToTransfer, targetNode.OrganizationId, destinationRepository, sourceStorageMode, targetStorageMode, _id, _serviceProvider);

            return step;
        }

        private StorageMode GetStorageModeFromNode(EngineNode node)
        {
            if (node.NodeType == "operator")
                return StorageMode.Temporal;
            if (node.NodeType == "dataSource")
                return StorageMode.Permanent;

            return StorageMode.Permanent;
        }


        private void AssociateNodeWithStep(Guid nodeId, Guid stepId)
        {
            _nodes[nodeId].AddAssociatedStep(stepId);
        }

        #endregion

    }
}
