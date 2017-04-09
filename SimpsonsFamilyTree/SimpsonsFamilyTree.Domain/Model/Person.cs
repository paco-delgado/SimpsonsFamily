using System;
using System.Collections.Generic;
using System.Text;

namespace SimpsonsFamilyTree.Domain.Model
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
