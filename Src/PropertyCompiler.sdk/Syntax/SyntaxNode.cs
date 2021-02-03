using Toolbox.Tools;

namespace PropertyCompiler.sdk.Syntax
{
    public abstract class SyntaxNode
    {
        internal SyntaxNode(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public SyntaxNode? Parent { get; private set; }

        public SyntaxTree SyntaxTree { get; }

        public SyntaxNode? Value { get; protected set; }

        public void SetParent(SyntaxNode syntaxNode) => Parent = syntaxNode.VerifyNotNull(nameof(syntaxNode));
    }
}