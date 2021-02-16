using FluentAssertions;
using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Generator;
using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Tools;
using Xunit;

namespace PropertyCompiler.sdk.Test.Generator
{
    public class ObjectGenerator
    {
        [Fact]
        public void Assembly_ShouldPass()
        {
            var raw = new List<string>
            {
                "objectName = {",
                "   Name1 = Value1",
                "};",
            };

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw.ToArray())
                .Build();

            SyntaxResponse response = new ObjectExpressionBuilder().Create(syntaxTree);
            Body body = (response.SyntaxNode as Body)!;

            IReadOnlyList<string> list = new TextCodeGenerator().Build(body);
            list.Should().NotBeNull();

            Enumerable.SequenceEqual(raw, list);
        }
    }
}
