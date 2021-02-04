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
    public class MultipleVariableAssignmentTests
    {
        [Fact]
        public void MultipleDepthVariableAssignment_WithComponentModel_ShouldPass()
        {
            List<string> dump = new List<string>();

            var commands = new[]
            {
                "objectName = {",
                    "Name1=Value1,",
                    "Name2 =Value2,",
                    "Name3 = Value2 with {",
                        "Name4 = Value4,",
                        "Name5 = Value5 with {",
                            "Name6 = Value6",
                    "}",
                    "Name7 = {",
                        "Name4 = Value4",
                    "}",
                "};"
            };

            var propertyValueAssignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var propertyObjectAssignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.LeftBrace;

            var propertyWithAssignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.With
                + LanguageSyntax.LeftBrace;


            var processingRulesReference = new Reference<TokenType>();

            var processingRules = new CodeBlock<TokenType>()
                + (new Choice<TokenType>()
                    + (new CodeBlock<TokenType>()
                        + propertyValueAssignment   // key = value
                        )

                    + (new CodeBlock<TokenType>()
                        + propertyObjectAssignment  // key = {
                        + propertyValueAssignment   // key = value

                        + (new Optional<TokenType>()
                            + (new Repeat<TokenType>()
                                + LanguageSyntax.Comma   // ,
                                + processingRulesReference
                                )
                        )

                        + LanguageSyntax.RightBrace  // }
                    )

                    + (new CodeBlock<TokenType>()
                        + propertyObjectAssignment   // key with {
                        + propertyWithAssignment     // key = value

                        + (new Optional<TokenType>()
                            + (new Repeat<TokenType>()
                                + LanguageSyntax.Comma   // ,
                                + processingRulesReference
                                )
                        )

                        + LanguageSyntax.RightBrace  // }
                    )
                );

            processingRulesReference.Set(processingRules);

            var parser = new SymbolParser<TokenType>(processingRules, x => dump.Add(x));

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
                new SymbolValue<TokenType>(TokenType.Constant, "Value2Name3"),
                new SymbolToken<TokenType>(TokenType.Comma),
                new SymbolValue<TokenType>(TokenType.VariableName, "Value2"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name4"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value4"),
                new SymbolToken<TokenType>(TokenType.Comma),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name5"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value5"),
                new SymbolToken<TokenType>(TokenType.RightBrace),
                new SymbolToken<TokenType>(TokenType.Comma),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name6"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value6"),
                new SymbolToken<TokenType>(TokenType.Comma),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name7"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name4"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value4"),
                new SymbolToken<TokenType>(TokenType.RightBrace),
                new SymbolToken<TokenType>(TokenType.RightBrace),
            };

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }
    }
}
