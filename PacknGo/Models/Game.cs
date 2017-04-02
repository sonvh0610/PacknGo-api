using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PacknGo.Models
{
    public class Game
    {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		
		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("AccountId")]
		public string AccountId { get; set; }

		[BsonElement("Url")]
		public string Url { get; set; }

		[BsonElement("Coords")]
		public List<List<int>> Coords { get; set; }

		[BsonElement("Answer")]
		public string Answer { get; set; }
    }
}
