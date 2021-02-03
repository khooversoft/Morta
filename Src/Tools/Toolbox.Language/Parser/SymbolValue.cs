using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.Language.Parser
{
    /// <summary>
    /// Represent a token value in string format.
    /// </summary>
    public class SymbolValue<T> : ISymbolToken where T : Enum
    {
        public SymbolValue(T grammarType, string value)
        {
            GrammarType = grammarType;
            Value = value;
        }

        public SymbolValue(T grammarType, string value, bool isQuotedString)
            : this(grammarType, value)
        {
            IsQuotedString = isQuotedString;
        }

        public T GrammarType { get; }

        public string Value { get; }

        public bool IsQuotedString { get; }

        public override bool Equals(object? obj)
        {
            return obj is SymbolValue<T> subject &&
                Value == subject.Value &&
                IsQuotedString == subject.IsQuotedString;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => $"{nameof(SymbolValue<T>)}: GrammarType={GrammarType}, Value={Value}, IsQuotedString={IsQuotedString}";
    }
}
