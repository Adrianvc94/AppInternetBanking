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
    public class PermisoesController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Permisoes
        public IQueryable<Permiso> GetPermiso()
        {
            return db.Permiso;
        }

        // GET: api/Permisoes/5
        [ResponseType(typeof(Permiso))]
        public IHttpActionResult GetPermiso(int id)
        {
            Permiso permiso = db.Permiso.Find(id);
            if (permiso == null)
            {
                return NotFound();
            }

            return Ok(permiso);
        }

        // PUT: api/Permisoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPermiso(Permiso permiso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(permiso).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisoExists(permiso.CodPermiso))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(permiso);
        }

        // POST: api/Permisoes
        [ResponseType(typeof(Permiso))]
        public IHttpActionResult PostPermiso(Permiso permiso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Permiso.Add(permiso);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = permiso.CodPermiso }, permiso);
        }

        // DELETE: api/Permisoes/5
        [ResponseType(typeof(Permiso))]
        public IHttpActionResult DeletePermiso(int id)
        {
            Permiso permiso = db.Permiso.Find(id);
            if (permiso == null)
            {
                return NotFound();
            }

            db.Permiso.Remove(permiso);
            db.SaveChanges();

            return Ok(permiso);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PermisoExists(int id)
        {
            return db.Permiso.Count(e => e.CodPermiso == id) > 0;
        }
    }
}