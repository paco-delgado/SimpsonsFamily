using Microsoft.AspNetCore.Mvc;
using SimpsonsFamilyTree.Domain.Model;
using SimpsonsFamilyTree.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpsonsFamilyTree.Web.Controllers
{
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        private IPeopleRepository _peopleRepository;

        public PeopleController(IPeopleRepository repository)
        {
            _peopleRepository = repository;
        }

        // GET /people/{id}
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                Person person = _peopleRepository.GetPerson(id);
                return person != null ? (IActionResult) Ok(person) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //GET /people/{id}/family
        [HttpGet("{id}/family")]
        public IActionResult GetFamily(long id)
        {
            try
            {
                List<PersonFamily> personFamily = _peopleRepository.GetFamily(id);
                return Ok(personFamily);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //POST /people/{id}/children
        [HttpPost("{id}/children")]
        public IActionResult Post(long id, [FromBody]Person body)
        {
            try
            {
                if (body == null)
                {
                    return BadRequest("Person can't be null.");
                }
                long partnerId = _peopleRepository.GetPartnerId(id);
                if (partnerId == -1)
                {
                    return BadRequest($"Partner not found for person with id {id}. Child can't be added");
                }
                long childId = _peopleRepository.AddChild(body, new List<long> { id, partnerId });
                if (childId == -1)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, $"Child couldn't be added. An error occurred.");
                }
                else
                {
                    return Created($"/people/{childId}", childId);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
