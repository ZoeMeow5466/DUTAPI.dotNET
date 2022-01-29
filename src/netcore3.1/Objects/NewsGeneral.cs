using System;

namespace ZoeMeow.DUTAPI.Objects
{
    public class NewsGeneral
    {
        /// <summary>
        /// ID thông báo (thông thường sẽ giữ nguyên).
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Tiêu đề thông báo.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nội dung của thông báo bằng HTML.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Nội dung của thông báo bằng văn bản.
        /// </summary>
        public string ContentString { get; set; }

        /// <summary>
        /// Ngày đăng thông báo.
        /// </summary>
        public Nullable<DateTime> Date { get; set; }

        public bool Equals(NewsGeneral news)
        {
            if (base.Equals(news))
                return true;

            if (news.ID != this.ID ||
                news.Title != this.Title ||
                news.Content != this.Content ||
                news.Date != this.Date
                )
                return false;
            
            return true;
        }
    }
}
