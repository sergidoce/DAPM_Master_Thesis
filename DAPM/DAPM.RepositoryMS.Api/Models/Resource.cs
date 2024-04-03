using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAPM.RepositoryMS.Api.Models
{
    public class Resource
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        public FileStream File { get; set; }

        public Resource(string name, FileStream file)
        {
            Name = name;
            File = file;
        }
    }
}
