using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using VBSharper.Plugins.QuickFixes.Base;

namespace VBSharper.Plugins.QuickFixes.UseShortCircuitOperators
{
    [QuickFix]
    public sealed class UseShortCircuitOperatorsQuickFix : BulkQuickFixBase<ITokenNode>
    {
        public override string ProgressIndicatorTaskName { get { return "Applying Short-circuit Operators"; } }
        public override string Text { get { return "Use short-circuit operator"; } }
        public override bool IsAvailable(IUserDataHolder cache) { return _highlighting.IsValid(); }

        private readonly UseShortCircuitOperatorsHighlighting _highlighting;

        public UseShortCircuitOperatorsQuickFix(UseShortCircuitOperatorsHighlighting highlighting)
            : base(highlighting.TokenNode, new UseShortCircuitOperatorsQuickFixHelper(), new UseShortCircuitOperatorsFileQuickFix()) {

            _highlighting = highlighting;
        }
    }

    public sealed class UseShortCircuitOperatorsFileQuickFix : FileQuickFixBase<ITokenNode>
    {
        public override string Text { get { return "Use short-circuit operators in file"; } }
        protected override IQuickFixHelper<ITokenNode> QuickFixHelper { get { return new UseShortCircuitOperatorsQuickFixHelper(); } }
    }
}
