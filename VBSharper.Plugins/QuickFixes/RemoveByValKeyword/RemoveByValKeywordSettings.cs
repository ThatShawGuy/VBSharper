using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace VBSharper.Plugins.QuickFixes.RemoveByValKeyword
{
    [SettingsKey(typeof(CodeInspectionSettings), "Remove ByVal Keyword")]
    public class RemoveByValKeywordSettings
    {
        [SettingsEntry(true, "Remove ByVal Keyword")]
        public bool Enable { get; set; }
    }
}
