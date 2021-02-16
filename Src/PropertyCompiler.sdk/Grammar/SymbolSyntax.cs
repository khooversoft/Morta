using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Grammar;

namespace PropertyCompiler.sdk.Grammar
{
    public static class SymbolSyntax
    {
        public static IGrammarToken<SymbolType> Space = new GrammarToken<SymbolType>(SymbolType.Space, " ");
        public static IGrammarToken<SymbolType> SemiColon = new GrammarToken<SymbolType>(SymbolType.SemiColon, ";", GrammarFlags.EndStatement);
        public static IGrammarToken<SymbolType> Equal = new GrammarToken<SymbolType>(SymbolType.Equal, "=");
        public static IGrammarToken<SymbolType> LeftParen = new GrammarToken<SymbolType>(SymbolType.LeftParen, "(", GrammarFlags.StartCodeBlock);
        public static IGrammarToken<SymbolType> RightParen = new GrammarToken<SymbolType>(SymbolType.RightParen, ")", GrammarFlags.EndCodeBlock);
        public static IGrammarToken<SymbolType> LeftBrace = new GrammarToken<SymbolType>(SymbolType.LeftBrace, "{", GrammarFlags.StartCodeBlock);
        public static IGrammarToken<SymbolType> RightBrace = new GrammarToken<SymbolType>(SymbolType.RightBrace, "}", GrammarFlags.EndCodeBlock);
        public static IGrammarToken<SymbolType> Comma = new GrammarToken<SymbolType>(SymbolType.Comma, ",");

        public static IGrammarToken<SymbolType> With = new GrammarToken<SymbolType>(SymbolType.With, "with", GrammarFlags.Keyword);
        public static IGrammarToken<SymbolType> Assembly = new GrammarToken<SymbolType>(SymbolType.Assembly, "assembly", GrammarFlags.Keyword);
        public static IGrammarToken<SymbolType> Include = new GrammarToken<SymbolType>(SymbolType.Include, "include", GrammarFlags.Keyword);
        public static IGrammarToken<SymbolType> Resource = new GrammarToken<SymbolType>(SymbolType.Resource, "resource", GrammarFlags.Keyword);

        public static IExpression<SymbolType> TypeName = new GrammarValue<SymbolType>(SymbolType.TypeName);
        public static IExpression<SymbolType> VariableName = new GrammarValue<SymbolType>(SymbolType.VariableName);
        public static IExpression<SymbolType> Constant = new GrammarValue<SymbolType>(SymbolType.Constant);
        public static IExpression<SymbolType> MethodName = new GrammarValue<SymbolType>(SymbolType.MethodName);
        public static IExpression<SymbolType> MethodParameterValue = new GrammarValue<SymbolType>(SymbolType.MethodParameterValue);
    }
}
