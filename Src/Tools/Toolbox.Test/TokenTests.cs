using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Tokenizer.Token;
using Xunit;

namespace Toolbox.Test
{
    public class TokenTests
    {
        [Theory]
        [InlineData(0, 3)]
        [InlineData(4, 2)]
        public void TextSpanTests_ShouldPass(int start, int length)
        {
            var x1 = new TextSpan(start, length);
            var x2 = new TextSpan(start, length);

            x1.Start.Should().Be(start);
            x1.Length.Should().Be(length);
            x1.IsEmpty.Should().BeFalse();
            x1.End.Should().Be(start + length);

            x2.Start.Should().Be(start);
            x2.Length.Should().Be(length);
            x2.IsEmpty.Should().BeFalse();
            x2.End.Should().Be(start + length);

            (x1 == x2).Should().BeTrue();
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(-5, 2)]
        public void InvalidSpanTests_ShouldFail(int start, int length)
        {
            Action act = () => new TextSpan(start, length);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void BlockTokenEqual_ShouldPass()
        {
            BlockToken token1 = new BlockToken("test", new TextSpan(0, 4));
            BlockToken token2 = new BlockToken("test", new TextSpan(0, 4));

            (token1 == token2).Should().BeTrue();

            BlockToken token3 = new BlockToken("tesxt", new TextSpan(0, 4));
            (token1 == token3).Should().BeFalse();

            BlockToken token4 = new BlockToken("test", new TextSpan(1, 4));
            (token1 == token4).Should().BeFalse();
        }

        [Fact]
        public void BlockTokenEquals_ShouldPass()
        {
            IToken token1 = new BlockToken("test", new TextSpan(0, 4));
            IToken token2 = new BlockToken("test", new TextSpan(0, 4));

            token1.Equals(token2).Should().BeTrue();

            IToken token3 = new BlockToken("tesxt", new TextSpan(0, 4));
            token1.Equals(token3).Should().BeFalse();

            IToken token4 = new BlockToken("test", new TextSpan(1, 4));
            token1.Equals(token4).Should().BeFalse();
        }

        [Fact]
        public void TokenValueEqual_ShouldPass()
        {
            TokenValue token1 = new TokenValue("test", TokenType.Data, new TextSpan(0, 4));
            TokenValue token2 = new TokenValue("test", TokenType.Data, new TextSpan(0, 4));

            (token1 == token2).Should().BeTrue();

            TokenValue token3 = new TokenValue("testx", TokenType.Data, new TextSpan(0, 4));
            (token1 == token3).Should().BeFalse();

            TokenValue token4 = new TokenValue("test", TokenType.Data, new TextSpan(1, 4));
            token1.Equals(token4).Should().BeFalse();
        }

        [Fact]
        public void TokenValueEquals_ShouldPass()
        {
            IToken token1 = new TokenValue("test", TokenType.Data, new TextSpan(0, 4));
            IToken token2 = new TokenValue("test", TokenType.Data, new TextSpan(0, 4));

            token1.Equals(token2).Should().BeTrue();

            IToken token3 = new TokenValue("testx", TokenType.Data, new TextSpan(0, 4));
            token1.Equals(token3).Should().BeFalse();

            IToken token4 = new TokenValue("test", TokenType.Data, new TextSpan(1, 4));
            token1.Equals(token4).Should().BeFalse();
        }

        [Fact]
        public void BlockToken_WhenStartDoesNotMatchEndSignal_ShouldFail()
        {
            Action act = () => new BlockToken("testx", new TextSpan(0, 4));
            act.Should().Throw<ArgumentException>();
        }
    }
}
