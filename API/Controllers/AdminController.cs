
using API.Data;
using API.Entities;
using API.Interfaces;
using API.Serives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        public UserManager<AppUser> _userManager;
        public IUnitOfWork _unitOfWork;
        public IPhotoService _photoService;
        public AdminController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();
            return Ok(users);
        }
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("couldnt find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove role");

            return Ok(await _userManager.GetRolesAsync(user));

        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForApproval()
        {
            var photos = await _unitOfWork.PhotoRepository.GetPhotos();
            return Ok(photos);
        }
        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("photo-approve/{id}")]
        public async Task<ActionResult> ApprovePhoto(int id)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(id);
            if(photo == null) return NotFound("No photo found");
            photo.IsApproved = true;
            var user = await _unitOfWork.UserRepository.GetUserByPhotoId(id);
            if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;
            await _unitOfWork.Complete();
            return Ok();
        }
        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("photo-disapprove/{id}")]
        public async Task<ActionResult> NonApprovePhoto(int id)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(id);
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                {
                    _unitOfWork.PhotoRepository.DeletePhoto(photo);
                }
            }
            else
            {
                _unitOfWork.PhotoRepository.DeletePhoto(photo);
            }
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}