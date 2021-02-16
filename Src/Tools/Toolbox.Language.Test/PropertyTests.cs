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

        private ILogger _logger = LoggerFactory
            .Create(x => x.AddDebug())
            .CreateLogger("default");

        public PropertyTests()
        {
            var assignment = new CodeBlock<TestTokenType>("assignment")
                + LanguageSyntax.VariableName
                + LanguageSyntax.Equal
                + LanguageSyntax.Constant;

            var propertiesReference = new Reference<TestTokenType>();

            var properties = new CodeBlock<TestTokenType>("properties")
                + LanguageSyntax.LeftBrace

                + (new Optional<TestTokenType>("properties.1")
                    + (new Repeat<TestTokenType>("properties.2")
                        + LanguageSyntax.VariableName
                        + LanguageSyntax.Equal

                        + (new Choice<TestTokenType>("properties.3")
                            + (new CodeBlock<TestTokenType>("properties.4")
                                + LanguageSyntax.With
                                + propertiesReference
                            )

                            + (new CodeBlock<TestTokenType>("properties.5")
                                + LanguageSyntax.Constant

                                + (new Optional<TestTokenType>("properties.6")
                                    + LanguageSyntax.With
                                    + propertiesReference
                                )
                            )

                            + propertiesReference
                        )

                        + (new Optional<TestTokenType>("properties.7")
                            + LanguageSyntax.Comma
                        )
                    )
                )

                + LanguageSyntax.RightBrace;

            propertiesReference.Set(properties);

            _processRules = new CodeBlock<TestTokenType>("rules")
                + (new Choice<TestTokenType>("rules.0")
                    + (new CodeBlock<TestTokenType>("rules.1")
                        + assignment
                        + LanguageSyntax.SemiColon
                        )
                        
                    + (new CodeBlock<TestTokenType>("rules.2")
                        + assignment
                        + LanguageSyntax.With
                        + properties
                        + LanguageSyntax.SemiColon
                        )

                    + (new CodeBlock<TestTokenType>("rules.3")
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

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands)
                .DumpDebugStack<TestTokenType>(_logger);

            response.Should().NotBeNull();

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
            string command = @"
                objectName = refValue with {
                    Name1 = Value1
                };
            ";

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(command)
                .DumpDebugStack(_logger);

            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "refValue"),

                new SymbolToken<TestTokenType>(TestTokenType.With),
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
        public void SingleObjectChildWithProperty_ShouldPass()
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
        public void SingleObjectEqualTwoProperties_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1 = {",
                        "Name2 = {",
                            "Name3 = Value3",
                        "}",
                    "}",
                "};"
            };

            var parser = new SymbolParser<TestTokenType>(_processRules);

            SymbolParserResponse<TestTokenType> response = parser.Parse(commands)
                .DumpDebugStack<TestTokenType>(_logger);

            response.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),

                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name1"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),

                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name2"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),

                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name3"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value3"),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
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
        public void WidthMultipleEmbedded_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1 = Value1 with {",
                        "Name2 = Value2 with {",
                            "Name3 = Value3 with {",
                                "Name4 = Value4",
                            "}",
                        "}",
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

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name3"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value3"),

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name4"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value4"),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),

                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void WidthMultipleEmbedded_Patther2_ShouldPass()
        {
            var commands = new[]
            {
                "objectName = {",
                    "Name1 = Value1 with {",
                        "Name2 = Value2 with {",
                            "Name3 = Value3 with {",
                                "Name4 = Value4",
                            "},",
                         "Name5 = Value5",
                        "}",
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

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name3"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value3"),

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name4"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value4"),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),

                new SymbolToken<TestTokenType>(TestTokenType.Comma),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name5"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value5"),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
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
                    "Name7 = Value7 with {",
                        "Name8= Value8",
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

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name6"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value6"),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.Comma),

                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name7"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value7"),

                new SymbolToken<TestTokenType>(TestTokenType.With),
                new SymbolToken<TestTokenType>(TestTokenType.LeftBrace),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "Name8"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
                new SymbolValue<TestTokenType>(TestTokenType.Constant, "Value8"),

                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.RightBrace),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(response.Nodes!, matchList).Should().BeTrue();
        }
    }
}