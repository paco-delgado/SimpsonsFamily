using Microsoft.AspNetCore.Mvc;
using SimpsonsFamilyTree.Domain.Model;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpsonsFamilyTree.Web.Controllers
{
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        // GET /people/{id}
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            return Ok(new Person { Id = id, Name = "Homer", LastName = "Simpson", BirthDate = new System.DateTime(1976, 10,15) });
        }

        //GET /people/{id}/family
        [HttpGet("{id}/family")]
        public IActionResult GetFamily(long id)
        {
            return Ok(new List<PersonFamily> {
                new PersonFamily { Id = 6, Name = "Homer", LastName = "Simpson", BirthDate = new System.DateTime(1976, 10, 15), Relation = "Parent" },
                new PersonFamily { Id = 38, Name = "Marge", LastName = "Bouvier", BirthDate = new System.DateTime(1978, 11, 5), Relation = "Parent" }
            });
        }

        //POST /people/{id}/children
        [HttpPost("{id}/children")]
        public IActionResult Post(string id, [FromBody]Person body)
        {
            return Ok(234);
        }
    }
}
