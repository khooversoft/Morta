﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;

namespace Toolbox.Language.Grammar
{
    public interface IExpression<T> : IGrammar<T> where T : Enum
    {
        T GrammarType { get; }

        ISymbolToken CreateToken(string value);
    }
}
