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
    public class ObjectGenerator
    {
        [Fact]
        public void SingleProperty_ShouldPass()
        {
            var raw = new List<string>
            {
                "objectName = {",
                "   Name1 = Value1",
                "};",
            };

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw.ToArray())
                .Build();

            syntaxTree.IsError.Should().BeFalse();

            Body body = (syntaxTree.SyntaxNodes.First() as Body).VerifyNotNull(nameof(Body));

            IReadOnlyList<string> list = new TextCodeGenerator().Build(body);
            list.Should().NotBeNull();

            Enumerable.SequenceEqual(raw, list).Should().BeTrue();
        }
        
        [Fact]
        public void MultipleProperties_ShouldPass()
        {
            var raw = new List<string>
            {
                "objectName = {",
                "   Name1 = Value1,",
                "   Name2 = Value2",
                "};",
            };

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw.ToArray())
                .Build();

            syntaxTree.IsError.Should().BeFalse();

            Body body = (syntaxTree.SyntaxNodes.First() as Body).VerifyNotNull(nameof(Body));

            IReadOnlyList<string> list = new TextCodeGenerator().Build(body);
            list.Should().NotBeNull();

            Enumerable.SequenceEqual(raw, list).Should().BeTrue();
        }
        
        [Fact]
        public void WithProperties_ShouldPass()
        {
            var raw = new List<string>
            {
                "objectName = refValue with {",
                "   Name1 = Value1",
                "};",
            };

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw.ToArray())
                .Build();

            syntaxTree.IsError.Should().BeFalse();

            Body body = (syntaxTree.SyntaxNodes.First() as Body).VerifyNotNull(nameof(Body));

            IReadOnlyList<string> list = new TextCodeGenerator().Build(body);
            list.Should().NotBeNull();

            Enumerable.SequenceEqual(raw, list).Should().BeTrue();
        }
        
        [Fact]
        public void InnerWithProperties_ShouldPass()
        {
            var raw = new List<string>
            {
                "objectName = {",
                "   Name1 = Value1 with {",
                "      Name2 = Value2,",
                "      Name3 = Value3",
                "   }",
                "};",
            };

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw.ToArray())
                .Build();

            syntaxTree.IsError.Should().BeFalse();

            Body body = (syntaxTree.SyntaxNodes.First() as Body).VerifyNotNull(nameof(Body));

            IReadOnlyList<string> list = new TextCodeGenerator().Build(body);
            list.Should().NotBeNull();

            Enumerable.SequenceEqual(raw, list).Should().BeTrue();
        }
    }
}
