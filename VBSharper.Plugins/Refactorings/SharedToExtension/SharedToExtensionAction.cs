using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.UI.ActionsRevised;
using JetBrains.UI.RichText;

namespace VBSharper.Plugins.Refactorings.SharedToExtension
{
    [Action(ActionId)]
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
