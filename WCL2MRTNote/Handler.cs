using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace WCL2MRTNote
{
    internal class Handler
    {
        public static string HttpRequest(string url)
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
        public static string SpellID2Name(string SpellID)
        {
            try
            {
                string Url = "https://ko.wowhead.com/spell=" + SpellID;
                string title = Regex.Match(HttpRequest(Url), @"<title>(.*?)</title>").Groups[1].Value;
                title = title.Substring(0, title.IndexOf("-"));
                return title;
            }
            catch (Exception)
            {
                
            }
            return "";
        }
    }
}

