using JetBrains.ActionManagement;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.UI.RichText;

namespace VBSharper.Plugins.Refactorings.SharedToExtension
{
    [ActionHandler(ActionId)]
    public class SharedToExtensionAction : ExtensibleRefactoringAction<ISharedToExtensionWorkflowProvider>
    {
        public const string ActionId = "SharedToExtensionAction";

        protected override RichText Caption {
            get {
                return "Shared To Extension Method";
            }
        }
    }
}
