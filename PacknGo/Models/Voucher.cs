using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PacknGo.Models
{
    public class Voucher
    {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("Name")]
		public string Name { get; set; }

		[BsonElement("CodeName")]
		public string CodeName { get; set; }

		[BsonElement("Score")]
		public int Score { get; set; }

		[BsonElement("Description")]
		public string Description { get; set; }

		[BsonElement("Sponsor")]
		public string Sponsor { get; set; }
    }
}
