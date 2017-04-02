using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
			JObject result = _handler.GetNearestPlace(HttpContext.Request.Headers);
			if (result["errorCode"] != null)
			{
				HttpContext.Response.StatusCode = (int) result["errorCode"];
			}
			return new JsonResult(result);
		}

	    [HttpGet("{id}")]
	    public IActionResult Get(string id)
	    {
			JObject result = _handler.GetPlaceById(HttpContext.Request.Headers, id);
			if (result["errorCode"] != null)
			{
				HttpContext.Response.StatusCode = (int)result["errorCode"];
			}
			return new JsonResult(result);
		}
    }
}