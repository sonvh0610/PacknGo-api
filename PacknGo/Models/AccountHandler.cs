using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Security.Cryptography;
using PacknGo.Utils;
using System.Collections.Generic;
using Jose;
using System;
using Newtonsoft.Json.Linq;

namespace PacknGo.Models
{
    public class AccountHandler
    {
		private readonly MongoCollection<Account> _coll;

		public AccountHandler()
		{
			_coll = new Database().GetDatabase().GetCollection<Account>("Account");
		}

		public JObject GetTokenByUser(Dictionary<string, string> body)
		{
			try
			{
				string email = body["Email"];
				string password = body["Password"];

				using (MD5 md5Hash = MD5.Create())
				{
					var query = Query.And(
						Query<Account>.EQ(p => p.Email, email),
						Query<Account>.EQ(p => p.Password, Helper.GetMd5Hash(md5Hash, password))
					);
					Account account = _coll.FindOne(query);
					
					if (account != null)
					{
						JObject payload = Helper.ConvertClassToJSON(account);
						payload.Add("exp", Constants.ExpiredTime);
						string token = JWT.Encode(payload, Constants.Secret, JwsAlgorithm.HS256);

						return JObject.FromObject(new
						{
							successful = true,
							accessToken = token,
							expiredIn = Constants.ExpiredTime
						});
					}

					return Response.ResponseError(404, "invalid_email_or_password");
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

		public JObject GetTokenGuest(Dictionary<string, string> body)
		{
			try
			{
				string deviceId = body["DeviceId"];
				string deviceOs = body["DeviceOs"];

				JObject json = JObject.FromObject(new
				{
					DeviceId = deviceId,
					DeviceOs = deviceOs,
					exp = Constants.ExpiredTime
				});
				string token = JWT.Encode(json, Constants.Secret, JwsAlgorithm.HS256);

				return JObject.FromObject(new
				{
					successful = true,
					accessToken = token,
					expiredIn = Constants.ExpiredTime
				});
			}
			catch (Exception ex)
			{
				return Response.ResponseError(401, ex.Message);
			}
		}

		public JObject GetUserByAccessToken(string accessToken)
		{
			try
			{
				string json = JWT.Decode(accessToken, Constants.Secret, JwsAlgorithm.HS256);
				JObject result = JObject.Parse(json);
				result.Remove("exp");

				if (result["Email"] != null)
				{
					result.Remove("Password");
					result.Add("Role", "Member");
				}
				else
				{
					result.Add("Role", "Guest");
				}

				return Response.ResponseOK(result);
			}
			catch (Exception ex)
			{
				return Response.ResponseError(401, ex.Message);
			}
		}

	    public Account GetMember(string accessToken)
	    {
		    try
		    {
			    string json = JWT.Decode(accessToken, Constants.Secret, JwsAlgorithm.HS256);
			    JObject result = JObject.Parse(json);
			    Account account = _coll.FindOne(Query<Account>.EQ(p => p.Id, result["Id"].Value<string>()));
			    return account;
		    }
		    catch (Exception ex)
		    {
			    throw new Exception(ex.Message);
		    }
	    }

	    public bool AddScore(string accessToken, int score)
	    {
			try
			{
				Account account = this.GetMember(accessToken);
				if (account.Score + score < 0)
				{
					throw new Exception("Not enough");
				}
				else
				{
					_coll.FindAndModify(Query<Account>.EQ(p => p.Id, account.Id), null, Update.Set("Score", account.Score + score));

					return true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
    }
}
