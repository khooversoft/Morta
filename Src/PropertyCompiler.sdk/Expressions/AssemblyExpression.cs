using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{

    public class AssemblyExpression : ISyntaxNode
    {
        public AssemblyExpression(SymbolValue<SymbolType> assemblyPath)
        {
            assemblyPath.VerifyNotNull(nameof(assemblyPath));

            AssemblyPath = assemblyPath;
        }

        public SymbolValue<SymbolType> AssemblyPath { get; }
    }
}
