using System;
using System.Linq;
using NUnit.Framework;
using Polly;
using Polly.CircuitBreaker;
using PollyKoans.Framework;

namespace PollyKoans
{
    public class _6_Circuit_Breaker_Koans
    {
        [Test]
        public void Handle_Exception_and_Retry()
        {
            var count = 0;
            CircuitBreakerPolicy breaker = Policy
                .Handle<Exception>()
                .CircuitBreaker(
                    exceptionsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromMilliseconds(100)
                );

            new[] { 0, 1, 2 }.ToList().ForEach(i =>
            {
                try
                {
                    breaker.Execute(() =>
                    {
                        count++;
                        throw new ApplicationException();
                    });
                }
                catch (ApplicationException)
                {
                }
                catch (BrokenCircuitException)
                {
                }

            }
                );
            Assert.That(count, Is_.Equal_To(2));
        }

    }
}
