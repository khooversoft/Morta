﻿using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{

    public class ObjectExpression : ISyntaxCollection, ISyntaxNode
    {
        public ObjectExpression(SymbolValue<SymbolType> variableName)
        {
            variableName.VerifyNotNull(nameof(variableName));

            VariableName = variableName;
        }

        public SymbolValue<SymbolType> VariableName { get; }

        public IList<ISyntaxNode> Children { get; } = new List<ISyntaxNode>();
    }
}
