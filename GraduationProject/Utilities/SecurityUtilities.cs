using GraduationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace GraduationProject.Utilities
{
    public class SecurityUtilities
    {
        public static bool IsEmailExists(string email)
        {
            using (GPEntities db = new GPEntities())
            {
                var exist = db.Patients.Where(p => p.Email == email).FirstOrDefault();
                return exist != null;
            }
        }
        public static HttpCookie CreateAuthenticationCookie(string name, string id)
        {
            int timeout = 60;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddMinutes(timeout), true, id);
            string encrypted = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
            {
                Expires = DateTime.Now.AddMinutes(timeout),
                HttpOnly = true
            };
            return cookie;
        }

        public static int GetAuthenticatedUserID()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            return int.Parse(ticket.UserData);
        }

        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
    }
}