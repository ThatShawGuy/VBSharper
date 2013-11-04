using System;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.VB.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Tree;
using VBSharper.Plugins.RemoveVBLegacyReferences;

[assembly: RegisterConfigurableSeverity(RemoveVBLegacyReferencesHighlighting.SeverityId,
    null,
    HighlightingGroupIds.BestPractice,
    "Remove VB Legacy References.",
    "The Microsoft.VisualBasic assembly houses a lot of VB6 style code that VB.NET developers do not prefer.",
    Severity.SUGGESTION,
    false)]

namespace VBSharper.Plugins.RemoveVBLegacyReferences
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class RemoveVBLegacyReferencesDaemonStage : VBDaemonStageBase
    {
        public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IVBFile file) {
            return new RemoveVBLegacyReferencesDaemonStageProcess(process, settings, file);
        }
    }

    public class RemoveVBLegacyReferencesDaemonStageProcess : VBDaemonStageProcessBase
    {
        public RemoveVBLegacyReferencesDaemonStageProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore, IVBFile file)
            : base(daemonProcess, settingsStore, file) {
        }

        public override void Execute(Action<DaemonStageResult> committer) {
            if (DaemonProcess.InterruptFlag) return;

            var highlights = new List<HighlightingInfo>();
            using (ReadLockCookie.Create()) {
                File.ProcessChildren<IVBTreeNode>(
                    treeNode => {
                        //TODO: Need to somehow pull namespace info for each identifier used on the page and look for "Microsoft.VisualBasic"
                        //var test = treeNode.GetContainingNamespaceDeclaration();
                        
                        //var isExpression = logicalNotExpression.Children<IIsExpression>().FirstOrDefault();
                        //if (isExpression == null) return;

                        //var docRange = logicalNotExpression.GetDocumentRange();
                        //highlights.Add(new HighlightingInfo(docRange, new RemoveVBLegacyReferencesHighlighting(logicalNotExpression)));
                    });
            }
            committer(new DaemonStageResult(highlights));
        }
    }

    [ConfigurableSeverityHighlighting(SeverityId, VBLanguage.Name)]
    public class RemoveVBLegacyReferencesHighlighting : VBHighlightingBase, IHighlighting
    {
        public const string SeverityId = "RemoveVBLegacyReferencesHighlighting";

        public RemoveVBLegacyReferencesHighlighting(ILogicalNotExpression expression) {
            Expression = expression;
        }

        public string ToolTip { get { return "Remove VB Legacy Reference"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
        public ILogicalNotExpression Expression { get; private set; }

        public override bool IsValid() {
            return Expression == null || Expression.IsValid();
        }
    }
}
