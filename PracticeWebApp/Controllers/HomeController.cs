using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using PracticeWebApi.Models;
using PracticeWebApp.Models;
using Library = PracticeWebApi.Models.Library;

namespace PracticeWebApp.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient client = new HttpClient();

        public HomeController()
        {
            var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() };
            client = new HttpClient(httpClientHandler);
            client.BaseAddress = new Uri("https://localhost:44364/");
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
