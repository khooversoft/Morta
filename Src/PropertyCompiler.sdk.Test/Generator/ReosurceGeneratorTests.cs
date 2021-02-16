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
    public class ReosurceGeneratorTests
    {
        [Fact]
        public void Resource_ShouldPass()
        {
            string raw = "resource resourceId = filePath;";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ResourceExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new ResourceExpressionBuilder().Create(syntaxTree);
            ResourceExpression subject = (response.SyntaxNode as ResourceExpression).VerifyNotNull(nameof(response.SyntaxNode));

            var body = new Body() + subject;

            IReadOnlyList<string> list = new TextCodeGenerator().Build(body);
            list.Should().NotBeNull();
            list.Count.Should().Be(1);
            list.First().Should().Be(raw);
        }
    }
}
