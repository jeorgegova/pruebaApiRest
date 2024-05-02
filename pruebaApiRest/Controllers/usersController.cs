using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using pruebaApiRest.Models;

namespace pruebaApiRest.Controllers
{
    public class usersController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/users
        public IQueryable<users> Getusers()
        {
            return db.users;
        }

        // GET: api/users/5
        [ResponseType(typeof(users))]
        public IHttpActionResult Getusers(int id)
        {
            users users = db.users.Find(id);
            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putusers(int id, users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.id)
            {
                return BadRequest();
            }

            db.Entry(users).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/users
        [ResponseType(typeof(users))]
        public IHttpActionResult Postusers(users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.users.Add(users);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = users.id }, users);
        }

        // DELETE: api/users/5
        [ResponseType(typeof(users))]
        public IHttpActionResult Deleteusers(int id)
        {
            users users = db.users.Find(id);
            if (users == null)
            {
                return NotFound();
            }

            db.users.Remove(users);
            db.SaveChanges();

            return Ok(users);
        }

        // Método para autenticar usuarios
        [HttpPost]
        [Route("api/users/authenticate")]
        public IHttpActionResult AuthenticateUser(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = db.users.FirstOrDefault(u => u.email == model.Email && u.password == model.Password );
                if (user != null)
                {
                    // Autenticación exitosa
                    var responseData = new { Email = user.email, UserId = user.id, Password = user.password };
                    return Ok(responseData);
                }
                else
                {
                    // Usuario no válido
                    return BadRequest("Usuario inválido :(");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Método para ejecutar un procedimiento almacenado
        [HttpPost]
        [Route("api/users/run-stored-procedure")]
        public IHttpActionResult RunStoredProcedure(StoredProcedureModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (testEntities1 db = new testEntities1())
            try
                   {               
                    db.spIserta(model.Email, model.Password);
                    db.SaveChanges();
                return Ok("1");
                
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Liberar recursos
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usersExists(int id)
        {
            return db.users.Count(e => e.id == id) > 0;
        }

        // Modelo para autenticar usuarios
        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // Modelo para ejecutar un procedimiento almacenado
        public class StoredProcedureModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
