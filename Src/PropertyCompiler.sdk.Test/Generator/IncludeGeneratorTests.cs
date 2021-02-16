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
    public class IncludeGeneratorTests
    {
        [Fact]
        public void Assembly_ShouldPass()
        {
            string raw = "include filePath;";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new IncludeExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new IncludeExpressionBuilder().Create(syntaxTree);
            IncludeExpression subject = (response.SyntaxNode as IncludeExpression).VerifyNotNull(nameof(response.SyntaxNode));

            var body = new Body() + subject;

            IReadOnlyList<string> list = new TextCodeGenerator().Build(body);
            list.Should().NotBeNull();
            list.Count.Should().Be(1);
            list.First().Should().Be(raw);
        }
    }
}
