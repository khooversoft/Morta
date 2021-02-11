using FluentAssertions;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Xunit;

namespace Toolbox.Language.Test
{
    public class ChoiceTests
    {
        [Fact]
        public void AssignmentChoice_ShouldPass()
        {
            string command = "name = value;";

            var assignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var with = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.With
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TestTokenType>()
                + (new Choice<TestTokenType>()
                    + assignment
                    + with
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> syntaxNode = parser.Parse(command);
            syntaxNode.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "name"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "value"),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(syntaxNode.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void WithChoice_ShouldPass()
        {
            string command = "name with value;";

            var assignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var with = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.With
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TestTokenType>()
                + (new Choice<TestTokenType>()
                    + assignment
                    + with
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(command);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "name"),
                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "value"),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }
    }
}