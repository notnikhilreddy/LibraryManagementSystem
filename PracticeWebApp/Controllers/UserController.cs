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
using Microsoft.Extensions.Configuration;

namespace PracticeWebApp.Controllers
{
    public class UserController : Controller
    {
        private HttpClient client = new HttpClient();
        private IConfiguration Configuration;

        public UserController(IConfiguration configuration)
        {
            this.Configuration = configuration;
            var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() };
            client = new HttpClient(httpClientHandler);
            string baseUri = this.Configuration.GetValue<string>("BaseURI");
            client.BaseAddress = new Uri(baseUri);
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

        public async Task<IActionResult> ListMyCheckouts()
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
        public async Task<IActionResult> Checkout(BookId bId)
        {
            string json = JsonConvert.SerializeObject(new CheckoutBook { bookId = bId.bookId, userId = Statics.userId });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync("api/booklibraryassociations/checkout", httpContent);
            return RedirectToAction("ListBooks", "User", new UserId { id = Statics.userId});
        }

        public async Task<IActionResult> Return(BookId bId)
        {
            string json = JsonConvert.SerializeObject(bId);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync("api/booklibraryassociations/return", httpContent);
            return RedirectToAction("ListMyCheckouts", "User");
        }
    }
}
