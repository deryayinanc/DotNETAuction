using DotNETAuction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DotNETAuction.Controllers
{
    public class AuctionsController : Controller
    {
        //
        // GET: /Auctions/

        [AllowAnonymous]
        public ActionResult Index()
        {
            var db = new AuctionsDataContext();
            var auctions = db.Auctions.ToArray();

            return View(auctions);
        }

        public ActionResult TempDataDemo()
        {
            TempData["SuccessMessage"] = "Success Achieved";
            return RedirectToAction("Index","Auctions");
        }

        public ActionResult Auction(long id)
        {

            var db = new AuctionsDataContext();
            var auction = db.Auctions.Find(id);
            return View(auction);
        }

        [HttpPost]
        public ActionResult Bid(Bid bid)
        {
            var db = new AuctionsDataContext();
            var auction = db.Auctions.Find(bid.AuctionId);

            if (auction == null)
            {
                ModelState.AddModelError("AuctionId", "Auction Id not Found.");
            }
            else if (auction.CurrentPrice >= bid.Amount)
            {
                ModelState.AddModelError("Amount", "Bid Amount must be greater than current price!!!");
            }
            else
            {
                bid.Username = User.Identity.Name;
                auction.Bids.Add(bid);
                auction.CurrentPrice = bid.Amount;
                db.SaveChanges();
            }

            if (!Request.IsAjaxRequest())
                return RedirectToAction("Auction", new { id = bid.AuctionId });

            return Json(new
            {
                CurrentPrice = bid.Amount.ToString("C"),
                BidCount = auction.BidCount

            });
        }

        public ActionResult AuctionXml()
        {
            var db = new AuctionsDataContext();
            var data = from a in db.Auctions
                       join b in db.Bids
                       on a.Id equals b.AuctionId
                       where a.Id == 1
                       select b.Amount;
            var returnStr = data.First().ToString();
            var returnXML = "<xml><id>" + returnStr + "</id></xml>";
            return Content(returnXML,"text/xml");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var categoryList = new SelectList(new[] { "Automotive", "Electronics", "Games", "Home" });
            ViewBag.CategoryList = categoryList;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Exclude = "CurrentPrice")]Models.Auction auction)
        {


            if(ModelState.IsValid){

                // Save to Database

                var db = new AuctionsDataContext();
                db.Auctions.Add(auction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return Create();
        }

    }
}
