using System;
using System.Collections.Generic;
using System.Linq;
using Jose;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PacknGo.Utils;

namespace PacknGo.Models
{
    public class GameHandler
    {
	    private readonly MongoCollection<Game> _coll;
	    private readonly AccountHandler _handler;

	    public GameHandler()
	    {
		    _coll = new Database().GetDatabase().GetCollection<Game>("Game");
			_handler = new AccountHandler();
	    }

	    public JObject GameResource(IHeaderDictionary header)
	    {
			try
			{
				string accessToken = header["AccessToken"];
				Account account = _handler.GetMember(accessToken);
				Game game = _coll.FindOne(Query<Game>.EQ(p => p.AccountId, account.Id));

				return Response.ResponseOK(new
				{
					successful = true,
					url = game.Url,
					coords = JArray.Parse(JsonConvert.SerializeObject(game.Coords))
				});

			}
			catch (TimeoutException)
			{
				return Response.ResponseError(500, "request_timeout");
			}
			catch (Exception ex)
			{
				return Response.ResponseError(401, ex.Message);
			}
		}
		public JObject UpdateCoord(IHeaderDictionary header)
		{
			try
			{
				string accessToken = header["AccessToken"];
				Account account = _handler.GetMember(accessToken);
				Game game = _coll.FindOne(Query<Game>.EQ(p => p.AccountId, account.Id));

				if (game.Coords.Count < 16)
				{
					game.Coords.Add(GenerateCoord(game.Coords));

					_coll.FindAndModify(
						Query<Game>.EQ(p => p.AccountId, account.Id),
						null,
						Update.Set("Coords", BsonValue.Create(game.Coords))
					);
					_handler.AddScore(accessToken, 25);

					return JObject.FromObject(new
					{
						successful = true
					});
				}
				else
				{
					throw new Exception("Hidden pieces not available");
				}

			}
			catch (TimeoutException)
			{
				return Response.ResponseError(500, "request_timeout");
			}
			catch (Exception ex)
			{
				return Response.ResponseError(401, ex.Message);
			}
		}

	    public JObject Answer(IHeaderDictionary header, JObject body)
	    {
			try
			{
				string accessToken = header["AccessToken"];
				Account account = _handler.GetMember(accessToken);
				Game game = _coll.FindOne(Query<Game>.EQ(p => p.AccountId, account.Id));

				if (string.Equals(body["Answer"].Value<string>(), game.Answer, StringComparison.CurrentCultureIgnoreCase))
				{
					AccountHandler handler = new AccountHandler();
					handler.AddScore(accessToken, (Constants.GamePieces - game.Coords.Count) * 50 + 50);
					return JObject.FromObject(new
					{
						successful = true
					});
				}
				else
				{
					throw new Exception("Wrong answer");
				}

			}
			catch (TimeoutException)
			{
				return Response.ResponseError(500, "request_timeout");
			}
			catch (Exception ex)
			{
				return Response.ResponseError(401, ex.Message);
			}
		}

	    private static List<int> GenerateCoord(List<List<int>> listCoords)
	    {
			List<int> list = new List<int>();
			while (true)
		    {
				Random rnd = new Random();
				int x = rnd.Next(0, 3);
				int y = rnd.Next(0, 3);

			    var found = listCoords.FirstOrDefault(l => l[0] == x && l[1] == y);

			    if (found != null) continue;

			    list.Add(x);
			    list.Add(y);
			    break;
		    }
			return list;
		}
	}
}
