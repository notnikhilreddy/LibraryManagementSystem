using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
using PracticeWebApi.Models;
using Books = PracticeWebApi.Models.Books;
using Newtonsoft.Json;
using System.Text;

namespace PracticeWebApp.Controllers
{
    public class AdminController : Controller
    {
        private HttpClient client = new HttpClient();
        //public static int libraryId;
        public AdminController()
        {
            var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() };
            client = new HttpClient(httpClientHandler);
            client.BaseAddress = new Uri("https://localhost:44364/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public class libId
        {
            public int id { get; set; }
        }
        // GET: HomeController1
        public IActionResult ListBooks(libId json)
        {
            Statics.libraryId = json.id;
            HttpResponseMessage response = client.
                     GetAsync("api/booklibraryassociations/library/" + json.id.ToString()).Result;
            List<Books> data = response.Content.
                         ReadAsAsync<List<Books>>().Result;
            return View(data);
        }

        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(Books book) {
            string json = JsonConvert.SerializeObject(book);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = client.PostAsync("api/books", httpContent);
            return RedirectToAction("ListBooks", new libId { id = Statics.libraryId });
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
