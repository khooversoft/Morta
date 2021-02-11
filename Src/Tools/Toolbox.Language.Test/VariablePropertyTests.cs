using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
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

            CodeBlock<TestTokenType> processingRules = new CodeBlock<TestTokenType>()
            {
                new CodeBlock<TestTokenType>()
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

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolToken<TestTokenType>(TestTokenType.DeclareObject),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
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

            CodeBlock<TestTokenType> processingRules = new CodeBlock<TestTokenType>()
            {
                new CodeBlock<TestTokenType>()
                    + LanguageSyntax.DeclareObject
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
                    + LanguageSyntax.LeftBrace
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
                    + LanguageSyntax.Constant

                    + (new Optional<TestTokenType>()
                        + (new Repeat<TestTokenType>()
                            + LanguageSyntax.Comma
                            + LanguageSyntax.VariableName
                            + LanguageSyntax.Equal
                            + LanguageSyntax.Constant
                            )
                        )

                    + LanguageSyntax.RightBrace
                    + LanguageSyntax.SemiColon
            };

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolToken<TestTokenType>(TestTokenType.DeclareObject),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value2"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
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

            var propertyAssignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            CodeBlock<TestTokenType> processingRules = new CodeBlock<TestTokenType>()
            {
                new CodeBlock<TestTokenType>()
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
                    + LanguageSyntax.LeftBrace
                    + propertyAssignment

                    + (new Optional<TestTokenType>()
                        + (new Repeat<TestTokenType>()
                            + LanguageSyntax.Comma
                            + propertyAssignment
                            )
                        )

                    + LanguageSyntax.RightBrace
                    + LanguageSyntax.SemiColon
            };

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> syntaxNode = parser.Parse(commands);
            syntaxNode.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value2"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(syntaxNode.Nodes!, matchList).Should().BeTrue();
        }
    }
}