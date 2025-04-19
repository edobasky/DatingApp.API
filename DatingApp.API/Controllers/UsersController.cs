using System.Security.Claims;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Repository;
using DatingApp.API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : ControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await userRepository.GetMembersAsync();
            return Ok(users);
        }


        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var users = await userRepository.GetMemberAsync(username);

            if (users == null) return NotFound();
            return users;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {

            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return BadRequest("Could not find user");

            mapper.Map(memberUpdateDto, user);

            if(await userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update the user");

        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("Could not find user");

            var result = await photoService.AddPhotosAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo { Url = result.SecureUrl.AbsoluteUri, PublicId = result.PublicId };

            user.Photos.Add(photo);
            // if (await userRepository.SaveAllAsync()) return mapper.Map<PhotoDto>(photo);

            if (await userRepository.SaveAllAsync()) return CreatedAtAction(nameof(GetUser),new { username = user.UserName}, mapper.Map<PhotoDto>(photo));

            return BadRequest("Problem adding photo");

        }

    }
}
