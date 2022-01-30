using System;
using System.Collections.Generic;
using System.Text;
using ZoeMeow.DUTAPI.Objects;

namespace ZoeMeow.DUTAPI
{
    public static partial class News
    {
        public static List<NewsGeneral> GetNewsGeneral(int page = 1)
        {
            WebClient web = new WebClient();
            List<NewsGeneral> list = null;

            try
            {
                list = new List<NewsGeneral>();

                var httpReturn = web.Get($"http://sv.dut.udn.vn/WebAjax/evLopHP_Load.aspx?E=CTRTBSV&PAGETB={ page }&COL=TieuDe&NAME=&TAB=1");
                var newsCollection = httpReturn.HTMLDocument.DocumentNode.SelectNodes("//div[@class='tbBox']");

                if (newsCollection != null)
                {
                    foreach (HtmlAgilityPack.HtmlNode htmlItem in newsCollection)
                    {
                        var item = new NewsGeneral();

                        var htmlTemp = new HtmlAgilityPack.HtmlDocument();
                        htmlTemp.LoadHtml(htmlItem.InnerHtml);

                        string title = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxCaption']")[0].InnerText;
                        string[] titleTemp = title.Split(new string[] { ":&nbsp;&nbsp;&nbsp;&nbsp; " }, StringSplitOptions.None);

                        if (titleTemp.Length == 2)
                        {
                            item.Date = Convert.ToDateTime(titleTemp[0].Replace(" ", ""));
                            item.Title = System.Net.WebUtility.HtmlDecode(titleTemp[1]);
                            item.Content = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerHtml;
                            item.ContentString = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerText;
                        }
                        else
                        {
                            item.Title = System.Net.WebUtility.HtmlDecode(title);
                            item.Content = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerHtml;
                            item.ContentString = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerText;
                        }

                        list.Add(item);
                    }
                }
            }
            catch
            {
                list.Clear();
                list = null;
            }
            finally
            {
                web.Dispose();
            }

            return list;
        }
    }
}
