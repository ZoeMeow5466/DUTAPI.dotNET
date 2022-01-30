using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using ZoeMeow.DUTAPI.Objects;

namespace ZoeMeow.DUTAPI
{
    public partial class Session
    {
        public List<SubjectFee> GetSubjectsFee(int year = 20, int semester = 1, bool studyAtSummer = false)
        {
            List<SubjectFee> list = null;

            try
            {
                list = new List<SubjectFee>();

                var httpReturn = webClient.Get(String.Format($"http://sv.dut.udn.vn/WebAjax/evLopHP_Load.aspx?E=THPhiLoad&Code={ year }{ semester }{ (studyAtSummer ? 1 : 0) }"));
                if (successfulStatusCode.Contains(httpReturn.StatusCode))
                {
                    HtmlDocument httpTemp = new HtmlDocument();
                    httpTemp.LoadHtml(httpReturn.HTMLDocument.GetElementbyId("THocPhi_GridInfo").InnerHtml);
                    var rowCollection = httpTemp.DocumentNode.SelectNodes("//tr[@class='GridRow']");

                    foreach (HtmlNode row in rowCollection)
                    {
                        HtmlDocument httpTemp2 = new HtmlDocument();
                        httpTemp2.LoadHtml(row.InnerHtml);
                        var cellCollection = httpTemp2.DocumentNode.SelectNodes("//td[contains(@class, 'GridCell')]");

                        SubjectFee item = new SubjectFee();
                        item.ID = cellCollection[1].InnerText;
                        item.Name = cellCollection[2].InnerText;
                        item.Credit = ConvertTo<float>(cellCollection[3].InnerText);
                        item.IsQuality = cellCollection[4].Attributes["class"].Value.Contains("GridCheck");
                        item.Price = ConvertTo<double>(cellCollection[5].InnerText.Replace(",", null));
                        item.Debt = cellCollection[6].Attributes["class"].Value.Contains("GridCheck");
                        item.IsReStudy = cellCollection[7].Attributes["class"].Value.Contains("GridCheck");
                        item.VerifiedPaymentAt = cellCollection[8].InnerText;

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

            }

            return list;
        }
    }
}
