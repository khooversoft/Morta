using FluentAssertions;
using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Tools;
using Xunit;

namespace PropertyCompiler.sdk.Test.Sources
{
    public class FullDocumentTests
    {
        private readonly string _source;

        public FullDocumentTests()
        {
            using Stream stream = Assembly.GetAssembly(typeof(FullDocumentTests))!
                .GetManifestResourceStream("PropertyCompiler.sdk.Test.Sources.SourceTest1.morta")
                .VerifyNotNull("Cannot read resource");

            using var memory = new StreamReader(stream);
            _source = memory.ReadToEnd();
        }

        [Fact]
        public void FullDocumentParsed_ShouldPass()
        {
            SyntaxTree tree = new SyntaxTreeBuilder()
                .AddStandardBuilders()
                .Add(_source)
                .Build();

            tree.IsError.Should().BeFalse();

            tree.SyntaxNodes.Count.Should().Be(8);
        }
    }
}
