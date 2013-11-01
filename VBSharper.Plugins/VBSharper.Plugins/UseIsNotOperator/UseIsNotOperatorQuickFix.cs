using System;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.CodeStyle;
using JetBrains.ReSharper.Psi.VB.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace VBSharper.Plugins.UseIsNotOperator
{
    [QuickFix]
    public sealed class UseIsNotOperatorQuickFix : QuickFixBase
    {
        private readonly UseIsNotOperatorHighlighting _highlighting;

        public UseIsNotOperatorQuickFix(UseIsNotOperatorHighlighting highlighting) {
            _highlighting = highlighting;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            var logicalNotExpression = _highlighting.Expression;

            logicalNotExpression.GetPsiServices().Transactions.Execute(GetType().Name,
                () => {
                    // WriteLock is used in our call to ReplaceByExtension
                    //using (WriteLockCookie.Create()) {
                    
                    // Confirm IsExpression exists under LogicalNotExpression
                    var isExpression = logicalNotExpression.Children<IIsExpression>().FirstOrDefault();
                    if ((isExpression) == null) return;

                    // Transform ILogicalNotExpression into IIsNotExpression
                    var elementFactory = VBElementFactory.GetInstance(logicalNotExpression.GetPsiModule());
                    var newIsNotExpression = elementFactory.CreateExpression("$0 IsNot $1", (object)isExpression.LeftExpr, (object)isExpression.RightExpr);
                    logicalNotExpression.ReplaceByExtension(newIsNotExpression);
                });

            return null;
        }

        public override string Text {
            get { return "Use IsNot operator"; }
        }

        public override bool IsAvailable(IUserDataHolder cache) {
            return _highlighting.IsValid();
        }
    }
}
