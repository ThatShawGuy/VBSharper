using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.VB.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Tree;
using VBSharper.Plugins.UseIsNotOperator;

[assembly: RegisterConfigurableSeverity(UseIsNotOperatorHighlighting.SeverityId,
    null,
    HighlightingGroupIds.BestPractice,
    "Use of IsNot operator is preferred.",
    "Use of IsNot operator is preferred over separate use of Not and Is operators",
    Severity.SUGGESTION,
    false)]

namespace VBSharper.Plugins.UseIsNotOperator
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class UseIsNotOperatorDaemonStage : VBDaemonStageBase
    {
        public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, IVBFile file) {
            return new UseIsNotOperatorDaemonStageProcess(process, settings, file);
        }
    }
    
    public class UseIsNotOperatorDaemonStageProcess : VBDaemonStageProcessBase
    {
        public UseIsNotOperatorDaemonStageProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore, IVBFile file) 
            : base(daemonProcess, settingsStore, file) {
        }

        public override void Execute(Action<DaemonStageResult> committer) {
            if (DaemonProcess.InterruptFlag) return;

            var expressions = UseIsNotOperatorUtil.GetLogicalNotExpressionsThatCanUseIsNotExpression(File);
            var highlights = Enumerable.ToList(expressions.Select(expression => new HighlightingInfo(expression.Value, new UseIsNotOperatorHighlighting(expression.Key))));

            committer(new DaemonStageResult(highlights));
        }
    }

    [ConfigurableSeverityHighlighting(SeverityId, VBLanguage.Name)]
    public class UseIsNotOperatorHighlighting : VBHighlightingBase, IHighlighting
    {
        public const string SeverityId = "UseIsNotOperatorHighlighting";

        public UseIsNotOperatorHighlighting(ILogicalNotExpression expression) {
            Expression = expression;
        }

        public string ToolTip { get { return "Use of IsNot operator is preferred"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
        public ILogicalNotExpression Expression { get; private set; }

        public override bool IsValid() {
            return Expression == null || Expression.IsValid();
        }
    }
}
