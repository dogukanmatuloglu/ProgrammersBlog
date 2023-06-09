using Microsoft.Extensions.Options;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.Concrete
{
    public class MailManager : IMailService
    {
        private readonly SmtpSettings _settings;

        public MailManager(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public IResult Send(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage()
            {
                From = new MailAddress(_settings.SenderEmail),
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = emailSendDto.Message
            };
            SmtpClient smtpClient = new SmtpClient()
            {
                Host = _settings.Server,
                Port = _settings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);
            return new Result(Shared.Utilities.Results.ComplexTypes.ResultStatus.Success, "E-Postanız başarıyla gönderilmiştir.");
        }

        public IResult SendCantactEmail(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage()
            {
                From=new MailAddress(_settings.SenderEmail),
                To = { new MailAddress("") },
                Subject=emailSendDto.Subject,
                IsBodyHtml=true,
                Body = $"Gönderen Kişi: {emailSendDto.Name}, Gönderen E-Mail Adresi: {emailSendDto.Email}<br/>{emailSendDto.Message}"
            };
            SmtpClient smtpClient = new SmtpClient()
            {
                Host=_settings.Server,
                Port = _settings.Port,
                EnableSsl=true,
                UseDefaultCredentials=false,
                Credentials=new NetworkCredential(_settings.Username, _settings.Password),
                DeliveryMethod=SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);   
            return new Result(Shared.Utilities.Results.ComplexTypes.ResultStatus.Success,"E-Postanız başarıyla gönderilmiştir.");
        }
    }
}
