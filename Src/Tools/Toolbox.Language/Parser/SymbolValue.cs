using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Tokenizer.Token;

namespace Toolbox.Language.Parser
{
    /// <summary>
    /// Represent a token value in string format.
    /// </summary>
    public class SymbolValue<T> : ISymbolToken, IEquatable<SymbolValue<T>?> where T : Enum
    {
        public SymbolValue(T grammarType, string value)
        {
            GrammarType = grammarType;
            Value = value;
        }

        public SymbolValue(T grammarType, IToken token)
        {
            GrammarType = grammarType;
            Token = token;
            Value = token.Value;
        }

        public SymbolValue(T grammarType, IToken token, bool isQuotedString)
            : this(grammarType, token)
        {
            IsQuotedString = isQuotedString;
        }

        public T GrammarType { get; }

        public IToken? Token { get; }

        public string Value { get; }

        public bool IsQuotedString { get; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SymbolValue<T>);
        }

        public bool Equals(SymbolValue<T>? subject) =>
            subject != null &&
            EqualityComparer<T>.Default.Equals(GrammarType, subject.GrammarType) &&
            Value == subject.Value &&
            IsQuotedString == subject.IsQuotedString;

        public override int GetHashCode() => HashCode.Combine(GrammarType, Value, IsQuotedString);

        public override string ToString() => $"{nameof(SymbolValue<T>)}: GrammarType={GrammarType}, Value={Value}, IsQuotedString={IsQuotedString}";

        public static bool operator ==(SymbolValue<T>? left, SymbolValue<T>? right) => EqualityComparer<SymbolValue<T>>.Default.Equals(left, right);

        public static bool operator !=(SymbolValue<T>? left, SymbolValue<T>? right) => !(left == right);
    }
}
