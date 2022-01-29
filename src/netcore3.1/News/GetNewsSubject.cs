using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ZoeMeow.DUTAPI.Objects;

namespace ZoeMeow.DUTAPI
{
    public static partial class News
    {
        public static List<NewsSubject> GetNewsSubjects(int page = 1)
        {
            List<NewsSubject> list = new List<NewsSubject>();
            WebClient web = new WebClient();

            try
            {
                var httpReturn = web.Get($"http://sv.dut.udn.vn/WebAjax/evLopHP_Load.aspx?E=CTRTBGV&PAGETB={ page }&COL=TieuDe&NAME=&TAB=1");
                var newsCollection = httpReturn.HTMLDocument.DocumentNode.SelectNodes("//div[@class='tbBox']");

                if (newsCollection != null)
                {
                    foreach (HtmlAgilityPack.HtmlNode htmlItem in newsCollection)
                    {
                        var item = new NewsSubject();

                        var htmlTemp = new HtmlAgilityPack.HtmlDocument();
                        htmlTemp.LoadHtml(htmlItem.InnerHtml);

                        string title = htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxCaption']")[0].InnerText;
                        string[] titleTemp = title.Split(new string[] { ":&nbsp;&nbsp;&nbsp;&nbsp; " }, StringSplitOptions.None);

                        // TODO: Lấy thông tin cơ bản - Chú thích ở đây
                        if (titleTemp.Length == 2)
                        {
                            item.Date = Convert.ToDateTime(titleTemp[0].Replace(" ", ""));
                            item.Title = WebUtility.HtmlDecode(titleTemp[1]);
                            item.Content = WebUtility.HtmlDecode(htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerText);
                            item.ContentString = WebUtility.HtmlDecode(htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerText);
                        }
                        else
                        {
                            item.Title = WebUtility.HtmlDecode(title);
                            item.Content = WebUtility.HtmlDecode(htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerText);
                            item.ContentString = WebUtility.HtmlDecode(htmlTemp.DocumentNode.SelectNodes("//div[@class='tbBoxContent']")[0].InnerText);
                        }

                        // TODO: Lấy thông tin nâng cao - Chú thích ở đây
                        item.Subjects = CrawlNews_LopHocPhan_FromTitle_ListLopHocPhan(item.Title);
                        item.Status = CrawlNews_LopHocPhan_FromContent_TrangThai(item.Content);
                        item.LessonRange = CrawlNews_LopHocPhan_FromContent_TietHocRaw(item.Content);
                        item.Date = CrawlNews_LopHocPhan_FromContent_NgayThucHien(item.Content);
                        item.DateStart = CrawlNews_LopHocPhan_FromContent_DateStart(item.Content);
                        item.DateEnd = CrawlNews_LopHocPhan_FromContent_DateEnd(item.Content);
                        item.Room = CrawlNews_LopHocPhan_FromContent_Room(item.Content);

                        list.Add(item);
                    }
                }
            }
            catch
            {
                list = null;
            }
            finally
            {
                web.Dispose();
            }

            return list;
        }


        private static Dictionary<string, TimeSpan> CrawlNews_LopHocPhan_TietThanhGio =
                new Dictionary<string, TimeSpan>()
                {
                    {"1", new TimeSpan(7, 0, 0)},
                    {"2", new TimeSpan(8, 0, 0)},
                    {"3", new TimeSpan(9, 0, 0)},
                    {"4", new TimeSpan(10, 0, 0)},
                    {"5", new TimeSpan(11, 0, 0)},
                    {"6", new TimeSpan(12, 30, 0)},
                    {"7", new TimeSpan(13, 30, 0)},
                    {"8", new TimeSpan(14, 30, 0)},
                    {"9", new TimeSpan(15, 30, 0)},
                    {"10", new TimeSpan(16, 30, 0)},
                };

        #region Crawl data from News - Lop hoc phan
        private static List<SubjectInfo> CrawlNews_LopHocPhan_FromTitle_ListLopHocPhan(string title)
        {
            string separator = " , ";

            List<SubjectInfo> list = null;

            try
            {
                list = new List<SubjectInfo>();

                title = title.Remove(0, title.IndexOf(" thông báo đến lớp: ") + " thông báo đến lớp: ".Length);
                var sArray = title.Split(new string[] { separator }, StringSplitOptions.None);

                for (int i = 0; i < sArray.Length; i++)
                {
                    if (sArray[i].Contains(".Nh"))
                        sArray[i] = sArray[i].Replace(".Nh", ".");

                    string beginStr = " [";
                    string endStr = "]";
                    int beginIndex = sArray[i].IndexOf(beginStr);
                    int endIndex = sArray[i].IndexOf(endStr);

                    SubjectInfo lhp = new SubjectInfo();
                    lhp.Name = sArray[i].Substring(0, beginIndex);
                    lhp.ID = sArray[i].Substring(
                        beginIndex + beginStr.Length,
                        endIndex - beginIndex - beginStr.Length
                        );
                    list.Add(lhp);
                }
            }
            catch
            {
                list = null;
            }

            return list;
        }

