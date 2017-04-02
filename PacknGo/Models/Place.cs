using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PacknGo.Models
{
    public class Place
    {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("Name")]
		public string Name { get; set; }

		[BsonElement("Location")]
		public Location Location { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("DistrictID")]
		public string DistrictId { get; set; }

		[BsonElement("Infomation")]
		public string Infomation { get; set; }

		[BsonElement("Category")]
		public List<string> Category { get; set; }

		[BsonElement("Type")]
		public int Type { get; set; }

		[BsonElement("Image")]
		public List<string> Image { get; set; }

		[BsonElement("Address")]
		public string Address { get; set; }
    }

	public class Location
	{
		[BsonElement("Latitude")]
		public double Latitude { get; set; }

		[BsonElement("Longitude")]
		public double Longitude { get; set; }
	}
}
