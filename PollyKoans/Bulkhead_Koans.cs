using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Polly;
using Polly.Bulkhead;
using PollyKoans.Framework;

namespace PollyKoans
{
    public class Bulkhead_Koans
    {
        [Test]
        public void Bulkhead_Queing_And_Parallel_Execution()
        {
            var maximumNumberOfActionsInParallel = 0;
            var minumumNumberOfSlotsAvailableInQueue = 10;
            var parallelActionCount = 0;
            var policy = Policy.Bulkhead(4, 8);
            Parallel.ForEach(Enumerable.Range(0, 9), p =>
            {
                policy.Execute(() =>
                    {
                        parallelActionCount++;
                        if (policy.QueueAvailableCount < minumumNumberOfSlotsAvailableInQueue)
                            minumumNumberOfSlotsAvailableInQueue = policy.QueueAvailableCount;
                        if (parallelActionCount > maximumNumberOfActionsInParallel)
                            maximumNumberOfActionsInParallel = parallelActionCount;
                        Thread.Sleep(100);
                        parallelActionCount--;
                    }
                );
            });
            Assert.That(FILL.__IN,
                Is_.Equal_To(maximumNumberOfActionsInParallel + minumumNumberOfSlotsAvailableInQueue));
        }

        [Test]
        public void Bulkhead_Queue_Full()
        {
            var exceptionCount = 0;
            var policy = Policy.Bulkhead(4, 2);
            try
            {
                Parallel.ForEach(Enumerable.Range(0, 9), p =>
                {
                    policy.Execute(() => { Thread.Sleep(100); }
                    );
                });
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                    if (ex is BulkheadRejectedException)
                        exceptionCount++;
            }

            Assert.That(FILL._IN, Is_.Equal_To(exceptionCount));
        }
    }
}