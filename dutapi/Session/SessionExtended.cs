using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoeMeow.DUTAPI
{
    public partial class Session
    {
        public class Cookie
        {
            private string key, value;

            public Cookie() { }

            public Cookie(string key, string value)
            {
                this.key = key;
                this.value = value;
            }

            public string Key
            {
                get { return key; }
                set { key = value; }
            }

            public string Value
            {
                get { return value; }
                set { this.value = value; }
            }

            public new string ToString()
            {
                return String.Format("{0}{1}{2};", this.key, String.IsNullOrEmpty(this.value) ? null : "=", this.value);
            }
        }

        public class Cookies
        {
            private List<Cookie> cookieList;

            public Cookies()
            {
                cookieList = new List<Cookie>();
            }

            ~Cookies()
            {
                cookieList.Clear();
                cookieList = null;
            }

            public void SetCookie(Cookie cookie)
            {
                if (cookieList.Where(c => c.Key == cookie.Key).Count() == 1)
                {
                    Cookie cookieItem = cookieList.Where(c => c.Key == cookie.Key).First();
                    cookieItem.Value = cookie.Value;
                }
                else cookieList.Add(cookie);
            }

            public Cookie GetCookie(string cookieKey)
            {
                if (cookieList.Where(c => c.Key == cookieKey).Count() == 1)
                {
                    return cookieList.Where(c => c.Key == cookieKey).First();
                }
                else throw new Exception(
                    String.Format("Error while getting cookie named \"{0}\"", cookieKey));
            }

            public new string ToString()
            {
                string result = "";

                foreach (Cookie cookie in cookieList)
                {
                    result += cookie.ToString();
                }

                if (String.IsNullOrEmpty(result))
                    return null;

                return result;
            }
        }

        private string __VIEWSTATE = "/wEPDwUKMTY2NjQ1OTEyNA8WAh4TVmFsaWRhdGVSZXF1ZXN0TW9kZQIBFgJmD2QWAgIFDxYCHglpbm5lcmh0bWwF4i08dWwgaWQ9J21lbnUnIHN0eWxlPSd3aWR0aDogMTI4MHB4OyBtYXJnaW46IDAgYXV0bzsgJz48bGk+PGEgSUQ9ICdsUGFIT01FJyBzdHlsZSA9J3dpZHRoOjY1cHgnIGhyZWY9J0RlZmF1bHQuYXNweCc+VHJhbmcgY2jhu6c8L2E+PGxpPjxhIElEPSAnbFBhQ1REVCcgc3R5bGUgPSd3aWR0aDo4NXB4JyBocmVmPScnPkNoxrDGoW5nIHRyw6xuaDwvYT48dWwgY2xhc3M9J3N1Ym1lbnUnPjxsaT48YSBJRCA9J2xDb0NURFRDMicgICBzdHlsZSA9J3dpZHRoOjE0MHB4JyBocmVmPSdHX0xpc3RDVERULmFzcHgnPkNoxrDGoW5nIHRyw6xuaCDEkcOgbyB04bqhbzwvYT48L2xpPjxsaT48YSBJRCA9J2xDb0NURFRDMScgICBzdHlsZSA9J3dpZHRoOjE0MHB4JyBocmVmPSdHX0xpc3RIb2NQaGFuLmFzcHgnPkjhu41jIHBo4bqnbjwvYT48L2xpPjxsaT48YSBJRCA9J2xDb0NURFRDMycgICBzdHlsZSA9J3dpZHRoOjIwMHB4JyBocmVmPSdHX0xpc3RDVERUQW5oLmFzcHgnPlByb2dyYW08L2E+PC9saT48L3VsPjwvbGk+PGxpPjxhIElEPSAnbFBhS0hEVCcgc3R5bGUgPSd3aWR0aDo2MHB4JyBocmVmPScnPkvhur8gaG/huqFjaDwvYT48dWwgY2xhc3M9J3N1Ym1lbnUnPjxsaT48YSBJRCA9J2xDb0tIRFRDMScgICBzdHlsZSA9J3dpZHRoOjIwMHB4JyBocmVmPSdodHRwczovLzFkcnYubXMvYi9zIUF0d0tsRFo2VnFidG5RY2JqUVFwS05rbUswX2g/ZT1uQ2I3eVAnPkvhur8gaG/huqFjaCDEkcOgbyB04bqhbyBuxINtIGjhu41jPC9hPjwvbGk+PGxpPjxhIElEID0nbENvS0hEVEMyJyAgIHN0eWxlID0nd2lkdGg6MjAwcHgnIGhyZWY9J2h0dHA6Ly9kazQuZHV0LnVkbi52bic+xJDEg25nIGvDvSBo4buNYzwvYT48L2xpPjxsaT48YSBJRCA9J2xDb0tIRFRDMycgICBzdHlsZSA9J3dpZHRoOjIwMHB4JyBocmVmPSdodHRwOi8vZGs0LmR1dC51ZG4udm4vR19Mb3BIb2NQaGFuLmFzcHgnPkzhu5twIGjhu41jIHBo4bqnbiAtIMSRYW5nIMSRxINuZyBrw708L2E+PC9saT48bGk+PGEgSUQgPSdsQ29LSERUQzQnICAgc3R5bGUgPSd3aWR0aDoyMDBweCcgaHJlZj0nR19Mb3BIb2NQaGFuLmFzcHgnPkzhu5twIGjhu41jIHBo4bqnbiAtIGNow61uaCB0aOG7qWM8L2E+PC9saT48bGk+PGEgSUQgPSdsQ29LSERUQzUnICAgc3R5bGUgPSd3aWR0aDoyMDBweCcgaHJlZj0naHR0cDovL2RrNC5kdXQudWRuLnZuL0dfREt5Tmh1Q2F1LmFzcHgnPkto4bqjbyBzw6F0IG5odSBj4bqndSBt4bufIHRow6ptIGzhu5twPC9hPjwvbGk+PGxpPjxhIElEID0nbENvS0hEVEM2JyAgIHN0eWxlID0nd2lkdGg6MjAwcHgnIGhyZWY9J2h0dHA6Ly9jYi5kdXQudWRuLnZuL1BhZ2VMaWNoVGhpS0guYXNweCc+VGhpIGN14buRaSBr4buzIGzhu5twIGjhu41jIHBo4bqnbjwvYT48L2xpPjxsaT48YSBJRCA9J2xDb0tIRFRDNycgICBzdHlsZSA9J3dpZHRoOjIwMHB4JyBocmVmPSdHX0RLVGhpTk4uYXNweCc+VGhpIFRp4bq/bmcgQW5oIMSR4buLbmgga+G7sywgxJHhuqd1IHJhPC9hPjwvbGk+PGxpPjxhIElEID0nbENvS0hEVEM4JyAgIHN0eWxlID0nd2lkdGg6MjAwcHgnIGhyZWY9J0dfTGlzdExpY2hTSC5hc3B4Jz5TaW5oIGhv4bqhdCBs4bubcCDEkeG7i25oIGvhu7M8L2E+PC9saT48bGk+PGEgSUQgPSdsQ29LSERUQzknICAgc3R5bGUgPSd3aWR0aDoyMDBweCcgaHJlZj0naHR0cDovL2ZiLmR1dC51ZG4udm4nPkto4bqjbyBzw6F0IMO9IGtp4bq/biBzaW5oIHZpw6puPC9hPjwvbGk+PGxpPjxhIElEID0nbENvS0hEVEM5JyAgIHN0eWxlID0nd2lkdGg6MjAwcHgnIGhyZWY9J0dfREtQVkNELmFzcHgnPkhv4bqhdCDEkeG7mW5nIHBo4bulYyB24bulIGPhu5luZyDEkeG7k25nPC9hPjwvbGk+PC91bD48L2xpPjxsaT48YSBJRD0gJ2xQYVRSQUNVVScgc3R5bGUgPSd3aWR0aDo3MHB4JyBocmVmPScnPkRhbmggc8OhY2g8L2E+PHVsIGNsYXNzPSdzdWJtZW51Jz48bGk+PGEgSUQgPSdsQ29UUkFDVVUwMScgICBzdHlsZSA9J3dpZHRoOjI0MHB4JyBocmVmPSdHX0xpc3ROZ3VuZ0hvYy5hc3B4Jz5TaW5oIHZpw6puIG5n4burbmcgaOG7jWM8L2E+PC9saT48bGk+PGEgSUQgPSdsQ29UUkFDVVUwMycgICBzdHlsZSA9J3dpZHRoOjI0MHB4JyBocmVmPSdHX0xpc3RMb3AuYXNweCc+U2luaCB2acOqbiDEkWFuZyBo4buNYyAtIGzhu5twPC9hPjwvbGk+PGxpPjxhIElEID0nbENvVFJBQ1VVMDQnICAgc3R5bGUgPSd3aWR0aDoyNDBweCcgaHJlZj0nR19MaXN0Q0NDTlRULmFzcHgnPlNpbmggdmnDqm4gY8OzIGNo4bupbmcgY2jhu4kgQ05UVDwvYT48L2xpPjxsaT48YSBJRCA9J2xDb1RSQUNVVTA1JyAgIHN0eWxlID0nd2lkdGg6MjQwcHgnIGhyZWY9J0dfTGlzdENDTk4uYXNweCc+U2luaCB2acOqbiBjw7MgY2jhu6luZyBjaOG7iSBuZ2/huqFpIG5n4buvPC9hPjwvbGk+PGxpPjxhIElEID0nbENvVFJBQ1VVMDYnICAgc3R5bGUgPSd3aWR0aDoyNDBweCcgaHJlZj0naHR0cDovL2Rhb3Rhby5kdXQudWRuLnZuL1NWL0dfS1F1YUFuaFZhbi5hc3B4Jz5TaW5oIHZpw6puIHRoaSBUaeG6v25nIEFuaCDEkeG7i25oIGvhu7M8L2E+PC9saT48bGk+PGEgSUQgPSdsQ29UUkFDVVUwNycgICBzdHlsZSA9J3dpZHRoOjI0MHB4JyBocmVmPSdHX0xpc3REb0FuVE4uYXNweCc+U2luaCB2acOqbiBsw6BtIMSQ4buTIMOhbiB04buRdCBuZ2hp4buHcDwvYT48L2xpPjxsaT48YSBJRCA9J2xDb1RSQUNVVTA4JyAgIHN0eWxlID0nd2lkdGg6MjQwcHgnIGhyZWY9J0dfTGlzdEhvYW5Ib2NQaGkuYXNweCc+U2luaCB2acOqbiDEkcaw4bujYyBob8OjbiDEkcOzbmcgaOG7jWMgcGjDrTwvYT48L2xpPjxsaT48YSBJRCA9J2xDb1RSQUNVVTE2JyAgIHN0eWxlID0nd2lkdGg6MjQwcHgnIGhyZWY9J0dfTGlzdEhvYW5fVGhpQlMuYXNweCc+U2luaCB2acOqbiDEkcaw4bujYyBob8OjbiB0aGksIHRoaSBi4buVIHN1bmc8L2E+PC9saT48bGk+PGEgSUQgPSdsQ29UUkFDVVUwOScgICBzdHlsZSA9J3dpZHRoOjI0MHB4JyBocmVmPSdHX0xpc3RIb2NMYWkuYXNweCc+U2luaCB2acOqbiBk4buxIHR1eeG7g24gdsOgbyBo4buNYyBs4bqhaTwvYT48L2xpPjxsaT48YSBJRCA9J2xDb1RSQUNVVTEwJyAgIHN0eWxlID0nd2lkdGg6MjQwcHgnIGhyZWY9J0dfTGlzdEt5THVhdC5hc3B4Jz5TaW5oIHZpw6puIGLhu4sga+G7tyBsdeG6rXQ8L2E+PC9saT48bGk+PGEgSUQgPSdsQ29UUkFDVVUxMScgICBzdHlsZSA9J3dpZHRoOjI0MHB4JyBocmVmPSdHX0xpc3RCaUh1eUhQLmFzcHgnPlNpbmggdmnDqm4gYuG7iyBo4buneSBo4buNYyBwaOG6p248L2E+PC9saT48bGk+PGEgSUQgPSdsQ29UUkFDVVUxMicgICBzdHlsZSA9J3dpZHRoOjI0MHB4JyBocmVmPSdHX0xpc3RMb2NrV2ViLmFzcHgnPlNpbmggdmnDqm4gYuG7iyBraMOzYSB3ZWJzaXRlPC9hPjwvbGk+PGxpPjxhIElEID0nbENvVFJBQ1VVMTMnICAgc3R5bGUgPSd3aWR0aDoyNDBweCcgaHJlZj0nR19MaXN0TG9ja1dlYlRhbS5hc3B4Jz5TaW5oIHZpw6puIGLhu4sgdOG6oW0ga2jDs2Egd2Vic2l0ZTwvYT48L2xpPjxsaT48YSBJRCA9J2xDb1RSQUNVVTE0JyAgIHN0eWxlID0nd2lkdGg6MjQwcHgnIGhyZWY9J0dfTGlzdEhhbkNoZVRDLmFzcHgnPlNpbmggdmnDqm4gYuG7iyBo4bqhbiBjaOG6vyB0w61uIGNo4buJIMSRxINuZyBrw708L2E+PC9saT48bGk+PGEgSUQgPSdsQ29UUkFDVVUxNScgICBzdHlsZSA9J3dpZHRoOjI0MHB4JyBocmVmPSdHX0xpc3RDYW5oQmFvS1FIVC5hc3B4Jz5TaW5oIHZpw6puIGLhu4sgY+G6o25oIGLDoW8ga+G6v3QgcXXhuqMgaOG7jWMgdOG6rXA8L2E+PC9saT48L3VsPjwvbGk+PGxpPjxhIElEPSAnbFBhQ1VVU1YnIHN0eWxlID0nd2lkdGg6ODhweCcgaHJlZj0nJz5D4buxdSBzaW5oIHZpw6puPC9hPjx1bCBjbGFzcz0nc3VibWVudSc+PGxpPjxhIElEID0nbENvQ1VVU1YxJyAgIHN0eWxlID0nd2lkdGg6MTEwcHgnIGhyZWY9J0dfTGlzdFROZ2hpZXAuYXNweCc+xJDDoyB04buRdCBuZ2hp4buHcDwvYT48L2xpPjxsaT48YSBJRCA9J2xDb0NVVVNWMicgICBzdHlsZSA9J3dpZHRoOjExMHB4JyBocmVmPSdHX0xpc3RLaG9uZ1ROLmFzcHgnPktow7RuZyB04buRdCBuZ2hp4buHcDwvYT48L2xpPjwvdWw+PC9saT48bGk+PGEgSUQ9ICdsUGFDU1ZDJyBzdHlsZSA9J3dpZHRoOjE0NXB4JyBocmVmPScnPlBow7JuZyBo4buNYyAmIEjhu4cgdGjhu5FuZzwvYT48dWwgY2xhc3M9J3N1Ym1lbnUnPjxsaT48YSBJRCA9J2xDb0NTVkMwMScgICBzdHlsZSA9J3dpZHRoOjI0MHB4JyBocmVmPSdodHRwOi8vY2IuZHV0LnVkbi52bi9QYWdlQ05QaG9uZ0hvYy5hc3B4Jz5Uw6xuaCBow6xuaCBz4butIGThu6VuZyBwaMOybmcgaOG7jWM8L2E+PC9saT48bGk+PGEgSUQgPSdsQ29DU1ZDMDInICAgc3R5bGUgPSd3aWR0aDoyNDBweCcgaHJlZj0nR19MaXN0VGhCaUhvbmcuYXNweCc+VGjhu5FuZyBrw6ogYsOhbyB0aGnhur90IGLhu4sgcGjDsm5nIGjhu41jIGjhu49uZzwvYT48L2xpPjxsaT48YSBJRCA9J2xDb0NTVkMwOScgICBzdHlsZSA9J3dpZHRoOjI0MHB4JyBocmVmPSdHX1N5c1N0YXR1cy5hc3B4Jz5UcuG6oW5nIHRow6FpIGjhu4cgdGjhu5FuZyB0aMO0bmcgdGluIHNpbmggdmnDqm48L2E+PC9saT48L3VsPjwvbGk+PGxpPjxhIElEPSAnbFBhTElFTktFVCcgc3R5bGUgPSd3aWR0aDo1MHB4JyBocmVmPScnPkxpw6puIGvhur90PC9hPjx1bCBjbGFzcz0nc3VibWVudSc+PGxpPjxhIElEID0nbENvTElFTktFVDEnICAgc3R5bGUgPSd3aWR0aDo3MHB4JyBocmVmPSdodHRwOi8vbGliLmR1dC51ZG4udm4nPlRoxrAgdmnhu4duPC9hPjwvbGk+PGxpPjxhIElEID0nbENvTElFTktFVDInICAgc3R5bGUgPSd3aWR0aDo3MHB4JyBocmVmPSdodHRwOi8vbG1zMS5kdXQudWRuLnZuJz5EVVQtTE1TPC9hPjwvbGk+PC91bD48L2xpPjxsaT48YSBJRD0gJ2xQYUhFTFAnIHN0eWxlID0nd2lkdGg6NDVweCcgaHJlZj0nJz5I4buXIHRy4bujPC9hPjx1bCBjbGFzcz0nc3VibWVudSc+PGxpPjxhIElEID0nbENvSEVMUDEnICAgc3R5bGUgPSd3aWR0aDoyMTBweCcgaHJlZj0naHR0cDovL2ZyLmR1dC51ZG4udm4nPkPhu5VuZyBo4buXIHRy4bujIHRow7RuZyB0aW4gdHLhu7FjIHR1eeG6v248L2E+PC9saT48bGk+PGEgSUQgPSdsQ29IRUxQMicgICBzdHlsZSA9J3dpZHRoOjIxMHB4JyBocmVmPSdodHRwczovL2RyaXZlLmdvb2dsZS5jb20vZmlsZS9kLzFaMHFsYmhLYVNHbXpFWkpEMnVCNGVVV2VlSGFROUhIbC92aWV3Jz5IxrDhu5tuZyBk4bqrbiDEkMSDbmcga8O9IGjhu41jPC9hPjwvbGk+PGxpPjxhIElEID0nbENvSEVMUDMnICAgc3R5bGUgPSd3aWR0aDoyMTBweCcgaHJlZj0naHR0cDovL2Rhb3Rhby5kdXQudWRuLnZuL2Rvd25sb2FkMi9FbWFpbF9HdWlkZS5wZGYnPkjGsOG7m25nIGThuqtuIFPhu60gZOG7pW5nIEVtYWlsIERVVDwvYT48L2xpPjxsaT48YSBJRCA9J2xDb0hFTFA3JyAgIHN0eWxlID0nd2lkdGg6MjEwcHgnIGhyZWY9J2h0dHBzOi8vMWRydi5tcy91L3MhQXR3S2xEWjZWcWJ0bzEwYmhIYzBLN3NleU5Hcj9lYUNUYjh4Jz5WxINuIGLhuqNuIFF1eSDEkeG7i25oIGPhu6dhIFRyxrDhu51uZzwvYT48L2xpPjxsaT48YSBJRCA9J2xDb0hFTFA4JyAgIHN0eWxlID0nd2lkdGg6MjEwcHgnIGhyZWY9J2h0dHBzOi8vdGlueXVybC5jb20veTRrZGozc3AnPkJp4buDdSBt4bqrdSB0aMaw4budbmcgZMO5bmc8L2E+PC9saT48L3VsPjwvbGk+PGxpPjxhIGlkID0nbGlua0RhbmdOaGFwJyBocmVmPSdQYWdlRGFuZ05oYXAuYXNweCcgc3R5bGUgPSd3aWR0aDo4MHB4Oyc+IMSQxINuZyBuaOG6rXAgPC9hPjwvbGk+PGxpPjxkaXYgY2xhc3M9J0xvZ2luRnJhbWUnPjxkaXYgc3R5bGUgPSdtaW4td2lkdGg6IDEwMHB4Oyc+PC9kaXY+PC9kaXY+PC9saT48L3VsPmRkFSwwNgHSdZ2bG7X5MK3ePxjwI3ZrE7W2esgf8K/1Yqk=";


    }
}
