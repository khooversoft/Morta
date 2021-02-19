using FluentAssertions;
using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Syntax;
using System.Linq;
using Toolbox.Tools;
using Xunit;

namespace PropertyCompiler.sdk.Test.Expressions
{
    public class ScalarExpressionTests
    {
        [Fact]
        public void ValidScalar_ShouldPass()
        {
            string raw = "name = value;";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ScalarAssignmentBuilder())
                .Add(raw)
                .Build();

            syntaxTree.IsError.Should().BeFalse();

            ScalarAssignment subject = (syntaxTree.SyntaxNodes.First() as ScalarAssignment).VerifyNotNull(nameof(ScalarAssignment));

            subject.Should().NotBeNull();
            subject.VariableName.Value.Should().Be("name");
            subject.Constant.Value.Should().Be("value");
        }
    }
}