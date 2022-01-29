using System;
using System.Collections.Generic;

namespace ZoeMeow.DUTAPI.Objects
{
    public class NewsSubject : NewsGeneral
    {
        /// <summary>
        /// Các lớp học phần bị ảnh hưởng bởi thông báo này.
        /// </summary>
        public List<SubjectInfo> Subjects { get; set; }

        /// <summary>
        /// Trạng thái lớp học phần.
        /// </summary>
        public SubjectStatus Status { get; set; }

        /// <summary>
        /// Phòng học lớp học phần (nếu có).
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Khoảng tiết học của lớp học phần (nếu có).
        /// </summary>
        public string LessonRange { get; set; }

        /// <summary>
        /// Ngày bắt đầu của lớp học phần (nếu có).
        /// </summary>
        public Nullable<DateTime> DateStart { get; set; }

        /// <summary>
        /// Ngày kết thúc của lớp học phần (nếu có).
        /// </summary>
        public Nullable<DateTime> DateEnd { get; set; }

        /// <summary>
        /// Thông báo của giáo viên trên thông báo lớp học phần (nếu có).
        /// </summary>
        public string Message { get; set; }

        public bool Equals(NewsSubject news)
        {
            if (base.Equals(news))
                return true;

            if (!base.Equals(news) ||
                this.Status != news.Status ||
                this.Room != news.Room ||
                this.LessonRange != news.LessonRange ||
                this.DateStart != news.DateStart ||
                this.DateEnd != news.DateEnd ||
                this.Message != news.Message
                ) return false;

            for (int i = 0; i < this.Subjects.Count; i++)
            {
                bool found = true;
                for (int j = 0; j < news.Subjects.Count; j++)
                {
                    if (!this.Subjects[i].Equals(news.Subjects[i]))
                    {
                        found = false;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            return true;
        }
    }
}
