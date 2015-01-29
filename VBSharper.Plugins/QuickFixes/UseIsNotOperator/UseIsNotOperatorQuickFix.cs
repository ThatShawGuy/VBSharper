using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.VB.Tree;
using JetBrains.Util;
using VBSharper.Plugins.QuickFixes.Base;

namespace VBSharper.Plugins.QuickFixes.UseIsNotOperator
{
    [QuickFix]
    public sealed class UseIsNotOperatorQuickFix : BulkQuickFixBase<ILogicalNotExpression>
    {
        public override string ProgressIndicatorTaskName { get { return "Applying IsNot Operator"; } }
        public override string Text { get { return "Use IsNot operator"; } }
        public override bool IsAvailable(IUserDataHolder cache) { return _highlighting.IsValid(); }

        private readonly UseIsNotOperatorHighlighting _highlighting;

        public UseIsNotOperatorQuickFix(UseIsNotOperatorHighlighting highlighting) 
            : base(highlighting.Expression, new UseIsNotOperatorQuickFixHelper(), new UseIsNotOperatorFileQuickFix()) {
            
            _highlighting = highlighting;
        }
    }

    public sealed class UseIsNotOperatorFileQuickFix : FileQuickFixBase<ILogicalNotExpression>
    {
        public override string Text { get { return "Use IsNot operator in file"; } }
        protected override IQuickFixHelper<ILogicalNotExpression> QuickFixHelper { get { return new UseIsNotOperatorQuickFixHelper(); } }
    }
}
