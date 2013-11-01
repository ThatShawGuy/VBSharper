using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace VBSharper.Plugins
{
    [ActionHandler("VBSharper.Plugins.About")]
    public class AboutAction : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate) {
            // return true or false to enable/disable this action
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute) {
            MessageBox.Show(
                "VBSharper\nThatShawGuy\n\nReSharper Extensions for VB.Net",
                "About VBSharper",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}