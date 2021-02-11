using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Grammar;

namespace Toolbox.Language.Test
{
    public enum TestTokenType
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
        Data,
    }

    public static class LanguageSyntax
    {
        public static IGrammarToken<TestTokenType> Space = new GrammarToken<TestTokenType>(TestTokenType.Space, " ");
        public static IGrammarToken<TestTokenType> SemiColon = new GrammarToken<TestTokenType>(TestTokenType.SemiColon, ";", GrammarFlags.EndStatement);
        public static IGrammarToken<TestTokenType> Equal = new GrammarToken<TestTokenType>(TestTokenType.Equal, "=");
        public static IGrammarToken<TestTokenType> LeftParen = new GrammarToken<TestTokenType>(TestTokenType.LeftParen, "(", GrammarFlags.StartCodeBlock);
        public static IGrammarToken<TestTokenType> RightParen = new GrammarToken<TestTokenType>(TestTokenType.RightParen, ")", GrammarFlags.EndCodeBlock);
        public static IGrammarToken<TestTokenType> LeftBrace = new GrammarToken<TestTokenType>(TestTokenType.LeftBrace, "{", GrammarFlags.StartCodeBlock);
        public static IGrammarToken<TestTokenType> RightBrace = new GrammarToken<TestTokenType>(TestTokenType.RightBrace, "}", GrammarFlags.EndCodeBlock);
        public static IGrammarToken<TestTokenType> Comma = new GrammarToken<TestTokenType>(TestTokenType.Comma, ",");

        public static IGrammarToken<TestTokenType> DeclareObject = new GrammarToken<TestTokenType>(TestTokenType.DeclareObject, "declare", GrammarFlags.Keyword);
        public static IGrammarToken<TestTokenType> With = new GrammarToken<TestTokenType>(TestTokenType.With, "with", GrammarFlags.Keyword);

        public static IExpression<TestTokenType> TypeName = new GrammarValue<TestTokenType>(TestTokenType.TypeName);
        public static IExpression<TestTokenType> VariableName = new GrammarValue<TestTokenType>(TestTokenType.VariableName);
        public static IExpression<TestTokenType> Constant = new GrammarValue<TestTokenType>(TestTokenType.Constant);
        public static IExpression<TestTokenType> MethodName = new GrammarValue<TestTokenType>(TestTokenType.MethodName);
        public static IExpression<TestTokenType> MethodParameterValue = new GrammarValue<TestTokenType>(TestTokenType.MethodParameterValue);
    }
}
