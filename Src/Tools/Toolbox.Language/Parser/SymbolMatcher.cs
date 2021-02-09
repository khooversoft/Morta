using Microsoft.Extensions.Logging;
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
        public SymbolNode<T>? Build(SymbolParserContext context, ICodeBlock<T> grammars)
        {
            bool passed = false;
            SymbolNode<T> syntaxNode = new SymbolNode<T>();

            context.InputTokens.SaveCursor();

            try
            {
                foreach (IGrammar<T> item in grammars)
                {
                    if (!context.InputTokens.TryNext(out IToken? token)) return PushToDebugStack("Out of input tokens");

                    switch (item)
                    {
                        case IExpression<T> expression:
                            if (token.TokenType != TokenType.Data) return PushToDebugStack($"{expression} is not TokenType.Data");

                            syntaxNode.Add(expression.CreateToken(token.Value));
                            break;

                        case IGrammarToken<T> grammar:
                            if (token.Value != grammar.Match) return PushToDebugStack($"{grammar} does not match token={token.Value}");

                            syntaxNode.Add(grammar.CreateToken());
                            break;

                        case ICodeBlock<T> ruleBlock:
                            context.InputTokens.Cursor--;

                            SymbolNode<T>? result = ruleBlock.Build(context);
                            if (result == null) return PushToDebugStack("Rule block returned empty");

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

            SymbolNode<T>? PushToDebugStack(string reason)
            {
                syntaxNode.Add(new MessageTrivia { Message = reason });

                context.DebugStack.Push(syntaxNode);
                return null;
            }
        }
    }
}
