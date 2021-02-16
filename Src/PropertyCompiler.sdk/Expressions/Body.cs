using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCompiler.sdk.Expressions
{
    public class Body : ISyntaxCollection, ISyntaxNode
    {
        public IList<ISyntaxNode> Children { get; } = new List<ISyntaxNode>();

        public static Body operator +(Body left, ISyntaxNode right)
        {
            left.Children.Add(right);
            return left;
        }
    }
}
