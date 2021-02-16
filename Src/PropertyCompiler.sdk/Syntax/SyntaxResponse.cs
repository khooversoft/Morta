using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Parser;

namespace PropertyCompiler.sdk.Syntax
{
    public record SyntaxResponse
    {
        public ISyntaxNode? SyntaxNode { get; init; }

        public IReadOnlyList<IReadOnlyList<ISymbolToken>> DebugStack { get; init; } = null!;
    }
}
