using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace ZoeMeow.DUTAPI
{
    public static partial class News
    {
        public static List<NewsItem> GetNews(NewsType newsType, int page = 1, string query = null)
        {
            List<NewsItem> result = null;

            try
            {
                result = new List<NewsItem>();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://sv.dut.udn.vn");

                HttpResponseMessage response = client.GetAsync($"/WebAjax/evLopHP_Load.aspx?E={(newsType == NewsType.Global ? "CTRTBSV" : "CTRTBGV")}&PAGETB={(page > 0 ? page : 1)}&COL=TieuDe&NAME={query}&TAB=1").Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception(String.Format("The request has return code {0}.", response.StatusCode));

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response.Content.ReadAsStringAsync().Result);

                HtmlNodeCollection htmlDocNews = htmlDoc.DocumentNode.SelectNodes("//div[@class='tbBox']");
                // TODO: Add exception here.
                if (htmlDocNews == null || htmlDocNews.Count == 0)
                    throw new Exception("");

                foreach (HtmlNode htmlItem in htmlDocNews)
                {
                    NewsItem item = new NewsItem();

                    var htmlTemp = new HtmlDocument();
                    htmlTemp.LoadHtml(htmlItem.InnerHtml);

                    string title = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxCaption']")[0].InnerText;
                    string[] titleTemp = title.Split(new string[] { ":&nbsp;&nbsp;&nbsp;&nbsp; " }, StringSplitOptions.None);

                    if (titleTemp.Length == 2)
                    {
                        item.Date = Convert.ToDateTime(titleTemp[0].Replace(" ", ""));
                        item.Title = WebUtility.HtmlDecode(titleTemp[1]);
                        item.ContentHTML = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerHtml;
                        item.Content = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerText;
                    }
                    else
                    {
                        item.Title = WebUtility.HtmlDecode(title);
                        item.ContentHTML = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerHtml;
                        item.Content = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerText;
                    }

                    result.Add(item);
                }
            }
            catch
            {
                if (result != null)
                {
                    result.Clear();
                    result = null;
                }
            }

            return result;
        }
    }
}
