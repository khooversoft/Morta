using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Syntax
{
    public class SyntaxTree
    {
        public SyntaxTree(SymbolParserContext symbolParserContext, IEnumerable<IExpressionBuilder> expressionBuilders, Action<string> logger)
        {
            symbolParserContext.VerifyNotNull(nameof(symbolParserContext));
            logger.VerifyNotNull(nameof(logger));

            expressionBuilders
                .VerifyNotNull(nameof(expressionBuilders))
                .VerifyAssert(x => x.Any(), $"{nameof(expressionBuilders)} is empty");

            ExpressionBuilders = expressionBuilders.ToList();
            SymbolParserContext = symbolParserContext;
            Logger = logger;
        }

        public IReadOnlyList<IExpressionBuilder> ExpressionBuilders { get; }

        public Action<string> Logger { get; }

        public SymbolParserContext SymbolParserContext { get; }
    }
}