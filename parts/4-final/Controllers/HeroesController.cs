using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace coreheroes.Controllers
{
    public class HeroesController : Controller
    {
    

        [Route("api/heroes")]
        [HttpGet]
        public ActionResult GetHeroes()
        {
            return Ok(new [] {
     new { id= 11, name= "Mr. Nic" },
     new { id= 12, name= "Narco" },
     new { id= 13, name= "Bombasto" },
     new { id= 14, name= "Celeritas" },
     new { id= 15, name= "Magneta" },
     new { id= 16, name= "RubberMan" },
     new { id= 17, name= "Dynama" },
     new { id= 18, name= "Dr IQ" },
     new { id= 19, name= "Magma" },
     new { id= 20, name= "Tornado" }
            });
        }


        [Route("api/heroes/{id}")]
        [HttpGet]
        public ActionResult GetHero(int id)
        {
            return Ok(new { id= 11, name= "Mr. Nic" });
        }


        [Route("api/heroes/search")]
        [HttpGet]
        public ActionResult Search(string q)
        {
            return GetHeroes();
        }

    }
}
