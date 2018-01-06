using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Polly;
using PollyKoans.Framework;
using RestSharp;

namespace PollyKoans
{
    public class _1_Retry_Koans
    {
        [Test]
        public void Handle_Exception_and_Retry()
        {
            //Replace FILL.__IN with value to make the test pass
            var count = FILL.__IN - 2;
            Policy
                .Handle<DivideByZeroException>()
                .Retry(1)
                .Execute(() => 8 / count++);
            Assert.That(count, Is_.Equal_To(2));
        }

        [Test]
        public void Handle_Exception_and_Retry_with_count()
        {
            var retries = 0;
            var policy = Policy
                .Handle<DivideByZeroException>()
                .Retry(FILL.__IN, (exception, retryCount, context) => { retries = retryCount; });
            try
            {
                policy.Execute(() => 8 / new[] {0}.First());
            }
            catch
            {
                ++retries;
            }

            Assert.That(retries, Is_.Equal_To(4));
        }
    }
}