
using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using MyXSpace.WebSPA.Model;

namespace MyXSpace.Web.Helpers
{
    public static class EmailTemplates
    {
        static IHostingEnvironment _hostingEnvironment;
        static string testEmailTemplate;
        static string plainTextTestEmailTemplate;

        public static void Initialize(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Invition letter to join candidate to Brand
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="from"></param>
        /// <param name="recepient"></param>
        /// <param name="inviteDate"></param>
        /// <returns></returns>
        public static string GetInviteRegistrationToTenant(string companyName, ConsultantModel from, CandidateModel toRecepient, DateTime inviteDate)
        {
            //if (from == null)
            //    throw NullArgumentException(from);// "Cannot send email from empty recepient");
            if (testEmailTemplate == null)
                testEmailTemplate = ReadPhysicalFile("Helpers/Templates/InviteRegistrationToTenant.template");

            string emailMessage = testEmailTemplate
                .Replace("{company}", companyName)
                .Replace("{Consultant_FirstName}", from.FirstName)
                .Replace("{Consultant_LastName}", from.LastName)
                .Replace("{First_Name}", toRecepient.FirstName)
                .Replace("{testDate}", inviteDate.ToShortDateString());

            return emailMessage;
        }

        /*
        public static string GetUserRegistrationConfirm(UserModel recepientName, DateTime testDate)
        {
            if (testEmailTemplate == null)
                testEmailTemplate = ReadPhysicalFile("Helpers/Templates/UserRegistrationConfirm.template");

            string emailMessage = testEmailTemplate
                .Replace("{user}", recepientName)
                .Replace("{testDate}", testDate.ToString());

            return emailMessage;
        }*/

        public static string GetPlainTextTestEmail(DateTime date)
        {
            if (plainTextTestEmailTemplate == null)
                plainTextTestEmailTemplate = ReadPhysicalFile("Helpers/Templates/PlainTextTestEmail.template");

            string emailMessage = plainTextTestEmailTemplate
                .Replace("{date}", date.ToString());

            return emailMessage;
        }

        private static string ReadPhysicalFile(string path)
        {
            if (_hostingEnvironment == null)
                throw new InvalidOperationException($"{nameof(EmailTemplates)} is not initialized");

            IFileInfo fileInfo = _hostingEnvironment.ContentRootFileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
                throw new FileNotFoundException($"Template file located at \"{path}\" was not found");

            using (var fs = fileInfo.CreateReadStream())
            {
                using (var sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
