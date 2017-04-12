using SimpsonsFamilyTree.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using SimpsonsFamilyTree.Domain.Model;
using Neo4j.Driver.V1;
using System.Linq;
using SimpsonsFamilyTree.Repository.Neo4j.Extensions;

namespace SimpsonsFamilyTree.Repository.Neo4j
{
    public class PeopleRepository : IPeopleRepository
    {
        public IDriver _neo4jDriver;

        public PeopleRepository(string dbUrl, string user, string password)
        {
            _neo4jDriver = GraphDatabase.Driver(dbUrl, AuthTokens.Basic(user, password));
        }

        public long AddChild(Person child, IEnumerable<long> parentsIds)
        {
            using (var session = _neo4jDriver.Session())
            {
                return session.WriteTransaction(tx => AddChild(tx, child, parentsIds));
            }
        }

        public static long AddChild(ITransaction tx, Person child, IEnumerable<long> parentsIds)
        {
            long childId = -1;
            try
            {
                var statementTemplate = "CREATE (person:Person { Name: {Name}, LastName: {LastName}, BirthDate: {BirthDate} }) RETURN ID(person) AS Id; ";
                var statementParameters = new Dictionary<string, object> { { "Name", child.Name }, { "LastName", child.LastName }, { "BirthDate", child.BirthDate.ToString("dd/MM/yyyy") } };

                var statementResult = tx.Run(statementTemplate, statementParameters);
                var record = statementResult.Single();
                childId = record["Id"].As<long>();

                var statementChildTemplate = "MATCH (child:Person), (parent:Person) WHERE ID(child) = {childId} AND ID(parent) = {parentId} CREATE (child)-[:IS_CHILD_OF]->(parent);";
                var statementParentTemplate = "MATCH (child:Person), (parent:Person) WHERE ID(child) = {childId} AND ID(parent) = {parentId} CREATE (child)<-[:IS_PARENT_OF]-(parent);";
                foreach (var parentId in parentsIds)
                {
                    statementParameters = new Dictionary<string, object> { { "childId", childId }, { "parentId", parentId } };
                    tx.Run(statementChildTemplate, statementParameters);
                    tx.Run(statementParentTemplate, statementParameters);
                }
            }
            catch (Exception)
            {
                tx.Failure();
            }
            tx.Success();
            return childId;
        }

        public long GetPartnerId(long personId)
        {
            long partnerId = -1;
            var statementTemplate = "MATCH (p:Person)-[:IS_PARTNER_OF]->(n:Person) WHERE ID(p) = {personId} RETURN ID(n) AS partnerId";
            var statementParameters = new Dictionary<string, object> { { "personId", personId } };
            using (var session = _neo4jDriver.Session())
            {
                var statementResult = session.Run(statementTemplate, statementParameters);
                var record = statementResult.SingleOrDefault();
                if (record != null)
                {
                    partnerId = Convert.ToInt64(record["partnerId"]);
                }
            }
            return partnerId;
        }

        public List<PersonFamily> GetFamily(long id)
        {
            var personFamilyList = new List<PersonFamily>();
            var statementTemplate = "MATCH (p:Person)-[r]->(m) WHERE ID(p) = {Id} RETURN m, r";
            var statementParameters = new Dictionary<string, object> { { "Id", id } };
            using (var session = _neo4jDriver.Session())
            {
                var statementResult = session.Run(statementTemplate, statementParameters);
                foreach (var record in statementResult)
                {
                    var personNode = record["m"].As<INode>();
                    var relation = record["r"].As<IRelationship>();
                    personFamilyList.Add(new PersonFamily
                    {
                        Id = personNode.Id,
                        Name = personNode["Name"].As<string>(),
                        LastName = personNode["LastName"].As<string>(),
                        BirthDate = personNode["BirthDate"].As<string>().ToDate(),
                        Relation = relation.Type.ToCommonName()
                    });
                }
            }
            return personFamilyList;
        }

        public ParentsTree GetParentsTree(long id)
        {
            ParentsTree parentsTree = null;
            string statementTemplate = "MATCH (parent:Person)-[r:IS_PARENT_OF*]->(child:Person) WHERE ID(child) = {Id} RETURN parent, child, r, LENGTH(r) AS c ORDER BY c";
            var statementParameters = new Dictionary<string, object> { { "Id", id } };
            using (var session = _neo4jDriver.Session())
            {
                var result = session.Run(statementTemplate, statementParameters);
                foreach (var record in result)
                {
                    if (parentsTree == null)
                    {
                        var child = record["child"].As<INode>();
                        parentsTree = new ParentsTree
                        {
                            Id = child.Id,
                            Name = child["Name"].As<string>(),
                            LastName = child["LastName"].As<string>(),
                            Parents = new List<ParentsTree>()
                        };
                    }
                    var relations = record["r"].As<List<IRelationship>>();
                    var parent = record["parent"].As<INode>();
                    if (relations[0].EndNodeId == parentsTree.Id)
                    {
                        parentsTree.Parents.Add(new ParentsTree
                        {
                            Id = parent.Id,
                            Name = parent["Name"].As<string>(),
                            LastName = parent["LastName"].As<string>(),
                            Parents = new List<ParentsTree>()
                        });
                    }
                    else
                    {
                        var parentFound = parentsTree.GetParents().SingleOrDefault(p => p.Id == relations[0].EndNodeId);
                        parentFound.Parents.Add(new ParentsTree
                        {
                            Id = parent.Id,
                            Name = parent["Name"].As<string>(),
                            LastName = parent["LastName"].As<string>(),
                            Parents = new List<ParentsTree>()
                        });
                    }
                }
            }
            return parentsTree;
        }

        public Person GetPerson(long id)
        {
            var statementTemplate = "MATCH (p:Person) WHERE ID(p) = {Id} RETURN p";
            var statementParameters = new Dictionary<string, object> { { "Id", id } };
            using (var session = _neo4jDriver.Session())
            {
                var statementResult = session.Run(statementTemplate, statementParameters);
                var record = statementResult.SingleOrDefault();
                if (record == null)
                    return null;
                var personNode = record["p"].As<INode>();
                Person person = new Person
                {
                    Id = personNode.Id,
                    Name = personNode["Name"].As<string>(),
                    LastName = personNode["LastName"].As<string>(),
                    BirthDate = personNode["BirthDate"].As<string>().ToDate()
                };
                return person;
            }
        }
    }
}
