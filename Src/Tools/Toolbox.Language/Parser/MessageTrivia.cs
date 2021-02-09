using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.Language.Parser
{
    public record MessageTrivia : ISymbolToken
    {
        public string Message { get; init; } = null!;
    }
}
