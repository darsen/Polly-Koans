using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Polly;
using PollyKoans.Framework;

namespace PollyKoans
{
    public class _0_Sample_Koan
    {
        [Test]
        public void Sample_Koan_Unresolved()
        {
            //Replace FILL.__IN with 2
            Assert.That(FILL.__IN, Is_.Equal_To(2));
        }

        [Test]
        public void Sample_Koan_Resolved()
        {
            //FILL.__IN was replaced with 2
            Assert.That(2, Is_.Equal_To(2));
        }

    }
}