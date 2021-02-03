using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolbox.Extensions;
using Toolbox.Language.Grammar;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tools;

namespace Toolbox.Parser
{
    /// <summary>
    /// Root block
    /// </summary>
    public class RuleBlock<T> : List<IRuleBlock<T>> where T : Enum
    {
        public IReadOnlyList<IGrammarToken<T>> GetGrammars() => this.Flatten<IGrammar<T>>().OfType<IGrammarToken<T>>().ToList();

        public static RuleBlock<T> operator +(RuleBlock<T> left, IRuleBlock<T> right)
        {
            left.VerifyNotNull(nameof(left));
            right.VerifyNotNull(nameof(right));

            left.Add(right);
            return left;
        }

        public static RuleBlock<T> operator +(RuleBlock<T> left, IEnumerable<IRuleBlock<T>> right)
        {
            left.VerifyNotNull(nameof(left));
            right.VerifyNotNull(nameof(right));

            right.ForEach(x => left.Add(x));
            return left;
        }
    }
}
