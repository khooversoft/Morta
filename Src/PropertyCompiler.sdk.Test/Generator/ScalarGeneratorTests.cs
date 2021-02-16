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
    public class ScalarGeneratorTests
    {
        [Fact]
        public void Assembly_ShouldPass()
        {
            string raw = "name = value;";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ScalarAssignmentBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new ScalarAssignmentBuilder().Create(syntaxTree);
            ScalarAssignment subject = (response.SyntaxNode as ScalarAssignment).VerifyNotNull(nameof(response.SyntaxNode));

            var body = new Body() + subject;

            IReadOnlyList<string> list = new TextCodeGenerator().Build(body);
            list.Should().NotBeNull();
            list.Count.Should().Be(1);
            list.First().Should().Be(raw);
        }
    }
}
