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
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AchievementController : Controller
    {
		AchievementHandler _handler;

		public AchievementController()
		{
			_handler = new AchievementHandler();
		}

	    [HttpGet]
	    public IActionResult Get()
	    {
			JObject result = _handler.GetAchievements(HttpContext.Request.Headers);
			if (result["errorCode"] != null)
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}
	}
}