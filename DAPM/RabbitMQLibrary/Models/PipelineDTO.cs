using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class Resource
    {
        public int OrganizationId { get; set; }
        public int RepositoryId { get; set; }
        public int? ResourceId { get; set; }
        public string? Name { get; set; }
    }
    public class HandleData
    {
        public string Id { get; set; }
    }
    public class TargetHandle
    {
        public HandleData HandleData { get; set; }
    }
    public class SourceHandle
    {
        public HandleData HandleData { get; set; }
    }
    public class InstantiationData
    {
        public Resource Resource { get; set; }
    }
    public class TemplateData
    {
        public IEnumerable<SourceHandle> SourceHandles { get; set; }
        public IEnumerable<TargetHandle> TargetHandles { get; set; }
    }
    public class Edge
    {
        public string SourceHandle { get; set; }
        public string TargetHandle { get; set; }
    }
    public class Node
    {
        public string Type { get; set; }
        public TemplateData TemplateData { get; set; }
        public InstantiationData InstantiationData { get; set; }
    }
    public class PipelineDTO
    {
        public IEnumerable<Node> Nodes { get; set; }
        public IEnumerable<Edge> Edges { get; set; }
    }
}
