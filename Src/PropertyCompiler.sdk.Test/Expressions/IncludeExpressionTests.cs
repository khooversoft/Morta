using FluentAssertions;
using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Syntax;
using Toolbox.Tools;
using Xunit;

namespace PropertyCompiler.sdk.Test.Expressions
{
    public class IncludeExpressionTests
    {
        [Fact]
        public void ValidAssembly_ShouldPass()
        {
            string raw = "include filePath;";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new IncludeExpressionBuilder())
                .Add(raw)
                .Build();

            IncludeExpression subject =
                (new IncludeExpressionBuilder().Create(syntaxTree) as IncludeExpression)
                .VerifyNotNull("Failed");

            subject.Should().NotBeNull();
            subject.IncludePath.Should().Be("filePath");
        }
    }
}