using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PracticeWebApi.Models;
using PracticeWebApp.Models;
using Library = PracticeWebApi.Models.Library;

namespace PracticeWebApp.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient client = new HttpClient();
        private IConfiguration Configuration;

        public HomeController(IConfiguration configuration)
        {
            this.Configuration = configuration;
            var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() };
            client = new HttpClient(httpClientHandler);
            string baseUri = this.Configuration.GetValue<string>("BaseURI");
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Admin()
        {
            HttpResponseMessage response = client.
                     GetAsync("api/libraries").Result;
            List<Library> data = response.Content.
                         ReadAsAsync<List<Library>>().Result;
            return View(data);
        }

        public class libId
        {
            public int libraryId { get; set; }
        }


        public IActionResult User()
        {
            HttpResponseMessage response = client.
                     GetAsync("api/users").Result;
            List<PracticeWebApi.Models.Users> data = response.Content.
                         ReadAsAsync<List<PracticeWebApi.Models.Users>>().Result;
            return View(data);
        }

        public class userId
        {
            public int id { get; set; }
        }

        public IActionResult AddUser()
        {
            return View("AddUser");
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Users user)
        {
            string json = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync("api/users", httpContent);
            return RedirectToAction("User");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
