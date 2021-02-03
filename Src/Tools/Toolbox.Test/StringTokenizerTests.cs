using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Tokenizer;
using Toolbox.Tokenizer.Token;
using Xunit;

namespace Toolbox.Test
{
    public class StringTokenizerTests
    {
        [Fact]
        public void BasicToken_WhenEmptyString_ShouldReturnNoTokens()
        {
            IReadOnlyList<IToken> tokens = new StringTokenizer()
                .UseCollapseWhitespace()
                .UseDoubleQuote()
                .UseSingleQuote()
                .Parse("");

            tokens.Count.Should().Be(0);
        }

        [Fact]
        public void BasicToken_WhenPadString_ShouldReturnSpaceToken()
        {
            IReadOnlyList<IToken> tokens = new StringTokenizer()
                .UseCollapseWhitespace()
                .UseDoubleQuote()
                .UseSingleQuote()
                .Parse("      ");

            tokens.Count.Should().Be(1);
            tokens[0].Value.Should().Be(" ");
        }

        [Fact]
        public void BasicToken_WhenTokenIsSpace_ShouldReturnValidTokens()
        {
            IReadOnlyList<IToken> tokens = new StringTokenizer()
                .UseCollapseWhitespace()
                .UseDoubleQuote()
                .UseSingleQuote()
                .Parse("abc def");

            var expectedTokens = new IToken[]
            {
                new TokenValue("abc", TokenType.Data),
                new TokenValue(" ", TokenType.ParseToken),
                new TokenValue("def", TokenType.Data),
            };

            tokens.Count.Should().Be(expectedTokens.Length);

            tokens
                .Zip(expectedTokens, (o, i) => (o, i))
                .All(x => x.o.Value == x.i.Value)
                .Should().BeTrue();
        }

        [Fact]
        public void BasicToken_WhenTokenIsSpaceAndPad_ShouldReturnValidTokens()
        {
            IReadOnlyList<IToken> tokens = new StringTokenizer()
                .UseCollapseWhitespace()
                .UseDoubleQuote()
                .UseSingleQuote()
                .Parse("  abc   def  ");

            var expectedTokens = new IToken[]
            {
                new TokenValue(" ", TokenType.ParseToken),
                new TokenValue("abc", TokenType.Data),
                new TokenValue(" ", TokenType.ParseToken),
                new TokenValue("def", TokenType.Data),
                new TokenValue(" ", TokenType.ParseToken),
            };

            tokens.Count.Should().Be(expectedTokens.Length);

            tokens
                .Zip(expectedTokens, (o, i) => (o, i))
                .All(x => x.o.Value == x.i.Value)
                .Should().BeTrue();
        }

        [Fact]
        public void BasicToken_WhenKnownTokenSpecified_ShouldReturnValidTokens()
        {
            IReadOnlyList<IToken> tokens = new StringTokenizer()
                .UseCollapseWhitespace()
                .UseDoubleQuote()
                .UseSingleQuote()
                .Add("[", "]")
                .Parse("  abc   [def]  ");

            var expectedTokens = new IToken[]
            {
                new TokenValue(" ", TokenType.ParseToken),
                new TokenValue("abc", TokenType.Data),
                new TokenValue(" ", TokenType.ParseToken),
                new TokenValue("[", TokenType.ParseToken),
                new TokenValue("def", TokenType.Data),
                new TokenValue("]", TokenType.ParseToken),
                new TokenValue(" ", TokenType.ParseToken),
            };

            tokens.Count.Should().Be(expectedTokens.Length);

            tokens
                .Zip(expectedTokens, (o, i) => (o, i))
                .All(x => x.o.Value == x.i.Value)
                .Should().BeTrue();
        }

        [Fact]
        public void PropertyName_WhenEscapeIsUsed_ShouldReturnValidTokens()
        {
            IReadOnlyList<IToken> tokens = new StringTokenizer()
                .Add("{", "}", "{{", "}}")
                .Parse("Escape {{firstName}} end");

            var expectedTokens = new IToken[]
            {
                new TokenValue("Escape ", TokenType.Data),
                new TokenValue("{{", TokenType.ParseToken),
                new TokenValue("firstName", TokenType.Data),
                new TokenValue("}}", TokenType.ParseToken),
                new TokenValue(" end", TokenType.Data),
            };

            tokens.Count.Should().Be(expectedTokens.Length);

            tokens
                .Zip(expectedTokens, (o, i) => (o, i))
                .All(x => x.o.Value == x.i.Value)
                .Should().BeTrue();
        }
    }
}