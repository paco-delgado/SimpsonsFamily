using Microsoft.Extensions.Configuration;
using SimpsonsFamilyTree.Domain.Repository;
using SimpsonsFamilyTree.Repository.Neo4j;

namespace SimpsonsFamilyTree.Web.Support
{
    public static class PeopleRepositoryFactory
    {
        public static IPeopleRepository Create(IConfigurationRoot config)
        {
            string url = config.GetSection("GraphDb").GetSection("Url").Value;
            string user = config.GetSection("GraphDb").GetSection("User").Value;
            string pass = config.GetSection("GraphDb").GetSection("Pass").Value;

            return new PeopleRepository(url, user, pass);
        }
    }
}
