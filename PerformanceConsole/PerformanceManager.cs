using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PerformanceConsole
{
    public class PerformanceManager
    {
        private static PerformanceCounterCategory _counterCategory = null;

        public static bool SetupCategoryAndCounters()
        {
            bool created = false;

            if (PerformanceCounterCategory.Exists(PerformanceCounterConfiguration.CategoryName))
                _counterCategory = PerformanceCounterCategory.GetCategories()
                    .First(c => c.CategoryName == PerformanceCounterConfiguration.CategoryName);
            else
            {
                var counters = new List<CounterCreationData>();
                foreach (var item in PerformanceCounterConfiguration.GetData())
                    counters.Add(new CounterCreationData(item.Name, item.Desc, item.Kind));

                _counterCategory = PerformanceCounterCategory.Create(
                    PerformanceCounterConfiguration.CategoryName,
                    PerformanceCounterConfiguration.CategoryDesc,
                    PerformanceCounterCategoryType.MultiInstance,
                    new CounterCreationDataCollection(counters.ToArray()));

                Console.WriteLine("Created the following performance counters:");
                counters.ForEach(c => Console.WriteLine($" > {c.CounterName} ({c.CounterHelp}) [{c.CounterType}]"));
                Console.WriteLine("");
                created = true;
            }

            return created;
        }

        public static void SetCounter(string counterName, string instanceName, Func<PerformanceCounter, object> operation)
        {
            var counter = new PerformanceCounter(PerformanceCounterConfiguration.CategoryName, counterName, instanceName, false);
            operation(counter);
        }

        public static PerformanceCounter GetCounter(string name, string instance = null)
        {
            PerformanceCounter[] counters = instance == null ? 
                _counterCategory.GetCounters() : _counterCategory.GetCounters(instance);
            
            PerformanceCounter counter = counters.FirstOrDefault(c => c.CounterName == name);

            if (counter.ReadOnly)
                counter.ReadOnly = false;

            return counter;
        }

        public static void OutputDebugData()
        {
            var categories = PerformanceCounterCategory.GetCategories();
            var category = categories.Where(c => c.CategoryName == PerformanceCounterConfiguration.CategoryName).FirstOrDefault();
            if (category != null)
            {
                Console.WriteLine($"Getting counters for {category.CategoryName}..");
                if (category.CategoryType == PerformanceCounterCategoryType.MultiInstance)
                {
                    Console.Write($"Getting instances..");
                    List<string> instanceNames = category.GetInstanceNames().ToList();
                    instanceNames.ForEach(i => Console.Write($" {i}"));
                    Console.WriteLine("");
                    if (instanceNames.Count() > 0)
                    {
                        foreach (string instance in instanceNames)
                        {
                            Console.WriteLine($"Instance counters for '{instance}':");
                            var lines = category.GetCounters(instance).Select(x => $" > {x.CounterName}: {GetRawValue(x)}").ToList();
                            lines.ForEach(c => Console.WriteLine(c));
                        }
                    }
                    else
                        Console.WriteLine($"No instances within {category.CategoryName}");
                }

                Console.WriteLine($"Finished getting counters for {category.CategoryName}");
                Console.WriteLine("");
            }
        }

        public static string GetRawValue(PerformanceCounter pc)
        {
            string result = "";

            switch (pc.CounterType)
            {
                case PerformanceCounterType.ElapsedTime:
                    int seconds = (int)((Stopwatch.GetTimestamp() - pc.RawValue) / Stopwatch.Frequency);
                    var span = new TimeSpan(0, 0, seconds);
                    result = $"{(span.Hours < 10 ? "0" : "")}{span.Hours}:{(span.Minutes < 10 ? "0" : "")}{span.Minutes}:{(span.Seconds < 10 ? "0" : "")}{span.Seconds}";
                    break;

                default:
                    result = pc.RawValue.ToString();
                    break;
            }

            return result;
        }
    }
}
