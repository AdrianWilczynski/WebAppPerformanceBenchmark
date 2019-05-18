using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreWebApp.Services
{
    public class DataProvider<T>
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public DataProvider(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void LoadData(string path)
        {
            var streamReader = new StreamReader(Path.Join(_hostingEnvironment.ContentRootPath, path));
            var csvReader = new CsvReader(streamReader);

            Records = csvReader.GetRecords<T>().ToList();
        }

        public IEnumerable<T> Records { get; private set; }
    }
}