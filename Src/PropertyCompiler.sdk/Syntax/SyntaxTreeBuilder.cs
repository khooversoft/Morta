using Microsoft.Extensions.Logging;
using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tokenizer.Token;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Syntax
{
    public class SyntaxTreeBuilder
    {
        private readonly List<IExpressionBuilder> _expressionBuilders = new List<IExpressionBuilder>();
        private readonly List<string> _rawDataLines = new List<string>();

        public SyntaxTreeBuilder Add(params IExpressionBuilder[] expressionBuilder) => this.Action(x => expressionBuilder.ForEach(y => x._expressionBuilders.Add(y)));

        public SyntaxTreeBuilder Add(params string[] rawData) => this.Action(x => rawData.ForEach(y => x._rawDataLines.Add(y)));

        public SyntaxTreeBuilder AddStandardBuilders()
        {
            Add(new AssemblyExpressionBuilder());
            Add(new IncludeExpressionBuilder());
            Add(new ScalarAssignmentBuilder());
            Add(new ResourceExpressionBuilder());
            Add(new ObjectExpressionBuilder());

            return this;
        }

        public SyntaxTree Build()
        {
            _expressionBuilders.VerifyAssert(x => x.Count > 0, "No expression nodes");
            _rawDataLines.VerifyAssert(x => x.Count > 0, "No raw data line(s)");

            CodeBlock<SymbolType> allRules = _expressionBuilders.Aggregate(new CodeBlock<SymbolType>(), (a, x) => a += x.ProcessingRules);
            string rawData = _rawDataLines.Aggregate(string.Empty, (a, x) => a += x);

            IReadOnlyList<IToken> tokens = new TokenParser<SymbolType>(allRules).Parse(rawData);
            SymbolParserContext context = new SymbolParserContext(tokens);

            return new SyntaxTree(context, _expressionBuilders).Build();
        }
    }
}
