using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PacknGo.Models;

namespace PacknGo.Controllers
{
    [Route("api/[controller]")]
    public class PlaceController : Controller
    {
	    private PlaceHandler _handler;

	    public PlaceController()
	    {
		    _handler = new PlaceHandler();
	    }

	    [HttpGet("nearme")]
	    public IActionResult Get()
	    {
			Dictionary<string, object> result = _handler.GetNearestPlace(HttpContext.Request.Headers);
			if (result.ContainsKey("errorCode"))
			{
				HttpContext.Response.StatusCode = (int) result["errorCode"];
			}
			return new JsonResult(result);
		}

	    [HttpGet("{id}")]
	    public IActionResult Get(string id)
	    {
			Dictionary<string, object> result = _handler.GetPlaceById(HttpContext.Request.Headers, id);
			if (result.ContainsKey("errorCode"))
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}
    }
}