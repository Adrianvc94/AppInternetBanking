using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;

namespace API.Controllers
{
    [Authorize]
    public class SobresController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/DepositoPlazos
        public IQueryable<Sobre> GetSobre()
        {
            return db.Sobre;
        }

        // GET: api/DepositoPlazos/5
        [ResponseType(typeof(Sobre))]
        public IHttpActionResult GetSobre(int id)
        {
            Sobre sobre = db.Sobre.Find(id);
            if (sobre == null)
            {
                return NotFound();
            }

            return Ok(sobre);
        }

        // PUT: api/Servicios/5
        [ResponseType(typeof(Sobre))]
        public IHttpActionResult PutSobre(Sobre sobre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(sobre).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SobreExists(sobre.CodSobre))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(sobre);
        }

        // POST: api/Servicios
        [ResponseType(typeof(Sobre))]
        public IHttpActionResult PostServicio(Sobre sobre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sobre.Add(sobre);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sobre.CodSobre }, sobre);
        }

        // DELETE: api/Servicios/5
        [ResponseType(typeof(Sobre))]
        public IHttpActionResult DeleteSobre(int id)
        {
            Sobre sobre = db.Sobre.Find(id);
            if (sobre == null)
            {
                return NotFound();
            }

            db.Sobre.Remove(sobre);
            db.SaveChanges();

            return Ok(sobre);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SobreExists(int id)
        {
            return db.Sobre.Count(e => e.CodSobre == id) > 0;
        }
    }
}