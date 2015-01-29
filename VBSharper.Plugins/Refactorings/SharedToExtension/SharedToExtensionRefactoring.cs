using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Pointers;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Refactorings.MoveStaticMembers;

namespace VBSharper.Plugins.Refactorings.SharedToExtension
{
    public class SharedToExtensionRefactoring : MoveStaticMembersRefactoring 
    {
        public List<IReferencePointer> ReferencePointers { get; protected set; }
        public SharedToExtensionWorkflow.WorkflowDirection Direction { get; protected set; }
        public List<IMethod> Methods { get; protected set; }

        public SharedToExtensionRefactoring(SharedToExtensionWorkflow sharedToExtensionWorkflow, ISolution solution, IRefactoringDriver driver, SharedToExtensionWorkflow.WorkflowDirection direction) 
            : base(sharedToExtensionWorkflow, solution, driver) {
                Direction = direction;
                ReferencePointers = new List<IReferencePointer>();
        }

        public override bool Execute(IProgressIndicator pi) {
            const int totalWorkUnits = 2;
            pi.Start(totalWorkUnits);
            return base.Execute(pi.CreateSubProgress(1.0)) && ConvertMethods(pi.CreateSubProgress(1.0));
        }

        private bool ConvertMethods(IProgressIndicator pi) {
            if (base.NewMembers == null) return false;

            const int totalWorkUnits = 3;
            pi.Start(totalWorkUnits);

            var myWorkflow = Workflow as SharedToExtensionWorkflow;
            if (myWorkflow != null)
                Methods = base.NewMembers.OfType<IMethod>().ToList();
            
            using (var progressIndicator = pi.CreateSubProgress(1.0))
                FindUsages(progressIndicator);

            var isSharedToExtension = (this.Direction == SharedToExtensionWorkflow.WorkflowDirection.SharedToExtension);

            this.Methods.ForEachWithProgress(pi.CreateSubProgress(1.0), "", 
                method => {
                    this.Constructors[method.PresentationLanguage].MakeFirstPrameterThis(method, isSharedToExtension, this.Driver);
                    method.GetPsiServices().Caches.Update();
                });
           
            if (isSharedToExtension)
                using (var progressIndicator = pi.CreateSubProgress(1.0))
                    MakeCallExtension(progressIndicator);
            else
                using (var progressIndicator = pi.CreateSubProgress(1.0))
                    MakeCallShared(progressIndicator);
                       
            return true;
        }

        private void MakeCallShared(IProgressIndicator pi)
        {
            var sharedToExtensionHelper = new SharedToExtensionHelper();
            var validReferences = ReferencePointers
                .Select(pointer => pointer.GetReference())
                .Where(reference => reference != null && reference.IsValid())
                .ToList();

            validReferences.ForEachWithProgress(pi, "Converting Extension Method Calls to Shared Method Calls...", sharedToExtensionHelper.MakeCallShared);
        }

        private void MakeCallExtension(IProgressIndicator pi)
        {
            var sharedToExtensionHelper = new SharedToExtensionHelper();
            var validReferences = ReferencePointers
                .Select(pointer => pointer.GetReference())
                .Where(reference => reference != null && reference.IsValid())
                .ToList();

            validReferences.ForEachWithProgress(pi, "Converting Shared Method Calls to Extension Method Calls...", sharedToExtensionHelper.MakeCallExtension);
        }

        private void FindUsages(IProgressIndicator pi) {
            pi.TaskName = "Finding usages...";
            pi.CurrentItemText = "";

            base.NewMembers.ToList().ForEachWithProgress(pi, "",
                newMember => {
                    ReferencePointers.AddRange(
                        newMember.GetPsiServices().Finder
                            .FindReferences(newMember, newMember.GetSearchDomain(), pi)
                            .ToList()
                            .Select(r => r.CreateReferencePointer()));
                });
        }
    }
}