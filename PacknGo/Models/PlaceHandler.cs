using MongoDB.Driver;
using PacknGo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jose;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using Newtonsoft.Json.Linq;

namespace PacknGo.Models
{
    public class PlaceHandler
    {
	    private readonly MongoCollection<Place> _coll;

		public PlaceHandler()
		{
			MongoDatabase db = new Database().GetDatabase();
			_coll = db.GetCollection<Place>("Place");
		}

		public JObject GetNearestPlace(IHeaderDictionary header)
		{
			try
			{
				string accessToken = header["AccessToken"];
				double lat = double.Parse(header["Latitude"]);
				double lng = double.Parse(header["Longitude"]);
				double distance = double.Parse(header["Distance"]);

				JWT.Decode(accessToken, Constants.Secret, JwsAlgorithm.HS256);

				Location from = new Location() {Latitude = lat, Longitude = lng};
				List<Place> places = _coll
					.FindAll()
					.ToList();

				places = places
					.Where(p => Helper.Measure(from, p.Location) <= distance)
					.OrderByDescending(p => Helper.Measure(from, p.Location))
					.ToList();

				return places.Count > 0 ? Response.ResponseOK(places) : Response.ResponseError(404, "place_not_found");
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

	    public JObject GetPlaceById(IHeaderDictionary header, string id)
	    {
		    try
		    {
				string accessToken = header["AccessToken"];
				JWT.Decode(accessToken, Constants.Secret, JwsAlgorithm.HS256);
				if (id == null) throw new ArgumentNullException();
			    Place place = _coll.FindOne(Query<Place>.EQ(p => p.Id, id));
				return (place != null) ? Response.ResponseOK(place) : Response.ResponseError(404, "place_not_found");
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
