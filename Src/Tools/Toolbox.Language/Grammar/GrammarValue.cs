using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Language.Parser;

namespace Toolbox.Language.Grammar
{
    public class GrammarValue<T> : IExpression<T> where T : Enum
    {
        public GrammarValue(T grammarType) => GrammarType = grammarType;

        public T GrammarType { get; }

        public ISymbolToken CreateToken(string value) => new SymbolValue<T>(GrammarType, value);

        public override string ToString() => $"{nameof(GrammarValue<T>)}: {GrammarType}";
    }
}
