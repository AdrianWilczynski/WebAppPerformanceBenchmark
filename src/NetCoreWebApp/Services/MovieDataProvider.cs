using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using NetCoreWebApp.Models;

namespace NetCoreWebApp.Services
{
    public class MovieDataProvider
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public MovieDataProvider(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void ReadData(string path)
        {
            var streamReader = new StreamReader(Path.Join(_hostingEnvironment.ContentRootPath, path));
            var csvReader = new CsvReader(streamReader);
            Movies = csvReader.GetRecords<Movie>().ToList();
        }

        public IEnumerable<Movie> Movies { get; private set; }
    }
}