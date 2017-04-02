using MongoDB.Driver;

namespace PacknGo.Utils
{
    public class Database
    {
	    private readonly MongoServer _server;

		public Database()
		{
			var client = new MongoClient(Constants.DatabaseEndpoint);
			_server = client.GetServer();
		}

		public MongoDatabase GetDatabase()
		{
			return _server.GetDatabase(Constants.DatabaseName);
		}
    }
}
