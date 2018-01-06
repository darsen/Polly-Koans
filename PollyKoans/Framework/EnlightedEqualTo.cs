using System;
using System.Net;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace PollyKoans.Framework
{
    public class FILL
    {
        public static string _IN;
        public static int __IN = 0;
        public static HttpStatusCode IN_STATUS_CODE;
    }

    public class EnlightedEqualTo : EqualConstraint
    {
        private readonly string enlightment = @"
                        _ooOoo_
                       o8888888o
                       88"" . ""88
                       (| -_- |)
                       O\  =  /O
                    ____/`---'\____
                  .'  \\|     |//  `.
                 /  \\|||  :  |||//  \
                /  _||||| -:- |||||_  \
                |   | \\\  -  /'| |   |
                | \_|  `\`---'//  |_/ |
                \  .-\__ `-. -'__/-.  /
              ___`. .'  /--.--\  `. .'___
           ."""" '<  `.___\_<|>_/___.' _> \"""".
          | | :  `- \`. ;`. _/; .'/ /  .' ; |
          \  \ `-.   \_\_`. _.'_/_/  -' _.' /
===========`-.`___`-.__\ \___  /__.-'_.'_.-'================
";

        public EnlightedEqualTo(object expected) : base(expected)
        {
        }

        public override string Description => enlightment;

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var result = base.ApplyTo(actual);
            if (result.IsSuccess) return result;
            return new ConstraintResult(this, "Śrāvaka");
        }
    }

    public class Is_ : Is
    {
        public static EqualConstraint Equal_To(object expected)
        {
            return new EnlightedEqualTo(expected);
        }
    }

    public class FILL_IN_EXEPTION : Exception
    {
    }
}