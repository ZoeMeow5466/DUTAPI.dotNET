using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ZoeMeow.DUTAPI
{
    public partial class Session
    {
        private bool isInitialized = false;
        private Cookies cookies;
        private Uri baseAddress;

        public Session()
        {
            baseAddress = new Uri("http://sv.dut.udn.vn");
            cookies = new Cookies();
        }

        private void SaveCookie(HttpResponseHeaders header)
        {
            string[] cookieHeaderList = new string[2] { "Set-Cookie", "Cookie" };
            foreach (string cookieHeader in cookieHeaderList)
            {
                if (header.TryGetValues(cookieHeader, out var cookieValue))
                {
                    foreach (var d in cookieValue)
                    {
                        string[] d1 = d.Split(";");
                        foreach (string d2 in d1)
                        {
                            string[] d3 = d2.Trim(' ').Split('=');
                            Cookie cookie = new Cookie(d3[0], d3.Length > 1 ? d3[1] : null);
                            cookies.SetCookie(cookie);
                        }
                    }
                    break;
                }
            }
        }

        private HttpClient CreateNewHttpClientInstance()
        {
            if (!isInitialized)
            {
                using (HttpClient clientInit = new HttpClient())
                {
                    clientInit.BaseAddress = baseAddress;
                    HttpResponseMessage response = clientInit.GetAsync("").Result;
                    SaveCookie(response.Headers);
                    clientInit.Dispose();
                }
                isInitialized = true;
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = baseAddress;
            if (!String.IsNullOrEmpty(cookies.ToString()))
                client.DefaultRequestHeaders.Add("Cookie", cookies.ToString());
            return client;
        }

        public LoginStatus IsLoggedIn()
        {
            LoginStatus result = LoginStatus.Unknown;

            HttpClient client = CreateNewHttpClientInstance();
            HttpResponseMessage response = client.GetAsync("/WebAjax/evLopHP_Load.aspx?E=TTKBLoad&Code=2110").Result;
            if (response.IsSuccessStatusCode)
                result = LoginStatus.LoggedIn;
            else result = LoginStatus.LoggedOut;

            SaveCookie(response.Headers);
            return result;
        }

        public bool Logout()
        {
            HttpClient client = CreateNewHttpClientInstance();
            HttpResponseMessage response = client.GetAsync("/PageLogout.aspx").Result;
            SaveCookie(response.Headers);

            return IsLoggedIn() == LoginStatus.LoggedOut;
        }

        public bool Login(string username, string password)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("__VIEWSTATE", __VIEWSTATE);
            dict.Add("__VIEWSTATEGENERATOR", "20CC0D2F");
            dict.Add("_ctl0:MainContent:DN_txtAcc", username);
            dict.Add("_ctl0:MainContent:DN_txtPass", password);
            dict.Add("_ctl0:MainContent:QLTH_btnLogin", "Đăng+nhập");
            FormUrlEncodedContent content = new FormUrlEncodedContent(dict);

            HttpClient client = CreateNewHttpClientInstance();
            HttpResponseMessage response = client.PostAsync("/PageDangNhap.aspx", content).Result;
            SaveCookie(response.Headers);

            return IsLoggedIn() == LoginStatus.LoggedIn;
        }


    }
}
