using Neo4j.Driver.V1;
using SimpsonsFamilyTree.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpsonsFamilyTree.Repository.Neo4j.Extensions
{
    public static class INodeExtensions
    {
        public static ParentsTree ConvertToParentsTree(this INode node)
        {
            return new ParentsTree
            {
                Id = node.Id,
                Name = node["Name"].As<string>(),
                LastName = node["LastName"].As<string>(),
                Parents = new List<ParentsTree>()
            };
        }
    }
}
