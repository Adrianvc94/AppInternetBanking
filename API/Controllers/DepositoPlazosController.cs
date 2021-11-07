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
    public class DepositoPlazosController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/DepositoPlazos
        public IQueryable<DepositoPlazo> GetDepositoPlazo()
        {
            return db.DepositoPlazo;
        }

        // GET: api/DepositoPlazos/5
        [ResponseType(typeof(DepositoPlazo))]
        public IHttpActionResult GetDepositoPlazo(int id)
        {
            DepositoPlazo depositoPlazo = db.DepositoPlazo.Find(id);
            if (depositoPlazo == null)
            {
                return NotFound();
            }

            return Ok(depositoPlazo);
        }

        // PUT: api/DepositoPlazos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDepositoPlazo(DepositoPlazo depositoPlazo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(depositoPlazo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepositoPlazoExists(depositoPlazo.CodPlazo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(depositoPlazo);
        }

        // POST: api/DepositoPlazos
        [ResponseType(typeof(DepositoPlazo))]
        public IHttpActionResult PostDepositoPlazo(DepositoPlazo depositoPlazo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DepositoPlazo.Add(depositoPlazo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = depositoPlazo.CodPlazo }, depositoPlazo);
        }

        // DELETE: api/DepositoPlazos/5
        [ResponseType(typeof(DepositoPlazo))]
        public IHttpActionResult DeleteDepositoPlazo(int id)
        {
            DepositoPlazo depositoPlazo = db.DepositoPlazo.Find(id);
            if (depositoPlazo == null)
            {
                return NotFound();
            }

            db.DepositoPlazo.Remove(depositoPlazo);
            db.SaveChanges();

            return Ok(depositoPlazo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepositoPlazoExists(int id)
        {
            return db.DepositoPlazo.Count(e => e.CodPlazo == id) > 0;
        }
    }
}