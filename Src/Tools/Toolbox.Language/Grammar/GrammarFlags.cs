using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.Language.Grammar
{
    [Flags]
    public enum GrammarFlags
    {
        None,
        StartCodeBlock = 0x1,
        EndCodeBlock = 0x2,
        EndStatement = 0x4,
        Keyword = 0x8,
    }
}
