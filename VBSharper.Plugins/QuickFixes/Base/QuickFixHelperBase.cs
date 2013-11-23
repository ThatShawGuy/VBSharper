using System.Collections.Generic;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Tree;

namespace VBSharper.Plugins.QuickFixes.Base
{
    public interface IQuickFixHelper<in T> where T : ITreeNode
    {
        void ApplyQuickFix(IFile file);
        void ApplyQuickFix(T treeNode);
    }

    public abstract class QuickFixHelperBase<T> : IQuickFixHelper<T> where T : ITreeNode
    {
        public class QuickFixTreeNodeDocumentRange : TreeNodeDocumentRange<T>
        {
            public QuickFixTreeNodeDocumentRange(T treeNode, DocumentRange documentRange) : base(treeNode, documentRange) {}
        }

        public void ApplyQuickFix(IFile file) {
            var quickFixIteration = 0;
            const int quickFixReprocessingLimit = 20;

            while (true) {
                quickFixIteration++;
                var treeNodes = GetTreeNodeDocumentRanges(file).Select(range => range.TreeNode).ToList();

                bool needToReanalyzeFileAndReapplyQuickFix = false;

                foreach (var treeNode in treeNodes) {
                    if (treeNode.IsValid()) {
                        ApplyQuickFix(treeNode);
                    }
                    else {
                        // A previous fix invalidated the current node, we need to re-analyze and re-process the file.
                        needToReanalyzeFileAndReapplyQuickFix = true;
                    }
                }

                if (needToReanalyzeFileAndReapplyQuickFix && quickFixIteration < quickFixReprocessingLimit) continue;
                break;
            }
        }

        public abstract IEnumerable<QuickFixTreeNodeDocumentRange> GetTreeNodeDocumentRanges(IFile file);
        public abstract void ApplyQuickFix(T treeNode);
    }

    public class TreeNodeDocumentRange<T> where T : ITreeNode
    {
        public T TreeNode;
        public DocumentRange DocumentRange;

        public TreeNodeDocumentRange(T treeNode, DocumentRange documentRange) {
            TreeNode = treeNode;
            DocumentRange = documentRange;
        }
    }
}
