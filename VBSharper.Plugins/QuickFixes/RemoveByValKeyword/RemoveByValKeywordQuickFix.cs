using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using VBSharper.Plugins.QuickFixes.Base;

namespace VBSharper.Plugins.QuickFixes.RemoveByValKeyword
{
    [QuickFix]
    public sealed class RemoveByValKeywordQuickFix : BulkQuickFixBase<ITokenNode>
    {
        public override string ProgressIndicatorTaskName { get { return "Removing ByVal Keywords"; } }
        public override string Text { get { return "Remove ByVal keyword"; } }
        public override bool IsAvailable(IUserDataHolder cache) { return _highlighting.IsValid(); }

        private readonly RemoveByValKeywordHighlighting _highlighting;

        public RemoveByValKeywordQuickFix(RemoveByValKeywordHighlighting highlighting) 
            : base(highlighting.TokenNode, new RemoveByValKeywordQuickFixHelper(), new RemoveByValKeywordFileQuickFix()) {
            
            _highlighting = highlighting;
        }
    }

    public sealed class RemoveByValKeywordFileQuickFix : FileQuickFixBase<ITokenNode>
    {
        public override string Text { get { return "Remove ByVal keywords in file"; } }
        protected override IQuickFixHelper<ITokenNode> QuickFixHelper { get { return new RemoveByValKeywordQuickFixHelper(); } }
    }
}
