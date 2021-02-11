using FluentAssertions;
using System.Linq;
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

            var assignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TestTokenType>()
                + assignment

                + (new Optional<TestTokenType>()
                    + LanguageSyntax.Comma
                    + assignment
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(command);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "name"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "value"),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleOptional_ShouldPass()
        {
            string command = "name = value,name1=value2;";

            var assignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TestTokenType>()
                + assignment

                + (new Optional<TestTokenType>()
                    + LanguageSyntax.Comma
                    + assignment
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(command);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "name"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "value"),

                new SymbolToken<TestTokenType>(TestTokenType.Comma),

                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "value2"),

                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleOptionalRepeat_ShouldPass()
        {
            string command = "name = value,name1=value1, name2 = value2;";

            var assignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TestTokenType>()
                + assignment

                + (new Optional<TestTokenType>()
                    + (new Repeat<TestTokenType>()
                        + LanguageSyntax.Comma
                        + assignment
                    )
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(command);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "name"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "value"),

                new SymbolToken<TestTokenType>(TestTokenType.Comma),

                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "value1"),

                new SymbolToken<TestTokenType>(TestTokenType.Comma),

                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "value2"),

                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }
    }
}