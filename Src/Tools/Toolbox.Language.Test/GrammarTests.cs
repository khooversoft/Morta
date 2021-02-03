using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Parser;
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
            RuleBlock<TokenType> processingRules = new RuleBlock<TokenType>()
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

            RuleBlock<TokenType> processingRules = new RuleBlock<TokenType>()
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

            RuleBlock<TokenType> processingRules = new RuleBlock<TokenType>()
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

            RuleBlock<TokenType> processingRules = new RuleBlock<TokenType>()
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
        public void DeclareVariableProperties_ShouldPass()
        {
            List<string> dump = new List<string>();

            var commands = new[]
            {
                "declare objectName = {",
                    "Name=Value",
                "};"
            };

            RuleBlock<TokenType> processingRules = new RuleBlock<TokenType>()
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

            var parser = new SymbolParser<TokenType>(processingRules, x => dump.Add(x));

            SymbolNode<TokenType>? syntaxNode = parser.Parse(commands);
            syntaxNode.Should().NotBeNull();

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

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleDeclareVariableProperties_ShouldPass()
        {
            List<string> dump = new List<string>();

            var commands = new[]
            {
                "declare objectName = {",
                    "Name1=Value1,",
                    "Name2 =Value2",
                "};"
            };

            RuleBlock<TokenType> processingRules = new RuleBlock<TokenType>()
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

            var parser = new SymbolParser<TokenType>(processingRules, x => dump.Add(x));

            SymbolNode<TokenType>? syntaxNode = parser.Parse(commands);
            syntaxNode.Should().NotBeNull();

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

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleDeclareVariableProperties_WithComponentModel_ShouldPass()
        {
            List<string> dump = new List<string>();

            var commands = new[]
            {
                "declare objectName = {",
                    "Name1=Value1,",
                    "Name2 =Value2",
                "};"
            };

            var propertyAssignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            RuleBlock<TokenType> processingRules = new RuleBlock<TokenType>()
            {
                new CodeBlock<TokenType>()
                    + LanguageSyntax.DeclareObject
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

            var parser = new SymbolParser<TokenType>(processingRules, x => dump.Add(x));

            SymbolNode<TokenType>? syntaxNode = parser.Parse(commands);
            syntaxNode.Should().NotBeNull();

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

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleVariableAssignment_WithComponentModel_ShouldPass()
        {
            List<string> dump = new List<string>();

            var commands = new[]
            {
                "objectName = {",
                    "Name1=Value1,",
                    "Name2 =Value2",
                    "Name3 = Value2 with {",
                        "Name4 = Value4,",
                    "}",
                "};"
            };

            var propertyAssignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;
                //+ (nNne)

            var propertyCollectionAssignment = new CodeBlock<TokenType>()
                + LanguageSyntax.LeftBrace
                + propertyAssignment

                + (new Optional<TokenType>()
                    + (new Repeat<TokenType>()
                        + LanguageSyntax.Comma
                        + propertyAssignment
                        )
                    )

                + LanguageSyntax.RightBrace
                + LanguageSyntax.SemiColon;

            var withPropertyAssignment = new CodeBlock<TokenType>()
                + LanguageSyntax.With
                + propertyCollectionAssignment;

            RuleBlock<TokenType> processingRules = new RuleBlock<TokenType>()
            {
                new CodeBlock<TokenType>()
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
                    + propertyCollectionAssignment
            };

            var parser = new SymbolParser<TokenType>(processingRules, x => dump.Add(x));

            SymbolNode<TokenType>? syntaxNode = parser.Parse(commands);
            syntaxNode.Should().NotBeNull();

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

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }
    }
}