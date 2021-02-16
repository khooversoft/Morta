using PropertyCompiler.sdk.Expressions;
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
    public class AssemblyGeneratorTests
    {
        [Fact]
        public void Assembly_ShouldPass()
        {
            string raw = "assembly filePath;";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new AssemblyExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new AssemblyExpressionBuilder().Create(syntaxTree);
            AssemblyExpression subject = (response.SyntaxNode as AssemblyExpression).VerifyNotNull(nameof(response.SyntaxNode));


        }
    }
}
