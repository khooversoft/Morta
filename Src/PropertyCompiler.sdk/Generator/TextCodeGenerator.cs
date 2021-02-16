using PropertyCompiler.sdk.Expressions;
using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Generator
{
    public class TextCodeGenerator
    {
        private int _tabSize = 3;

        public TextCodeGenerator() { }

        public TextCodeGenerator SetTabSize(int size) => this.Action(x => x._tabSize = size);

        public IReadOnlyList<string> Build(Body body)
        {
            List<string>? list = new List<string>();
            int tab = 0;

            var tokens = BuildTokens(body);
            var stack = new Stack<string>(tokens.Reverse());

            var builder = new StringBuilder();

            while (stack.TryPop(out string? token))
            {
                builder.Append(token);

                switch (token)
                {
                    case ";":
                        addToList();
                        break;

                    case "{":
                        builder.Append(token);
                        addToList();
                        tab++;
                        break;

                    case "}":
                        builder.Append(token);
                        addToList();
                        tab--;
                        break;
                }
            }

            addToList();

            return list;

            void addToList()
            {
                if (builder.Length > 0)
                {
                    tab.VerifyAssert(x => x >= 0, "tab out of bounds");
                    list.Add(new string(' ', tab * _tabSize) + builder.ToString());
                    builder.Clear();
                }

            }
        }

        public IReadOnlyList<string> BuildTokens(Body body)
        {
            body.VerifyNotNull(nameof(body));

            var list = new List<string>();
            var stack = new Stack<ISyntaxNode>(body.Children.Reverse());

            while (stack.TryPop(out ISyntaxNode? syntaxNode))
            {
                switch (syntaxNode)
                {
                    case AssemblyExpression assemblyExpression:
                        list.Add($"{Build(assemblyExpression)}");
                        list.Add($";");
                        break;

                    case IncludeExpression includeExpression:
                        list.Add($"{Build(includeExpression)}");
                        list.Add($";");
                        break;

                    case ResourceExpression resourceExpression:
                        list.Add($"{Build(resourceExpression)}");
                        list.Add($";");
                        break;

                    case ScalarAssignment scalarAssignment:
                        list.Add($"{Build(scalarAssignment)}");
                        list.Add($";");
                        break;

                    case ObjectExpression objectExpression:
                        list.Add($"{objectExpression.VariableName.Value} = ");
                        list.Add("{");
                        break;

                    case ISyntaxCollection collection:
                        collection.Children
                            .Reverse()
                            .ForEach(x => stack.Push(x));
                        break;

                    default:
                        list.Add(syntaxNode?.ToString() ?? "<none>");
                        break;
                }
            }

            return list;
        }

        private void BuildExpressionTokens(IList<string> list, ObjectExpression objectExpression)
        {
            list.Add($"{objectExpression.VariableName.Value} = ");
            list.Add("{");

            foreach (var item in objectExpression.Children)
            {
                switch (item)
                {
                    case ScalarAssignment scalarAssignment:
                        list.Add(Build(scalarAssignment));
                        list.Add(",");
                        break;

                    case WithObjectExpression withObjectExpression:
                        list.Add("with");
                        list.Add("{");
                        BuildExpressionTokens(list, withObjectExpression);
                        list.Add("}");
                        break;

                    case ObjectExpression objExpression:
                        BuildExpressionTokens(list, objExpression);
                        break;

                    default:
                        throw new ArgumentException($"Invalid expression {item}");
                }
            }

            list.Add("}");
        }

        private string Build(AssemblyExpression assemblyExpression) => $"assembly {assemblyExpression.AssemblyPath.Value}";
        private string Build(IncludeExpression includeExpression) => $"include {includeExpression.IncludePath.Value}";
        private string Build(ResourceExpression resourceExpression) => $"resource {resourceExpression.ResourceId.Value} = {resourceExpression.FilePath.Value}";
        private string Build(ScalarAssignment scalarAssignment) => $"{scalarAssignment.VariableName.Value} = {scalarAssignment.Constant.Value}";
    }
}
