using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DBSet;
using FormulaOne.Entities.Dtos.Requests;
using FormulaOne.Entities.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers
{
    public class AchievementsController : BaseController
    {
        public AchievementsController(
            IUnitOfWork unitOfWork, 
            IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        [HttpGet]
        [Route("{driverId:guid}")]
        public async Task<IActionResult> GetDriverAchievements(Guid driverId)
        {
            var driverAchievements = await _unitOfWork.Achievements.GetDriverAchievementAsync(driverId);
            if (driverAchievements == null)
            {
                return NotFound("Achievement not Found");
            }
            var result = _mapper.Map<DriverAchievementResponse>(driverAchievements);
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAchivement([FromBody] CreateDriverAchievementRequest achievement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _mapper.Map<Achievement>(achievement);
            await _unitOfWork.Achievements.Add(result);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetDriverAchievements), new { driverId = result.DriverId }, result);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateAchievements([FromBody] UpdateDriverAchievementRequest achievementRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the achievement exists
            var result = _mapper.Map<Achievement>(achievementRequest);
            var existingAchievement = await _unitOfWork.Achievements.GetDriverAchievementAsync(achievementRequest.DriverId);
            if (existingAchievement == null)
            {
                return NotFound("Achievement not found");
            }

            // Map the updated properties
            _mapper.Map(achievementRequest, existingAchievement);

            // Update the achievement
            await _unitOfWork.Achievements.Update(result);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

    }
}
