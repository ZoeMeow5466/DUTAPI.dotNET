using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace ZoeMeow.DUTAPI
{
    public partial class Session
    {
        private string GetIDFromTitleBar(string stringTitleBar)
        {
            try
            {
                int openCase = stringTitleBar.IndexOf("(");
                int closeCase = stringTitleBar.IndexOf(")");
                int stringLength = (stringTitleBar.IndexOf(")")) - (stringTitleBar.IndexOf("(") + 1);
                return stringTitleBar.Substring(openCase + 1, stringLength);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                return null;
            }
        }

        public AccountInformation GetAccountInformation()
        {
            AccountInformation accInfo = null;

            try
            {
                if (IsLoggedIn() != LoginStatus.LoggedIn)
                    throw new Exception("You are logged out! This function cannot continue.");

                accInfo = new AccountInformation();

                HttpClient client = CreateNewHttpClientInstance();
                HttpResponseMessage response = client.GetAsync(@"/PageCaNhan.aspx").Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception(String.Format("The request has return code {0}.", response.StatusCode));

                HtmlDocument httpTemp = new HtmlDocument();
                httpTemp.LoadHtml(response.Content.ReadAsStringAsync().Result);
                accInfo.ID = GetIDFromTitleBar(httpTemp.GetElementbyId("Main_lblHoTen").InnerText);
                accInfo.Name = httpTemp.GetElementbyId("CN_txtHoTen").GetAttributeValue("value", null);
                accInfo.DateOfBirth = Convert.ToDateTime(
                    httpTemp.GetElementbyId("CN_txtNgaySinh").GetAttributeValue("value", null),
                    new CultureInfo("vi-VN")
                    );
                switch (httpTemp.GetElementbyId("CN_txtGioiTinh").GetAttributeValue("value", null).ToLower())
                {
                    case "nam":
                        accInfo.Gender = Gender.Male;
                        break;
                    case "nữ":
                        accInfo.Gender = Gender.Female;
                        break;
                    default:
                        accInfo.Gender = Gender.Unknown;
                        break;
                }
                accInfo.IdentityID = httpTemp.GetElementbyId("CN_txtSoCMND").GetAttributeValue("value", null);
                accInfo.ClassName = httpTemp.GetElementbyId("CN_txtLop").GetAttributeValue("value", null);
                accInfo.BankInfo =
                    String.Format(
                        "{0} ({1})",
                        httpTemp.GetElementbyId("CN_txtTKNHang").GetAttributeValue("value", null),
                        httpTemp.GetElementbyId("CN_txtNgHang").GetAttributeValue("value", null)
                        );
                accInfo.HIID = httpTemp.GetElementbyId("CN_txtSoBHYT").GetAttributeValue("value", null);
                accInfo.PersonalEmail = httpTemp.GetElementbyId("CN_txtMail2").GetAttributeValue("value", null);
                accInfo.PhoneNumber = httpTemp.GetElementbyId("CN_txtPhone").GetAttributeValue("value", null);
                accInfo.EducationEmail = httpTemp.GetElementbyId("CN_txtMail1").GetAttributeValue("value", null);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                accInfo = null;
            }

            return accInfo;
        }
    }
}
