using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using VBSharper.Plugins.QuickFixes.Base;

namespace VBSharper.Plugins.QuickFixes.UseImplicitLineContinuation
{
    [QuickFix]
    public sealed class UseImplicitLineContinuationQuickFix : BulkQuickFixBase<ITokenNode>
    {
        public override string ProgressIndicatorTaskName { get { return "Removing explicit line continuations"; } }
        public override string Text { get { return "Remove explicit line continuation"; } }
        public override bool IsAvailable(IUserDataHolder cache) { return _highlighting.IsValid(); }

        private readonly UseImplicitLineContinuationHighlighting _highlighting;

        public UseImplicitLineContinuationQuickFix(UseImplicitLineContinuationHighlighting highlighting)
            : base(highlighting.TokenNode, new UseImplicitLineContinuationQuickFixHelper(), new UseImplicitLineContinuationFileQuickFix()) {

            _highlighting = highlighting;
        }
    }

    public sealed class UseImplicitLineContinuationFileQuickFix : FileQuickFixBase<ITokenNode>
    {
        public override string Text { get { return "Remove explicit line continuations in file"; } }
        protected override IQuickFixHelper<ITokenNode> QuickFixHelper { get { return new UseImplicitLineContinuationQuickFixHelper(); } }
    }
}
