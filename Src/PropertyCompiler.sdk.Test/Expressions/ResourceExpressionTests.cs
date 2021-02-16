using FluentAssertions;
using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Syntax;
using Toolbox.Tools;
using Xunit;

namespace PropertyCompiler.sdk.Test.Expressions
{
    public class ResourceExpressionTests
    {
        [Fact]
        public void ValidResource_ShouldPass()
        {
            string raw = "resource resourceId = filePath;";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ResourceExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new ResourceExpressionBuilder().Create(syntaxTree);
            ResourceExpression subject = (response.SyntaxNode as ResourceExpression)!;

            subject.Should().NotBeNull();
            subject.ResourceId.Value.Should().Be("resourceId");
            subject.FilePath.Value.Should().Be("filePath");
        }
    }
}