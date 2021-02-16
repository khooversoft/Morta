using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCompiler.sdk.Syntax
{
    public class ExpressionCollection : List<ISyntaxNode>, ISyntaxNode
    {
        public ISyntaxNode Parent { get; set; } = null!;

        public ISyntaxNode? Left { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        public ISyntaxNode? Right { get; set; }
    }
}
