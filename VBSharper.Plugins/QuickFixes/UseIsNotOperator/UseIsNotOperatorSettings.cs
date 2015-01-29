using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace VBSharper.Plugins.QuickFixes.UseIsNotOperator
{
    [SettingsKey(typeof(CodeInspectionSettings),"Use IsNot operator")]
    public class UseIsNotOperatorSettings
    {
        [SettingsEntry(true, "Enable usage of IsNot operator")]
        public bool Enable { get; set; }
    }
}
