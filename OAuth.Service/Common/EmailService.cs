using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Service.Common
{
    public class EmailService
    {
        string email = EncryptHelper.Decrypt(System.Configuration.ConfigurationManager.AppSettings["system.Email"]);
        string password = EncryptHelper.Decrypt(System.Configuration.ConfigurationManager.AppSettings["system.Password"]);
        public void SendEmail(string toEmail, string title, string content)//参数是收件人的email,项目名称,项目开始竞标时间,项目竞标结束时间
        {
            if (!string.IsNullOrEmpty(toEmail))
            {
                //获取邮件发送人的用户名和密码
                try
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.EnableSsl = false;
                    smtp.Host = "smtp.wuxiao.cn";
                    smtp.Port = 25;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(email, password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    MailMessage mm = new MailMessage();//实例化一个邮件类
                    mm.Priority = MailPriority.Normal;
                    mm.From = new MailAddress(email);//, "青岛普瑞尔设备制造有限公司", Encoding.GetEncoding(936)
                    mm.ReplyTo = new MailAddress(email, "我的接收邮箱", Encoding.GetEncoding(936));
                    mm.Subject = title;//设置邮件的标题
                    mm.SubjectEncoding = Encoding.GetEncoding(936);
                    mm.IsBodyHtml = true;
                    mm.BodyEncoding = Encoding.GetEncoding(936);
                    mm.Body = content;
                    mm.To.Add(toEmail);
                    smtp.Send(mm);
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                return;
            }
        }
    }
}
