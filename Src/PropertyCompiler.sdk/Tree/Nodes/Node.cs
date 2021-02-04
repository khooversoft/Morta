using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyCompiler.sdk.Tree.Nodes
{
    public class Node : NodeBase, INode
    {
        public string Name { get; init; } = null!;
    }
}
