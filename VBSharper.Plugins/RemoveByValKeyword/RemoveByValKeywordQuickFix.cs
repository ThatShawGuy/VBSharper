using System;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace VBSharper.Plugins.RemoveByValKeyword
{
    [QuickFix]
    public sealed class RemoveByValKeywordQuickFix : QuickFixBase
    {
        private readonly RemoveByValKeywordHighlighting _highlighting;

        public RemoveByValKeywordQuickFix(RemoveByValKeywordHighlighting highlighting) {
            _highlighting = highlighting;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            var tokenNode = _highlighting.TokenNode;

            using (WriteLockCookie.Create(tokenNode.IsPhysical())) {
                ModificationUtil.DeleteChild(tokenNode);
            }

            return null;
        }

        public override string Text {
            get { return "Remove ByVal keyword"; }
        }

        public override bool IsAvailable(IUserDataHolder cache) {
            return _highlighting.IsValid();
        }
    }
}
