using System.Text;

namespace PacknGo.Utils
{
    public static class Constants
    {
		public static string DatabaseEndpoint = "mongodb://vohoaison:taodaubietpass@ds058739.mlab.com:58739/packngo";
		public static string DatabaseName = "packngo";

		//public static string DatabaseEndpoint = "mongodb://localhost:27017/packngo";
		//public static string DatabaseName = "PacknGo";
		public static byte[] Secret = Encoding.ASCII.GetBytes("packngodimuonnoiohyeah");
		public static int ExpiredTime = 360000;

	    public static int PlaceLimit = 3;
	    public static int GamePieces = 16;
    }
}