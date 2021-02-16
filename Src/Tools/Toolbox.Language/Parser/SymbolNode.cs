using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.Language.Parser
{
    public class SymbolNode<T> : List<ISymbolToken>, ISymbolToken where T : Enum
    {
        public static SymbolNode<T> operator +(SymbolNode<T> left, SymbolNode<T> right)
        {
            right.ForEach(x => left.Add(x));
            return left;
        }

        public static SymbolNode<T> operator +(SymbolNode<T> left, ISymbolToken right)
        {
            left.Add(right);
            return left;
        }
    }
}
