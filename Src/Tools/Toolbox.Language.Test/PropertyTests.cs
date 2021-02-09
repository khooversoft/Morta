using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Xunit;

namespace Toolbox.Language.Test
{
    public class PropertyTests
    {
        private readonly CodeBlock<TokenType> _processRules;
        private readonly ILoggerFactory _loggerFactory;

        public PropertyTests()
        {
            _loggerFactory = LoggerFactory.Create(x => x.AddDebug());


            var assignment = new CodeBlock<TokenType>()  // key = value
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var createObject = new CodeBlock<TokenType>()  // key = {
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.LeftBrace;

            var withObject = new CodeBlock<TokenType>()  // with {
                + LanguageSyntax.With
                + LanguageSyntax.LeftBrace;

            var propertiesReference = new Reference<TokenType>();

            var properties = new CodeBlock<TokenType>()
                + assignment                                // key = value

                + (new Optional<TokenType>()
                    + withObject                    // with {
                    + propertiesReference           // key = value, ...
                    + LanguageSyntax.RightBrace     // }
                )

                + (new Optional<TokenType>()
                    + (new Repeat<TokenType>()
                        + LanguageSyntax.Comma              // ,
                        + assignment                        // key = value

                        + (new Optional<TokenType>()
                            + withObject                    // with {
                            + propertiesReference           // key = value, ...
                            + LanguageSyntax.RightBrace     // }
                        )

                    )
                );

            propertiesReference.Set(properties);

            _processRules = new CodeBlock<TokenType>()
                + (new Choice<TokenType>()
                    + (new CodeBlock<TokenType>()  // key = value;
                        + assignment
                        + LanguageSyntax.SemiColon
                        )

                    + (new CodeBlock<TokenType>()       // variable = { key = value[, key = value] }
                        + createObject
                        + properties
                        + LanguageSyntax.RightBrace     // }
                        + LanguageSyntax.SemiColon
                        )
                );

        }

        [Fact]
        public void SimpleAssignment_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = Value1;",
            };

            var parser = new SymbolParser<TokenType>(_processRules);

            SymbolParserResponse<TokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value1"),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void SimpleProperty_ShouldPass()
        {
            var list = new List<string>();


            var commands = new[]
            {
                "objectName = {",
                    "Name1 = Value1",
                "};"
            };

            var parser = new SymbolParser<TokenType>(_processRules);

            SymbolParserResponse<TokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name1"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value1"),
                new SymbolToken<TokenType>(TokenType.RightBrace),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void DoubleProperty_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1 = Value1,",
                    "Name2=Value2",
                "};"
            };

            var parser = new SymbolParser<TokenType>(_processRules);

            SymbolParserResponse<TokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name1"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value1"),

                new SymbolToken<TokenType>(TokenType.Comma),

                new SymbolValue<TokenType>(TokenType.VariableName, "Name2"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value2"),
                new SymbolToken<TokenType>(TokenType.RightBrace),
                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void WithProperty_Without_Assignment_ShouldFail()
        {
            var commands = new[]
            {
                "objectName with {",
                    "Name1 = Value1,",
                    "Name2=Value2",
                "};"
            };

            var parser = new SymbolParser<TokenType>(_processRules);

            SymbolParserResponse<TokenType>? response = parser.Parse(commands);
            response.Nodes.Should().BeNull();
        }

        [Fact]
        public void SingleObjevtWithProperty_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1 = Value1 with {",
                        "Name2 = Value2",
                    "}",
                "};"
            };

            var parser = new SymbolParser<TokenType>(_processRules);

            SymbolParserResponse<TokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TokenType>(TokenType.VariableName, "objectName"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name1"),
                new SymbolToken<TokenType>(TokenType.With),
                new SymbolValue<TokenType>(TokenType.Constant, "Value1"),

                new SymbolToken<TokenType>(TokenType.LeftBrace),
                new SymbolValue<TokenType>(TokenType.VariableName, "Name2"),
                new SymbolToken<TokenType>(TokenType.Equal),
                new SymbolValue<TokenType>(TokenType.Constant, "Value2"),
                new SymbolToken<TokenType>(TokenType.RightBrace),

                new SymbolToken<TokenType>(TokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }
    }
}
