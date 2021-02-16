using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{

    public class ResourceExpression : ISyntaxNode
    {
        public ResourceExpression(SymbolValue<SymbolType> resourceId, SymbolValue<SymbolType> filePath)
        {
            resourceId.VerifyNotNull(nameof(resourceId));
            filePath.VerifyNotNull(nameof(filePath));

            ResourceId = resourceId;
            FilePath = filePath;
        }

        public SymbolValue<SymbolType> ResourceId { get; }

        public SymbolValue<SymbolType> FilePath { get; }
    }
}
