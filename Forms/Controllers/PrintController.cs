using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

namespace Forms.Controllers
{
    public class PrintController : Controller
    {
        FormsContext db = new FormsContext();

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PrintCyberAttack(int id)
        {

            var attack = db.CyberAttacks.Include(a => a.User).Include(a => a.Attack).Where(a => a.CybetAttackId == id).FirstOrDefault();
            return View(attack);
        }

        public IActionResult PrintContentProvider(int id)
        {
            var provider = db.ContentProviders.Include(a => a.User).Include(a=>a.Purpose).Where(a => a.ContentProviderId == id).FirstOrDefault();

            return View(provider);
        }
        
    }
} 
