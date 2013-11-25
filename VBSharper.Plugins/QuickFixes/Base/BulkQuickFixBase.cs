using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Intentions.Bulk;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Intentions.QuickFixes.Bulk;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Tree;
using JetBrains.TextControl;

namespace VBSharper.Plugins.QuickFixes.Base
{
    public abstract class BulkQuickFixBase<T> : QuickFixBase where T : ITreeNode
    {
        public abstract string ProgressIndicatorTaskName { get; }
        
        protected readonly T TreeNode;
        protected readonly IQuickFixHelper<T> QuickFixHelper;
        protected readonly FileQuickFixBase<T> FileQuickFix;
        
        protected BulkQuickFixBase(T treeNode, IQuickFixHelper<T> quickFixHelper, FileQuickFixBase<T> fileQuickFix) {
            TreeNode = treeNode;
            QuickFixHelper = quickFixHelper;
            FileQuickFix = fileQuickFix;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            QuickFixHelper.ApplyQuickFix(TreeNode);

            return null;
        }

        public override IEnumerable<IntentionAction> CreateBulbItems() {
            var file = TreeNode.GetContainingFile();
            var sourceFile = TreeNode.GetSourceFile();
            var projectFile = sourceFile.ToProjectFile();
            if (projectFile == null) return base.CreateBulbItems();

            var solution = projectFile.GetSolution();
            var psiFiles = solution.GetComponent<IPsiFiles>();

            Action<IDocument, IPsiSourceFile, IProgressIndicator> psiTransactionAction =
                (document, psiSourceFile, progressIndicator) => {
                    progressIndicator.TaskName = ProgressIndicatorTaskName;
                    foreach (var psiFile in psiFiles.GetPsiFiles<VBLanguage>(psiSourceFile).OfType<IVBFile>()) {
                        QuickFixHelper.ApplyQuickFix(psiFile);
                    }
                };

            FileQuickFix.File = file;
            var predicateByPsiLanaguage = BulkItentionsBuilderEx.CreateAcceptFilePredicateByPsiLanaguage<VBLanguage>(solution);
            var bulkQuickFixBuilder = new BulkQuickFixWithCommonTransactionBuilder(this, FileQuickFix, solution, this.Text + "s", psiTransactionAction, predicateByPsiLanaguage);
            return bulkQuickFixBuilder.CreateBulkActions(projectFile, IntentionsAnchors.QuickFixesAnchor, IntentionsAnchors.QuickFixesAnchorPosition);
        }
    }
}