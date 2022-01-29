using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ZoeMeow.DUTAPI
{
    public class WebClient : IDisposable
    {
        private HttpClient http = null;

        public WebClient()
        {
            http = new HttpClient();
        }

        public void Dispose()
        {
            if (http != null)
                http.Dispose();
        }

        public bool Executing { get; set; }

        public bool Initialized
        {
            get { return (http != null); }
        }

        public HttpResponseEventArgs Get(string url)
        {
            HttpResponseEventArgs result = new HttpResponseEventArgs();
            try
            {
                if (Executing)
                    throw new Exception("Another request is executing.");

                if (!Initialized)
                    throw new Exception("Client was not initialized.");

                Executing = true;

                using (HttpResponseMessage response = http.GetAsync(url).Result)
                {
                    result.HTMLInString = response.Content.ReadAsStringAsync().Result;
                    result.SuccessfulRequest = response.IsSuccessStatusCode;
                    result.StatusCode = Convert.ToInt32(response.StatusCode);

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(result.HTMLInString);
                    result.HTMLDocument = doc;
                }
            }
            catch (Exception ex)
            {
                result.SuccessfulRequest = false;
                result.Message = ex.Message;
            }
            finally
            {
                Executing = false;
            }
            return result;
        }

        public HttpResponseEventArgs Post(string url, Dictionary<string, string> args)
        {
            HttpResponseEventArgs result = new HttpResponseEventArgs();
            try
            {
                if (Executing)
                    throw new Exception("Another request is executing.");

                if (!Initialized)
                    throw new Exception("Client was not initialized.");

                Executing = true;

                var content = new FormUrlEncodedContent(args);
                using (HttpResponseMessage response = http.PostAsync(url, content).Result)
                {
                    result.SuccessfulRequest = response.IsSuccessStatusCode;
                    result.StatusCode = Convert.ToInt32(response.StatusCode);
                    result.HTMLInString = response.Content.ReadAsStringAsync().Result;
                }
                content.Dispose();
            }
            catch (Exception ex)
            {
                result.SuccessfulRequest = false;
                result.Message = ex.Message;
            }
            finally
            {
                Executing = false;
            }
            return result;
        }
    }
}
