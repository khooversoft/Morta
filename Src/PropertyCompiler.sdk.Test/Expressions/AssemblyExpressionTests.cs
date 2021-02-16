﻿using FluentAssertions;
using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Syntax;
using Toolbox.Tools;
using Xunit;

namespace PropertyCompiler.sdk.Test.Expressions
{
    public class AssemblyExpressionTests
    {
        [Fact]
        public void ValidAssembly_ShouldPass()
        {
            string raw = "assembly filePath;";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new AssemblyExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new AssemblyExpressionBuilder().Create(syntaxTree);
            AssemblyExpression subject = (response.SyntaxNode as AssemblyExpression)!;

            subject.Should().NotBeNull();
            subject.AssemblyPath.Value.Should().Be("filePath");
        }
    }
}