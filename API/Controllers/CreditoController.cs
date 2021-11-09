using API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    public class CreditoController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();


        // GET: api/Credito
        [Authorize]
        public IQueryable<Credito> GetCredito()
        {
            return db.Credito;
        }

        // GET: api/Credito/5
        [Authorize]
        [ResponseType(typeof(Credito))]
        public IHttpActionResult GetCredito(int id)
        {
            Credito credito = db.Credito.Find(id);
            if (credito == null)
            {
                return NotFound();
            }

            return Ok(credito);
        }

        // PUT: api/Credito/5

        [ResponseType(typeof(Credito))]
        public IHttpActionResult PutCredito(Credito credito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(credito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreditoExists(credito.CodCredito))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(credito);
        }

        // POST: api/Credito
        [Authorize]
        [ResponseType(typeof(Credito))]
        public IHttpActionResult PostCredito(Credito credito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Credito.Add(credito);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = credito.CodCredito }, credito);
        }

        // DELETE: api/Credito/5
        [Authorize]
        [ResponseType(typeof(Credito))]
        public IHttpActionResult DeleteCredito(int id)
        {
            Credito credito = db.Credito.Find(id);
            if (credito == null)
            {
                return NotFound();
            }

            db.Credito.Remove(credito);
            db.SaveChanges();

            return Ok(credito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CreditoExists(int id)
        {
            return db.Credito.Count(e => e.CodCredito == id) > 0;
        }
    }
}