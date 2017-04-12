using SimpsonsFamilyTree.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpsonsFamilyTree.Domain.Repository
{
    public interface IPeopleRepository
    {
        Person GetPerson(long id);
        List<PersonFamily> GetFamily(long id);
        ParentsTree GetParentsTree(long id);
        long AddChild(Person child, IEnumerable<long> parentsIds);
        long GetPartnerId(long personId);
    }
}
