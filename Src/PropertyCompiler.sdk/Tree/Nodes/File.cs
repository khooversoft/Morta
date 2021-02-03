using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCompiler.sdk.Tree.Nodes
{
    public class File : NodeBase, INode
    {
        public string Path { get; init; } = null!;
    }
}
