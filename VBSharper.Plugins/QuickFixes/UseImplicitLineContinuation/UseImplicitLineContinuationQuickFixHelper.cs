using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB.Parsing;
using JetBrains.ReSharper.Resources.Shell;
using VBSharper.Plugins.QuickFixes.Base;

namespace VBSharper.Plugins.QuickFixes.UseImplicitLineContinuation
{
    public class UseImplicitLineContinuationQuickFixHelper : QuickFixHelperBase<ITokenNode>
    {
        public override IEnumerable<QuickFixTreeNodeDocumentRange> GetTreeNodeDocumentRanges(IFile file) {
            using (ReadLockCookie.Create()) {
                var explicitLineContinuations =
                    file.ThisAndDescendants()
                        .OfType<ITokenNode>()
                        .ToEnumerable()
                        .Where(node => node.GetTokenType() == VBTokenType.LINE_CONTINUATION)
                        .ToList();

                foreach (var lineContinuation in explicitLineContinuations) {
                    var previousToken = lineContinuation.GetPreviousMeaningfulToken();
                    var nextToken = lineContinuation.GetNextMeaningfulToken();
                    if (previousToken == null || nextToken == null) continue;

                    var canUseImplicitLineContinuation =
                        VBImplicitLineContinuationHelper.CanUseImplicitLineContinuationBetween(previousToken, nextToken);

                    if (!canUseImplicitLineContinuation) continue;

                    var documentRange = lineContinuation.GetDocumentRange();
                    yield return new QuickFixTreeNodeDocumentRange(lineContinuation, documentRange);
                }
            }
        }

        public override void ApplyQuickFix(ITokenNode tokenNode) {
            using (WriteLockCookie.Create(tokenNode.IsPhysical())) {
                ITreeNode lineTerminator = VBTokenType.LINE_TERMINATOR.CreateTreeElement();
                ModificationUtil.ReplaceChild(tokenNode, lineTerminator);
            }
        }
    }
}
