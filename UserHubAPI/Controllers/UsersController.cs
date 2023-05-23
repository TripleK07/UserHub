
using Microsoft.AspNetCore.Mvc;
using UserHubAPI.Entities;
using UserHubAPI.Entities.Data;

namespace UserHubAPI.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserHubContext _context;

        public UsersController(ILogger<UsersController> logger, UserHubContext context)
		{
             _logger = logger;
            _context = context;
		}

        /// <summary>
        /// to use custom route name
        /// [Route("getUsers")]
        /// </summary>
        [HttpGet(Name = "getUsers")]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Users> users = _context.Users.AsEnumerable().Where(x => x.RecordStatus == 1).OrderBy(x => x.UserName);

                ///for returning custom response message
                //return Ok(new ResponseMsg("201", "User created", userList));
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("getByUserId")]
        public IActionResult GetByUserId(Guid userId)
        {
            try
            {
                var result = _context.Users.FirstOrDefault(x => x.ID == userId && x.RecordStatus == 1);

                if (result != null)
                {
                    return Ok(result);
                }
                else {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.InnerException, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost(Name = "addUsers")]
        public IActionResult Add(Users user)
        {
            try
            {
                user.CreatedDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                user.IsActive = true;

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok(user);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.InnerException, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut(Name = "editUsers")]
        public IActionResult Edit(Users user)
        {
            try
            {
                var result = _context.Users.SingleOrDefault(x=> x.ID == user.ID && x.RecordStatus == 1);
                if (result != null)
                {
                    result.UserName = user.UserName;
                    result.Email = user.Email;
                    result.IsActive = user.IsActive;
                    result.RecordStatus = user.RecordStatus;
                    result.LoginID = user.LoginID;
                    result.Password = user.Password;
                    result.ModifiedDate = DateTime.Now;

                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();

                    return Ok(user);
                }
                else {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.InnerException, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete(Name = "deleteUsers")]
        public IActionResult Delete(Guid userID)
        {
            try
            {
                var result = _context.Users.SingleOrDefault(x => x.ID == userID);
                if (result != null)
                {
                    _context.Remove(result);
                    _context.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.InnerException, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}