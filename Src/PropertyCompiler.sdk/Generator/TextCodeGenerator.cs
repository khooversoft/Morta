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
            body.VerifyNotNull(nameof(body));

            return BuildTokens(body);
        }

        public IReadOnlyList<string> BuildTokens(ISyntaxCollection body)
        {
            body.VerifyNotNull(nameof(body));

            int tab = 0;
            List<string> list = new List<string>();
            Stack<object> stack = new Stack<object>(body.Children.Reverse());

            while (stack.TryPop(out object? syntaxNode))
            {
                switch (syntaxNode)
                {
                    case AssemblyExpression assemblyExpression:
                        addToList($"{Build(assemblyExpression)};");
                        break;

                    case IncludeExpression includeExpression:
                        addToList($"{Build(includeExpression)};");
                        break;

                    case ResourceExpression resourceExpression:
                        addToList($"{Build(resourceExpression)};");
                        break;

                    case ScalarAssignment scalarAssignment:
                        string trailing = tab == 0 ? ";" : ",";
                        addToList($"{Build(scalarAssignment)}{trailing}");
                        break;

                    case WithObjectExpression withObjectExpression when list.Count > 0:
                        string assignment = list[^1][..^1];
                        list.RemoveAt(list.Count - 1);

                        list.Add($"{assignment} with {{");

                        tab++;
                        stack.Push((Action)(() => stack.Push(tab == 0 ? "};" : "}")));
                        stack.Push((Action)(() => tab--));

                        withObjectExpression.Children
                            .Reverse()
                            .ForEach(x => stack.Push(x));

                        break;


                    case ObjectExpression objectExpression:
                        addToList($"{objectExpression.VariableName.Value} = {{");

                        tab++;
                        stack.Push((Action)(() => stack.Push(tab == 0 ? "};" : "}")));
                        stack.Push((Action)(() => tab--));

                        objectExpression.Children
                            .Reverse()
                            .ForEach(x => stack.Push(x));

                        break;

                    case Action action:
                        action();
                        break;

                    case string subject when subject.StartsWith("}") && list.Count > 1 && list[^1].EndsWith(","):
                        list[^1] = list[^1][..^1];
                        addToList(subject);
                        break;

                    case string subject:
                        addToList(subject);
                        break;

                    default:
                        throw new ArgumentException("Unknown syntax");
                }
            }

            return list;

            void addToList(string subject)
            {
                tab.VerifyAssert(x => x >= 0, "tab out of bounds");
                list.Add(new string(' ', tab * _tabSize) + subject);
            }
        }

        private string Build(AssemblyExpression assemblyExpression) => $"assembly {assemblyExpression.AssemblyPath.Value}";
        private string Build(IncludeExpression includeExpression) => $"include {includeExpression.IncludePath.Value}";
        private string Build(ResourceExpression resourceExpression) => $"resource {resourceExpression.ResourceId.Value} = {resourceExpression.FilePath.Value}";
        private string Build(ScalarAssignment scalarAssignment) => $"{scalarAssignment.VariableName.Value} = {scalarAssignment.Constant.Value}";
    }
}
