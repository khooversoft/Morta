using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Parser;

namespace Toolbox.Language.ProcessingRules
{
    public static class Extensions
    {
        public static void LogStarting<T>(this SymbolParserContext context, ICodeBlock<T> rule) where T: Enum
        {
            context.DebugStack.Add(new SymbolNode<T>() + new MessageTrivia { Message = $"Starting {rule}" });
        }

        public static void LogCompleted<T>(this SymbolParserContext context, ICodeBlock<T> rule) where T : Enum
        {
            context.DebugStack.Add(new SymbolNode<T>() + new MessageTrivia { Message = $"Completed {rule}" });
        }

        public static void LogFail<T>(this SymbolParserContext context, ICodeBlock<T> rule) where T : Enum
        {
            context.DebugStack.Add(new SymbolNode<T>() + new MessageTrivia { Message = $"Failed  {rule}" });
        }

        public static void LogSkipped<T>(this SymbolParserContext context, ICodeBlock<T> rule) where T : Enum
        {
            context.DebugStack.Add(new SymbolNode<T>() + new MessageTrivia { Message = $"Skipped  {rule}" });
        }

        public static SymbolParserResponse<T> DumpDebugStack<T>(this SymbolParserResponse<T> response, ILogger logger) where T : Enum
        {
            if (response.Nodes == null) new DumpErrors<T>().Dump(response.DebugStack, logger);

            return response;
        }
    }
}
