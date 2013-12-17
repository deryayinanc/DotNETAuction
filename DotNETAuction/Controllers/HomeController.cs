using DotNETAuction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNETAuction.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [OutputCache(Duration=5)]
        public ActionResult Index()
        {

            ViewBag.Message = "This page is rendered at " + DateTime.Now;

            return View();
        }
        [OutputCache(Duration=3600)]
        public ActionResult CategoryNavigation()
        {
            var db = new AuctionsDataContext();
            var categories = db.Auctions.Select(x => x.Category).Distinct();
            ViewBag.Categories = categories.ToArray();

            return PartialView();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LinqTest()
        {
            var youShould = from c
            in "3%.$@9/52@2%35-%@4/@./3,!#+%23 !2#526%N#/-"
             select (char)(c ^ 3 << 5);

            char[] chars = youShould.ToArray();
            string stringOut = string.Join("",chars);
            return Content(stringOut,"text");
        }
    }
}
