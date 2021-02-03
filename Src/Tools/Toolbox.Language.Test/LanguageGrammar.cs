using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Grammar;

namespace Toolbox.Language.Test
{
    public enum TokenType
    {
        Space,
        SemiColon,
        Equal,
        LeftParen,
        RightParen,
        LeftBrace,
        RightBrace,
        Comma,

        TypeName,
        VariableName,
        Constant,
        MethodName,
        MethodParameterValue,

        DeclareObject,
        With,
    }

    public static class LanguageSyntax
    {
        public static IGrammarToken<TokenType> Space = new GrammarToken<TokenType>(TokenType.Space, " ");
        public static IGrammarToken<TokenType> SemiColon = new GrammarToken<TokenType>(TokenType.SemiColon, ";", GrammarFlags.EndStatement);
        public static IGrammarToken<TokenType> Equal = new GrammarToken<TokenType>(TokenType.Equal, "=");
        public static IGrammarToken<TokenType> LeftParen = new GrammarToken<TokenType>(TokenType.LeftParen, "(", GrammarFlags.StartCodeBlock);
        public static IGrammarToken<TokenType> RightParen = new GrammarToken<TokenType>(TokenType.RightParen, ")", GrammarFlags.EndCodeBlock);
        public static IGrammarToken<TokenType> LeftBrace = new GrammarToken<TokenType>(TokenType.LeftBrace, "{", GrammarFlags.StartCodeBlock);
        public static IGrammarToken<TokenType> RightBrace = new GrammarToken<TokenType>(TokenType.RightBrace, "}", GrammarFlags.EndCodeBlock);
        public static IGrammarToken<TokenType> Comma = new GrammarToken<TokenType>(TokenType.Comma, ",");

        public static IGrammarToken<TokenType> DeclareObject = new GrammarToken<TokenType>(TokenType.DeclareObject, "declare", GrammarFlags.Keyword);
        public static IGrammarToken<TokenType> With = new GrammarToken<TokenType>(TokenType.DeclareObject, "with", GrammarFlags.Keyword);

        public static IExpression<TokenType> TypeName = new GrammarValue<TokenType>(TokenType.TypeName);
        public static IExpression<TokenType> VariableName = new GrammarValue<TokenType>(TokenType.VariableName);
        public static IExpression<TokenType> Constant = new GrammarValue<TokenType>(TokenType.Constant);
        public static IExpression<TokenType> MethodName = new GrammarValue<TokenType>(TokenType.MethodName);
        public static IExpression<TokenType> MethodParameterValue = new GrammarValue<TokenType>(TokenType.MethodParameterValue);
    }
}
