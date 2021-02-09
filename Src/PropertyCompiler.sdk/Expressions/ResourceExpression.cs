using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{
    /// <summary>
    /// 
    /// resource resourceId = filePath;
    ///     
    /// </summary>
    /// 
    public class ResourceExpressionBuilder : IExpressionBuilder
    {
        private static readonly CodeBlock<SymbolType> _processingRules = new CodeBlock<SymbolType>()
        {
            new CodeBlock<SymbolType>()
                + Symbols.Resource
                + Symbols.VariableName
                + Symbols.Equal
                + Symbols.Constant
                + Symbols.SemiColon
        };

        public CodeBlock<SymbolType> ProcessingRules => _processingRules;

        public SyntaxNode? Create(SyntaxTree syntaxTree)
        {
            SymbolParserResponse<SymbolType> response = new SymbolParser<SymbolType>(_processingRules)
                .Parse(syntaxTree.SymbolParserContext);

            if (response.Nodes == null) return null;

            var stack = new Stack<ISymbolToken>(response.Nodes.Reverse<ISymbolToken>());

            string resourceId = stack.GetNextValue().Value;
            string filePath = stack.GetNextValue().Value;

            return new ResourceExpression(syntaxTree, resourceId, filePath);
        }
    }

    public class ResourceExpression : SyntaxNode
    {
        public ResourceExpression(SyntaxTree syntaxTree, string resourceId, string filePath)
            : base(syntaxTree)
        {
            resourceId.VerifyNotNull(nameof(resourceId));
            filePath.VerifyNotNull(nameof(filePath));

            ResourceId = resourceId;
            FilePath = filePath;
        }

        public string ResourceId { get; }

        public string FilePath { get; }
    }
}
