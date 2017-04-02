using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PacknGo.Models
{
    public class Achievement
    {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("Name")]
		public string Name { get; set; }

		[BsonElement("Icon")]
		public string Icon { get; set; }

		[BsonElement("Description")]
		public string Description { get; set; }

		[BsonElement("CurrentValue")]
		public int CurrentValue { get; set; }

		[BsonElement("MaxValue")]
		public int MaxValue { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("AccountId")]
		public string AccountId { get; set; }
    }
}