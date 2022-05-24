using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ZoeMeow.DUTAPI
{
    public partial class Session
    {
        public List<SubjectSchedule> GetSubjectsSchedule(int year = 20, int semester = 1, bool studyAtSummer = false)
        {
            List<SubjectSchedule> result = null;

            try
            {
                result = new List<SubjectSchedule>();
                HttpClient client = CreateNewHttpClientInstance();
                HttpResponseMessage response = client.GetAsync($"/WebAjax/evLopHP_Load.aspx?E=TTKBLoad&Code={year}{semester}{(studyAtSummer ? 1 : 0)}").Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception(String.Format("The request has return code {0}.", response.StatusCode));

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response.Content.ReadAsStringAsync().Result);

                // TODO: Schedule Study
                HtmlDocument htmlDocSchStudy = new HtmlDocument();
                htmlDocSchStudy.LoadHtml(htmlDoc.GetElementbyId("TTKB_GridInfo").InnerHtml);
                var rowListStudy = htmlDocSchStudy.DocumentNode.SelectNodes("//tr[@class='GridRow']");

                if (rowListStudy != null && rowListStudy.Count > 0)
                {
                    foreach (HtmlNode row in rowListStudy)
                    {
                        HtmlDocument httpTempStudyItem = new HtmlDocument();
                        httpTempStudyItem.LoadHtml(row.InnerHtml);
                        var cellCollection = httpTempStudyItem.DocumentNode.SelectNodes("//td[contains(@class, 'GridCell')]");

                        SubjectSchedule item = new SubjectSchedule();
                        item.ID = cellCollection[1].InnerText;
                        item.Name = cellCollection[2].InnerText;
                        item.Credit = ConvertTo<float>(cellCollection[3].InnerText);
                        item.IsHighQuality = cellCollection[5].Attributes["class"].Value.Contains("GridCheck");
                        item.Lecturer = cellCollection[6].InnerText;
                        item.ScheduleStudy = cellCollection[7].InnerText;
                        item.Weeks = cellCollection[8].InnerText;
                        item.PointFomula = cellCollection[10].InnerText;

                        result.Add(item);
                    }
                }
                    

                // TODO: Schedule Examination
                HtmlDocument htmlDocSchExam = new HtmlDocument();
                htmlDocSchExam.LoadHtml(htmlDoc.GetElementbyId("TTKB_GridLT").InnerHtml);
                var rowListExam = htmlDocSchExam.DocumentNode.SelectNodes("//tr[@class='GridRow']");
                if (rowListExam != null && rowListExam.Count > 0)
                    foreach (HtmlNode row in rowListExam)
                    {
                        HtmlDocument httpTempExamItem = new HtmlDocument();
                        httpTempExamItem.LoadHtml(row.InnerHtml);
                        var cellCollection = httpTempExamItem.DocumentNode.SelectNodes("//td[contains(@class, 'GridCell')]");

                        var item = result.Where(p => p.ID == cellCollection[1].InnerText).First();
                        if (item == null)
                            continue;

                        item.GroupExam = cellCollection[3].InnerText;
                        item.IsGlobalExam = cellCollection[4].Attributes["class"].Value.Contains("GridCheck");
                        item.DateExamInString = cellCollection[5].InnerText;

                        if (item.DateExamInString == null)
                            continue;

                        DateTime? dateTime = null;
                        string[] splited = item.DateExamInString.Split(new string[] { ", " }, StringSplitOptions.None);
                        string time = null;
                        for (int i = 0; i < splited.Length; i++)
                        {
                            switch (splited[i].Split(new string[] { ": " }, StringSplitOptions.None)[0])
                            {
                                case "Phòng":
                                    item.RoomExam = splited[i].Split(new string[] { ": " }, StringSplitOptions.None)[1];
                                    break;
                                case "Ngày":
                                    dateTime = Convert.ToDateTime(splited[i].Split(new string[] { ": " }, StringSplitOptions.None)[1]);
                                    break;
                                case "Giờ":
                                    time = splited[i].Split(new string[] { ": " }, StringSplitOptions.None)[1];
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (dateTime != null && time != null)
                        {
                            dateTime = dateTime.Value.AddHours(Convert.ToInt32(time.Split('h')[0]));
                            if (time.Split('h').Length == 2)
                            {
                                int minute = 0;
                                if (int.TryParse(time.Split('h')[1], out minute))
                                    dateTime = dateTime.Value.AddMinutes(Convert.ToInt32(minute));
                            }
                            // -new DateTime(1970, 1, 1) for UnixTimeStamp.
                            // -7 because of GMT + 7.
                            item.DateExamInUnix = (long)dateTime.Value.Subtract(new DateTime(1970, 1, 1)).Add(new TimeSpan(-7, 0, 0)).TotalSeconds;
                        }
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

        public List<SubjectFee> GetSubjectsFee(int year = 20, int semester = 1, bool studyAtSummer = false)
        {
            List<SubjectFee> result = null;

            try
            {
                result = new List<SubjectFee>();

                HttpClient client = CreateNewHttpClientInstance();
                HttpResponseMessage response = client.GetAsync($"/WebAjax/evLopHP_Load.aspx?E=THPhiLoad&Code={year}{semester}{(studyAtSummer ? 1 : 0)}").Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception(String.Format("The request has return code {0}.", response.StatusCode));

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response.Content.ReadAsStringAsync().Result);

                HtmlDocument htmlDocFee = new HtmlDocument();
                htmlDocFee.LoadHtml(htmlDoc.GetElementbyId("THocPhi_GridInfo").InnerHtml);
                var rowListFee = htmlDocFee.DocumentNode.SelectNodes("//tr[@class='GridRow']");

                if (rowListFee != null && rowListFee.Count > 0)
                    foreach (HtmlNode row in rowListFee)
                    {
                        HtmlDocument httpTemp2 = new HtmlDocument();
                        httpTemp2.LoadHtml(row.InnerHtml);
                        var cellCollection = httpTemp2.DocumentNode.SelectNodes("//td[contains(@class, 'GridCell')]");

                        SubjectFee item = new SubjectFee();
                        item.ID = cellCollection[1].InnerText;
                        item.Name = cellCollection[2].InnerText;
                        item.Credit = ConvertTo<float>(cellCollection[3].InnerText);
                        item.IsHighQuality = cellCollection[4].Attributes["class"].Value.Contains("GridCheck");
                        item.Price = ConvertTo<double>(cellCollection[5].InnerText.Replace(",", null));
                        item.Debt = cellCollection[6].Attributes["class"].Value.Contains("GridCheck");
                        item.IsReStudy = cellCollection[7].Attributes["class"].Value.Contains("GridCheck");
                        item.VerifiedPaymentAt = cellCollection[8].InnerText;

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

        private static T ConvertTo<T>(string str)
        {
            T result = default(T);

            Type t = typeof(T);
            try
            {
                if (t == typeof(int))
                {
                    int valueInt = Convert.ToInt32(str);
                    result = (T)Convert.ChangeType(valueInt, typeof(T));
                }
                else if (t == typeof(float))
                {
                    float valueFloat = Convert.ToSingle(str);
                    result = (T)Convert.ChangeType(valueFloat, typeof(T));
                }
                else if (t == typeof(double))
                {
                    double valueDouble = Convert.ToInt32(str);
                    result = (T)Convert.ChangeType(valueDouble, typeof(T));
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }

            return result;
        }
    }
}
