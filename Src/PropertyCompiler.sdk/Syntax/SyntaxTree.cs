using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Syntax
{
    public class SyntaxTree
    {
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

        public IReadOnlyList<ISyntaxNode>? SyntaxNodes { get; set; }
    }
}