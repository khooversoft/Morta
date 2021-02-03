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

            ResourceExpression subject =
                (new ResourceExpressionBuilder().Create(syntaxTree) as ResourceExpression)
                .VerifyNotNull("Failed");

            subject.Should().NotBeNull();
            subject.ResourceId.Should().Be("resourceId");
            subject.FilePath.Should().Be("filePath");
        }
    }
}