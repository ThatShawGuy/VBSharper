using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.UI.ActionsRevised;
using JetBrains.UI.RichText;

namespace VBSharper.Plugins.Refactorings.SharedToExtension
{
    [Action(ActionId)]
    public class ExtensionToSharedAction : ExtensibleRefactoringAction<ISharedToExtensionWorkflowProvider>
    {
        public const string ActionId = "ExtensionToSharedAction";

        protected override RichText Caption {
            get { return "Extension Method To Shared"; }
        }
    }
}
