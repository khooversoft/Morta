using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Language.Parser;
using Toolbox.Tokenizer.Token;

namespace Toolbox.Language.Grammar
{
    public class GrammarValue<T> : IExpression<T> where T : Enum
    {
        public GrammarValue(T grammarType) => GrammarType = grammarType;

        public T GrammarType { get; }

        public ISymbolToken CreateToken(IToken token) => new SymbolValue<T>(GrammarType, token);

        public override string ToString() => $"{nameof(GrammarValue<T>)}: {GrammarType}";
    }
}
