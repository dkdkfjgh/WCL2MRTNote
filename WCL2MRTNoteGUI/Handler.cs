using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
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
                MessageBox.Show("[예외] : " + e.Message);
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
        public static string SpellID2Name(int SpellID)
        {
            try
            {
                string Url = "https://ko.wowhead.com/spell=" + SpellID.ToString();
                string title = Regex.Match(HttpRequest(Url), @"<title>(.*?)</title>").Groups[1].Value;
                title = title.Substring(0, title.IndexOf("-"));
                return title;
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류 발생 : " + ex.Message);
            }
            return "";
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Boss
    {
        public int source_id { get; set; }
        public List<Cast> casts { get; set; }
        public string boss_slug { get; set; }
    }

    public class Cast
    {
        public int id { get; set; }
        public int ts { get; set; }
        public int? d { get; set; }
    }

    public class Fight
    {
        public int fight_id { get; set; }
        public DateTime start_time { get; set; }
        public int duration { get; set; }
        public List<Player> players { get; set; }
        public Boss boss { get; set; }
    }

    public class Player
    {
        public int source_id { get; set; }
        public List<Cast> casts { get; set; }
        public string name { get; set; }
        public string spec_slug { get; set; }
        public double total { get; set; }
    }

    public class Report
    {
        public string report_id { get; set; }
        public DateTime start_time { get; set; }
        public List<Fight> fights { get; set; }
    }

    public class Root
    {
        public string spec_slug { get; set; }
        public string boss_slug { get; set; }
        public string difficulty { get; set; }
        public string metric { get; set; }
        public List<Report> reports { get; set; }
    }
}