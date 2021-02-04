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
    public class ChoiceTests
    {
        [Fact]
        public void AssignmentChoice_ShouldPass()
        {
            string command = "name = value;";

            var assignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var with = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.With
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TokenType>()
                + (new Choice<TokenType>()
                    + assignment
                    + with
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TokenType>(processingRules, x => { });

            SymbolNode<TokenType>? syntaxNode = parser.Parse(command);
            syntaxNode.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "name"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "value"),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }

        [Fact]
        public void WithChoice_ShouldPass()
        {
            string command = "name with value;";

            var assignment = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var with = new CodeBlock<TokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.With
                + LanguageSyntax.Constant;

            var processingRules = new CodeBlock<TokenType>()
                + (new Choice<TokenType>()
                    + assignment
                    + with
                )

                + LanguageSyntax.SemiColon;

            var parser = new SymbolParser<TokenType>(processingRules, x => { });

            SymbolNode<TokenType>? syntaxNode = parser.Parse(command);
            syntaxNode.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "name"),
                new SymbolToken<TokenType>(TokenType.With),
                new SymbolValue<TokenType>(TokenType.Constant, "value"),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
        }
    }
}
