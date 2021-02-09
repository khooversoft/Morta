using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.Language.Parser
{
    public record SymbolParserResponse<T> where T : Enum
    {
        public SymbolNode<T>? Nodes { get; init; }

        public IReadOnlyList<IReadOnlyList<ISymbolToken>> DebugStack { get; init; } = Enumerable.Empty<IReadOnlyList<ISymbolToken>>().ToList();
    }
}
