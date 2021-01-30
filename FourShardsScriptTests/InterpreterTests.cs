using System;
using FourShardsScript;
using FourShardsScript.Symbols;
using NUnit.Framework;
using Shouldly;

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
        [TestCase("1+1", 2)]
        [TestCase("13+210", 223)]
        [TestCase("13 + 210", 223)]
        [TestCase("    13 + 213   ", 226)]
        [TestCase("    13 - 2   ", 11)]
        [TestCase("13 + 2 - 5", 10)]
        [TestCase("13 + 2 + 6", 21)]
        [TestCase("13 * 2", 26)]
        [TestCase("14 / 2", 7)]
        [TestCase("14 * 2 + 3", 31)]
        [TestCase("14 / 2 + 3", 10)]
        [TestCase("4 + 14 * 2", 32)]
        [TestCase("4 + 14 / 2", 11)]
        [TestCase("14 + 2 * 3 - 6 / 2", 17)]
        [TestCase("7 + 3 * (10 / (12 / (3 + 1) - 1))", 22)]
        [TestCase("7 + 3 * (10 / (12 / (3 + 1) - 1)) / (2 + 3) - 5 - 3 + (8)", 10)]
        [TestCase("7 + (((3 + 2)))", 12)]
        [TestCase("5+-2", 3)]
        [TestCase("5---2", 3)]
        [TestCase("5 - - - + - (3 + 4) - +2", 10)]
        [TestCase("let a = 12 + 4;", 16)]
        [TestCase("let $ = 13 + 4;", 17)]
        [TestCase("let _ = 14 + 4;", 18)]
        [TestCase("let a_ = 15 + 4;", 19)]
        [TestCase("let a$ = 16 + 4;", 20)]
        [TestCase("let a$ = 16; let b = 5; let c = a$ + b;", 21)]
        [TestCase("let a = 16; a = 5;", 5)]
        [TestCase("let a = 16; a = a + 9;", 25)]
        [TestCase("const a = 16;", 16)]
        [TestCase("true", true)]
        [TestCase("false", false)]
        [TestCase("let a = 10; a += 3; a;", 13)]
        [TestCase("let a = 10; a -= 3; a;", 7)]
        [TestCase("let a = 10; a *= 3; a;", 30)]
        [TestCase("let a = 12; a /= 3; a;", 4)]
        [TestCase("let a = true; a &&= true; a;", true)]
        [TestCase("let a = true; a &&= false; a;", false)]
        [TestCase("let a = false; a &&= true; a;", false)]
        [TestCase("let a = false; a &&= false; a;", false)]
        [TestCase("let a = true; a ||= true; a;", true)]
        [TestCase("let a = true; a ||= false; a;", true)]
        [TestCase("let a = false; a ||= true; a;", true)]
        [TestCase("let a = false; a ||= false; a;", false)]
        [TestCase("let a = false; a ||= false; a;", false)]
        [TestCase("let a = true ? 1 : 2; a;", 1)]
        [TestCase("let a = false ? 1 : 2; a;", 2)]
        [TestCase("let a = true || true; a;", true)]
        [TestCase("let a = true || false; a;", true)]
        [TestCase("let a = false || true; a;", true)]
        [TestCase("let a = false || false; a;", false)]
        [TestCase("let a = true && true; a;", true)]
        [TestCase("let a = true && false; a;", false)]
        [TestCase("let a = false && true; a;", false)]
        [TestCase("let a = false && false; a;", false)]
        [TestCase("let a = false && false && true; a;", false)]
        [TestCase("let a = false && false || true; a;", true)]
        [TestCase("let a = false && (false || true); a;", false)]
        [TestCase("let a = 1 == 1; a;", true)]
        [TestCase("let a = 1 == 2; a;", false)]
        [TestCase("let a = 1 != 1; a;", false)]
        [TestCase("let a = 1 != 2; a;", true)]
        [TestCase("let a = 1 < 2; a;", true)]
        [TestCase("let a = 1 < 1; a;", false)]
        [TestCase("let a = 1 < 0; a;", false)]
        [TestCase("let a = 1 <= 2; a;", true)]
        [TestCase("let a = 1 <= 1; a;", true)]
        [TestCase("let a = 1 <= 0; a;", false)]
        [TestCase("let a = 1 > 2; a;", false)]
        [TestCase("let a = 1 > 1; a;", false)]
        [TestCase("let a = 1 > 0; a;", true)]
        [TestCase("let a = 1 >= 2; a;", false)]
        [TestCase("let a = 1 >= 1; a;", true)]
        [TestCase("let a = 1 >= 0; a;", true)]
        [TestCase("let a = 10 % 2; a;", 0)]
        [TestCase("let a = 10 % 3; a;", 1)]
        [TestCase("let a = 10 ; a %= 4; a;", 2)]
        [TestCase("let a = !true; a;", false)]
        [TestCase("let a = !false; a;", true)]
        [TestCase("let a = 1; let b = 2; if (a < b) a = 3; a;", 3)]
        [TestCase("let a = 3; let b = 2; if (a < b) a = 1; a;", 3)]
        [TestCase("let a = 1; let b = 2; if (a < b) a = 3; else a = 4; a;", 3)]
        [TestCase("let a = 3; let b = 2; if (a < b) a = 1; else a = 5; a;", 5)]
        [TestCase("let a = 0; for (let i = 0; i < 10; i+=1) a += i * 2; a;", 90)]
        [TestCase("let a = 1; for ( ; a < 100; ) a += a * 2; a;", 243)]
        [TestCase("let a = 0; for ( let i = 0; i < 10; i+=1 ) { if ( i == 5 ) continue; a += i; } a;", 40)]
        [TestCase("let a = 0; for ( let i = 0; i < 10; i+=1 ) { if ( i == 5 ) break; a += i; } a;", 10)]
        [TestCase("let a = 0; while ( a < 10 ) a += 1; a;", 10)]
        [TestCase("let a = 0; while ( a < 10 ) { if ( a == 5 ) break ; a += 1; } a;", 5)]
        [TestCase("let a = 0; let i = 0; while ( i < 10 ) { i += 1; if ( i == 5 ) continue; a += 1; } a;", 9)]
        [TestCase("function f() {return true;} f();", true)]
        [TestCase("function f(a) {return a * 2;} f(10);", 20)]
        [TestCase("function f(a,b) { return a + b; } f(12, 14);", 26)]
        public void CanEvaluate(string expression, object expectedResult)
        {
            var lexer = new Lexer(expression);
            var parser = new Parser(lexer);
            var tree = parser.Parse();
            new SymbolTableBuilder().Visit(tree);
            var interpreter = new Interpreter(tree);
            var result = interpreter.Interpret();
            Assert.AreEqual(expectedResult, result);
        }
        
        [TestCase("const a = 16.2;", 16.2)]
        [TestCase("let a = 16.2 + 3.789 - 2.15;", 17.839)]
        public void WhenEval_ReturnsResultOfStatementAsDouble(string statement, double result)
        {
            var lexer = new Lexer(statement);
            var parser = new Parser(lexer);
            var tree = parser.Parse();
            new SymbolTableBuilder().Visit(tree);
            var interpreter = new Interpreter(tree);
            ShouldlyConfiguration.DefaultFloatingPointTolerance = 0.00000001;
            ((double) interpreter.Interpret()).ShouldBe(result);
        }

        [TestCase("{ let a = !false; a; }", true)]
        [TestCase("let a = false; { let a = !false; a; } a;", false)]
        [TestCase("let a = false; { a = !false; } a;", true)]
        public void WhenEval_HandlesScopesCorrectly(string statement, object result)
        {
            var lexer = new Lexer(statement);
            var parser = new Parser(lexer);
            var tree = parser.Parse();
            new SymbolTableBuilder().Visit(tree);
            var interpreter = new Interpreter(tree);
            interpreter.Interpret().ShouldBe(result);
        }

        [TestCase("a = 4;")]
        [TestCase("let a = b;")]
        [TestCase("let a = 4; let a = 10;")]
        [TestCase("const a = 4; const a = 10;")]
        [TestCase("const a = 4; let a = 10;")]
        [TestCase("let a = 4; const a = 10;")]
        [TestCase("const a = 4; a = 10;")]
        [TestCase("{ const a = 4; a = 10; } a;")]
        public void WhenEval_ThrowsException(string statement)
        {
            var lexer = new Lexer(statement);
            var parser = new Parser(lexer);
            var tree = parser.Parse();
            Should.Throw<Exception>(() => new SymbolTableBuilder().Visit(tree));
        }

    }
}