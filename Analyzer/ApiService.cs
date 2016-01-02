using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Analyzer
{
    public class ApiService : IApiService
    {
        protected readonly string apiKey;

        public ApiService(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("No key specified.");

            apiKey = key;
        }


        public JObject GetData(ApiRegion region, string requestParams)
        {
            var requestUrl = GetRequestUrl(region, requestParams);

            var request = WebRequest.Create(requestUrl + "?api_key=" + apiKey);
            try
            {
                var response = (HttpWebResponse)(request.GetResponse());

                var responseStream = response.GetResponseStream();
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                        return JObject.Parse(reader.ReadToEnd());

                    return JObject.Parse(response.StatusCode.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private string GetRequestUrl(ApiRegion region, string requestParams)
        {
            string baseurl = null;
            switch (region)
            {
                case ApiRegion.NA:
                    baseurl = "https://na.api.pvp.net/";
                    break;
                case ApiRegion.EUW:
                    baseurl = "https://euw.api.pvp.net/";
                    break;
                default:
                    baseurl = null;
                    break;
            }
            return baseurl + requestParams;
        }
    }
}
