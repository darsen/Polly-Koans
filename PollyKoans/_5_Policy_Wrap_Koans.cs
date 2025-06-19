using System;
using NUnit.Framework;
using Polly;
using PollyKoans.Framework;

namespace PollyKoans
{
    public class _5_Policy_Wrap_Koans
    {
        [Test]
        public void Retry_inside_Timeout()
        {
            var count = 0;
            var whatHappened = "";
            var timeout = Policy.Timeout(TimeSpan.FromMilliseconds(5));
            var retry = Policy.Handle<Exception>()
                .WaitAndRetryForever(attempt => TimeSpan.FromMilliseconds(4));
            var retryWithTimeout = timeout.Wrap(retry);
            try
            {
                retryWithTimeout.Execute(() => 8 / count++);
            }
            catch (DivideByZeroException)
            {
                whatHappened = "exception";
            }
            catch (Polly.Timeout.TimeoutRejectedException)
            {
                whatHappened = "timed out";
            }
            Assert.That(whatHappened, Is_.Equal_To(FILL._IN));
        }
    }
}