using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
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

            var propertyValueAssignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var propertyObjectAssignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.LeftBrace;

            var propertyWithAssignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.With
                + LanguageSyntax.LeftBrace;

            var processingRulesReference = new Reference<TestTokenType>();

            var processingRules = new CodeBlock<TestTokenType>()
                + (new Choice<TestTokenType>()
                    + (new CodeBlock<TestTokenType>()
                        + propertyValueAssignment   // key = value
                        )

                    + (new CodeBlock<TestTokenType>()
                        + propertyObjectAssignment  // key = {
                        + propertyValueAssignment   // key = value

                        + (new Optional<TestTokenType>()
                            + (new Repeat<TestTokenType>()
                                + LanguageSyntax.Comma   // ,
                                + processingRulesReference
                                )
                        )

                        + LanguageSyntax.RightBrace  // }
                    )

                    + (new CodeBlock<TestTokenType>()
                        + propertyObjectAssignment   // key with {
                        + propertyWithAssignment     // key = value

                        + (new Optional<TestTokenType>()
                            + (new Repeat<TestTokenType>()
                                + LanguageSyntax.Comma   // ,
                                + processingRulesReference
                                )
                        )

                        + LanguageSyntax.RightBrace  // }
                    )
                );

            processingRulesReference.Set(processingRules);

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

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
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value2Name3"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Value2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name4"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value4"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name5"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value5"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name6"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value6"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name7"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name4"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value4"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }
    }
}