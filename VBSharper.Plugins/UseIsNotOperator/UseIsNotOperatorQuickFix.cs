using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
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
            UseIsNotOperatorUtil.ApplyIsNotOperatorQuickFix(logicalNotExpression);

            return null;
        }
        
        public override IEnumerable<IntentionAction> CreateBulbItems() {
            var file = _highlighting.Expression.GetContainingFile();
            var sourceFile = _highlighting.Expression.GetSourceFile();
            var projectFile = sourceFile.ToProjectFile();
            if (projectFile == null) return base.CreateBulbItems();

            var solution = projectFile.GetSolution();
            var psiFiles = solution.GetComponent<IPsiFiles>();

            Action<IDocument, IPsiSourceFile, IProgressIndicator> psiTransactionAction = 
                (document, psiSourceFile, indicator) => {
                    foreach (var psiFile in psiFiles.GetPsiFiles<VBLanguage>(psiSourceFile).OfType<IVBFile>()) {
                        UseIsNotOperatorUtil.ApplyIsNotOperatorQuickFix(psiFile);
                    }
                };

            var predicateByPsiLanaguage = BulkItentionsBuilderEx.CreateAcceptFilePredicateByPsiLanaguage<VBLanguage>(solution);
            var fileQuickFix = new UseIsNotOperatorFileQuickFix(file);
            var bulkQuickFixBuilder = new BulkQuickFixWithCommonTransactionBuilder(this, fileQuickFix, solution, this.Text, psiTransactionAction, predicateByPsiLanaguage);
            return bulkQuickFixBuilder.CreateBulkActions(projectFile, IntentionsAnchors.QuickFixesAnchor, IntentionsAnchors.QuickFixesAnchorPosition);
        }

        public override string Text {
            get { return "Use IsNot operator"; }
        }

        public override bool IsAvailable(IUserDataHolder cache) {
            return _highlighting.IsValid();
        }
    }

    public sealed class UseIsNotOperatorFileQuickFix : QuickFixBase
    {
        private readonly IFile _file;

        public UseIsNotOperatorFileQuickFix(IFile file) {
            _file = file;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            UseIsNotOperatorUtil.ApplyIsNotOperatorQuickFix(_file);
            return null;
        }

        public override string Text { get { return "Use IsNot operator in file"; } }
        public override bool IsAvailable(IUserDataHolder cache) { return true; }
    }
}
