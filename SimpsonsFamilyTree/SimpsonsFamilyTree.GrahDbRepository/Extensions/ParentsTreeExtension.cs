using SimpsonsFamilyTree.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpsonsFamilyTree.Repository.Neo4j.Extensions
{
    public static class ParentsTreeExtension
    {
        public static IEnumerable<ParentsTree> GetParents(this ParentsTree root)
        {
            var parentTrees = new Stack<ParentsTree>(new[] { root });
            while (parentTrees.Any())
            {
                ParentsTree parentTree = parentTrees.Pop();
                yield return parentTree;
                foreach (var p in parentTree.Parents) parentTrees.Push(p);
            }
        }
    }
}
