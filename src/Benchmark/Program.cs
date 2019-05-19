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
            var result = await Measure(profile);
            WriteToFile(profile, result.durations, result.failed,
                result.durations.Average(), result.durations.Median(), result.durations.Total());
        }

        public static Profile LoadProfile(string path)
            => JsonConvert.DeserializeObject<Profile>(File.ReadAllText(path));

        public async static Task<(IEnumerable<TimeSpan> durations, int failed)> Measure(Profile profile)
        {
            var failed = 0;
            var durations = new List<TimeSpan>();
            using (var httpClinet = new HttpClient())
            {
                for (int i = 0; i < profile.Repeat; i++)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var response = await httpClinet.GetAsync(profile.Url);

                    stopwatch.Stop();

                    if (response.IsSuccessStatusCode)
                    {
                        durations.Add(stopwatch.Elapsed);
                    }
                    else
                    {
                        failed++;
                    }

                    Console.WriteLine(profile.LogResponse
                        ? await response.Content.ReadAsStringAsync()
                        : $"{i + 1}/{profile.Repeat}");
                }
            }

            return (durations.Skip(profile.Discard), failed);
        }

        public static TimeSpan Median(this IEnumerable<TimeSpan> durations)
        {
            var sorted = durations.OrderBy(d => d.Ticks);
            var count = durations.Count();

            return (sorted.ElementAt(count / 2) + sorted.ElementAt((count - 1) / 2)) / 2;
        }

        public static TimeSpan Average(this IEnumerable<TimeSpan> durations)
            => new TimeSpan((long)durations.Average(d => d.Ticks));

        public static TimeSpan Total(this IEnumerable<TimeSpan> durations)
            => durations.Aggregate((sum, current) => sum + current);

        public static void WriteToFile(Profile profile, IEnumerable<TimeSpan> durations, int failed,
            TimeSpan average, TimeSpan median, TimeSpan total)
        {
            var content = JsonConvert.SerializeObject(new
            {
                profile.Name,
                Durations = durations,
                Count = new
                {
                    Requested = profile.Repeat,
                    Discarded = profile.Discard,
                    Measured = durations.Count(),
                    Failed = failed,
                },
                Analysis = new
                {
                    Average = average,
                    Median = median,
                    Total = total
                }
            }, Formatting.Indented);

            File.WriteAllText(profile.Name + ".result.json", content);
        }
    }
}
