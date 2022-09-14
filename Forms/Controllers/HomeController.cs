using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Extensions.Logging;

namespace Forms.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        FormsContext db = new FormsContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int id)

        {

            var form = db.Forms.Where(a => a.Active == true && a.Passive == false && a.FormId == id).FirstOrDefault();
            return View(form);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }







        public IActionResult CyberAttack()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            int id2 = Convert.ToInt32(id);
            var attack = db.CyberAttacks.Include(a => a.User).Include(a => a.Attack).Where(a => a.UserId == id2).OrderBy(f => f.CybetAttackId).ToList();
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
            c.UserId = Convert.ToInt32(id);
            db.CyberAttacks.Add(c);
            db.SaveChanges();
            return RedirectToAction("CyberAttack");
        }



        public IActionResult GetCyberAttack(int id)
        {
            var attack = db.CyberAttacks.Include(a => a.User).Include(a => a.Attack).Where(a => a.CybetAttackId == id).FirstOrDefault();
            var attacks = (from k in db.Attacks.ToList()
                           select new SelectListItem
                           {
                               Text = k.AttackName,
                               Value = k.AttackId.ToString()
                           });
            ViewBag.AttackId = attacks;
            return View("EditCyberAttack", attack);
        }

        public IActionResult EditCyberAttack(CyberAttack a)
        {
            var attack = db.CyberAttacks.Where(f => f.CybetAttackId == a.CybetAttackId).FirstOrDefault();
            attack.CybetAttackId = a.CybetAttackId;
            attack.AttackId = a.AttackId;
            attack.Description = a.Description;
            attack.DetectionDate = a.DetectionDate;
            attack.StartDate = a.StartDate;
            attack.SystemOutage = a.SystemOutage;

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
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            int id2 = Convert.ToInt32(id);
            var provider = db.ContentProviders.Include(a => a.User).Include(a=>a.Purpose).Where(a => a.UserId == id2).OrderBy(f => f.ContentProviderId).ToList();
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
            var authorized = (from k in db.Users.Where(a => a.Authority == true).ToList()
                              select new SelectListItem
                              {
                                  Text = k.UserName + " " + k.UserSurname,
                                  Value = k.UserId.ToString()
                              });
            ViewBag.AuthorizedId = authorized;

            return View();
        }
        [HttpPost]
        public IActionResult NewContentProvider(ContentProvider c)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            int id2 = Convert.ToInt32(id);
            c.UserId = id2;
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
            provider.DomainName = a.DomainName;
            provider.DatabaseRequest = a.DatabaseRequest;
            provider.DatabasePassword = a.DatabasePassword;
            provider.AuthorizedId = a.AuthorizedId;
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





        [AllowAnonymous]
        public IActionResult Signup()
        {

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Signup(User u)
        {
            u.Active = true;
            u.Passive = false;
            u.UserPassword = MD5Sifrele(u.UserPassword);
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult MyInformation()
        {
            int kulid = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = db.Users.Find(kulid);
            u.UserPassword = "";
            return View(u);
        }
        [AllowAnonymous]
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
