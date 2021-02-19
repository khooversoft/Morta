using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Tokenizer;
using Toolbox.Tokenizer.Token;
using Xunit;

namespace Toolbox.Test
{
    public class CommentMultipleLineTests
    {
        [Fact]
        public void CommentedLine_ShouldPass()
        {
            var list = new[]
            {
                "// Comment line",
                "name = value;   // comment2 line2",
                "name2 = value2;"
            };

            IReadOnlyList<IToken> tokens = new StringTokenizer()
                .UseCollapseWhitespace()
                .UseDoubleQuote()
                .UseSingleQuote()
                .UseComments()
                .Parse(list.ToArray());

            tokens.Count.Should().Be(13);

            var stack = new Stack<IToken>(tokens.Reverse());

            stack.Pop().As<CommentBlock>().Value.Should().Be("// Comment line");
            stack.Pop().As<TokenValue>().Value.Should().Be("name");
            stack.Pop().As<TokenValue>().Value.Should().Be(" ");
            stack.Pop().As<TokenValue>().Value.Should().Be("=");
            stack.Pop().As<TokenValue>().Value.Should().Be(" ");
            stack.Pop().As<TokenValue>().Value.Should().Be("value;");
            stack.Pop().As<TokenValue>().Value.Should().Be(" ");
            stack.Pop().As<CommentBlock>().Value.Should().Be("// comment2 line2");
            stack.Pop().As<TokenValue>().Value.Should().Be("name2");
            stack.Pop().As<TokenValue>().Value.Should().Be(" ");
            stack.Pop().As<TokenValue>().Value.Should().Be("=");
            stack.Pop().As<TokenValue>().Value.Should().Be(" ");
            stack.Pop().As<TokenValue>().Value.Should().Be("value2;");

            stack.Count.Should().Be(0);
        }
    }
}
