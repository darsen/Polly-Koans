using System;
using NUnit.Framework;
using Polly;

namespace PollyKoans
{
    public class _5_Policy_Wrap_Koans
    {
        [Test]
        public void Timeout_with_Retry()
        {
            //var timeout = Policy.Timeout(TimeSpan.FromMilliseconds(100));
            //var retry = Policy.Handle<Exception>()
            //    .Retry(2);
            //var retryWithTimeout =
            //    timeout.Wrap(
            //        retry); // Wrap them in the other order if you want the timeout per call, not per overall strategy
            //retryWithTimeout.Execute(cancellationToken => DoSomething(cancellationToken));

            //timeout.Execute();
        }
    }
}