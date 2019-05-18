using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebApp.Services;

namespace NetCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieDataProvider _movieDataProvider;

        public HomeController(MovieDataProvider movieDataProvider)
        {
            _movieDataProvider = movieDataProvider;
        }

        public IActionResult AsTable(int take = 100) => View(_movieDataProvider.Movies.Take(take));

        public IActionResult AsJson(int take = 100) => Json(_movieDataProvider.Movies.Take(take));

        public string Error() => "Whoops...";
    }
}
