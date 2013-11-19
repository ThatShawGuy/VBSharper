using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB.Parsing;
using VBSharper.Plugins.QuickFixes.Base;

namespace VBSharper.Plugins.QuickFixes.RemoveByValKeyword
{
    public class RemoveByValKeywordQuickFixHelper : QuickFixHelperBase<ITokenNode>
    {
        public override IEnumerable<QuickFixTreeNodeDocumentRange> GetTreeNodeDocumentRanges(IFile file) {
            using (ReadLockCookie.Create()) {
                foreach (var parameterDeclaration in file.EnumerateSubTree().OfType<IParameterDeclaration>()) {
                    var byValKeyword = parameterDeclaration.Children<ITokenNode>().FirstOrDefault(node => node.GetTokenType() == VBTokenType.BYVAL_KEYWORD);
                    if (byValKeyword == null) continue;

                    var documentRange = byValKeyword.GetDocumentRange();
                    yield return new QuickFixTreeNodeDocumentRange(byValKeyword, documentRange);
                }
            }
        }
        
        public override void ApplyQuickFix(ITokenNode tokenNode) {
            using (WriteLockCookie.Create(tokenNode.IsPhysical())) {
                ModificationUtil.DeleteChild(tokenNode);
            }
        }
    }
}
