using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace Toolbox.Language.ProcessingRules
{
    public class DumpErrors<T> where T : Enum
    {
        public void Dump(IReadOnlyList<IReadOnlyList<ISymbolToken>> debugStack, ILogger logger)
        {
            debugStack.VerifyNotNull(nameof(debugStack));
            logger.VerifyNotNull(nameof(logger));

            int nodeCount = 0;
            int level = 0;

            foreach (IReadOnlyList<ISymbolToken> item in debugStack)
            {
                logger.LogInformation($"Node: {nodeCount++}");

                int itemCount = 0;

                item
                    .ForEach(x => logger.LogInformation($"  {(level > 0 ? new String(' ', level * 3) : string.Empty)}({itemCount++}) {x}"));

                switch (item.FirstOrDefault())
                {
                    case MessageTrivia trivia when trivia.Message.IndexOf("Starting") >= 0:
                        level++;
                        break;

                    case MessageTrivia trivia when trivia.Message.IndexOf("Completed") >= 0:
                        level--;
                        break;

                    case MessageTrivia trivia when trivia.Message.IndexOf("Failed") >= 0:
                        level--;
                        break;

                    case MessageTrivia trivia when trivia.Message.IndexOf("Skipped") >= 0:
                        level--;
                        break;
                }
            }
        }
    }
}
