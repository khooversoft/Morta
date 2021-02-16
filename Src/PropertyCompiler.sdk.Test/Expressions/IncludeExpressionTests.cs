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

            SyntaxResponse response = new IncludeExpressionBuilder().Create(syntaxTree);
            IncludeExpression subject = (response.SyntaxNode as IncludeExpression).VerifyNotNull(nameof(response.SyntaxNode));

            subject.Should().NotBeNull();
            subject!.IncludePath.Value.Should().Be("filePath");
        }
    }
}