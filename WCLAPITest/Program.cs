/*
Your client was successfully created. The client ID is 98203601-1376-4144-a5d4-08cd39d3f76e and the client secret is LJaZUjv2PFigV5ZQvTabHyKiFhhcaN2CWNAkIrrN. 
This secret will never be shown again, so make sure to copy it now. If you lose the secret, you will have to delete this client and make a new one.
*/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace WCLAPITest
{
    class Program
    {
        static readonly string apiURL = "https://www.warcraftlogs.com/api/v2/client";
        static readonly string tokenURL = "https://www.warcraftlogs.com/oauth/token";

        static readonly string reportData = @"query ($code:String){
    reportData{
        report(code:$code){
            fights(difficulty:3){
                id
                name
                startTime
                endTime
            }
        }
    }
}";
        // Retrieves access token for a given client id and secret.
        static Dictionary<string, string> data = new Dictionary<string, string>
    {
        { "grant_type", "client_credentials" }
    };
        
        static void Main(string[] args)
        {
            // Step I: retrieve access token for our application
            var response = GetToken(store: true);
            // Step II: get data for our application
            // example code
            string code = "AGF3JyRmg27fN4TV";
            Console.WriteLine(GetData(reportData, ditchItems: new string[] { "data", "reportData", "report", "fights" }, code));
        }
        static HttpResponseMessage GetToken(bool store)
        {
            var auth = ("98203601-1376-4144-a5d4-08cd39d3f76e", "LJaZUjv2PFigV5ZQvTabHyKiFhhcaN2CWNAkIrrN");
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(tokenURL, new FormUrlEncodedContent(data)).Result;

                // stores the token if the the request was authorized
                if (store && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    StoreToken(response);
                }
                return response;
            }
        }
        static void StoreToken(HttpResponseMessage response)
        {
            // Stores token to a hidden json file.
            try
            {
                string json = response.Content.ReadAsStringAsync().Result;
                var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                using (var file = new System.IO.StreamWriter(".credentials.json", false, System.Text.Encoding.UTF8))
                {
                    file.Write(JsonConvert.SerializeObject(token));
                }
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Could not create the file!");
            }
        }

        static Dictionary<string, string> RetrieveToken()
        {
            // Extracts access token from json file.
            try
            {
                using (var file = new System.IO.StreamReader(".credentials.json", System.Text.Encoding.UTF8))
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(file.ReadToEnd());
                }
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        static object GetData(string query, string[] ditchItems, string code)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {RetrieveToken()["access_token"]}");
                using (var request = new HttpRequestMessage(HttpMethod.Get, apiURL))
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                    var response = client.SendAsync(request).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string json = response.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                        if (ditchItems == null)
                        {
                            return result;
                        }
                        return ditchItems.Aggregate(result, (acc, val) => (Dictionary<string, object>)acc[val]);
                    }
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result)["error"];
                }
            }
        }
    }
}