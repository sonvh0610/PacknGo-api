using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PacknGo.Models;

namespace PacknGo.Controllers
{
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
		private readonly ReviewHandler _handler;

		public ReviewController()
		{
			_handler = new ReviewHandler();
		}

	    [HttpPost]
	    public IActionResult Post([FromBody]JObject body)
	    {
			JObject result = _handler.AddCommentRate(HttpContext.Request.Headers, body);
			if (result["errorCode"] != null)
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
	    }

	    [HttpGet("{id}")]
	    public IActionResult GetCommentsByPlaceId(string id)
	    {
			JObject result = _handler.GetCommentsByPlaceId(HttpContext.Request.Headers, id);
			if (result["errorCode"] != null)
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}
    }
}