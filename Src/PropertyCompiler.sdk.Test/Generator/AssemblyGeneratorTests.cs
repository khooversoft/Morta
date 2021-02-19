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
    public class AssemblyGeneratorTests
    {
        [Fact]
        public void Assembly_ShouldPass()
        {
            string raw = "assembly filePath;";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new AssemblyExpressionBuilder())
                .Add(raw)
                .Build();

            syntaxTree.IsError.Should().BeFalse();

            AssemblyExpression subject = (syntaxTree.SyntaxNodes.First() as AssemblyExpression).VerifyNotNull(nameof(AssemblyExpression));

            var body = new Body() + subject;

            IReadOnlyList<string> list = new TextCodeGenerator().Build(body);
            list.Should().NotBeNull();
            list.Count.Should().Be(1);
            list.First().Should().Be(raw);
        }
    }
}
