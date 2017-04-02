using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PacknGo.Utils
{
    public class Response
    {
	    public static JObject ResponseOK(object data)
	    {
			return JObject.FromObject(new
			{
				successful = true,
				data = data
			});
		}

	    public static JObject ResponseError(int errorCode, string errorDescription)
	    {
			return JObject.FromObject(new
			{
				successful = false,
				errorCode = errorCode,
				errorDescription = errorDescription
			});
		}
    }
}