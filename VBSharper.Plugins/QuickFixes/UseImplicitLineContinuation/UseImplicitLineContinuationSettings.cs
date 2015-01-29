using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace VBSharper.Plugins.QuickFixes.UseImplicitLineContinuation
{
    [SettingsKey(typeof(CodeInspectionSettings), SettingsName)]
    public class UseImplicitLineContinuationSettings
    {
        private const string SettingsName = "Use Implicit Line Continuations";
        [SettingsEntry(true, SettingsName)]
        public bool Enable { get; set; }
    }
}
