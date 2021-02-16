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

            throw new InvalidOperationException("No value");
        }

        public static SymbolToken<SymbolType>? GetNextToken(this Stack<ISymbolToken> stack, SymbolType symbolType)
        {
            if (stack.TryPop(out ISymbolToken? symbolToken))
            {
                if (symbolToken is SymbolToken<SymbolType> token && token.GrammarType == symbolType) return token;
            }

            throw new InvalidOperationException($"{symbolType} not next token or stack empty");
        }

        public static ISymbolToken GetNext(this Stack<ISymbolToken> stack)
        {
            if (stack.TryPop(out ISymbolToken? symbolToken)) return symbolToken;

            throw new InvalidOperationException("End of stack");
        }
    }
}
