using GraduationProject.Models;
using GraduationProject.Models.Extended;
using GraduationProject.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;

namespace GraduationProject.Controllers
{
    public class PatientController : Controller
    {
        private GPEntities db = new GPEntities();

        [AllowAnonymous]
        public ActionResult Register()
        {
            //if account is already login, redirect to home page 
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.BloodGroupID = new SelectList(db.BloodGroups,"ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Patient patient , HttpPostedFileBase uploaded)
        {
            if (ModelState.IsValid)
            {
                var isExist = SecurityUtilities.IsEmailExists(patient.Email);
                if (isExist)
                {
                    //username is registered before
                    ModelState.AddModelError("EmailExist", "Email already exists .");
                    ViewBag.BloodGroupID = new SelectList(db.BloodGroups, "ID", "Name",patient.BloodGroupID);
                    return View(patient);
                }
                //Completing user model data
                patient.PW = SecurityUtilities.Hash(patient.PW);
                if (uploaded != null && uploaded.ContentLength > 0)
                {
                    string extension = Path.GetExtension(uploaded.FileName);
                    string pattern = @".(jpg|JPG|jpeg|JPEG|png|PNG)$";
                    if (Regex.IsMatch(extension, pattern))
                    {
                        // convert image to array of binary 
                        patient.Img = new byte[uploaded.ContentLength];
                        uploaded.InputStream.Read(patient.Img, 0, uploaded.ContentLength);
                    }
                    else
                    {
                        ModelState.AddModelError("ImgError", "Only Images allowed .");
                        ViewBag.BloodGroupID = new SelectList(db.BloodGroups, "ID", "Name", patient.BloodGroupID);
                        return View(patient);
                    }
                }
                // insert user data in User and UserRoles tables in one transaction

                db.Patients.Add(patient);
                db.SaveChanges();

                //Registeration succeeded, Sign in this account 
                Response.Cookies.Add(SecurityUtilities.CreateAuthenticationCookie(patient.FName, patient.ID.ToString()));

                // Redirect to Dashboard
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                ModelState.AddModelError("RegisterError", "An error occured while registeration .");
            }
            // If we got this far, something failed, redisplay form
            ViewBag.BloodGroupID = new SelectList(db.BloodGroups, "ID", "Name", patient.BloodGroupID);
            return View(patient);
        }

        [Authorize]
        public new ActionResult Profile()
        {
            int id = SecurityUtilities.GetAuthenticatedUserID();
            Patient patient = db.Patients.Where(p => p.ID == id).FirstOrDefault();
            ViewBag.BloodGroupID = new SelectList(db.BloodGroups, "ID", "Name",patient.BloodGroupID);
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public new ActionResult Profile(Patient patient)
        {
            if (ModelState.IsValid)
            {
                var isExist = SecurityUtilities.IsEmailExists(patient.Email);
                if (isExist)
                {
                    if (db.Patients.Where(p=>p.ID==patient.ID).Select(p=>p.Email).FirstOrDefault() != patient.Email)
                    {
                        //username is registered before
                        ModelState.AddModelError("EmailExist", "Email already exists .");
                        ViewBag.BloodGroupID = new SelectList(db.BloodGroups, "ID", "Name", patient.BloodGroupID);
                        return View(patient);
                    }

                }

                // saving data
                patient.PW = SecurityUtilities.Hash(patient.PW);
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            //if we reach here something went wrong
            ModelState.AddModelError("ProfileError", "An error occured while editing profile");
            ViewBag.BloodGroupID = new SelectList(db.BloodGroups, "ID", "Name", patient.BloodGroupID);
            return View(patient);
        }

        [Authorize]
        public ActionResult Analysis() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Analysis(HttpPostedFileBase uploaded)
        {
            if (uploaded != null && uploaded.ContentLength > 0)
            {
                string extension = Path.GetExtension(uploaded.FileName);
                string pattern = @".(jpg|JPG|jpeg|JPEG|png|PNG)$";
                if (Regex.IsMatch(extension, pattern))
                {
                    // convert image to array of binary 
                    var Img = new byte[uploaded.ContentLength];
                    uploaded.InputStream.Read(Img, 0, uploaded.ContentLength);

                    List<string> result = new List<string>(){ "Positive", "Negative" };
                    Random rnd = new Random();
                    int index = rnd.Next(2);
                    PatientHistory PH = new PatientHistory() {
                        ADate = DateTime.Now.Date,
                        Img = Img,
                        PatientID = SecurityUtilities.GetAuthenticatedUserID(),
                        Result = result[index]
                    };
                    db.PatientHistories.Add(PH);
                    db.SaveChanges();
                    return RedirectToAction("History");
                }
                else
                {
                    ModelState.AddModelError("ImgError", "Only Images allowed .");
                }
            }
            ModelState.AddModelError("Error", "An error occured while checking your image .");
            return View();
        }

        [Authorize]
        public ActionResult History()
        {
            int id = SecurityUtilities.GetAuthenticatedUserID();
            return View(db.PatientHistories.Where(h=>h.PatientID==id).ToList());
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            //if account is already login, redirect to home page 
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel LoginVM)
        {
            if (ModelState.IsValid)
            {
                var patient = db.Patients.Where(u => u.Email == LoginVM.Email).FirstOrDefault();
                if (patient != null)
                {
                    //valid email address
                    if (string.Compare(SecurityUtilities.Hash(LoginVM.PW), patient.PW) == 0)
                    {
                        //valid login password, reset access faild counter and create FormAuthentication Cookie
                        Response.Cookies.Add(SecurityUtilities.CreateAuthenticationCookie(patient.FName, patient.ID.ToString()));

                        //redirect to home page
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //invalid password
                        ModelState.AddModelError("InvalidPassword", "Invalid Password. ");
                    }
                }
                else
                {
                    //wrong email address
                    ModelState.AddModelError("InvalidEmail", "Invalid Email Address. ");
                }
            }
            else 
            {
                ModelState.AddModelError("loginerror", "An error occured while sign in .");
            }
            // If we got this far, something failed, redisplay form
            return View(LoginVM);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ChangeImg(HttpPostedFileBase upload)
        {
            // get user from database
            int id = SecurityUtilities.GetAuthenticatedUserID();
            var patient = db.Patients.Where(p => p.ID == id).FirstOrDefault();
            if (upload != null && upload.ContentLength > 0)
            {
                // convert image to array of binary then store it into database
                patient.Img = new byte[upload.ContentLength];
                upload.InputStream.Read(patient.Img, 0, upload.ContentLength);
                db.SaveChanges();
            }
            //redirect to profile after changing profile picture  
            return RedirectToAction("Profile");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}