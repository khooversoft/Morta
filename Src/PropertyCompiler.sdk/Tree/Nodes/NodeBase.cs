using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCompiler.sdk.Tree.Nodes
{
    public abstract class NodeBase
    {
        readonly List<INode> _children = new List<INode>();

        public IList<INode> Children => _children;
    }
}
