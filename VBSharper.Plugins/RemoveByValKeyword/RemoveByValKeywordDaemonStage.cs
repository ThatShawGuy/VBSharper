using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.VB.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Parsing;
using JetBrains.ReSharper.Psi.VB.Tree;
using VBSharper.Plugins.RemoveByValKeyword;

[assembly: RegisterConfigurableSeverity(RemoveByValKeywordHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeRedundancy,
    "ByVal Keyword is redundant.",
    "ByVal Keyword is the implicit default and does not need to be stated explicitly.",
    Severity.HINT,
    false)]

namespace VBSharper.Plugins.RemoveByValKeyword
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

            var highlights = new List<HighlightingInfo>();
            using (ReadLockCookie.Create()) {
                File.ProcessChildren<IParameterDeclaration>(
                    parameterDeclaration => {
                        var byValKeyword = parameterDeclaration.Children<ITokenNode>().FirstOrDefault(node => node.GetTokenType() == VBTokenType.BYVAL_KEYWORD);
                        if (byValKeyword == null) return;

                        var docRange = byValKeyword.GetDocumentRange();
                        highlights.Add(new HighlightingInfo(docRange, new RemoveByValKeywordHighlighting(byValKeyword)));
                    });
            }
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
