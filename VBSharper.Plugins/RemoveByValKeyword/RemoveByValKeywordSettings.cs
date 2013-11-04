using JetBrains.Application.Settings;
using JetBrains.ReSharper.Settings;

namespace VBSharper.Plugins.RemoveByValKeyword
{
    [SettingsKey(typeof(CodeInspectionSettings), "Remove ByVal Keyword")]
    public class RemoveByValKeywordSettings
    {
        [SettingsEntry(true, "Remove ByVal Keyword")]
        public bool Enable { get; set; }
    }
}
