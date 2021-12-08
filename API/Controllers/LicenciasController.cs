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
    
    public class LicenciasController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Licencias
        public IQueryable<Licencia> GetLicencia()
        {
            return db.Licencia;
        }

        // GET: api/Licencias/5
        [ResponseType(typeof(Licencia))]
        public IHttpActionResult GetLicencia(int id)
        {
            Licencia licencia = db.Licencia.Find(id);
            if (licencia == null)
            {
                return NotFound();
            }

            return Ok(licencia);
        }

        // PUT: api/Licencias/5
        [ResponseType(typeof(Licencia))]
        public IHttpActionResult PutLicencia(Licencia licencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(licencia).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LicenciaExists(licencia.CodLicencia))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(licencia);
        }

        // POST: api/Licencias
        [ResponseType(typeof(Licencia))]
        public IHttpActionResult PostLicencia(Licencia licencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Licencia.Add(licencia);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = licencia.CodLicencia }, licencia);
        }

        // DELETE: api/Licencias/5
        [ResponseType(typeof(Licencia))]
        public IHttpActionResult DeleteLicencia(int id)
        {
            Licencia licencia = db.Licencia.Find(id);
            if (licencia == null)
            {
                return NotFound();
            }

            db.Licencia.Remove(licencia);
            db.SaveChanges();

            return Ok(licencia);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LicenciaExists(int id)
        {
            return db.Licencia.Count(e => e.CodLicencia == id) > 0;
        }
    }
}