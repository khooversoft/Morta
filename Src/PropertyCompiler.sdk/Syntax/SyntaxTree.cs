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

        public IReadOnlyList<ISyntaxNode>? SyntaxNodes => (List<ISyntaxNode>)_syntaxNodes;

        public IReadOnlyList<SyntaxResponse> SyntaxResponses => (List<SyntaxResponse>)_syntaxResponses;

        public SyntaxTree Build()
        {
        }
    }
}