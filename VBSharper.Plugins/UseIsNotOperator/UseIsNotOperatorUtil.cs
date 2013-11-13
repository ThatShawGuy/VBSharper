using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Tree;
using JetBrains.Util;

namespace VBSharper.Plugins.UseIsNotOperator
{
    public static class UseIsNotOperatorUtil
    {
        public static Dictionary<ILogicalNotExpression, DocumentRange> GetLogicalNotExpressionsThatCanUseIsNotExpression(IFile file) {
            var expressions = new Dictionary<ILogicalNotExpression, DocumentRange>();

            using (ReadLockCookie.Create()) {
                file.ProcessChildren<ILogicalNotExpression>(
                    logicalNotExpression => {
                        var isExpression = logicalNotExpression.Children<IIsExpression>().FirstOrDefault();
                        if (isExpression == null) return;

                        var documentRange = logicalNotExpression.GetDocumentRange();
                        expressions.Add(logicalNotExpression, documentRange);
                    });
            }

            return expressions;
        }

        public static void ApplyIsNotOperatorQuickFix(IFile file) {
            GetLogicalNotExpressionsThatCanUseIsNotExpression(file).Keys
                .ForEach(ApplyIsNotOperatorQuickFix);
        }

        public static void ApplyIsNotOperatorQuickFix(ILogicalNotExpression logicalNotExpression) {
            
            logicalNotExpression.GetPsiServices().Transactions.Execute("UseIsNotOperatorQuickFix",
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
        }
    }
}
