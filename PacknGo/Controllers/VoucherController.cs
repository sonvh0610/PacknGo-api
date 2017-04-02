using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PacknGo.Models;

namespace PacknGo.Controllers
{
    [Route("api/[controller]")]
    public class VoucherController : Controller
    {
		private readonly VoucherHandler _handler;

		public VoucherController()
		{
			_handler = new VoucherHandler();
		}

		[HttpGet]
	    public IActionResult GetVoucher()
	    {
			JObject result = _handler.GetVoucher(HttpContext.Request.Headers);
			if (result["errorCode"] != null)
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}
    }
}