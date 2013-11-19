using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Tree;
using VBSharper.Plugins.QuickFixes.Base;

namespace VBSharper.Plugins.QuickFixes.UseIsNotOperator
{
    public class UseIsNotOperatorQuickFixHelper : QuickFixHelperBase<ILogicalNotExpression>
    {
        public override IEnumerable<QuickFixTreeNodeDocumentRange> GetTreeNodeDocumentRanges(IFile file) {
            using (ReadLockCookie.Create()) {
                foreach (var logicalNotExpression in file.EnumerateSubTree().OfType<ILogicalNotExpression>()) {
                    var isExpression = logicalNotExpression.Children<IIsExpression>().FirstOrDefault();
                    if (isExpression == null) continue;

                    var documentRange = logicalNotExpression.GetDocumentRange();
                    yield return new QuickFixTreeNodeDocumentRange(logicalNotExpression, documentRange);
                }
            }
        }

        public override void ApplyQuickFix(ILogicalNotExpression logicalNotExpression) {           
            logicalNotExpression.GetPsiServices().Transactions.Execute("UseIsNotOperatorQuickFix",
                () => {
                    // WriteLock is used in our call to ReplaceByExtension
                    //using (WriteLockCookie.Create()) {

                    // Confirm IsExpression exists under LogicalNotExpression
                    var isExpression = logicalNotExpression.Children<IIsExpression>().FirstOrDefault();
                    if ((isExpression) == null) return;

                    // Transform ILogicalNotExpression into IIsNotExpression
                    var elementFactory = VBElementFactory.GetInstance(logicalNotExpression.GetPsiModule());
                    var newIsNotExpression = elementFactory.CreateExpression("$0 IsNot $1", isExpression.LeftExpr, isExpression.RightExpr);
                    logicalNotExpression.ReplaceByExtension(newIsNotExpression);
                });
        }
    }
}
