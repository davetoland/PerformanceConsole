using System.Diagnostics;

namespace PerformanceConsole
{
    public class PerformanceCounterConfiguration
    {
        public const string CategoryName = "TestTradingPerformanceCounters";
        public const string CategoryDesc = "Performance counters for a dummy trading service";
                
        public static dynamic GetData()
        {
            var data = new [] {
                new
                {
                    Name = "TotalProcessed",
                    Desc = "Total orders processed",
                    Kind = PerformanceCounterType.NumberOfItems64
                },
                new
                {
                    Name = "ProcessedLastRun",
                    Desc = "Order processed last run",
                    Kind = PerformanceCounterType.NumberOfItems32
                },
                new
                {
                    Name = "TotalSuccess",
                    Desc = "Total success",
                    Kind = PerformanceCounterType.NumberOfItems64
                },
                new
                {
                    Name = "SuccessLastRun",
                    Desc = "Success last run",
                    Kind = PerformanceCounterType.NumberOfItems32
                },
                new
                {
                    Name = "TotalErrors",
                    Desc = "Total errors",
                    Kind = PerformanceCounterType.NumberOfItems64
                },
                new
                {
                    Name = "ErrorsLastRun",
                    Desc = "Errors last run",
                    Kind = PerformanceCounterType.NumberOfItems32
                },
                new
                {
                    Name = "TotalSuccessToError",
                    Desc = "Total percentage of success to error",
                    Kind = PerformanceCounterType.RawFraction
                },
                new
                {
                    Name = "TotalSuccessToErrorBase",
                    Desc = "The value of total processed",
                    Kind = PerformanceCounterType.RawBase
                },
                new
                {
                    Name = "SuccessToErrorLastRun",
                    Desc = "Percentage of success to error last run",
                    Kind = PerformanceCounterType.RawFraction
                },
                new
                {
                    Name = "SuccessToErrorLastRunBase",
                    Desc = "The value of processed last run",
                    Kind = PerformanceCounterType.RawBase
                },
                new
                {
                    Name = "TimeSinceLastRun",
                    Desc = "Time since last run",
                    Kind = PerformanceCounterType.ElapsedTime
                }
            };

            return data;
        }
    }
}
