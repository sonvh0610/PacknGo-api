using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Security.Cryptography;
using PacknGo.Utils;
using System.Text;
using System.Collections.Generic;
using Jose;
using System;
using System.Linq;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace PacknGo.Models
{
    public class AccountHandler
    {
	    private readonly MongoDatabase _db;

		public AccountHandler()
		{
			_db = new Database().GetDatabase();
		}

		public Dictionary<string, object> FindUserByEmail(string email, string password)
		{
			try
			{
				using (MD5 md5Hash = MD5.Create())
				{
					var query = Query.And(
						Query<Account>.EQ(p => p.Email, email),
						Query<Account>.EQ(p => p.Password, Helper.GetMd5Hash(md5Hash, password))
					);
					Account account = _db.GetCollection<Account>("Account").FindOne(query);
					
					if (account != null)
					{
						Dictionary<string, object> payload = Helper.ConvertClassToDictionary(account);
						payload.Add("exp", Constants.ExpiredTime);
						string token = JWT.Encode(payload, Encoding.ASCII.GetBytes(Constants.Secret), JwsAlgorithm.HS256);

						return new Dictionary<string, object>()
						{
							{ "successful", true },
							{ "accessToken", token },
							{ "expiredIn", Constants.ExpiredTime }
						};
					}
					else
					{
						return new Dictionary<string, object>()
						{
							{ "successful", false },
							{ "errorCode", 404 },
							{ "errorDescription", "invalid_email_or_password" }
						};
					}
				}
			}
			catch (TimeoutException)
			{
				return new Dictionary<string, object>()
				{
					{ "successful", false },
					{ "errorCode", 500 },
					{ "errorDescription", "request_timeout" }
				};
			};
		}

		public Dictionary<string, object> GetUserByAccessToken(string accessToken)
		{
			try
			{
				string resultCode = JWT.Decode(accessToken, Encoding.ASCII.GetBytes(Constants.Secret), JwsAlgorithm.HS256);
				Regex pattern = new Regex("[\\{\\}\"]");
				resultCode = pattern.Replace(resultCode, string.Empty);

				var dict = resultCode.Split(',')
					.Select(part => part.Split(':'))
					.ToDictionary(split => split[0], split => split[1]);

				Account account = new Account();
				account.Id = new ObjectId(dict["Id"]);
				account.Name = dict["Name"];
				account.Email = dict["Email"];
				account.Password = dict["Password"];
				account.Avatar = (dict["Avatar"] == "null") ? null : dict["Avatar"];
				account.RegisterDate = dict["RegisterDate"];

				var dictAccount = Helper.ConvertClassToDictionary(account);
				dictAccount.Remove("Password");

				var result = new Dictionary<string, object>()
				{
					{"successful", true},
					{"data", dictAccount}
				};

				return result;
			}
			catch (IntegrityException)
			{
				return new Dictionary<string, object>()
				{
					{"successful", false},
					{"errorCode", 401},
					{"errorDescription", "access_token_invalid"}
				};
			}
			catch (ArgumentException)
			{
				return new Dictionary<string, object>()
				{
					{"successful", false},
					{"errorCode", 401},
					{"errorDescription", "missing_access_token"}
				};
			}
		}
    }
}
