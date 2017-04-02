using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PacknGo.Models
{
    public class Account
    {
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("Email")]
		public string Email { get; set; }

		[BsonElement("Password")]
		public string Password { get; set; }

		[BsonElement("Name")]
		public string Name { get; set; }

		[BsonElement("Avatar")]
		public string Avatar { get; set; }

		[BsonElement("RegisterDate")]
		public string RegisterDate { get; set; }

		[BsonElement("Level")]
		public int Level { get; set; }

		[BsonElement("Score")]
		public int Score { get; set; }
    }
}