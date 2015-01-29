using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace VBSharper.Plugins.QuickFixes.UseShortCircuitOperators
{
    [SettingsKey(typeof(CodeInspectionSettings), "Use short-circuit operators")]
    public class UseShortCircuitOperatorsSettings
    {
        [SettingsEntry(true, "Enable usage of short-circuit operators")]
        public bool Enable { get; set; }
    }
}