        private static SubjectStatus CrawlNews_LopHocPhan_FromContent_TrangThai(string content)
        {
            if (content.Contains("Lớp NGHỈ HỌC"))
                return SubjectStatus.Leaving;
            if (content.Contains("Lớp HỌC BÙ"))
                return SubjectStatus.MakeUp;
            return SubjectStatus.Notify;
        }

        private static DateTime CrawlNews_LopHocPhan_FromContent_NgayThucHien(string content)
        {
            DateTime dt = new DateTime();

            try
            {
                SubjectStatus lhpState = CrawlNews_LopHocPhan_FromContent_TrangThai(content);
                string beginStr = " ngày: ";
                string endStr = ",";
                int beginIndex = content.IndexOf(beginStr);
                int endIndex = content.IndexOf(endStr);

                if (lhpState == SubjectStatus.Leaving)
                {
                    dt = Convert.ToDateTime(
                        content.Substring(
                            beginIndex + beginStr.Length,
                            content.Length - beginIndex - beginStr.Length
                            )
                        );
                }
                else if (lhpState == SubjectStatus.MakeUp)
                {
                    dt = Convert.ToDateTime(
                        content.Substring(
                            beginIndex + beginStr.Length,
                            endIndex - beginIndex - beginStr.Length
                            )
                        );
                }
            }
            catch
            {
                dt = new DateTime();
            }

            return dt;
        }

        private static string CrawlNews_LopHocPhan_FromContent_TietHocRaw(string content)
        {
            string result = null;

            try
            {
                SubjectStatus lhpState = CrawlNews_LopHocPhan_FromContent_TrangThai(content);
                string beginStr = null;
                string endStr = null;
                if (lhpState == SubjectStatus.Leaving)
                {
                    beginStr = "(Tiết:";
                    endStr = ")";
                }
                else if (lhpState == SubjectStatus.MakeUp)
                {
                    beginStr = ",Tiết: ";
                    endStr = ", ";
                }
                int beginIndex = content.IndexOf(beginStr);
                int endIndex = content.IndexOf(endStr, beginIndex);

                result = content.Substring(
                    beginIndex + beginStr.Length,
                    endIndex - beginIndex - beginStr.Length
                    );
            }
            catch
            {
                result = null;
            }

            return result;
        }

        private static string[] CrawlNews_LopHocPhan_FromContent_TietHocArray(string content)
        {
            string[] result = null;

            string separator = "-";
            try
            {
                string sTemp = CrawlNews_LopHocPhan_FromContent_TietHocRaw(content);
                if (sTemp != null)
                    result = sTemp.Split(new string[] { separator }, StringSplitOptions.None);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        private static string CrawlNews_LopHocPhan_FromContent_Room(string content)
        {
            string result = null;

            try
            {
                SubjectStatus lhpState = CrawlNews_LopHocPhan_FromContent_TrangThai(content);
                string beginStr = null;

                if (lhpState == SubjectStatus.MakeUp)
                {
                    beginStr = ", phòng: ";
                }
                int beginIndex = content.IndexOf(beginStr);

                result = content.Substring(
                    beginIndex + beginStr.Length,
                    content.Length - beginIndex - beginStr.Length
                    );
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public static DateTime CrawlNews_LopHocPhan_FromContent_DateStart(string content)
        {
            DateTime dt = new DateTime();

            try
            {
                SubjectStatus lhpState = CrawlNews_LopHocPhan_FromContent_TrangThai(content);
                DateTime ngayThucHien = CrawlNews_LopHocPhan_FromContent_NgayThucHien(content);
                string[] tietHoc = CrawlNews_LopHocPhan_FromContent_TietHocArray(content);

                if (tietHoc != null)
                    if (tietHoc.Length == 2)
                        dt = ngayThucHien.Add(CrawlNews_LopHocPhan_TietThanhGio[tietHoc[0]]);
            }
            catch
            {
                dt = new DateTime();
            }

            return dt;
        }

        public static DateTime CrawlNews_LopHocPhan_FromContent_DateEnd(string content)
        {
            DateTime dt = new DateTime();

            try
            {
                SubjectStatus lhpState = CrawlNews_LopHocPhan_FromContent_TrangThai(content);
                DateTime ngayThucHien = CrawlNews_LopHocPhan_FromContent_NgayThucHien(content);
                string[] tietHoc = CrawlNews_LopHocPhan_FromContent_TietHocArray(content);

                if (tietHoc != null)
                    if (tietHoc.Length == 2)
                        dt = ngayThucHien.Add(CrawlNews_LopHocPhan_TietThanhGio[tietHoc[1]]);
            }
            catch
            {
                dt = new DateTime();
            }

            return dt;
        }
        #endregion
    }
}
