using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpsonsFamilyTree.Domain.Model;
using SimpsonsFamilyTree.Domain.Repository;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpsonsFamilyTree.Web.Controllers
{
    [Route("[controller]")]
    public class TreeController : Controller
    {
        private IPeopleRepository PeopleRepository { get; set; }

        public TreeController(IPeopleRepository repository)
        {
            PeopleRepository = repository;
        }

        // GET /tree/{id}
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                ParentsTree parentsTree = PeopleRepository.GetParentsTree(id);
                return parentsTree != null ? (IActionResult)Ok(parentsTree) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
