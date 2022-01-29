using HtmlAgilityPack;
using System;

namespace ZoeMeow.DUTAPI
{
    public class HttpResponseEventArgs : EventArgs
    {
        public string HTMLInString { get; set; }

        public HtmlDocument HTMLDocument { get; set; }

        public int StatusCode { get; set; }

        public bool SuccessfulRequest { get; set; } = false;

        public string Message { get; set; } = null;
    }
}
