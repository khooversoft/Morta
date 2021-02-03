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
                .Add(new ScalarAssignmentExpressionBuilder())
                .Add(raw)
                .Build();

            ScalarAssignmentExpression subject =
                (new ScalarAssignmentExpressionBuilder().Create(syntaxTree) as ScalarAssignmentExpression)
                .VerifyNotNull("failed");

            subject.Should().NotBeNull();
            subject.VariableName.Should().Be("name");
            subject.Constant.Should().Be("value");
        }
    }
}