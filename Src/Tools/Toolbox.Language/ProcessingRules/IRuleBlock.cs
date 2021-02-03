using System;
using System.Collections.Generic;
using Toolbox.Language.Grammar;
using Toolbox.Language.Parser;

namespace Toolbox.Language.ProcessingRules
{
    public interface IRuleBlock<T> : IEnumerable<IGrammar<T>>, IGrammar<T> where T : Enum
    {
        SymbolNode<T>? Build(SymbolParserContext context);
    }
}
