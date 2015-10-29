using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace iSim.Controllers
{
    public class HomeController : Controller
    {
        public RedirectResult Index()
        {
            return Redirect("index.html");
        }
        //public ActionResult Email()
        //{
        //    return View();
        //}
        public string sendMail(ADMail md)
        {
            ResultInfo r = new ResultInfo();
            try
            {
                #region 信件發送
                //建立MailMessage物件
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(md.email, md.contact_person);//寄件人
                #region 標題
                var Title_format1 = "i-Sim活動 申請廣告合作-{0}";
                var Title_format2 = "i-Sim活動 申請聯名合作-{0}";
                if (md.email_type == 1)//信件標題
                {
                    mms.Subject = string.Format(Title_format1, md.company_name);
                }
                else
                {
                    mms.Subject = string.Format(Title_format2, md.company_name);
                }
                #endregion 
                #region 收信人
                string s = System.Configuration.ConfigurationManager.AppSettings["MailToList"];
                string[] MailTos = s.Split(',');//收信人

                if (MailTos != null)//防呆
                {
                    foreach (var str in MailTos)
                    {
                        if (str != "")
                        {
                            var m = str.Split(':');
                            if (m.Length == 2)
                            {
                                mms.To.Add(new MailAddress(m[1], m[0]));
                            }
                            else if (m.Length == 1)
                            {
                                mms.To.Add(new MailAddress(m[0]));
                            }
                        }
                    }
                }//End if (MailTos !=null)//防呆
                #endregion
                mms.Body = getMailBody("Email", md);//套用信件版面
                mms.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["MailServer"]))//或公司、客戶的smtp_server

                    client.Send(mms);//寄出一封信

                //釋放每個附件，才不會Lock住
                if (mms.Attachments != null && mms.Attachments.Count > 0)
                {
                    for (int i = 0; i < mms.Attachments.Count; i++)
                    {
                        mms.Attachments[i].Dispose();
                        mms.Attachments[i] = null;
                    }
                }
                #endregion
                r.result = true;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
            }
            return defJSON(r);
        }


        #region 寄信相關
        public class StringResult : ViewResult
        {
            public string ToHtmlString { get; set; }
            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                if (string.IsNullOrEmpty(this.ViewName))
                {
                    this.ViewName = context.RouteData.GetRequiredString("action");
                }

                ViewEngineResult result = null;

                if (this.View == null)
                {
                    result = this.FindView(context);
                    this.View = result.View;
                }

                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);

                ViewContext viewContext = new ViewContext(context, this.View, this.ViewData, this.TempData, writer);

                this.View.Render(viewContext, writer);

                writer.Flush();

                ToHtmlString = Encoding.UTF8.GetString(stream.ToArray());

                if (result != null)
                    result.ViewEngine.ReleaseView(context, this.View);
            }
        }
        //將變數套用至信件版面
        public string getMailBody(string EmailView, object md)
        {
            ViewResult resultView = View(EmailView, md);

            StringResult sr = new StringResult();
            sr.ViewName = resultView.ViewName;
            sr.MasterName = resultView.MasterName;
            sr.ViewData = resultView.ViewData;
            sr.TempData = resultView.TempData;
            sr.ExecuteResult(this.ControllerContext);

            return sr.ToHtmlString;
        }

        #endregion
        protected string defJSON(object o)
        {
            return JsonConvert.SerializeObject(o, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

    }
    public class ADMail
    {
        public int email_type { get; set; }
        public string company_name { get; set; }
        public string contact_person { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public string content { get; set; }
        public string theme { get; set; }
        public string occupation { get; set; }
    }
    public class ResultInfo
    {
        public string message { get; set; }
        public bool result { get; set; }
    }

}