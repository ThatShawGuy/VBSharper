using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.VB.Stages;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Tree;
using VBSharper.Plugins.QuickFixes.RemoveByValKeyword;

[assembly: RegisterConfigurableSeverity(RemoveByValKeywordHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeRedundancy,
    "ByVal Keyword is redundant.",
    "ByVal Keyword is the implicit default and does not need to be stated explicitly.",
    Severity.HINT,
    false)]

namespace VBSharper.Plugins.QuickFixes.RemoveByValKeyword
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class RemoveByValKeywordDaemonStage : VBDaemonStageBase
    {
        public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IVBFile file) {
            return new RemoveByValKeywordDaemonStageProcess(process, settings, file);
        }
    }

    public class RemoveByValKeywordDaemonStageProcess : VBDaemonStageProcessBase
    {
        public RemoveByValKeywordDaemonStageProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore, IVBFile file)
            : base(daemonProcess, settingsStore, file) {
        }

        public override void Execute(Action<DaemonStageResult> committer) {
            if (DaemonProcess.InterruptFlag) return;

            var util = new RemoveByValKeywordQuickFixHelper();
            var byValKeywords = util.GetTreeNodeDocumentRanges(File);
            var highlights = byValKeywords.Select(expression => new HighlightingInfo(expression.DocumentRange, new RemoveByValKeywordHighlighting(expression.TreeNode))).ToList();

            committer(new DaemonStageResult(highlights));
        }
    }

    [ConfigurableSeverityHighlighting(SeverityId, VBLanguage.Name, AttributeId = HighlightingAttributeIds.DEADCODE_ATTRIBUTE, OverlapResolve = OverlapResolveKind.DEADCODE)]
    public class RemoveByValKeywordHighlighting : VBHighlightingBase, IHighlighting
    {
        public const string SeverityId = "RemoveByValKeywordHighlighting";

        public RemoveByValKeywordHighlighting(ITokenNode tokenNode) {
            TokenNode = tokenNode;
        }

        public string ToolTip { get { return "ByVal Keyword is redundant"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
        public ITokenNode TokenNode { get; private set; }

        public override bool IsValid() {
            return TokenNode == null || TokenNode.IsValid();
        }
    }


}
