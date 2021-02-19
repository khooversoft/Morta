using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Syntax
{
    public class SyntaxTree
    {
        private readonly IList<ISyntaxNode> _syntaxNodes = new List<ISyntaxNode>();
        private readonly IList<SyntaxResponse> _syntaxResponses = new List<SyntaxResponse>();
        private bool _error;

        public SyntaxTree(SymbolParserContext symbolParserContext, IEnumerable<IExpressionBuilder> expressionBuilders)
        {
            symbolParserContext.VerifyNotNull(nameof(symbolParserContext));

            expressionBuilders
                .VerifyNotNull(nameof(expressionBuilders))
                .VerifyAssert(x => x.Any(), $"{nameof(expressionBuilders)} is empty");

            ExpressionBuilders = expressionBuilders.ToList();
            SymbolParserContext = symbolParserContext;
        }

        public IReadOnlyList<IExpressionBuilder> ExpressionBuilders { get; }

        public SymbolParserContext SymbolParserContext { get; }

        public IReadOnlyList<ISyntaxNode> SyntaxNodes => (List<ISyntaxNode>)_syntaxNodes;

        public IReadOnlyList<SyntaxResponse> SyntaxResponses => (List<SyntaxResponse>)_syntaxResponses;

        public bool IsError => _error;

        public SyntaxTree Build()
        {
            SymbolParserContext.DebugStack.Clear();
            SymbolParserContext.InputTokens.Cursor = 0;

            var errorResponses = new List<SyntaxResponse>();
            _error = false;

            while (!SymbolParserContext.InputTokens.EndOfList)
            {
                bool found = false;
                errorResponses.Clear();

                foreach (var builder in ExpressionBuilders)
                {
                    SyntaxResponse syntaxResponse = builder.Create(this);
                    errorResponses.Add(syntaxResponse);

                    if (syntaxResponse.SyntaxNode == null) continue;

                    _syntaxNodes.Add(syntaxResponse.SyntaxNode);
                    found = true;
                    break;
                }

                if( !found )
                {
                    errorResponses.ForEach(x => _syntaxResponses.Add(x));
                    _error = true;
                    return this;
                }
            }

            return this;
        }
    }
}