using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Xunit;

namespace Toolbox.Language.Test
{
    public class PropertyTests
    {
        private readonly CodeBlock<TestTokenType> _processRules;
        private readonly ILoggerFactory _loggerFactory;

        public PropertyTests()
        {
            _loggerFactory = LoggerFactory.Create(x => x.AddDebug());

            var assignment = new CodeBlock<TestTokenType>()
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var propertiesReference = new Reference<TestTokenType>();

            var properties = new CodeBlock<TestTokenType>()
                + LanguageSyntax.LeftBrace
                + assignment

                + (new Optional<TestTokenType>()
                    + LanguageSyntax.With
                    + propertiesReference
                )

                + (new Optional<TestTokenType>()
                    + (new Repeat<TestTokenType>()
                        + LanguageSyntax.Comma
                        + assignment

                        + (new Optional<TestTokenType>()
                            + LanguageSyntax.With
                            + propertiesReference
                        )
                    )
                )

                + LanguageSyntax.RightBrace;

            propertiesReference.Set(properties);

            _processRules = new CodeBlock<TestTokenType>()
                + (new Choice<TestTokenType>()
                    + (new CodeBlock<TestTokenType>()
                        + assignment
                        + LanguageSyntax.SemiColon
                        )

                    + (new CodeBlock<TestTokenType>()
                        + LanguageSyntax.VariableName
                        + LanguageSyntax.Equal
                        + properties
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

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
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

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
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

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),

                new SymbolToken<TestTokenType>(TestTokenType.Comma),

                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value2"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
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

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType>? response = parser.Parse(commands);
            response.Nodes.Should().BeNull();
        }

        [Fact]
        public void SingleObjectWithProperty_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1 = Value1 with {",
                        "Name2 = Value2",
                    "}",
                "};"
            };

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value2"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void SingleObjectWithTwoProperties_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1 = Value1 with {",
                        "Name2 = Value2,",
                        "Name3 = Value3",
                    "}",
                "};"
            };

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value2"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name3"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value3"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void ObjectWithMultiplePropertiesAndWith_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1=Value1,",
                    "Name2 =Value2,",
                    "Name3 = Value3 with {",
                        "Name4 = Value4,",
                        "Name5 = Value5",
                    "}",
                "};"
            };

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value2"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name3"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value3"),

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name4"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value4"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name5"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value5"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void ObjectWithMultiplePropertiesAndWithEmbedded_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1=Value1,",
                    "Name3 = Value3 with {",
                        "Name4 = Value4,",
                        "Name5 = Value5",
                    "},",
                    "Name2 =Value2",
                "};"
            };

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),

                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name3"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value3"),
                new SymbolToken<TestTokenType>(TestTokenType.With),

                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name4"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value4"),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name5"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value5"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),

                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value2"),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void MultipleObjectsWithProperty_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1=Value1,",
                    "Name2 =Value2,",
                    "Name3 = Value3 with {",
                        "Name4 = Value4,",
                        "Name5 = Value5 with {",
                            "Name6 = Value6",
                        "}",
                    "},",
                    "Name7 = {",
                        "Name4 = Value4",
                    "}",
                "};"
            };

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands);
            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value1"),

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value2"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }
    }
}