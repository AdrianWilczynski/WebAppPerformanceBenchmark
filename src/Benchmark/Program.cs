using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Benchmark
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var profile = LoadProfile(args[0]);
            var durations = await Measure(profile);
            WriteToFile(profile, durations, durations.Average(), durations.Median());
        }

        public static Profile LoadProfile(string path)
            => JsonConvert.DeserializeObject<Profile>(File.ReadAllText(path));

        public async static Task<IEnumerable<TimeSpan>> Measure(Profile profile)
        {
            var durations = new List<TimeSpan>();
            using (var httpClinet = new HttpClient())
            {
                for (int i = 0; i < profile.Repeat; i++)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var response = await httpClinet.GetStringAsync(profile.Url);

                    stopwatch.Stop();

                    durations.Add(stopwatch.Elapsed);

                    Console.WriteLine(profile.LogResponse ? response : $"{i + 1}/{profile.Repeat}");
                }
            }

            return durations.Skip(profile.Discard);
        }

        public static TimeSpan Median(this IEnumerable<TimeSpan> durations)
        {
            var sorted = durations.OrderBy(d => d.Ticks);
            var count = durations.Count();

            return (sorted.ElementAt(count / 2) + sorted.ElementAt((count - 1) / 2)) / 2;
        }

        public static TimeSpan Average(this IEnumerable<TimeSpan> durations)
            => new TimeSpan((long)durations.Average(d => d.Ticks));

        public static void WriteToFile(Profile profile, IEnumerable<TimeSpan> durations,
            TimeSpan average, TimeSpan median)
        {
            var content = JsonConvert.SerializeObject(new
            {
                profile.Name,
                Durations = durations,
                Count = new
                {
                    All = profile.Repeat,
                    Discarded = profile.Discard,
                    Measured = durations.Count()
                },
                Analysis = new
                {
                    Average = average,
                    Median = median
                }
            }, Formatting.Indented);

            File.WriteAllText(profile.Name + ".result.json", content);
        }
    }
}
