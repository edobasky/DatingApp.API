using DatingApp.API.Data;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class BuggyController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public BuggyController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuth()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _dataContext.AppUsers.Find(-1);
            if (thing == null) return NotFound();

            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult<AppUser> GetServerError()
        {
            var thing = _dataContext.AppUsers.Find(-1) ?? throw new Exception("A bad thing happened");
            return thing;


        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("A bad request");
        }
    }
}
