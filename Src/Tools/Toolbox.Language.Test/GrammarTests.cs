using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Xunit;

namespace Toolbox.Language.Test
{
    public class GrammarTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("int")]
        [InlineData("int i")]
        [InlineData("int ;")]
        public void SimpleVariableDefine_ShouldFail(string command)
        {
            CodeBlock<TokenType> processingRules = new CodeBlock<TokenType>()
            {
                new CodeBlock<TokenType>() + LanguageSyntax.TypeName + LanguageSyntax.VariableName + LanguageSyntax.SemiColon,
            };

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolNode<TokenType>? syntaxNode = parser.Parse(command);
            syntaxNode.Should().BeNull();
        }

        [Fact]
        public void SimpleVariableDefine_ShouldPass()
        {
            string command = "int i;";

            CodeBlock<TokenType> processingRules = new CodeBlock<TokenType>()
            {
                new CodeBlock<TokenType>() + LanguageSyntax.TypeName + LanguageSyntax.VariableName + LanguageSyntax.SemiColon,
            };

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolNode<TokenType>? syntaxNode = parser.Parse(command);
            syntaxNode.Should().NotBeNull();

            ISymbolToken[] matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.TypeName, "int"),
                new SymbolValue<TokenType>(TokenType.VariableName, "i"),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }

        [Fact]
        public void DeclareVariable_ShouldPass()
        {
            string command = "declare objectName =";

            CodeBlock<TokenType> processingRules = new CodeBlock<TokenType>()
            {
                new CodeBlock<TokenType>()
                    + LanguageSyntax.DeclareObject
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
            };

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolNode<TokenType>? syntaxNode = parser.Parse(command);
            syntaxNode.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolToken<TokenType>(TokenType.DeclareObject),
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
            };

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }

        [Fact]
        public void DeclareVariableWithExtracCommand_ShouldPass()
        {
            string command = "declare objectName =; name=value";

            CodeBlock<TokenType> processingRules = new CodeBlock<TokenType>()
            {
                new CodeBlock<TokenType>()
                    + LanguageSyntax.DeclareObject
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
            };

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolNode<TokenType>? syntaxNode = parser.Parse(command);
            syntaxNode.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolToken<TokenType>(TokenType.DeclareObject),
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
            };

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }
    }
}