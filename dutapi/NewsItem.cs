using System;

namespace ZoeMeow.DUTAPI
{
    public class NewsItem
    {
        /// <summary>
        /// News title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// News content in HTML.
        /// </summary>
        public string ContentHTML { get; set; }

        /// <summary>
        /// News content in plain text.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// News date when it posted.
        /// </summary>
        public Nullable<DateTime> Date { get; set; }

        public bool Equals(NewsItem news)
        {
            if (base.Equals(news))
                return true;

            if (news.Title != this.Title ||
                news.ContentHTML != this.ContentHTML ||
                news.Content != this.Content ||
                news.Date != this.Date
                )
                return false;

            return true;
        }
    }
}
