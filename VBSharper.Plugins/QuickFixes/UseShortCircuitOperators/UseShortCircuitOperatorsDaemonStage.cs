using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon.VB.Stages;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.VB.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Tree;
using VBSharper.Plugins.QuickFixes.UseShortCircuitOperators;

[assembly: RegisterConfigurableSeverity(UseShortCircuitOperatorsHighlighting.SeverityId,
    null,
    HighlightingGroupIds.BestPractice,
    "Use of short-circuit operators is preferred.",
    "Use of short-circuit logical operators is preferred over regular logical operators",
    Severity.SUGGESTION,
    false)]

namespace VBSharper.Plugins.QuickFixes.UseShortCircuitOperators
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class UseShortCircuitOperatorsDaemonStage : VBDaemonStageBase
    {
        public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IVBFile file) {
            return new UseShortCircuitOperatorsDaemonStageProcess(process, settings, file);
        }
    }

    public class UseShortCircuitOperatorsDaemonStageProcess : VBDaemonStageProcessBase
    {
        public UseShortCircuitOperatorsDaemonStageProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore, IVBFile file)
            : base(daemonProcess, settingsStore, file) {
        }

        public override void Execute(Action<DaemonStageResult> committer) {
            if (DaemonProcess.InterruptFlag) return;

            var expressions = new UseShortCircuitOperatorsQuickFixHelper().GetTreeNodeDocumentRanges(File);
            var highlights = expressions.Select(expression => new HighlightingInfo(expression.DocumentRange, new UseShortCircuitOperatorsHighlighting(expression.TreeNode))).ToList();

            committer(new DaemonStageResult(highlights));
        }
    }

    [ConfigurableSeverityHighlighting(SeverityId, VBLanguage.Name)]
    public class UseShortCircuitOperatorsHighlighting : VBHighlightingBase, IHighlighting
    {
        public const string SeverityId = "UseShortCircuitOperatorsHighlighting";

        public UseShortCircuitOperatorsHighlighting(ITokenNode tokenNode) {
            TokenNode = tokenNode;
        }

        public override DocumentRange CalculateRange() {
            throw new NotImplementedException();
        }

        public string ToolTip { get { return "Use of short-circuit operator is preferred"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
        public ITokenNode TokenNode { get; private set; }

        public override bool IsValid() {
            return TokenNode == null || TokenNode.IsValid();
        }
    }   
}
