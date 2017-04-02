using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PacknGo.Models
{
    public class Review
    {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("PlaceId")]
		public string PlaceId { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("AccountId")]
		public string AccountId { get; set; }

	    private int _rate;
		[BsonElement("Rate")]
		public int Rate
	    {
		    get { return _rate; }
		    set
		    {
			    if (value > 5) _rate = 5;
				else if (value < 1) _rate = 1;
			    else _rate = value;
		    }
	    }

		[BsonElement("Comment")]
		public string Comment { get; set; }

		[BsonElement("ReviewDate")]
		[BsonRepresentation(BsonType.String)]
		public DateTime ReviewDate { get; set; }
    }
}
