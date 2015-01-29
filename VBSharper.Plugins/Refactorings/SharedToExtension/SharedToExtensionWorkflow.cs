using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Pointers;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Refactorings.MoveStaticMembers;
using VBSharper.Plugins.Core.ExtensionMethods;

namespace VBSharper.Plugins.Refactorings.SharedToExtension
{
    public class SharedToExtensionWorkflow : MoveStaticMembersWorkflow 
    {
        public enum WorkflowDirection {
            SharedToExtension,
            ExtensionToShared,
        }

        protected readonly WorkflowDirection Direction;
        
        public List<IMethod> Methods { get; protected set; }
        public override RefactoringActionGroup ActionGroup { get { return RefactoringActionGroup.Convert; } }
        public override string HelpKeyword { get { return null; } }
        public override bool MightModifyManyDocuments { get { return true; } }
        public override string Title {
            get {
                return Direction != WorkflowDirection.SharedToExtension ? "Extension Method To &Plain Shared" : "C&onvert Shared Method To Extension";
            }
        }

        public SharedToExtensionWorkflow(ISolution solution, WorkflowDirection direction, string actionId)
            : base(solution, actionId) {
            Direction = direction;
        }

        public override IRefactoringExecuter CreateRefactoring(IRefactoringDriver driver) {
            return new SharedToExtensionRefactoring(this, Solution, driver, Direction);
        }

        public override bool IsAvailable(IDataContext context) {
            IList<ITypeMember> typeMembers;
            ITypeElement ownerType;
            var moveStaticMembersIsAvailable = IsAvailable(context, out typeMembers, out ownerType);
            if (!moveStaticMembersIsAvailable) return false;
            Methods = new List<IMethod>();

            foreach (var typeMember in typeMembers) {
                var method = typeMember != null ? typeMember as IMethod : null;
                if (method == null) return false;

                // Do a quick check to see if Extension Methods are allowed.
                var methodDeclaration = method.GetDeclarations().FirstOrDefault();
                if (methodDeclaration != null) {
                    var treeNode = methodDeclaration.CreateTreeElementPointer().GetTreeNode();
                    if (treeNode != null && !treeNode.IsVB9Supported()) return false;
                }

                if (!method.IsStatic || method.Parameters.None() || method.Parameters.First().Kind != ParameterKind.VALUE) return false;
                if (method.IsExtensionMethod ^ Direction == WorkflowDirection.ExtensionToShared) return false;

                Methods.Add(method);
            }

            return true;
        }
    }
}
