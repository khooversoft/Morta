using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Grammar;
using Toolbox.Tokenizer.Token;

namespace Toolbox.Language.Parser
{
    public class SymbolToken<T> : ISymbolToken, IEquatable<SymbolToken<T>?> where T : Enum
    {
        public SymbolToken(T grammarType, GrammarFlags flags = GrammarFlags.None)
        {
            GrammarType = grammarType;
            Flags = flags;
        }

        public SymbolToken(T grammarType, IToken token, GrammarFlags flags = GrammarFlags.None)
        {
            GrammarType = grammarType;
            Token = token;
            Flags = flags;
        }

        public T GrammarType { get; }

        public IToken? Token { get; }

        public GrammarFlags Flags { get; }

        public override string ToString() => $"{nameof(SymbolToken<T>)}: Grammar Type:{GrammarType}, Flags={Flags}, {Token}";

        public override bool Equals(object? obj) => Equals(obj as SymbolToken<T>);

        public bool Equals(SymbolToken<T>? subject) =>
            subject != null &&
            EqualityComparer<T>.Default.Equals(GrammarType, subject.GrammarType);

        public override int GetHashCode() => HashCode.Combine(GrammarType);

        public static bool operator ==(SymbolToken<T>? left, SymbolToken<T>? right) => EqualityComparer<SymbolToken<T>>.Default.Equals(left, right);

        public static bool operator !=(SymbolToken<T>? left, SymbolToken<T>? right) => !(left == right);
    }
}
