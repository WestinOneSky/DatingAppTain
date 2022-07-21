using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext2 _context2;
        public BuggyController(DataContext2 context2)
        {
            _context2 = context2;
        }



        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSercret()
        {
            return "Sercet Text";
        }


        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context2.Users.Find(-1);

            if (thing == null) return NotFound();

            return Ok(thing);
        }


        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context2.Users.Find(-1);

            var thingtoReturn = thing.ToString();

            return thingtoReturn;
        }


        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRRequest()
        {
            return "This was not a good Request";
        }

    }
}