using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{

    public class IncludeExpression : ISyntaxNode
    {
        public IncludeExpression(SymbolValue<SymbolType> includePath)
        {
            includePath.VerifyNotNull(nameof(includePath));

            IncludePath = includePath;
        }

        public SymbolValue<SymbolType> IncludePath { get; }
    }
}
