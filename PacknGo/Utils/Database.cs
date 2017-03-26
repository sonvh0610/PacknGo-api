using MongoDB.Driver;

namespace PacknGo.Utils
{
    public class Database
    {
		MongoClient _client;
		MongoServer _server;

		public Database()
		{
			_client = new MongoClient(Constants.DatabaseEndpoint);
			_server = _client.GetServer();
		}

		public MongoDatabase GetDatabase()
		{
			return _server.GetDatabase(Constants.DatabaseName);
		}
    }
}
