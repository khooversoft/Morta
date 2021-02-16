using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{
    public class ScalarAssignment : ISyntaxNode
    {
        public ScalarAssignment(SymbolValue<SymbolType> variableName, SymbolValue<SymbolType> constant)
        {
            variableName.VerifyNotNull(nameof(variableName));
            constant.VerifyNotNull(nameof(constant));

            VariableName = variableName;
            Constant = constant;
        }

        public SymbolValue<SymbolType> VariableName { get; }

        public SymbolValue<SymbolType> Constant { get; }
    }
}
