using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Net;

namespace PollyKoans
{

    public class Fill
    {
        public static object obj;
        public static string _in;
        public static int __in = 0;
        public static HttpStatusCode status_code;
    }

    public class EnlightedEqualTo : EqualConstraint
    {
        private object expected;
        private string enlightment = @"
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
        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var result = base.ApplyTo(actual);
            if (result.IsSuccess)
            {
                return result;
            }
            return new ConstraintResult(this, "Śrāvaka");
        }
        public override string Description => enlightment;
    }

    public class Is_ : Is
    {
        public static EqualConstraint EqualTo_(object expected)
        {
            return new EnlightedEqualTo(expected);
        }
    }
}
