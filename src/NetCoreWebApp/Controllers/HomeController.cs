using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebApp.Models;
using NetCoreWebApp.Services;

namespace NetCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataProvider<Movie> _movieDataProvider;

        public HomeController(DataProvider<Movie> movieDataProvider)
        {
            _movieDataProvider = movieDataProvider;
        }

        public IActionResult AsTable(int take = 100) => View(_movieDataProvider.Records.Take(take));

        public IActionResult AsJson(int take = 100) => Json(_movieDataProvider.Records.Take(take));

        public string Error() => "Whoops...";
    }
}
