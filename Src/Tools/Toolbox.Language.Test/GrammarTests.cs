using FluentAssertions;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Xunit;

namespace Toolbox.Language.Test
{
    public class GrammarTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("int")]
        [InlineData("int i")]
        [InlineData("int ;")]
        public void SimpleVariableDefine_ShouldFail(string command)
        {
            CodeBlock<TestTokenType> processingRules = new CodeBlock<TestTokenType>()
            {
                new CodeBlock<TestTokenType>() + LanguageSyntax.TypeName + LanguageSyntax.VariableName + LanguageSyntax.SemiColon,
            };

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> syntaxNode = parser.Parse(command);
            syntaxNode.Nodes.Should().BeNull();
        }

        [Fact]
        public void SimpleVariableDefine_ShouldPass()
        {
            string command = "int i;";

            CodeBlock<TestTokenType> processingRules = new CodeBlock<TestTokenType>()
            {
                new CodeBlock<TestTokenType>() + LanguageSyntax.TypeName + LanguageSyntax.VariableName + LanguageSyntax.SemiColon,
            };

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> reponse = parser.Parse(command);
            reponse.Nodes.Should().NotBeNull();

            ISymbolToken[] matchList = new ISymbolToken[]
            {
                new SymbolValue<TestTokenType>(TestTokenType.TypeName, "int"),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "i"),
                new SymbolToken<TestTokenType>(TestTokenType.SemiColon),
            };

            Enumerable.SequenceEqual(reponse.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void DeclareVariable_ShouldPass()
        {
            string command = "declare objectName =";

            CodeBlock<TestTokenType> processingRules = new CodeBlock<TestTokenType>()
            {
                new CodeBlock<TestTokenType>()
                    + LanguageSyntax.DeclareObject
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
            };

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> reponse = parser.Parse(command);
            reponse.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolToken<TestTokenType>(TestTokenType.DeclareObject),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
            };

            Enumerable.SequenceEqual(reponse.Nodes!, matchList).Should().BeTrue();
        }

        [Fact]
        public void DeclareVariableWithExtracCommand_ShouldPass()
        {
            string command = "declare objectName =; name=value";

            CodeBlock<TestTokenType> processingRules = new CodeBlock<TestTokenType>()
            {
                new CodeBlock<TestTokenType>()
                    + LanguageSyntax.DeclareObject
                    + LanguageSyntax.VariableName
                    + LanguageSyntax.Equal
            };

            var parser = new SymbolParser<TestTokenType>(processingRules);

            SymbolParserResponse<TestTokenType> reponse = parser.Parse(command);
            reponse.Nodes.Should().NotBeNull();

            var matchList = new ISymbolToken[]
            {
                new SymbolToken<TestTokenType>(TestTokenType.DeclareObject),
                new SymbolValue<TestTokenType>(TestTokenType.VariableName, "objectName"),
                new SymbolToken<TestTokenType>(TestTokenType.Equal),
            };

            Enumerable.SequenceEqual(reponse.Nodes!, matchList).Should().BeTrue();
        }
    }
}