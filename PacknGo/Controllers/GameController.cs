using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PacknGo.Models;

namespace PacknGo.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
	    public GameHandler _handler;

	    public GameController()
	    {
		    _handler = new GameHandler();
	    }
	    [HttpGet]
	    public IActionResult Get(string accountId)
	    {
			JObject result = _handler.GameResource(HttpContext.Request.Headers);
			if (result["errorCode"] != null)
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}

	    [HttpPost]
	    public IActionResult FlipOneBox(string accountId)
	    {
			JObject result = _handler.UpdateCoord(HttpContext.Request.Headers);
			if (result["errorCode"] != null)
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}

	    [HttpPost("answer")]
	    public IActionResult Answer([FromBody]JObject body)
	    {
			JObject result = _handler.Answer(HttpContext.Request.Headers, body);
			if (result["errorCode"] != null)
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}
    }
}