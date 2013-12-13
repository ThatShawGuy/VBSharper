using JetBrains.ActionManagement;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.UI.RichText;

namespace VBSharper.Plugins.Refactorings.SharedToExtension
{
    [ActionHandler(ActionId)]
    public class ExtensionToSharedAction : ExtensibleRefactoringAction<ISharedToExtensionWorkflowProvider>
    {
        public const string ActionId = "ExtensionToSharedAction";

        protected override RichText Caption {
            get { return "Extension Method To Shared"; }
        }
    }
}
