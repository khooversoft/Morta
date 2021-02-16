using System;
using System.Collections.Generic;
using Toolbox.Language.Grammar;
using Toolbox.Language.Parser;

namespace Toolbox.Language.ProcessingRules
{
    public interface ICodeBlock<T> : IEnumerable<IGrammar<T>>, IGrammar<T> where T : Enum
    {
        string? Name { get; }

        SymbolNode<T>? Build(SymbolParserContext context);
    }
}
