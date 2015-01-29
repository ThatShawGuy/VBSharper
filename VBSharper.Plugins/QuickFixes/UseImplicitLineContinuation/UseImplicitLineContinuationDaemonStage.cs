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
using VBSharper.Plugins.QuickFixes.UseImplicitLineContinuation;

[assembly: RegisterConfigurableSeverity(UseImplicitLineContinuationHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeRedundancy,
    "Explicit line continuation character is redundant.",
    "Explicit line continuation characters are no longer needed in many situations.",
    Severity.HINT,
    false)]

namespace VBSharper.Plugins.QuickFixes.UseImplicitLineContinuation
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class UseImplicitLineContinuationDaemonStage : VBDaemonStageBase
    {
        public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IVBFile file) {
            return new UseImplicitLineContinuationDaemonStageProcess(process, settings, file);
        }
    }

    public class UseImplicitLineContinuationDaemonStageProcess : VBDaemonStageProcessBase
    {
        public UseImplicitLineContinuationDaemonStageProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore, IVBFile file)
            : base(daemonProcess, settingsStore, file) {
        }

        public override void Execute(Action<DaemonStageResult> committer) {
            if (DaemonProcess.InterruptFlag) return;

            var util = new UseImplicitLineContinuationQuickFixHelper();
            var explicitLineContinuations = util.GetTreeNodeDocumentRanges(File);
            var highlights = explicitLineContinuations
                .Select(expression => new HighlightingInfo(expression.DocumentRange, new UseImplicitLineContinuationHighlighting(expression.TreeNode)))
                .ToList();

            committer(new DaemonStageResult(highlights));
        }
    }

    [ConfigurableSeverityHighlighting(SeverityId, VBLanguage.Name, AttributeId = HighlightingAttributeIds.DEADCODE_ATTRIBUTE, OverlapResolve = OverlapResolveKind.DEADCODE)]
    public class UseImplicitLineContinuationHighlighting : VBHighlightingBase, IHighlighting
    {
        public const string SeverityId = "UseImplicitLineContinuationHighlighting";

        public UseImplicitLineContinuationHighlighting(ITokenNode tokenNode) {
            TokenNode = tokenNode;
        }

        public override DocumentRange CalculateRange() {
            throw new NotImplementedException();
        }

        public string ToolTip { get { return "Explicit line continuation character is redundant"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
        public ITokenNode TokenNode { get; private set; }

        public override bool IsValid() {
            return TokenNode == null || (TokenNode.IsValid() && TokenNode.IsVB10Supported());
        }
    }
}
