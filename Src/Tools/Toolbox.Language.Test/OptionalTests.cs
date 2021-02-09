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
    public class OptionalTests
    {
        [Fact]
        public void SingleOptional_ShouldPass()
        {
            string command = "name = value;";

            var assignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TokenType>()
                + assignment

                + (new Optional<TokenType>()
                    + LanguageSyntax.Comma
                    + assignment
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolParserResponse<TokenType> response = parser.Parse(command);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "name"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "value"),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleOptional_ShouldPass()
        {
            string command = "name = value,name1=value2;";

            var assignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TokenType>()
                + assignment

                + (new Optional<TokenType>()
                    + LanguageSyntax.Comma
                    + assignment
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolParserResponse<TokenType> response = parser.Parse(command);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "name"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "value"),

                new SymbolToken<TokenType>(TokenType.Comma),

                new SymbolValue<TokenType>(TokenType.VariableName, "name1"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "value2"),

                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleOptionalRepeat_ShouldPass()
        {
            string command = "name = value,name1=value1, name2 = value2;";

            var assignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TokenType>()
                + assignment

                + (new Optional<TokenType>()
                    + (new Repeat<TokenType>()
                        + LanguageSyntax.Comma
                        + assignment
                    )
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TokenType>(processingRules);

            SymbolParserResponse<TokenType> response = parser.Parse(command);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "name"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "value"),

                new SymbolToken<TokenType>(TokenType.Comma),

                new SymbolValue<TokenType>(TokenType.VariableName, "name1"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "value1"),

                new SymbolToken<TokenType>(TokenType.Comma),

                new SymbolValue<TokenType>(TokenType.VariableName, "name2"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "value2"),

                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }
    }
}
