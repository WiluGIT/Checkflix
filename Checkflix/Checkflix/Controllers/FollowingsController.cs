using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Checkflix.Data.Persistance;
using Checkflix.Models;
using Checkflix.Models.Enums;
using Checkflix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkflix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FollowingsController : ControllerBase
    {
        private readonly ICheckflixRepository _repository;
        private readonly ILogger<ProductionsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public FollowingsController(ICheckflixRepository repository, ILogger<ProductionsController> logger, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostFollowing([FromBody] string followeeId)
        {
            try
            {
                var follower = await _userManager.GetUserAsync(User);

                if (follower != null)
                {
                    if (follower.Id == followeeId)
                    {
                        return BadRequest("Nie możesz zaobserować siebie");
                    }

                    if (_repository.ValidateFollowing(follower.Id, followeeId))
                    {
                        var existingFollowing = await _repository.GetFollowing(follower.Id, followeeId);
                        _repository.RemoveFollowing(existingFollowing);

                        if (await _repository.SaveAll())
                        {
                            return Ok("Odobserwowano");
                        }

                        return BadRequest("Nie udało się odobserwować");
                    }

                    var following = new Following
                    {
                        FollowerId = follower.Id,
                        FolloweeId = followeeId
                    };

                    _repository.AddFollowing(following);

                    if (await _repository.SaveAll())
                        return Ok("Zaobserwowano");

                }
                return BadRequest("Wystąpił błąd przy obserwacji");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<FollowingCountViewModel>> GetFollowingCount()
        {
            try
            {
                var user = await _repository.GetUserFollowings(_userManager.GetUserId(User));
                if (user != null)
                {
                    var followingCountVm = new FollowingCountViewModel
                    {
                        FolloweesCount = user.Followees.Count(),
                        FollowersCount = user.Followers.Count()
                    };

                    return Ok(followingCountVm);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetFollowers()
        {
            try
            {
                var user = await _repository.GetUserFollowings(_userManager.GetUserId(User));
                if (user != null)
                {
                    var userFollowers = user.Followers.Select(x => new UserViewModel
                    {
                        Id = x.Follower.Id,
                        UserName = x.Follower.UserName
                    });
                    return Ok(userFollowers);
                }

                return BadRequest("Nie udało się pobrać obserwatorów");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetFollowees()
        {
            try
            {
                var user = await _repository.GetUserFollowings(_userManager.GetUserId(User));
                if (user != null)
                {
                    var userFollowees = user.Followees.Select(x => new UserViewModel
                    {
                        Id = x.Followe.Id,
                        UserName = x.Followe.UserName
                    });
                    return Ok(userFollowees);
                }

                return BadRequest("Nie udało się pobrać obserwowanych");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<FollowingCountViewModel>> GetUsers([FromQuery]string searchQuery)
        {
            try
            {
                var users = await _repository.GetUsersSearch(searchQuery);
                if (users != null)
                {
                    return Ok(users);
                }
                return BadRequest("Nie udało się pobrać uytkowników");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get users{ex}");
                return BadRequest("Bad request");
            }
        }
    }
}