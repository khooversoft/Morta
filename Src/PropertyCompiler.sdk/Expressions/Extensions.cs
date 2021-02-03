using PropertyCompiler.sdk.Grammar;
using System;
using System.Collections.Generic;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{
    public static class Extensions
    {
        public static SymbolValue<SymbolType> GetNextValue(this Stack<ISymbolToken> stack)
        {
            while (stack.TryPop(out ISymbolToken? symbolToken))
            {
                if (symbolToken is SymbolValue<SymbolType> value) return value;
            }

            throw new ArgumentException("End of stack, no symbol value");
        }
    }
}
