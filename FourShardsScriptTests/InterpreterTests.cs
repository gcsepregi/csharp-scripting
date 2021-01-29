using FourShardsScript;
using NUnit.Framework;

namespace FourShardsScriptTests
{
    [TestFixture]
    public class InterpreterTests
    {
        [TestCase("1+1", 2)]
        [TestCase("7+3", 10)]
        [TestCase("7-3", 4)]
        [TestCase("5-8", -3)]
        [TestCase("15+28", 43)]
        [TestCase("15 + 29", 44)]
        [TestCase("16 - 29", -13)]
        [TestCase("16 - 29 + 4", -9)]
        [TestCase("16 - 29 + 4 -  17", -26)]
        [TestCase("1 + 3 * 6", 19)]
        [TestCase("1 - 3 * 6", -17)]
        [TestCase("(1 - 3) * 6", -12)]
        [TestCase("7 + 3 * (10 / (12 / (3 + 1) - 1)) / (2 + 3) - 5 - 3 + (8)", 10)]
        [TestCase("5---2", 3)]
        public void CanEvaluate(string expression, int expectedResult)
        {
            var lexer = new Lexer(expression);
            var parser = new Parser(lexer);
            var interpreter = new Interpreter(parser);
            var result = (int) interpreter.Interpret();
            Assert.AreEqual(expectedResult, result);
        }
    }
}