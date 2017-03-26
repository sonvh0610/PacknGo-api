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

		public Dictionary<string, object> GetNearestPlace(IHeaderDictionary header)
		{
			try
			{
				string accessToken = header["AccessToken"];
				double lat = double.Parse(header["Latitude"]);
				double lng = double.Parse(header["Longitude"]);
				double meter = double.Parse(header["Meter"]);
				int pageIdx = int.Parse(header["Index"]);

				JWT.Decode(accessToken, Encoding.ASCII.GetBytes(Constants.Secret), JwsAlgorithm.HS256);

				Location from = new Location() {Latitude = lat, Longitude = lng};
				List<Place> places = _coll
					.FindAll()
					.ToList();

				places = places
					.Where(p => Helper.Measure(from, p.Location) <= meter)
					.OrderByDescending(p => Helper.Measure(from, p.Location))
					.Skip(Constants.PlaceLimit * pageIdx)
					.Take(Constants.PlaceLimit)
					.ToList();

				return places.Count > 0 ? Response.ResponseOK(places) : Response.ResponseError(404, "place_not_found");
			}
			catch (TimeoutException)
			{
				return Response.ResponseError(500, "request_timeout");
			}
			catch (ArgumentException)
			{
				return Response.ResponseError(401, "missing_arguments");
			}
			catch (FormatException)
			{
				return Response.ResponseError(401, "invalid_arguments");
			}
			catch (IntegrityException)
			{
				return Response.ResponseError(401, "invalid_access_token");
			}
		}

	    public Dictionary<string, object> GetPlaceById(IHeaderDictionary header, string id)
	    {
		    try
		    {
				string accessToken = header["AccessToken"];
				JWT.Decode(accessToken, Encoding.ASCII.GetBytes(Constants.Secret), JwsAlgorithm.HS256);
				if (id == null) throw new ArgumentNullException();
			    Place place = _coll.FindOne(Query<Place>.EQ(p => p.Id, id));
			    var response = Helper.ConvertClassToDictionary(place);
				return (place != null) ? Response.ResponseOK(response) : Response.ResponseError(404, "place_not_found");
			}
			catch (TimeoutException)
			{
				return Response.ResponseError(500, "request_timeout");
			}
			catch (ArgumentException)
			{
				return Response.ResponseError(401, "missing_arguments");
			}
			catch (IntegrityException)
			{
				return Response.ResponseError(401, "invalid_access_token");
			}
		}
	}
}
