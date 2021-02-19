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
        public void BasicTokenWithQuotedString_ShouldReturnValidTokens()
        {
            IReadOnlyList<IToken> tokens = new StringTokenizer()
                .UseCollapseWhitespace()
                .UseDoubleQuote()
                .UseSingleQuote()
                .Parse("abc \"def\"");

            var expectedTokens = new IToken[]
            {
                new TokenValue("abc", TokenType.Data, new TextSpan(0, 3)),
                new TokenValue(" ", TokenType.ParseToken, new TextSpan(3, 1)),
                new BlockToken("\"def\"", new TextSpan(4, 5)),
            };

            tokens.Count.Should().Be(expectedTokens.Length);

            tokens
                .Zip(expectedTokens, (o, i) => (o, i))
                .All(x => x.o.Equals(x.i))
                .Should().BeTrue();
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
                new TokenValue("abc", TokenType.Data, new TextSpan(0, 3)),
                new TokenValue(" ", TokenType.ParseToken, new TextSpan(3, 1)),
                new TokenValue("def", TokenType.Data, new TextSpan(4, 3)),
            };

            tokens.Count.Should().Be(expectedTokens.Length);

            tokens
                .Zip(expectedTokens, (o, i) => (o, i))
                .All(x => x.o.Equals(x.i))
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
                new TokenValue(" ", TokenType.ParseToken, new TextSpan(0, 2)),
                new TokenValue("abc", TokenType.Data, new TextSpan(2, 3)),
                new TokenValue(" ", TokenType.ParseToken, new TextSpan(5, 3)),
                new TokenValue("def", TokenType.Data, new TextSpan(8, 3)),
                new TokenValue(" ", TokenType.ParseToken, new TextSpan(11, 2)),
            };

            tokens.Count.Should().Be(expectedTokens.Length);

            tokens
                .Zip(expectedTokens, (o, i) => (o, i))
                .All(x => x.o.Equals(x.i))
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
                new TokenValue(" ", TokenType.ParseToken, new TextSpan(0, 2)),
                new TokenValue("abc", TokenType.Data, new TextSpan(2, 3)),
                new TokenValue(" ", TokenType.ParseToken, new TextSpan(5, 3)),
                new TokenValue("[", TokenType.ParseToken, new TextSpan(8, 1)),
                new TokenValue("def", TokenType.Data, new TextSpan(9, 3)),
                new TokenValue("]", TokenType.ParseToken, new TextSpan(12, 1)),
                new TokenValue(" ", TokenType.ParseToken, new TextSpan(13, 2)),
            };

            tokens.Count.Should().Be(expectedTokens.Length);

            tokens
                .Zip(expectedTokens, (o, i) => (o, i))
                .All(x => x.o.Equals(x.i))
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
                new TokenValue("Escape ", TokenType.Data, new TextSpan(0, 7)),
                new TokenValue("{{", TokenType.ParseToken, new TextSpan(7, 2)),
                new TokenValue("firstName", TokenType.Data, new TextSpan(9, 9)),
                new TokenValue("}}", TokenType.ParseToken, new TextSpan(18, 2)),
                new TokenValue(" end", TokenType.Data, new TextSpan(20, 4)),
            };

            tokens.Count.Should().Be(expectedTokens.Length);

            tokens
                .Zip(expectedTokens, (o, i) => (o, i))
                .All(x => x.o.Equals(x.i))
                .Should().BeTrue();
        }
    }
}