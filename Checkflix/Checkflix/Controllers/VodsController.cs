using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkflix.Data.Persistance;
using Checkflix.Models;
using Checkflix.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkflix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VodsController : ControllerBase
    {
        private readonly ICheckflixRepository _repository;
        private readonly ILogger<ProductionsController> _logger;
        private readonly IMapper _mapper;

        public VodsController(ICheckflixRepository repository, ILogger<ProductionsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

        }

        // GET: api/Vods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VodViewModel>>> GetVods()
        {
            try
            {
                var vods = await _repository.GetAllVods();
                if (vods == null)
                    return NotFound();
                     
                return Ok(_mapper.Map<IEnumerable<Vod>, IEnumerable<VodViewModel>>(vods));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get vods {ex}");
                return BadRequest("Couldn't get vods");
            }

        }
        [HttpGet]
        public async Task<ActionResult<VodCountViewModel>> GetVodCount()
        {
            var countViewModel = await _repository.GetVodCount();

            return countViewModel;
        }
    }
}