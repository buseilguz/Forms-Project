using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Forms.Models;


namespace Forms.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User k, string ReturnUrl)
        {
            FormsContext db = new FormsContext();

            var user = db.Users.FirstOrDefault(kul => kul.UserEmail == k.UserEmail && kul.UserPassword == MD5Sifrele(k.UserPassword) && kul.Passive == false && kul.Active == true);
            if (user != null)
            {
                string yetki =(bool)user.Authority ? "Admin" : "Kullanici";
                var talepler = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email,user.UserEmail.ToString()),
                    new Claim(ClaimTypes.Role,yetki),
                    new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString())

                };
                ClaimsIdentity kimlik = new ClaimsIdentity(talepler, "Login");
                ClaimsPrincipal kural = new ClaimsPrincipal(kimlik);
                await HttpContext.SignInAsync(kural);
                if (!String.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    if ((bool)user.Authority)
                    {
                        return Redirect("/Admin/Index");
                    }
                    else
                    {
                        return Redirect("/Home/Index");
                    }
                }
            }
            return View();
        }


        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }








        public string MD5Sifrele(string sifrelenecekMetin)
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
