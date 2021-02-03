//using System.Collections.Generic;
//using TinglerCompiler.sdk.Tree.Nodes;
//using Toolbox.Parser;

//namespace TinglerCompiler.sdk.Tree
//{
//    public class TreeBuilder
//    {
//        private readonly StringTokenizer _tokenizer = new StringTokenizer()
//            .UseCollapseWhitespace()
//            .UseSingleQuote()
//            .UseDoubleQuote()
//            .Add("=", "[", "]", "{", "}", ".");

//        public TreeBuilder()
//        {
//        }

//        public INode Parse(string raw)
//        {
//            IReadOnlyList<IToken> tokens = _tokenizer.Parse(raw);
//            return null;
//        }
//    }
//}