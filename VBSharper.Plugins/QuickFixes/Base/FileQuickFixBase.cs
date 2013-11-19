using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace VBSharper.Plugins.QuickFixes.Base
{
    public abstract class FileQuickFixBase<T> : QuickFixBase where T : ITreeNode
    {
        public IFile File { get; set; }
        protected abstract IQuickFixHelper<T> QuickFixHelper { get; }
        public override bool IsAvailable(IUserDataHolder cache) { return true; }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            QuickFixHelper.ApplyQuickFix(File);
            return null;
        }
    }
}