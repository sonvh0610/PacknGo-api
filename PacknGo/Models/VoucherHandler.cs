using System;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json.Linq;
using PacknGo.Utils;

namespace PacknGo.Models
{
    public class VoucherHandler
    {
		private readonly MongoCollection<Voucher> _coll;
	    private readonly AccountHandler _handler;

		public VoucherHandler()
		{
			_coll = new Database().GetDatabase().GetCollection<Voucher>("Voucher");
			_handler = new AccountHandler();
		}

	    public JObject GetVoucher(IHeaderDictionary header)
	    {
		    try
		    {
				string accessToken = header["AccessToken"];
			    Voucher voucher = _coll.FindOne(Query<Voucher>.EQ(p => p.Sponsor, header["Sponsor"].ToString()));

			    if (voucher != null)
			    {
				    _handler.AddScore(accessToken, -voucher.Score);
				    return Response.ResponseOK(voucher);
			    }
			    else
			    {
				    throw new Exception("Cannot find sponsor");
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
	}
}
