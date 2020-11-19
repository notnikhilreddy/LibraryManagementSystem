using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeWebApi.Models;
using Newtonsoft.Json;
using System.Text;
using System.ComponentModel.Design;

namespace PracticeWebApp.Controllers
{
    public class UserController : Controller
    {
        private HttpClient client = new HttpClient();
        //public static int libraryId;
        public UserController()
        {
            var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() };
            client = new HttpClient(httpClientHandler);
            client.BaseAddress = new Uri("https://localhost:44364/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public class UserId
        {
            public int id { get; set; }
        }
        // GET: HomeController1
        public IActionResult ListBooks(UserId json)
        {
            Statics.userId = json.id;
            HttpResponseMessage response = client.
                     GetAsync("api/booklibraryassociations/checkablebooks").Result;
            List<Books> data = response.Content.
                         ReadAsAsync<List<Books>>().Result;
            return View(data);
        }

        public IActionResult ListMyCheckouts()
        {
            HttpResponseMessage response = client.
                    GetAsync("api/userbookassociations/mycheckouts/"+Statics.userId.ToString()).Result;
            List<BookWithDuedate> data = response.Content.
                         ReadAsAsync<List<BookWithDuedate>>().Result;
            return View(data);
        }

        public class BookId
        {
            public int bookId { get; set; }
        }
        public class CheckoutBook
        {
            public int bookId { get; set; }
            public int userId { get; set; }
        }
        public IActionResult Checkout(BookId bId)
        {
            string json = JsonConvert.SerializeObject(new CheckoutBook { bookId = bId.bookId, userId = Statics.userId });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = client.PostAsync("api/booklibraryassociations/checkout", httpContent);
            return RedirectToAction("ListBooks", "User", new UserId { id = Statics.userId});
        }

        public IActionResult Return(BookId bId)
        {
            string json = JsonConvert.SerializeObject(bId);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = client.PostAsync("api/booklibraryassociations/return", httpContent);
            return RedirectToAction("ListMyCheckouts", "User");
        }
        // GET: HomeController1
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
