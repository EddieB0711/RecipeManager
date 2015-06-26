using RecipeManager.Generic.DataAccess;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace RecipeManager.ExpressionTraversal.UnitTests {

    public class TreeTraversalUnitTests {

        [Fact]
        public void TestSingleExpression() {
            var translator = new QueryTranslator();
            Expression<Func<TestRecord, bool>> expression = x => x.Col1 == 1;
            string result = translator.Translate(expression);

            Assert.True(result == "(Col1 = 1)");
        }

        [Fact]
        public void TestMultiExpression() {
            var translator = new QueryTranslator();
            Expression<Func<TestRecord, bool>> expression = x => x.Col1 == 1 && x.Col2 == 2.3;
            string result = translator.Translate(expression);

            Assert.True(result == "((Col1 = 1) AND (Col2 = 2.3))");
        }

        [Fact]
        public void TestAndOrExpression() {
            var translator = new QueryTranslator();
            Expression<Func<TestRecord, bool>> expression = x => x.Col1 == 1 && x.Col2 == 2.3 || x.Col3 == "test";
            string result = translator.Translate(expression);

            Assert.True(result == "(((Col1 = 1) AND (Col2 = 2.3)) OR (Col3 = 'test'))");
        }

        [Fact]
        public void TestContainsExpression() {
            var translator = new QueryTranslator();
            var items = Enumerable.Range(1, 5).Cast<int?>();
            Expression<Func<TestRecord, bool>> expression = x => items.Contains(x.Col1);
            string result = translator.Translate(expression);

            Assert.True(result == "(Col1 IN(1, 2, 3, 4, 5))");
        }
    }

    internal class TestRecord {

        public TestRecord() {
            Col1 = 1;
            Col2 = 2;
            Col3 = "test";
        }

        public int? Col1 { get; set; }
        public double? Col2 { get; set; }
        public string Col3 { get; set; }
    }
}