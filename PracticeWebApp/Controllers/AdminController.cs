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
using Microsoft.Extensions.Configuration;

namespace PracticeWebApp.Controllers
{
    public class AdminController : Controller
    {
        private HttpClient client = new HttpClient();
        private IConfiguration Configuration;

        public AdminController(IConfiguration configuration)
        {
            this.Configuration = configuration;
            var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() };
            client = new HttpClient(httpClientHandler);
            string baseUri = this.Configuration.GetValue<string>("BaseURI");
            client.BaseAddress = new Uri(baseUri);
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
            if (json.id == 0)
            {
                json.id = Statics.libraryId;
            }
            else
            {
                Statics.libraryId = json.id;
            }
            HttpResponseMessage response = client.
                     GetAsync("api/booklibraryassociations/library/" + json.id.ToString()).Result;
            List<Books> data = response.Content.
                         ReadAsAsync<List<Books>>().Result;
            List<bool> avails = new List<bool>();
            List <BookWithStatus> bws = new List<BookWithStatus>();
            foreach(var book in data)
            {
                HttpResponseMessage response2 = client.
                     GetAsync("api/booklibraryassociations/getAvailability/" + book.BookId.ToString()).Result;
                bool data2 = response2.Content.
                         ReadAsAsync<bool>().Result;
                avails.Add(data2);
            }
            for(int i=0; i < data.Count; i++)
            {
                bws.Add(new BookWithStatus { BookId = data[i].BookId, Title = data[i].Title, Author = data[i].Author, Price = data[i].Price, Genre = data[i].Genre, isAvailable = avails[i]});
            }
            

            return View(bws);
        }

        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Books book) {
            string json = JsonConvert.SerializeObject(book);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync("api/books/"+Statics.libraryId.ToString(), httpContent);
            return RedirectToAction("ListBooks", new libId { id = Statics.libraryId });
        }


        public async Task<IActionResult> ChangeAvailability(BookAvailability bookAvailability)
        {
            string json = JsonConvert.SerializeObject(bookAvailability);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await client.PutAsync("api/booklibraryassociations/update", httpContent);
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
