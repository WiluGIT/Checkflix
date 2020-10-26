using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Checkflix.Data.Persistance;
using Checkflix.Models;
using Checkflix.Models.Enums;
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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NotificationViewModel>>> GetNotifications()
        {
            try
            {
                var notifications = await _repository.GetNotifications(_userManager.GetUserId(User));
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
        public async Task<ActionResult<ResponseViewModel>> MarkAsSeen()
        {
            try
            {
                var validationResponse = new ResponseViewModel
                {
                    Status = ResponseStatus.Success,
                    Messages = new List<string>()
                };
                var unseenNotifications = await _repository.GetUnseenUserNotifications(_userManager.GetUserId(User));

                if (unseenNotifications != null)
                {
                    foreach (var notification in unseenNotifications)
                    {
                        notification.IsSeen = true;
                    }
                    _repository.UpdateUserNotification(unseenNotifications);

                    if (await _repository.SaveAll())
                    {
                        validationResponse.Messages.Add("Zaktualiozowano notyfikacje");
                        return Ok(validationResponse);
                    }
                }
                validationResponse.Status = ResponseStatus.Error;
                validationResponse.Messages.Add("Nie udało się zaktualizować notyfikacji");
                return Ok(validationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseViewModel>> PostNotification([FromBody] NotificationFormViewModel notificationFormViewModel)
        {
            var mapNotification = _mapper.Map<NotificationFormViewModel, Notification>(notificationFormViewModel);

            var response = ValidateNotificaiton(mapNotification);
            if (response.Status == ResponseStatus.Error)
                return response;

            var usersToSend = await _repository.GetUsersByNotificationPreferences(notificationFormViewModel);

            foreach (var userId in usersToSend)
            {
                var applicationUserNotification = new ApplicationUserNotification
                {
                    Notification = mapNotification,
                    ApplicationUserId = userId
                };
                _repository.AddApplicationUserNotification(applicationUserNotification);
            }

            if (await _repository.SaveAll())
            {
                response.Messages.Add("Notyfikacja została wysłana");
                response.Data = (NotificationViewModel)_mapper.Map<Notification, NotificationViewModel>(mapNotification);
                return CreatedAtAction("PostNotification", new { id = mapNotification.NotificationId }, response);
            }

            response.Messages.Add("Notyfikacja nie została wysłana");
                response.Status = ResponseStatus.Error;
                return BadRequest(response);
        }

        private ResponseViewModel ValidateNotificaiton(Notification notification)
        {
            var validationResponse = new ResponseViewModel
            {
                Status = ResponseStatus.Success,
                Messages = new List<string>()
            };

            if (notification == null)
            {
                validationResponse.Status = ResponseStatus.Error;
                validationResponse.Messages.Add("Podana notyfikacja jest pusta");
                return validationResponse;
            }

            return validationResponse;
        }
    }
}