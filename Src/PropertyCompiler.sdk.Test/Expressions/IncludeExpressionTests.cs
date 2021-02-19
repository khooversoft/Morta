using FluentAssertions;
using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Syntax;
using System.Linq;
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

            syntaxTree.IsError.Should().BeFalse();

            IncludeExpression subject = (syntaxTree.SyntaxNodes.First() as IncludeExpression).VerifyNotNull(nameof(IncludeExpression));

            subject.Should().NotBeNull();
            subject!.IncludePath.Value.Should().Be("filePath");
        }
    }
}