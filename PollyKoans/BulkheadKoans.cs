using NUnit.Framework;
using Polly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PollyKoans
{
    public class BulkheadKoans
    {
        [Test]
        public void Bulkhead_Queing_And_Parallel_Execution() {
            var maximumNumberOfActionsInParallel = 0;
            var minumumNumberOfSlotsAvailableInQueue = 10;
            var parallelActionCount = 0;
            var policy = Policy.Bulkhead(maxParallelization: 4, maxQueuingActions: 8);
            Parallel.ForEach(Enumerable.Range(0, 9), (p)=> {
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
            Assert.That(Fill.__in, Is_.EqualTo_(maximumNumberOfActionsInParallel + minumumNumberOfSlotsAvailableInQueue));
        }

        [Test]
        public void Bulkhead_Queue_Full()
        {
            var exceptionCount = 0;
            var policy = Policy.Bulkhead(maxParallelization: 4, maxQueuingActions: 2);
            try
            {
                Parallel.ForEach(Enumerable.Range(0, 9), (p) =>
                {
                    policy.Execute(() =>
                        {
                            Thread.Sleep(100);
                        }
                    );
                });
            }
            catch (AggregateException ae) {
                foreach (var ex in ae.InnerExceptions)
                {
                    if (ex is Polly.Bulkhead.BulkheadRejectedException)
                        exceptionCount++;
                }
            }
            Assert.That(Fill._in, Is_.EqualTo_(exceptionCount));
        }

    }

}
