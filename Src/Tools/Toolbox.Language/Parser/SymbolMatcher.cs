using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Grammar;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tokenizer.Token;

namespace Toolbox.Language.Parser
{
    public class SymbolMatcher<T> where T : Enum
    {
        public SymbolNode<T>? Build(SymbolParserContext context, IRuleBlock<T> grammars)
        {
            bool passed = false;
            SymbolNode<T> syntaxNode = new SymbolNode<T>();

            context.InputTokens.SaveCursor();

            try
            {
                foreach (IGrammar<T> item in grammars)
                {
                    if (!context.InputTokens.TryNext(out IToken? token)) return DumpSyntaxNode("End of input tokens");

                    switch (item)
                    {
                        case IExpression<T> expression:
                            if (token.TokenType != TokenType.Data) return DumpSyntaxNode($"ERR: not expression, expression={expression}");

                            syntaxNode.Add(expression.CreateToken(token.Value));
                            break;

                        case IGrammarToken<T> grammar:
                            syntaxNode.Add(grammar.CreateToken());
                            break;

                        case IRuleBlock<T> ruleBlock:
                            context.InputTokens.Cursor--;

                            SymbolNode<T>? result = ruleBlock.Build(context);
                            if (result == null) return DumpSyntaxNode($"ERR: Rule block failed, ruleBlock={ruleBlock}");

                            syntaxNode += result;
                            break;

                        default:
                            throw new InvalidOperationException("Unknown rule");
                    }
                }

                passed = true;
            }
            finally
            {
                if (!passed)
                    context.InputTokens.RestoreCursor();
                else
                    context.InputTokens.AbandonSavedCursor();
            }

            return new SymbolNode<T>() + syntaxNode;

            SymbolNode<T>? DumpSyntaxNode(string reason)
            {
                syntaxNode
                    .Select(x => x.ToString() ?? "<none>")
                    .Prepend("Failed to match")
                    .Append($"Failed because of {reason}")
                    .ForEach(x => context.Logger(x));

                return null;
            }

        }
    }
}
