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
    public class PropertyTests
    {
        [Fact]
        public void SimpleProperty_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1 = Value1",
                "};"
            };

            var assignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var createObject = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.LeftBrace;

            var withObject = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.LeftBrace;

            var processingRules = new CodeBlock<TokenType>()
                + (new Choice<TokenType>()
                    + (new CodeBlock<TokenType>()
                        + createObject
                        + assignment

                        + (new Optional<TokenType>()
                            + (new Repeat<TokenType>()
                                + LanguageSyntax.Comma
                                + assignment
                                )
                            )
                        )

                    + (new CodeBlock<TokenType>()
                        + withObject
                        + assignment

                        + (new Optional<TokenType>()
                            + (new Repeat<TokenType>()
                                + LanguageSyntax.Comma
                                + assignment
                                )
                            )
                        )
                    )

                + LanguageSyntax.RightBrace
                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TokenType>(processingRules, x => { });

            SymbolNode<TokenType>? syntaxNode = parser.Parse(commands);
            syntaxNode.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name1"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value1"),
                new SymbolToken<TokenType>(TokenType.RightBrace),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }

        [Fact]
        public void DoubleProperty_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1 = Value1,",
                    "Name2=Value2",
                "};"
            };

            var assignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.LeftBrace
                + assignment

                + (new Optional<TokenType>()
                    + (new Repeat<TokenType>()
                        + LanguageSyntax.Comma
                        + assignment
                    )
                )

                + LanguageSyntax.RightBrace
                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TokenType>(processingRules, x => { });

            SymbolNode<TokenType>? syntaxNode = parser.Parse(commands);
            syntaxNode.Should().NotBeNull();

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

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }
    }
}
