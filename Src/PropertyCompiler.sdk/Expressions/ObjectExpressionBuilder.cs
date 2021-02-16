using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{
    /// <summary>
    /// 
    /// fullName = { First = "Jeff", Last = "Roberts" }
    /// 
    /// address = {
    ///     street = "One street",
    ///     city = "City",
    ///     State = "state",
    ///     Zip = "zipCode"
    /// }
    /// 
    /// profile = {
    ///     firstName = firstName,
    ///     fullName = fullName {
    ///         middleInitial = "M"
    ///     },
    ///     address = address {
    ///         zipCode = "98033"
    ///     }
    /// }
    ///     
    /// </summary>
    public class ObjectExpressionBuilder : IExpressionBuilder
    {
        private readonly CodeBlock<SymbolType> _processingRules;

        public ObjectExpressionBuilder()
        {
            var assignment = new CodeBlock<SymbolType>("assignment")
                + SymbolSyntax.VariableName
                + SymbolSyntax.Equal
                + SymbolSyntax.Constant;

            var propertiesReference = new Reference<SymbolType>();

            var properties = new CodeBlock<SymbolType>("properties")
                + SymbolSyntax.LeftBrace

                + (new Optional<SymbolType>("properties.1")
                    + (new Repeat<SymbolType>("properties.2")
                        + SymbolSyntax.VariableName
                        + SymbolSyntax.Equal

                        + (new Choice<SymbolType>("properties.3")
                            + (new CodeBlock<SymbolType>("properties.4")
                                + SymbolSyntax.With
                                + propertiesReference
                            )

                            + (new CodeBlock<SymbolType>("properties.5")
                                + SymbolSyntax.Constant

                                + (new Optional<SymbolType>("properties.6")
                                    + SymbolSyntax.With
                                    + propertiesReference
                                )
                            )

                            + propertiesReference
                        )

                        + (new Optional<SymbolType>("properties.7")
                            + SymbolSyntax.Comma
                        )
                    )
                )

                + SymbolSyntax.RightBrace;

            propertiesReference.Set(properties);

            _processingRules = new CodeBlock<SymbolType>("rules")
                + (new Choice<SymbolType>("rules.0")
                    + (new CodeBlock<SymbolType>("rules.1")
                        + SymbolSyntax.VariableName
                        + SymbolSyntax.Equal
                        + properties
                        + SymbolSyntax.SemiColon
                    )

                    + (new CodeBlock<SymbolType>("rules.2")
                        + assignment
                        + SymbolSyntax.With
                        + properties
                        + SymbolSyntax.SemiColon
                        )
                );

        }

        public CodeBlock<SymbolType> ProcessingRules => _processingRules;

        public SyntaxResponse Create(SyntaxTree syntaxTree)
        {
            SymbolParserResponse<SymbolType> response = new SymbolParser<SymbolType>(_processingRules)
                .Parse(syntaxTree.SymbolParserContext);

            if (response.Nodes == null) return new SyntaxResponse { DebugStack = response.DebugStack };

            var stack = new Stack<ISymbolToken>(response.Nodes.Reverse<ISymbolToken>());

            SymbolValue<SymbolType>? variableName = null;
            SymbolValue<SymbolType>? constantValue = null;

            var rootStack = new Stack<ISyntaxCollection>();
            Body body = new Body();
            rootStack.Push(body);

            bool running = true;

            while (running)
            {
                ISymbolToken symbolToken = stack.GetNext();

                switch (symbolToken)
                {
                    case SymbolValue<SymbolType> variable when variable.GrammarType == SymbolType.VariableName:
                        variableName = variable;
                        break;

                    case SymbolToken<SymbolType> token when token.GrammarType == SymbolType.Equal:
                        break;

                    case SymbolValue<SymbolType> constant when constant.GrammarType == SymbolType.Constant:
                        constantValue = constant;
                        var scalar = new ScalarAssignment(variableName!, constant);
                        rootStack.Peek().Children.Add(scalar);
                        break;

                    case SymbolToken<SymbolType> token when token.GrammarType == SymbolType.LeftBrace:
                        var newRoot = new ObjectExpression(variableName!);
                        rootStack.Peek().Children.Add(newRoot);
                        rootStack.Push(newRoot);
                        break;

                    case SymbolToken<SymbolType> token when token.GrammarType == SymbolType.With:
                        var withRoot = new WithObjectExpression(constantValue!);
                        rootStack.Peek().Children.Add(withRoot);
                        rootStack.Push(withRoot);

                        stack.GetNextToken(SymbolType.LeftBrace);
                        break;

                    case SymbolToken<SymbolType> token when token.GrammarType == SymbolType.RightBrace:
                        rootStack.Pop();
                        break;

                    case SymbolToken<SymbolType> token when token.GrammarType == SymbolType.Comma:
                        break;

                    case SymbolToken<SymbolType> token when token.GrammarType == SymbolType.SemiColon:
                        running = false;
                        break;

                    default:
                        throw new ArgumentException($"Unknown symbol={symbolToken}");
                }
            }

            rootStack.Count.VerifyAssert(x => x == 1, "Unbalanced braces");

            return new SyntaxResponse
            {
                SyntaxNode = body,
                DebugStack = response.DebugStack
            };
        }
    }
}
