using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace WCL2MRTNote
{
    internal class Handler
    {
        public string Request_Json(string url)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader stream = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            result = stream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[예외] : " + e.Message);
            }
            return result;
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static string ConvertMilliseconds(int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).ToString(@"mm\:ss");
        }
    }

    public class Boss
    {
        public string name { get; set; }
        public List<Cast> casts { get; set; }
    }

    public class Cast
    {
        public int ts { get; set; }
        public int id { get; set; }
        public int d { get; set; }
    }

    public class Fight
    {
        public string report_id { get; set; }
        public int fight_id { get; set; }
        public double percent { get; set; }
        public bool kill { get; set; }
        public int duration { get; set; }
        public double time { get; set; }
        public Boss boss { get; set; }
        public List<Player> players { get; set; }
    }

    public class Player
    {
        public string name { get; set; }
        public int source_id { get; set; }
        public string @class { get; set; }
        public string spec { get; set; }
        public string role { get; set; }
        public int total { get; set; }
        public string covenant { get; set; }
        public List<Cast> casts { get; set; }
        public List<object> deaths { get; set; }
        public List<object> resurrects { get; set; }
    }

    public class Root
    {
        public List<Fight> fights { get; set; }
        public int updated { get; set; }
        public string difficulty { get; set; }
        public string metric { get; set; }
    }


}
