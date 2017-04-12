using System;
using System.Collections.Generic;
using System.Text;

namespace SimpsonsFamilyTree.Domain.Model
{
    public class ParentsTree
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<ParentsTree> Parents { get; set; }
    }
}
