using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Parser;
using Toolbox.Tokenizer.Token;
using Toolbox.Tools;

namespace Toolbox.Language.Grammar
{
    public class GrammarToken<T> : IGrammarToken<T> where T : Enum
    {
        public GrammarToken(T grammarType, string match, GrammarFlags flags = GrammarFlags.None)
        {
            match.VerifyNotNull(nameof(match));

            GrammarType = grammarType;
            Match = match;
            Flags = flags;
        }

        public T GrammarType { get; }

        public string Match { get; }

        public GrammarFlags Flags { get; }

        public ISymbolToken CreateToken(IToken token) => new SymbolToken<T>(GrammarType, token, Flags);

        public override bool Equals(object? obj)
        {
            return obj is GrammarToken<T> subject &&
                GrammarType.Equals(subject.GrammarType) &&
                Match == subject.Match;
        }

        public override string ToString() => $"{nameof(GrammarToken<T>)}: GrammarType={GrammarType}, Match={Match}";

        public override int GetHashCode() => Match.GetHashCode();
    }
}
