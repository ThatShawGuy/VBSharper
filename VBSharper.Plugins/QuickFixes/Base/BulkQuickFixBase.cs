using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Bulk;
using JetBrains.ReSharper.Feature.Services.Bulk.Actions;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Tree;
using JetBrains.TextControl;
using JetBrains.UI.BulbMenu;

namespace VBSharper.Plugins.QuickFixes.Base
{
    public abstract class BulkQuickFixBase<T> : QuickFixBase, IScopeBulkAction where T : ITreeNode
    {
        private readonly FileCollectorInfo _fileCollectorInfo;
        protected readonly T TreeNode;
        protected readonly IQuickFixHelper<T> QuickFixHelper;
        protected readonly FileQuickFixBase<T> FileQuickFix;
        public abstract string ProgressIndicatorTaskName { get; }

        protected BulkQuickFixBase(T treeNode, IQuickFixHelper<T> quickFixHelper, FileQuickFixBase<T> fileQuickFix) {
            TreeNode = treeNode;
            QuickFixHelper = quickFixHelper;
            FileQuickFix = fileQuickFix;

            _fileCollectorInfo = new FileCollectorInfo(TreeNode.GetSourceFile().ToProjectFile(), VBLanguage.Instance);
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            QuickFixHelper.ApplyQuickFix(TreeNode);

            return null;
        }

        public string BulkText {
            get { return Text + "s"; }
        }

        public FileCollectorInfo FileCollectorInfo {
            get { return _fileCollectorInfo; }
        }

        public bool Single {
            get { return true; }
        }

        public Action<ITextControl> ExecuteAction(ISolution solution, Scope scope, IProgressIndicator singleFileProgress) {
            foreach (IProjectFile projectFile in scope.GetFilesToProcess(singleFileProgress)) {
                var sourceFile = projectFile.ToSourceFile();
                if (sourceFile == null) continue;
                if (sourceFile.Properties.IsGeneratedFile) continue;

                singleFileProgress.TaskName = ProgressIndicatorTaskName;

                foreach (var psiFile in sourceFile.GetPsiFiles<VBLanguage>().OfType<IVBFile>()) {
                    QuickFixHelper.ApplyQuickFix(psiFile);
                }
            }

            return null;
        }

        public IEnumerable<IntentionAction> ToIntentionAction(IBulbAction bulbAction, IAnchor anchor) {
            return bulbAction.ToQuickFixAction(anchor);
        }
    }
}