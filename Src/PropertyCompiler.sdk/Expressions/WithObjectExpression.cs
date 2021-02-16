using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using Toolbox.Language.Parser;

namespace PropertyCompiler.sdk.Expressions
{
    public class WithObjectExpression : ObjectExpression, ISyntaxNode
    {
        public WithObjectExpression(SymbolValue<SymbolType> variableName)
            : base(variableName)
        {
        }
    }
}
