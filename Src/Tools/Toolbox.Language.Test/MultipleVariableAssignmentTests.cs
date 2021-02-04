//using FluentAssertions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Toolbox.Language.Parser;
//using Toolbox.Language.ProcessingRules;
//using Xunit;

//namespace Toolbox.Language.Test
//{
//    public class MultipleVariableAssignmentTests
//    {
//        [Fact]
//        public void MultipleDepthVariableAssignment_WithComponentModel_ShouldPass()
//        {
//            List<string> dump = new List<string>();

//            var commands = new[]
//            {
//                "objectName = {",
//                    "Name1=Value1,",
//                    "Name2 =Value2",
//                    "Name3 = Value2 with {",
//                        "Name4 = Value4,",
//                        "Name5 = Value5 with {",
//                            "Name6 = Value6",
//                    "}",
//                    "Name7 = {",
//                        "Name4 = Value4",
//                    "}",
//                "};"
//            };

//            var propertyValueAssignment = new CodeBlock<TokenType>()
//                + LanguageSyntax.VariableName
//                + LanguageSyntax.Equal
//                + LanguageSyntax.Constant;

//            var propertyObjectAssignment = new CodeBlock<TokenType>()
//                + LanguageSyntax.VariableName
//                + LanguageSyntax.Equal
//                + LanguageSyntax.LeftBrace;

//            var propertyWithAssignment = new CodeBlock<TokenType>()
//                + LanguageSyntax.VariableName
//                + LanguageSyntax.With
//                + LanguageSyntax.LeftBrace;


//            var entryReference = new Reference<TokenType>();

//            var entry = new CodeBlock<TokenType>()
//                + (new Choice<TokenType>()
//                    + (new CodeBlock<TokenType>()
//                        + propertyValueAssignment   // key = value
//                        )

//                    + (new CodeBlock<TokenType>()
//                        + propertyObjectAssignment  // key = {
//                        + propertyValueAssignment   // key = value

//                        + (new Optional<TokenType>()
//                            + (new Repeat<TokenType>()
//                                + LanguageSyntax.Comma   // ,
//                                + entryReference
//                                )
//                        )

//                        + LanguageSyntax.RightBrace  // }
//                    )

//                    + (new CodeBlock<TokenType>()
//                        + propertyObjectAssignment   // key with {
//                        + propertyWithAssignment     // key = value

//                        + (new Optional<TokenType>()
//                            + (new Repeat<TokenType>()
//                                + LanguageSyntax.Comma   // ,
//                                + entryReference
//                                )
//                        )

//                        + LanguageSyntax.RightBrace  // }
//                    )
//                );

//            entryReference.Set(entry);


//            RuleBlock<TokenType> processingRules = new RuleBlock<TokenType>()
//            {
//                new CodeBlock<TokenType>()
//                    + LanguageSyntax.VariableName
//                    + LanguageSyntax.Equal
//                    + propertyCollectionAssignment
//                    + LanguageSyntax.SemiColon
//        };

//            var parser = new SymbolParser<TokenType>(processingRules, x => dump.Add(x));

//            SymbolNode<TokenType>? syntaxNode = parser.Parse(commands);
//            syntaxNode.Should().NotBeNull();

//            var matchList = new ISymbolToken[]
//            {
//                new SymbolToken<TokenType>(TokenType.DeclareObject),
//                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
//                new SymbolToken<TokenType>(TokenType.Equal),
//                new SymbolToken<TokenType>(TokenType.LeftBrace),
//                new SymbolValue<TokenType>(TokenType.VariableName, "Name1"),
//                new SymbolToken<TokenType>(TokenType.Equal),
//                new SymbolValue<TokenType>(TokenType.Constant, "Value1"),
//                new SymbolToken<TokenType>(TokenType.Comma),
//                new SymbolValue<TokenType>(TokenType.VariableName, "Name2"),
//                new SymbolToken<TokenType>(TokenType.Equal),
//                new SymbolValue<TokenType>(TokenType.Constant, "Value2"),
//                new SymbolToken<TokenType>(TokenType.RightBrace),
//                new SymbolToken<TokenType>(TokenType.SemiColon),
//            };

//            Enumerable.SequenceEqual(syntaxNode!, matchList).Should().BeTrue();
//        }
//    }
//}
