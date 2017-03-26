using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PacknGo.Models;

namespace PacknGo.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
		AccountHandler _handler;

		public AccountController()
		{
			_handler = new AccountHandler();
		}

		[HttpPost]
		public IActionResult Post(Dictionary<string, string> body)
		{
			Dictionary<string, object> result = _handler.FindUserByEmail(body["Email"], body["Password"]);
			if (result.ContainsKey("errorCode"))
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}

		[HttpGet]
		public IActionResult Get()
		{
			Dictionary<string, object> result = _handler.GetUserByAccessToken(HttpContext.Request.Headers["AccessToken"]);
			if (result.ContainsKey("errorCode"))
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}
    }
}