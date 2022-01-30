using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZoeMeow.DUTAPI.Objects;

namespace ZoeMeow.DUTAPI
{
    public partial class Session
    {
        public List<SubjectInfo> GetScheduleSubjects(int year = 20, int semester = 1, bool studyAtSummer = false)
        {
            List<SubjectInfo> list = null;

            // try
            {
                list = new List<SubjectInfo>();

                var httpReturn = webClient.Get(String.Format($"http://sv.dut.udn.vn/WebAjax/evLopHP_Load.aspx?E=TTKBLoad&Code={ year }{ semester }{ (studyAtSummer ? 1 : 0) }"));
                if (successfulStatusCode.Contains(httpReturn.StatusCode))
                {
                    HtmlDocument httpTemp;
                    
                    // Schedule at study
                    httpTemp = new HtmlDocument();
                    httpTemp.LoadHtml(httpReturn.HTMLDocument.GetElementbyId("TTKB_GridInfo").InnerHtml);
                    var rowListStudy = httpTemp.DocumentNode.SelectNodes("//tr[@class='GridRow']");
                    if (rowListStudy != null)
                        foreach (HtmlNode row in rowListStudy)
                        {
                            HtmlDocument httpTempStudyItem = new HtmlDocument();
                            httpTempStudyItem.LoadHtml(row.InnerHtml);
                            var cellCollection = httpTempStudyItem.DocumentNode.SelectNodes("//td[contains(@class, 'GridCell')]");

                            SubjectInfo item = new SubjectInfo();
                            item.ID = cellCollection[1].InnerText;
                            item.Name = cellCollection[2].InnerText;
                            item.Credit = ConvertTo<float>(cellCollection[3].InnerText);
                            item.IsHighQuality = cellCollection[5].Attributes["class"].Value.Contains("GridCheck");
                            item.Lecturer = cellCollection[6].InnerText;
                            item.ScheduleStudy = cellCollection[7].InnerText;
                            item.Weeks = cellCollection[8].InnerText;
                            item.PointFomula = cellCollection[10].InnerText;
                            list.Add(item);
                        }

                    // Schedule at examination
                    httpTemp = new HtmlDocument();
                    httpTemp.LoadHtml(httpReturn.HTMLDocument.GetElementbyId("TTKB_GridLT").InnerHtml);
                    var rowCollection = httpTemp.DocumentNode.SelectNodes("//tr[@class='GridRow']");
                    if (rowCollection != null)
                        foreach (HtmlNode row in rowCollection)
                        {
                            HtmlDocument httpTempExamItem = new HtmlDocument();
                            httpTempExamItem.LoadHtml(row.InnerHtml);
                            var cellCollection = httpTempExamItem.DocumentNode.SelectNodes("//td[contains(@class, 'GridCell')]");

                            var item = list.Where(p => p.ID == cellCollection[1].InnerText).First();
                            if (item != null)
                            {
                                item.GroupExam = cellCollection[3].InnerText;
                                item.IsGlobalExam = cellCollection[4].Attributes["class"].Value.Contains("GridCheck");
                                item.DateExamInString = cellCollection[5].InnerText;

                                if (item.DateExamInString != null)
                                {
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
                                                item.DateExam = Convert.ToDateTime(splited[i].Split(new string[] { ": " }, StringSplitOptions.None)[1]);
                                                break;
                                            case "Giờ":
                                                time = splited[i].Split(new string[] { ": " }, StringSplitOptions.None)[1];
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    if (item.DateExam != null && time != null)
                                    {
                                        item.DateExam = item.DateExam.Value.AddHours(Convert.ToInt32(time.Split('h')[0]));
                                        if (time.Split('h').Length == 2)
                                        {
                                            int minute = 0;
                                            if (int.TryParse(time.Split('h')[1], out minute))
                                                item.DateExam = item.DateExam.Value.AddMinutes(Convert.ToInt32(minute));
                                        }
                                    }
                                }
                            }
                        }
                }
            }
            // catch
            // {
            //     list.Clear();
            //     list = null;
            // }
            // finally
            // {
            // 
            // }

            return list;
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
