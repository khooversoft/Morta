﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Toolbox.Parser
//{
//    /// <summary>
//    /// Bracket definition, used in body
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public class Bracket<T> : IRule where T : System.Enum
//    {
//        public Bracket(Symbol<T> startSymbol, Symbol<T> endSymbol)
//        {
//            StartSymbol = startSymbol;
//            EndSymbol = endSymbol;
//        }

//        public Symbol<T> StartSymbol { get; }

//        public Symbol<T> EndSymbol { get; }

//        public IEnumerable<IGrammar<T>> Grammars
//        {
//            get
//            {
//                yield return new Grammar<T>(StartSymbol.TokenType, StartSymbol.GrammarMatch);
//                yield return new Grammar<T>(EndSymbol.TokenType, EndSymbol.GrammarMatch);
//            }
//        }

//        public RootNode IfBracket(SyntaxToken<T> token)
//        {
//            bool isStartSymbol = token?.GrammarType.Equals(StartSymbol.TokenType) == true;
//            bool isEndSymbol = token?.GrammarType.Equals(EndSymbol.TokenType) == true;

//            if (!isStartSymbol && !isEndSymbol)
//            {
//                return null;
//            }

//            RootNode astNodes = new RootNode();
//            astNodes += new Symbol<T>(token.GrammarType);
//            return astNodes;
//        }

//        public override string ToString()
//        {
//            return $"{nameof(Bracket<T>)}: StartSymbol={StartSymbol}, EndSymbol={EndSymbol}";
//        }

//        public override bool Equals(object obj)
//        {
//            Bracket<T> context = obj as Bracket<T>;
//            return StartSymbol.Equals(context?.StartSymbol) && EndSymbol.Equals(context?.EndSymbol);
//        }

//        public override int GetHashCode()
//        {
//            return StartSymbol.GetHashCode() ^ EndSymbol.GetHashCode();
//        }
//    }
//}
