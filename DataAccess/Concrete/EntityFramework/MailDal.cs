using DataAccess.Abstract;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
	public class MailDal : IMailDal
	{
		public void SendMail(SendMailDto sendMailDto)
		{
			using(MailMessage mail = new MailMessage())
			{
				mail.From = new MailAddress(sendMailDto.MailParameter.Email);
				mail.To.Add(sendMailDto.Email);
				mail.Subject = sendMailDto.Subject;
				mail.Body = sendMailDto.Body;
				mail.IsBodyHtml = true; 

				using(SmtpClient smtp = new SmtpClient(sendMailDto.MailParameter.SMTP))
				{
					smtp.UseDefaultCredentials = false;
					smtp.Credentials = new NetworkCredential(sendMailDto.MailParameter.Email, sendMailDto.MailParameter.EmailPassword);
					smtp.EnableSsl = sendMailDto.MailParameter.SSL;
					smtp.Port = sendMailDto.MailParameter.Port;
					smtp.Send(mail);
				}


			}
		}
	}
}
