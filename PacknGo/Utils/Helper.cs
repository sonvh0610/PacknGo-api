using PacknGo.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PacknGo.Utils
{
    public class Helper
    {
		public static string GetMd5Hash(MD5 md5Hash, string input)
		{
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder strBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; ++i)
			{
				strBuilder.Append(data[i].ToString("x2"));
			}
			return strBuilder.ToString();
		}

		public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
		{
			string hashOfInput = GetMd5Hash(md5Hash, input);
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			return (0 == comparer.Compare(hashOfInput, hash));
		}

		public static JObject ConvertClassToJSON<T>(T target)
		{
			PropertyInfo[] infos = target.GetType().GetProperties();
			JObject json = new JObject();
			foreach (PropertyInfo info in infos)
			{
				try
				{
					json.Add(info.Name, info.GetValue(target, null).ToString());
				}
				catch (NullReferenceException)
				{
					json.Add(info.Name, null);
				}
			}
			return json;
		}

		public static double Measure(Location from, Location to)
		{
			double R = 6378.137; // Radius of earth in km
			double deltaLatitude = to.Latitude * Math.PI / 180 - from.Latitude * Math.PI / 180;
			double deltaLongitude = to.Longitude * Math.PI / 180 - from.Longitude * Math.PI / 180;
			double a = Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) + Math.Cos(from.Latitude * Math.PI / 180) * Math.Cos(to.Latitude * Math.PI / 180) * Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2);
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			double d = R * c;
			return d * 1000; // in Meter
		}
	}
}
