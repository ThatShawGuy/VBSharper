using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Parsing;
using JetBrains.ReSharper.Psi.VB.Tree;
using VBSharper.Plugins.QuickFixes.Base;

namespace VBSharper.Plugins.QuickFixes.UseShortCircuitOperators
{
    public class UseShortCircuitOperatorsQuickFixHelper : QuickFixHelperBase<ITokenNode>
    {
        public override IEnumerable<QuickFixTreeNodeDocumentRange> GetTreeNodeDocumentRanges(IFile file) {
            using (ReadLockCookie.Create()) {
                foreach (var binaryExpression in file.EnumerateSubTree().OfType<IVBBinaryExpression>()) {
                    if (!(binaryExpression is ILogicalAndExpression || binaryExpression is ILogicalOrExpression)) continue;
                    if (!binaryExpression.LeftExpr.Type().IsBool() || !binaryExpression.RightExpr.Type().IsBool()) continue;

                    var logicalOperator = binaryExpression.Children<ITokenNode>()
                        .FirstOrDefault(node => node.GetTokenType() == VBTokenType.OR_KEYWORD || node.GetTokenType() == VBTokenType.AND_KEYWORD);
                    if (logicalOperator == null) continue;

                    var documentRange = logicalOperator.GetDocumentRange();
                    yield return new QuickFixTreeNodeDocumentRange(logicalOperator, documentRange);
                }
            }
        }

        public override void ApplyQuickFix(ITokenNode tokenNode) {
            using (WriteLockCookie.Create(tokenNode.IsPhysical())) {
                var logicalExpression = tokenNode.Parent as IVBBinaryExpression;
                if (logicalExpression == null) return;

                string shortCircuitOperator = null;
                
                if (tokenNode.GetTokenType() == VBTokenType.OR_KEYWORD) {
                    shortCircuitOperator = VBTokenType.ORELSE_KEYWORD.TokenRepresentation;
                }
                else if (tokenNode.GetTokenType() == VBTokenType.AND_KEYWORD) {
                    shortCircuitOperator = VBTokenType.ANDALSO_KEYWORD.TokenRepresentation;
                }

                if (shortCircuitOperator == null) return;

                var elementFactory = VBElementFactory.GetInstance(tokenNode.GetPsiModule());
                var newlogicalExpression = elementFactory.CreateExpression("$0 " + shortCircuitOperator + " $1", logicalExpression.LeftExpr, logicalExpression.RightExpr);
                logicalExpression.ReplaceByExtension(newlogicalExpression);
            }
        }
    }
}
