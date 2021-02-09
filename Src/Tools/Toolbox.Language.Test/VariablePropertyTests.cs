using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Xunit;

namespace Toolbox.Language.Test
{
    public class VariablePropertyTests
    {

        [Fact]
        public void DeclareVariableProperties_ShouldPass()
        {
            List<string> dump = new List<string>();

            var commands = new[]
            {
                "declare objectName = {",
                    "Name=Value",
                "};"
            };

            CodeBlock<TokenType> processingRules = new CodeBlock<TokenType>()
            {
                new CodeBlock<TokenType>()
                    + LanguageSyntax.DeclareObject
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
                    + LanguageSyntax.LeftBrace
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
                    + LanguageSyntax.Constant
                    + LanguageSyntax.RightBrace
                    + LanguageSyntax.SemiColon
            };

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolParserResponse<TokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolToken<TokenType>(TokenType.DeclareObject),
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value"),
                new SymbolToken<TokenType>(TokenType.RightBrace),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleDeclareVariableProperties_ShouldPass()
        {
            var commands = new[]
            {
                "declare objectName = {",
                    "Name1=Value1,",
                    "Name2 =Value2",
                "};"
            };

            CodeBlock<TokenType> processingRules = new CodeBlock<TokenType>()
            {
                new CodeBlock<TokenType>()
                    + LanguageSyntax.DeclareObject
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
                    + LanguageSyntax.LeftBrace
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
                    + LanguageSyntax.Constant

                    + (new Optional<TokenType>()
                        + (new Repeat<TokenType>()
                            + LanguageSyntax.Comma
                            + LanguageSyntax.VariableName
                            + LanguageSyntax.Equal
                            + LanguageSyntax.Constant
                            )
                        )

                    + LanguageSyntax.RightBrace
                    + LanguageSyntax.SemiColon
            };

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolParserResponse<TokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolToken<TokenType>(TokenType.DeclareObject),
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name1"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value1"),
                new SymbolToken<TokenType>(TokenType.Comma),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name2"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value2"),
                new SymbolToken<TokenType>(TokenType.RightBrace),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleDeclareVariableProperties_WithComponentModel_ShouldPass()
        {
            List<string> dump = new List<string>();

            var commands = new[]
            {
                "objectName = {",
                    "Name1=Value1,",
                    "Name2 =Value2",
                "};"
            };

            var propertyAssignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            CodeBlock<TokenType> processingRules = new CodeBlock<TokenType>()
            {
                new CodeBlock<TokenType>()
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
                    + LanguageSyntax.LeftBrace
                    + propertyAssignment

                    + (new Optional<TokenType>()
                        + (new Repeat<TokenType>()
                            + LanguageSyntax.Comma
                            + propertyAssignment
                            )
                        )

                    + LanguageSyntax.RightBrace
                    + LanguageSyntax.SemiColon
            };

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolParserResponse<TokenType> syntaxNode = parser.Parse(commands);
            syntaxNode.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name1"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value1"),
                new SymbolToken<TokenType>(TokenType.Comma),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name2"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value2"),
                new SymbolToken<TokenType>(TokenType.RightBrace),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(syntaxNode.Nodes!, matchList).Should().BeTrue();
        }
    }
}
