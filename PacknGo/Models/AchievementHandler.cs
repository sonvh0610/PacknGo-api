using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json.Linq;
using PacknGo.Utils;

namespace PacknGo.Models
{
    public class AchievementHandler
    {
		private readonly MongoCollection<Achievement> _coll;
	    private readonly AccountHandler _handler;

		public AchievementHandler()
		{
			_coll = new Database().GetDatabase().GetCollection<Achievement>("Achievement");
			_handler = new AccountHandler();
		}

	    public JObject GetAchievements(IHeaderDictionary header)
	    {
		    try
		    {
				string accessToken = header["AccessToken"];
				Account account = _handler.GetMember(accessToken);
			    List<Achievement> achievements = _coll.Find(Query<Achievement>.EQ(p => p.AccountId, account.Id)).ToList();

			    if (achievements.Count > 0)
			    {
				    return Response.ResponseOK(achievements);
			    }
				return Response.ResponseError(404, "Achievement not found");
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
	}
}
