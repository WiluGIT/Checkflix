using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Checkflix.Data.Persistance;
using Checkflix.Models;
using Checkflix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkflix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ICheckflixRepository _repository;
        private readonly ILogger<ProductionsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(ICheckflixRepository repository, ILogger<ProductionsController> logger, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<int>> GetUnseenNotificationsCount()
        {
            try
            {
                var notificationsCount = await _repository.GetUnseenNotificationsCount(_userManager.GetUserId(User));
                return Ok(notificationsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NotificationViewModel>>> GetUnseenNotifications()
        {
            try
            {
                var notifications = await _repository.GetUnseenNotifications(_userManager.GetUserId(User));
                return Ok(_mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationViewModel>>(notifications));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> MarkAsSeen()
        {
            try
            {
                var unseenNotifications = await _repository.GetUnseenNotifications(_userManager.GetUserId(User));

                foreach (var notification in unseenNotifications)
                {
                    notification.IsSeen = true;
                }
                _repository.UpdateNotification(unseenNotifications);

                if (await _repository.SaveAll())
                {
                    return Ok("Zaktualiozowano notyfikacje");
                }
                return BadRequest("Nie udało się zaktualizować notyfikacji");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }
        }
    }
}