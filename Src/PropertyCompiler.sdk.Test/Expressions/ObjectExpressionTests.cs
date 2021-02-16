using FluentAssertions;
using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Tools;
using Xunit;

namespace PropertyCompiler.sdk.Test.Expressions
{
    public class ObjectExpressionTests
    {
        [Fact]
        public void ValidObject_ShouldPass()
        {
            string raw = @"
            objectName = {
                Name1 = Value1
            };";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new ObjectExpressionBuilder().Create(syntaxTree);
            Body body = (response.SyntaxNode as Body)!;

            body.Should().NotBeNull();
            body.Children.Count.Should().Be(1);

            body.Children.First().As<ObjectExpression>().Action(x =>
            {
                x.VariableName.Value.Should().Be("objectName");

                x.Children.Count.Should().Be(1);
                x.Children.First().As<ScalarAssignment>().Action(y =>
                {
                    y.VariableName.Value.Should().Be("Name1");
                    y.Constant!.Value.Should().Be("Value1");
                });
            });
        }

        [Fact]
        public void MultiplePropertiesInObject_ShouldPass()
        {
            string raw = @"
            objectName={
                Name1=Value1,
                Name2=Value2
            };";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new ObjectExpressionBuilder().Create(syntaxTree);
            Body body = (response.SyntaxNode as Body)!;

            body.Should().NotBeNull();
            body.Children.Count.Should().Be(1);

            body.Children.First().As<ObjectExpression>().Action(x =>
            {
                x.VariableName.Value.Should().Be("objectName");

                x.Children.Count.Should().Be(2);
                x.Children.First().As<ScalarAssignment>().Action(y =>
                {
                    y.VariableName.Value.Should().Be("Name1");
                    y.Constant!.Value.Should().Be("Value1");
                });

                x.Children.Skip(1).First().As<ScalarAssignment>().Action(y =>
                {
                    y.VariableName.Value.Should().Be("Name2");
                    y.Constant!.Value.Should().Be("Value2");
                });
            });
        }

        [Fact]
        public void SingleObjectWithReferencePlusProperty_ShouldPass()
        {
            string raw = @"
                objectName = refValue with {
                    Name1 = Value1
                };
            ";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new ObjectExpressionBuilder().Create(syntaxTree);
            Body body = (response.SyntaxNode as Body)!;
            body.Should().NotBeNull();

            body.Children.Count.Should().Be(2);
            body.Children.First().As<ScalarAssignment>().Action(x =>
            {
                x.VariableName.Value.Should().Be("objectName");
                x.Constant.Value.Should().Be("refValue");
            });

            body.Children.Skip(1).First().As<WithObjectExpression>().Action(x =>
            {
                x.VariableName.Value.Should().Be("refValue");
                x.Children.Count.Should().Be(1);

                x.Children.First().As<ScalarAssignment>().Action(y =>
                {
                    y.VariableName.Value.Should().Be("Name1");
                    y.Constant.Value.Should().Be("Value1");
                });
            });
        }

        [Fact]
        public void SingleObjectWithProperty_ShouldPass()
        {
            string raw = @"
                objectName = {
                    Name1 = Value1 with {
                        Name2 = Value2,
                    },
                };
            ";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new ObjectExpressionBuilder().Create(syntaxTree);
            Body body = (response.SyntaxNode as Body)!;
            body.Should().NotBeNull();

            body.Children.Count.Should().Be(1);
            body.Children.First().As<ObjectExpression>().Action(x =>
            {
                x.VariableName.Value.Should().Be("objectName");

                x.Children.Count.Should().Be(2);
                x.Children.First().As<ScalarAssignment>().Action(y =>
                {
                    y.VariableName.Value.Should().Be("Name1");
                    y.Constant.Value.Should().Be("Value1");
                });

                x.Children.Skip(1).First().As<WithObjectExpression>().Action(with =>
                {
                    with.VariableName.Value.Should().Be("Value1");

                    with.Children.Count.Should().Be(1);
                    with.Children.First().As<ScalarAssignment>().Action(scalar =>
                    {
                        scalar.VariableName.Value.Should().Be("Name2");
                        scalar.Constant.Value.Should().Be("Value2");
                    });
                });
            });
        }

        [Fact]
        public void SingleObjectWithTwoProperties_ShouldPass()
        {
            string raw = @"
                objectName = {
                    Name1 = Value1 with {
                        Name2 = Value2,
                        Name3 = Value3
                    }
                };
            ";

            SyntaxTree syntaxTree = new SyntaxTreeBuilder()
                .Add(new ObjectExpressionBuilder())
                .Add(raw)
                .Build();

            SyntaxResponse response = new ObjectExpressionBuilder().Create(syntaxTree);
            Body body = (response.SyntaxNode as Body)!;
            body.Should().NotBeNull();

            body.Children.Count.Should().Be(1);
            body.Children.First().As<ObjectExpression>().Action(x =>
            {
                x.VariableName.Value.Should().Be("objectName");

                x.Children.Count.Should().Be(2);
                x.Children.First().As<ScalarAssignment>().Action(y =>
                {
                    y.VariableName.Value.Should().Be("Name1");
                    y.Constant.Value.Should().Be("Value1");
                });

                x.Children.Skip(1).First().As<WithObjectExpression>().Action(with =>
                {
                    with.VariableName.Value.Should().Be("Value1");

                    with.Children.Count.Should().Be(2);
                    with.Children.First().As<ScalarAssignment>().Action(scalar =>
                    {
                        scalar.VariableName.Value.Should().Be("Name2");
                        scalar.Constant.Value.Should().Be("Value2");
                    });

                    with.Children.Skip(1).First().As<ScalarAssignment>().Action(scalar =>
                    {
                        scalar.VariableName.Value.Should().Be("Name3");
                        scalar.Constant.Value.Should().Be("Value3");
                    });
                });
            });
        }
    }
}
