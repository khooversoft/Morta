using FluentAssertions;
using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Syntax;
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

            SyntaxResponse response = new ScalarAssignmentBuilder().Create(syntaxTree);
            ScalarAssignment subject = (response.SyntaxNode as ScalarAssignment).VerifyNotNull(nameof(response.SyntaxNode));

            subject.Should().NotBeNull();
            subject.VariableName.Value.Should().Be("name");
            subject.Constant.Value.Should().Be("value");
        }
    }
}