using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PerformanceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (PerformanceCounterCategory.Exists(PerformanceCounterConfiguration.CategoryName))
                PerformanceCounterCategory.Delete(PerformanceCounterConfiguration.CategoryName);

            PerformanceManager.SetupCategoryAndCounters();
            PerformanceManager.OutputDebugData();

            while (true)
            {
                //Each turn of the loop simulates a submission run
                //The results of which would then be set into the performance counters
                //'Total' type counters are ongoing and are therefore incremented (or decremented), whereas
                //'LastRun' type counters are a snapshot of the most recent activity

                var random = new Random(DateTime.Now.Millisecond);
                int total = random.Next(25, 250);
                double min = (total / 100D) * 90; //min 90% success rate
                int success = random.Next((int)min, total);
                int errors = total - success;

                PerformanceManager.SetCounter("TotalProcessed", "Submission", c => c.IncrementBy(total));
                PerformanceManager.SetCounter("ProcessedLastRun", "Submission", c => c.RawValue = total);
                PerformanceManager.SetCounter("TotalSuccess", "Submission", c => c.IncrementBy(success));
                PerformanceManager.SetCounter("SuccessLastRun", "Submission", c => c.RawValue = success);
                PerformanceManager.SetCounter("TotalErrors", "Submission", c => c.IncrementBy(errors));
                PerformanceManager.SetCounter("ErrorsLastRun", "Submission", c => c.RawValue = errors);
                PerformanceManager.SetCounter("TotalSuccessToError", "Submission", c => c.IncrementBy(success));
                PerformanceManager.SetCounter("TotalSuccessToErrorBase", "Submission", c => c.IncrementBy(total));
                PerformanceManager.SetCounter("SuccessToErrorLastRun", "Submission", c => c.RawValue = success);
                PerformanceManager.SetCounter("SuccessToErrorLastRunBase", "Submission", c => c.RawValue = total);
                PerformanceManager.SetCounter("TimeSinceLastRun", "Submission", c => c.RawValue = Stopwatch.GetTimestamp());
                PerformanceManager.OutputDebugData();

                //pause for user input to continue..
                Console.Read();
            }
        }
    }
}
