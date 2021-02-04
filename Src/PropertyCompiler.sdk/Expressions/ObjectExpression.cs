using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{
    /// <summary>
    /// 
    /// fullName = { First = "Jeff", Last = "Roberts" }
    /// 
    /// address = {
    ///     street = "One street",
    ///     city = "City",
    ///     State = "state",
    ///     Zip = "zipCode"
    /// }
    /// 
    /// profile = {
    ///     firstName = firstName,
    ///     fullName = fullName {
    ///         middleInitial = "M"
    ///     },
    ///     address = address {
    ///         zipCode = "98033"
    ///     }
    /// }
    ///     
    /// </summary>
    public class ObjectExpression : SyntaxNode
    {
        private static readonly CodeBlock<SymbolType> _propertyAssignment = new CodeBlock<SymbolType>()
            + Symbols.VariableName
            + Symbols.Equal
            + Symbols.Constant;

        private static readonly RuleBlock<SymbolType> _processingRules = new RuleBlock<SymbolType>()
        {
            new CodeBlock<SymbolType>()
                + Symbols.VariableName
                + Symbols.Equal
                + Symbols.LeftBrace
                + Symbols.VariableName
                + Symbols.Equal
                + Symbols.Constant

                + (new Optional<SymbolType>()
                    + (new Repeat<SymbolType>()
                        + Symbols.Comma
                        + Symbols.VariableName
                        + Symbols.Equal
                        + Symbols.Constant
                        )
                    )

                + Symbols.RightBrace
                + Symbols.SemiColon
        };

        public ObjectExpression(SyntaxTree syntaxTree, SymbolToken<SymbolType> variableName)
            : base(syntaxTree)
        {
            variableName.VerifyNotNull(nameof(variableName));

            VariableName = variableName;
        }

        public SymbolToken<SymbolType> VariableName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="parentNode"></param>
        /// <param name="syntaxTokens"></param>
        /// <returns></returns>
        public static ObjectExpression? Create(SyntaxTree syntaxTree)
        {

            return null;
        }
    }
}
