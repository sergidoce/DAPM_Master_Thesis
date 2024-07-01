using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class Resource
    {
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid ResourceId { get; set; }
        public string? Name { get; set; }
    }
   
    public class Handle
    {
        public string Id { get; set; }
    }
    
    public class InstantiationData
    {
        public Resource Resource { get; set; }
    }
    public class TemplateData
    {
        public IEnumerable<Handle> SourceHandles { get; set; }
        public IEnumerable<Handle> TargetHandles { get; set; }
        public string? Hint { get; set; }
    }
    public class Edge
    {
        public string SourceHandle { get; set; }
        public string TargetHandle { get; set; }
    }

    public class NodePosition
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class NodeData
    {
        public string Label { get; set; }
        public TemplateData TemplateData { get; set; }
        public InstantiationData InstantiationData { get; set; }
    }

    public class Node
    {
        public string Id { get; set;}
        public string Type { get; set; }
        public NodePosition Position { get; set; }
        public NodeData Data { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
    public class Pipeline
    {
        public IEnumerable<Node> Nodes { get; set; }
        public IEnumerable<Edge> Edges { get; set; }
    }

    public class PipelineDTO
    {
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Pipeline? Pipeline { get; set; }
    }
}
