using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237assignment6.Models;

namespace cis237assignment6.Controllers
{
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageMBoonEntities1 db = new BeverageMBoonEntities1();

        // GET: Beverages
        public ActionResult Index()
        {
            //Variables to hold data from the filter
            string nameFilter = "";
            string packFilter = "";
            string minPriceFilter = "";
            string maxPriceFilter = "";

            //Variables for min and max price but as decimals
            decimal minPriceDecimal = 0;
            decimal maxPriceDecimal = 999999;

            //Variable to hold beverage data
            DbSet<Beverage> BeverageToFind = db.Beverages;

            //Checks to see if data has been input to the different filters
            //If data has been entered then the variable is assigned the value from the filter
            if (!String.IsNullOrWhiteSpace((string)Session["name"]))
            {
                nameFilter = (string) Session["name"];
            }
            if (!String.IsNullOrWhiteSpace((string)Session["pack"]))
            {
                packFilter = (string)Session["pack"];
            }
            if (!String.IsNullOrWhiteSpace((string)Session["minPrice"]))
            {
                minPriceFilter = (string)Session["minPrice"];
                minPriceDecimal = decimal.Parse(minPriceFilter);
            }
            if (!String.IsNullOrWhiteSpace((string)Session["maxPrice"]))
            {
                maxPriceFilter = (string)Session["maxPrice"];
                maxPriceDecimal = decimal.Parse(maxPriceFilter);
            }

            //Filters the data
            IEnumerable<Beverage> filteredBeverages =
                BeverageToFind.Where(
                    beverage =>
                        beverage.name.Contains(nameFilter) && beverage.pack.Contains(packFilter) &&
                        beverage.price >= minPriceDecimal && beverage.price <= maxPriceDecimal);

            //Places used filter values into the Viewbag so they can be used again.
            ViewBag.nameFilter = nameFilter;
            ViewBag.packFilter = packFilter;
            ViewBag.minPriceFilter = minPriceFilter;
            ViewBag.maxPriceFilter = maxPriceFilter;

            //Sends the filtered view to the user
            return View(filteredBeverages);
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Used to hold the user inputed data for the filter in the session and refresh the page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            //Gets form data from the request
            string nameSessionData = Request.Form.Get("name");
            string packSessionData = Request.Form.Get("pack");
            string minPriceSessionData = Request.Form.Get("minPrice");
            string maxPriceSessionData = Request.Form.Get("maxPrice");

            //Store data in the session
            Session["name"] = nameSessionData;
            Session["pack"] = packSessionData;
            Session["minPrice"] = minPriceSessionData;
            Session["maxPrice"] = maxPriceSessionData;

            //Sends use to index page
            return RedirectToAction("Index");
        }
    }
}
