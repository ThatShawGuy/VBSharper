using JetBrains.Application.Settings;
using JetBrains.ReSharper.Settings;

namespace VBSharper.Plugins.UseIsNotOperator
{
    [SettingsKey(typeof(CodeInspectionSettings),"Use IsNot operator")]
    public class UseIsNotOperatorSettings
    {
        [SettingsEntry(true, "Enable usage of IsNot operator")]
        public bool Enable { get; set; }
    }
}
