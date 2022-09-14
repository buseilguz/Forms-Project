using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Forms.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Forms.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        FormsContext db = new FormsContext();
        public IActionResult Index()
        {
            return View();
        }
       

       


        public IActionResult MyInformation()
        {
            int kulid = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = db.Users.Find(kulid);
            u.UserPassword = "";
            return View(u);
        }

        public IActionResult EditMyInformation(User u)
        {
            var user = db.Users.Where(s => s.Passive == false && s.UserId == u.UserId).FirstOrDefault();

            user.UserName = u.UserName;
            user.UserSurname = u.UserSurname;
            user.UserEmail = u.UserEmail;
            user.UserMobilePhone = u.UserMobilePhone;
            user.UserPhone = u.UserPhone;
            user.UserTask = u.UserTask;
            user.Authority = u.Authority;
            user.Passive = false;
            user.Active = true;
            if (u.UserPassword != null)
            {
                user.UserPassword = MD5Sifrele(u.UserPassword.Trim());
            }

            db.Users.Update(user);
            db.SaveChanges();
            return RedirectToAction("MyInformation");
        }





        public IActionResult Forms()
        {

            var formlar = db.Forms.Where(f => f.Passive == false).OrderBy(f => f.Sira).ToList();


            return View(formlar);
        }
        public IActionResult CreateForm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateForm(Form f)
        {
            f.UstId = 2;
            f.Passive = false;
            db.Forms.Add(f);
            db.SaveChanges();
            return RedirectToAction("Forms");
        }

        public IActionResult GetForm(int id)
        {
            var form = db.Forms.Where(f => f.Passive == false&&f.FormId==id).FirstOrDefault();
            return View("EditForm",form);
        }
        public IActionResult EditForm(Form a)
        {
            var form = db.Forms.Where(f => f.Passive == false && f.FormId == a.FormId).FirstOrDefault();
            form.FormName = a.FormName;
            form.Active = a.Active;
            form.FormUrl = a.FormUrl;
            form.Sira = a.Sira;
            form.Passive = false;
            db.Forms.Update(form);
            db.SaveChanges();
            return RedirectToAction("Forms");
        }

        public IActionResult DeleteForm(int id)
        {
            var form = db.Forms.Where(f => f.Passive == false && f.FormId == id).FirstOrDefault();
            form.Passive = true;
            db.Forms.Update(form);
            db.SaveChanges();
            return RedirectToAction("Forms");
        }

        public IActionResult CikisYap()
        {
            return View();
        }





        public IActionResult CyberAttack()
        { 
            var attack = db.CyberAttacks.Include(a=>a.User).Include(a=>a.Attack).OrderBy(f => f.CybetAttackId).ToList();
            return View(attack);
        }

        public IActionResult NewCyberAttack()
        {
            var attack = (from k in db.Attacks.ToList()
                          select new SelectListItem
                          {
                              Text = k.AttackName,
                              Value = k.AttackId.ToString()
                          });
            ViewBag.AttackId = attack;
            return View();
        }
        [HttpPost]
        public IActionResult NewCyberAttack(CyberAttack c)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            int id2 = Convert.ToInt32(id);
            c.UserId = id2;
            db.CyberAttacks.Add(c);
            db.SaveChanges();
            return RedirectToAction("CyberAttack");
        }



        public IActionResult GetCyberAttack(int id)
        {
            var attack = db.CyberAttacks.Include(a => a.User).Include(a => a.Attack).Where(a=>a.CybetAttackId==id).FirstOrDefault();
            var attacks = (from k in db.Attacks.ToList()
                          select new SelectListItem
                          {
                              Text = k.AttackName,
                              Value = k.AttackId.ToString()
                          });
            ViewBag.AttackId = attacks;
            return View("EditCyberAttack",attack);
        }

        public IActionResult EditCyberAttack(CyberAttack a)
        {
            var attack = db.CyberAttacks.Where(f => f.CybetAttackId == a.CybetAttackId).FirstOrDefault();
            attack.CybetAttackId = a.CybetAttackId;
            attack.AttackId = a.AttackId;
            attack.Description = a.Description;
            attack.DetectionDate = a.DetectionDate;
            attack.StartDate = a.StartDate;
            attack.SystemOutage= a.SystemOutage;
           
            db.CyberAttacks.Update(attack);
            db.SaveChanges();
            return RedirectToAction("CyberAttack");
        }


        public IActionResult DeleteCyberAttack(int id)
        {
            var attack = db.CyberAttacks.Where(f => f.CybetAttackId == id).FirstOrDefault();
           
            db.CyberAttacks.Remove(attack);
            db.SaveChanges();
            return RedirectToAction("CyberAttack");
        }







        public IActionResult ContentProvider()
        {
            var provider = db.ContentProviders.Include(a => a.User).Include(a=>a.Purpose).OrderBy(f => f.ContentProviderId).ToList();
            return View(provider);
        }

        public IActionResult NewContentProvider()
        {
            var purpose = (from k in db.Purposes.ToList()
                          select new SelectListItem
                          {
                              Text = k.PurposeName,
                              Value = k.PurposeId.ToString()
                          });
            ViewBag.PurposeId = purpose;

            var authorized = (from k in db.Users.Where(a=>a.Authority==true).ToList()
                              select new SelectListItem
                              {
                                  Text = k.UserName +" "+ k.UserSurname,
                                  Value = k.UserId.ToString()
                              }) ;
            ViewBag.AuthorizedId = authorized;

            return View();
        }
        [HttpPost]
        public IActionResult NewContentProvider(ContentProvider c)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            int id2 = Convert.ToInt32(id);
            c.UserId= id2;
            db.ContentProviders.Add(c);
            db.SaveChanges();
            return RedirectToAction("ContentProvider");
        }



        public IActionResult GetContentProvider(int id)
        {
            var purpose = (from k in db.Purposes.ToList()
                           select new SelectListItem
                           {
                               Text = k.PurposeName,
                               Value = k.PurposeId.ToString()
                           });
            ViewBag.PurposeId = purpose;
            var authorized = (from k in db.Users.Where(a => a.Authority == true).ToList()
                              select new SelectListItem
                              {
                                  Text = k.UserName + " " + k.UserSurname,
                                  Value = k.UserId.ToString()
                              });
            ViewBag.AuthorizedId = authorized;
            var provider = db.ContentProviders.Include(a => a.User).Include(a=>a.Purpose).Where(a => a.ContentProviderId == id).FirstOrDefault();
            return View("EditContentProvider", provider);
        }

        public IActionResult EditContentProvider(ContentProvider a)
        {
            var provider = db.ContentProviders.Where(f => f.ContentProviderId == a.ContentProviderId).FirstOrDefault();
            provider.ContentProviderId = a.ContentProviderId;
            provider.DatabaseUserName = a.DatabaseUserName;
            provider.Date = a.Date;
            provider.DomainName  = a.DomainName;
            provider.DatabaseRequest   = a.DatabaseRequest;
            provider.PurposeId = a.PurposeId;
            provider.AuthorizedId = a.AuthorizedId;
            provider.DatabasePassword = a.DatabasePassword;
            

            db.ContentProviders.Update(provider);
            db.SaveChanges();
            return RedirectToAction("ContentProvider");
        }


        public IActionResult DeleteContentProvider(int id)
        {
            var provider = db.ContentProviders.Where(f => f.ContentProviderId == id).FirstOrDefault();

            db.ContentProviders.Remove(provider);
            db.SaveChanges();
            return RedirectToAction("ContentProvider");
        }









        public IActionResult Users()
        {
            var users = db.Users.Where(s => s.Passive == false).OrderBy(k => k.UserId).ToList();
            return View(users);
        }

        public IActionResult NewUser()
        {

            return View();
        }
        [HttpPost]
        public IActionResult NewUser(User u)
        {
            u.Passive = false;
            u.UserPassword = MD5Sifrele(u.UserPassword);
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("Users");
        }


        public IActionResult GetUser(int id)
        {
            var user = db.Users.Where(k => k.Passive == false && k.UserId == id).FirstOrDefault();
            user.UserPassword = "";
            return View("EditUser", user);
        }

        public IActionResult EditUser(User u)
        {

            var user = db.Users.Where(s => s.Passive == false && s.UserId == u.UserId).FirstOrDefault();
            user.Active = u.Active;
            user.UserName = u.UserName;
            user.UserSurname = u.UserSurname;
            user.UserEmail = u.UserEmail;
            user.UserMobilePhone = u.UserMobilePhone;
            user.UserTask = u.UserTask;
            user.Authority = u.Authority;
            if (u.UserPassword != null)
            {
                user.UserPassword = MD5Sifrele(u.UserPassword.Trim());
            }

            db.Users.Update(user);
            db.SaveChanges();
            return RedirectToAction("Users");
        }




        public IActionResult DeleteUser(int id)
        {

            var user = db.Users.Where(s => s.Passive == false && s.UserId == id).FirstOrDefault();
            user.Passive = true;
            db.Users.Update(user);
            db.SaveChanges();
            return RedirectToAction("Users");
        }

        
        public static string MD5Sifrele(string sifrelenecekMetin)
        {

            // MD5CryptoServiceProvider sınıfının bir örneğini oluşturduk.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //Parametre olarak gelen veriyi byte dizisine dönüştürdük.
            byte[] dizi = Encoding.UTF8.GetBytes(sifrelenecekMetin);
            //dizinin hash'ini hesaplattık.
            dizi = md5.ComputeHash(dizi);
            //Hashlenmiş verileri depolamak için StringBuilder nesnesi oluşturduk.
            StringBuilder sb = new StringBuilder();
            //Her byte'i dizi içerisinden alarak string türüne dönüştürdük.

            foreach (byte ba in dizi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            //hexadecimal(onaltılık) stringi geri döndürdük.
            return sb.ToString();
        }


    }
}
