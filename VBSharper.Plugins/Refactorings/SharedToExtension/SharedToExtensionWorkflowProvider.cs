using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Refactorings.Workflow;
using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

namespace VBSharper.Plugins.Refactorings.SharedToExtension
{
    [RefactoringWorkflowProvider]
    public class SharedToExtensionWorkflowProvider : ISharedToExtensionWorkflowProvider
    {
        public IEnumerable<IRefactoringWorkflow> CreateWorkflow(IDataContext dataContext) {
            yield return new SharedToExtensionWorkflow(
                dataContext.GetData(DataConstants.SOLUTION),
                SharedToExtensionWorkflow.WorkflowDirection.SharedToExtension,
                SharedToExtensionAction.ActionId);
        }
    }
}
