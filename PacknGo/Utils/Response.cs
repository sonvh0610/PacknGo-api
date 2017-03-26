using System.Collections.Generic;

namespace PacknGo.Utils
{
    public class Response
    {
	    public static Dictionary<string, object> ResponseOK(object data)
	    {
			return new Dictionary<string, object>()
			{
				{"successful", true},
				{"data", data}
			};
		}

	    public static Dictionary<string, object> ResponseError(int errorCode, string errorDescription)
	    {
			return new Dictionary<string, object>()
			{
				{"successful", false},
				{"errorCode", errorCode},
				{"errorDescription", errorDescription}
			};
		}
    }
}
