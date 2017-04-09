using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpsonsFamilyTree.Domain.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpsonsFamilyTree.Web.Controllers
{
    [Route("[controller]")]
    public class TreeController : Controller
    {
        // GET /tree/{id}
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            return Ok(new ParentsTree {
                Id = id,
                Name = "Homer",
                LastName = "Simpson",
                Parents = new List<ParentsTree>
                {
                    new ParentsTree { Id = 654, Name = "Abraham", LastName = "Simpson" },
                    new ParentsTree { Id = 987, Name = "Penelope", LastName = "Olsen" }
                }
            });
        }
    }
}
