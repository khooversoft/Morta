using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Grammar;

namespace Toolbox.Language.Parser
{
    public class SymbolToken<T> : ISymbolToken where T : Enum
    {
        public SymbolToken(T grammarType, GrammarFlags flags = GrammarFlags.None)
        {
            GrammarType = grammarType;
            Flags = flags;
        }

        public T GrammarType { get; }

        public GrammarFlags Flags { get; }

        public override bool Equals(object? obj)
        {
            return obj is SymbolToken<T> subject &&
                GrammarType.Equals(subject.GrammarType);
        }

        public override int GetHashCode() => GrammarType.GetHashCode();

        public override string ToString() => $"{nameof(SymbolToken<T>)}: Grammar Type:{GrammarType}, Flags={Flags}";
    }
}
