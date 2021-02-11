using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tokenizer.Token;

namespace Toolbox.Language.Grammar
{
    public interface IGrammarToken<T> : IGrammar<T> where T : Enum
    {
        T GrammarType { get; }

        string Match { get; }

        GrammarFlags Flags { get; }

        ISymbolToken CreateToken(IToken token);
    }
}
