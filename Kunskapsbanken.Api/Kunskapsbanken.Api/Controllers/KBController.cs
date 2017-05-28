using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kunskapsbanken.Api.Models;

namespace Kunskapsbanken.Api.Controllers
{
    [RoutePrefix("api/kunskapsbanken")]
    public class KBController : ApiController
    {
        // Visa alla tillgängliga KB
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllHowTos()
        {
            try
            {
                return Ok(new Database().SqlGetAllHowTos());
            }

            catch (Exception e)
            {
                return Content(HttpStatusCode.Forbidden, "Error code: " + e.Message);
            }
        }






        // Hämta ett KB utifrån id
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetHowToById(int id)
        {
            try
            {
                HowTo howTo = new Database().SqlGetHowToById(id);

                if (howTo != null)
                {
                    return Ok(howTo);
                }

                else
                {
                    return BadRequest("KB som du har valt, finns inte!");
                }
            }

            catch (Exception e)
            {
                return Content(HttpStatusCode.Forbidden, "Error code: " + e.Message);
            }
        }




        // Lägg till ny KB
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddHowTo(HowTo howTo)
        {
            try
            {
                new Database().SqlAddHowTo(howTo);
                return Ok(new Database().SqlGetAllHowTos().OrderByDescending(x => x.Created).FirstOrDefault());
            }

            catch (Exception e)
            {
                string msg = "Object reference not set to an instance of an object.";

                if (e.Message == msg)
                {
                    return BadRequest("Du har inte matat in något värde alls!");
                }

                else
                {
                    return Content(HttpStatusCode.Forbidden, "Error code: " + e.Message);
                }
            }

        }



        // Uppdatera en befintlig KB
        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateHowTo(HowTo howTo)
        {
            try
            {
                if (howTo.Id == 0)
                {
                    return BadRequest("Du har inte angett vilket KB som du vill uppdatera!");
                }

                else
                {
                    new Database().SqlUpdateHowTo(howTo);
                    return Ok(new Database().SqlGetAllHowTos().Where(x => x.Id == howTo.Id).FirstOrDefault());
                }
            }

            catch (Exception e)
            {
                string msg = "Object reference not set to an instance of an object.";

                if (e.Message == msg)
                {
                    return BadRequest("Du har inte matat in något värde alls!");
                }


                else
                {
                    return Content(HttpStatusCode.Forbidden, "Error code: " + e.Message);
                }
            }

        }




        //Ta bort ett KB utifrån id
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteHowTo(int id)
        {
            try
            {
                if (new Database().SqlGetHowToById(id) != null)
                {
                    new Database().SqlDeleteHowTo(id);
                    return Ok("KB med id " + id + " är nu borttagen");
                }

                else
                {
                    return BadRequest("KB som du har valt, finns inte!");
                }
            }

            catch (Exception e)
            {
                return Content(HttpStatusCode.Forbidden, "Error code: " + e.Message);
            }
        }







        // Visa alla tillgängliga KB som matchar sökord. Den försöker matcha ett ord i Description.
        [HttpGet]
        [Route("Search/{search}")]
        public IHttpActionResult SearchAllHowTos(string search)
        {
            try
            {
                List<HowTo> myHowTos = new Database().SqlSearchAllHowTos(search);

                if (myHowTos.Count() == 0)
                {
                    return BadRequest("Ingenting hittades. Sök med annat ord!");

                }
                else
                {
                    return Ok(myHowTos);
                }
            }

            catch (Exception e)
            {
                return Content(HttpStatusCode.Forbidden, "Error code: " + e.Message);
            }
        }







    }
}
