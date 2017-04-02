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
    public class ReviewHandler
    {
	    private readonly MongoCollection<Review> _coll;
	    private readonly AccountHandler _handler;

	    public ReviewHandler()
	    {
		    _coll = new Database().GetDatabase().GetCollection<Review>("Review");
			_handler = new AccountHandler();
	    }

	    public JObject GetCommentsByPlaceId(IHeaderDictionary header, string placeId)
	    {
		    try
		    {
			    string accessToken = header["AccessToken"];
			    JWT.Decode(accessToken, Constants.Secret, JwsAlgorithm.HS256);
			    List<Review> reviews = _coll.Find(Query<Review>.EQ(p => p.PlaceId, placeId)).ToList();

			    return Response.ResponseOK(JArray.Parse(JsonConvert.SerializeObject(reviews)));
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

	    public JObject AddCommentRate(IHeaderDictionary header, JObject body)
	    {
		    try
		    {
			    string accessToken = header["AccessToken"];
			    string placeId = body["PlaceId"].Value<string>();
			    string comment = body["Comment"].Value<string>();
			    int rate = body["Rate"].Value<int>();

			    Account account = _handler.GetMember(accessToken);
				Review review = new Review
				{
					AccountId = account.Id,
					Comment = comment,
					PlaceId = placeId,
					Rate = rate,
					ReviewDate = DateTime.Now.ToUniversalTime()
				};
				_coll.Insert(review);
				return JObject.FromObject(new { successful = true });
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
