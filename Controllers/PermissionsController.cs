using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PermissionsController : ControllerBase
    {
        private IPermissionService _permissionService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public PermissionsController(
            IPermissionService permissionService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _permissionService = permissionService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var permissions =  _permissionService.GetAll();
            //var userDtos = _mapper.Map<IList<UserDto>>(users);
            //return Ok(userDtos);
            return Ok(permissions);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]PermissionDbDto permissionDbDto)
        {
            
            // map dto to entity and set id
            /*
            var user = _mapper.Map<Rol>(rolDto);
            user.Id = id;
            */
            var myPermission = new Permission();
            myPermission.Id = permissionDbDto.Id;
            myPermission.Type = permissionDbDto.Type;
            myPermission.Status = permissionDbDto.Status== "Activo" ? 1 : 0;
            
            try 
            {
                // save 
                
                _permissionService.Update(myPermission);
                return Ok();
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
             
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]PermissionDbDto permissionDbDto)
        {
            // map dto to entity
            

            try 
            {
                var myPermission = new Permission();
                myPermission.Id = permissionDbDto.Id;
                myPermission.Type = permissionDbDto.Type;
                myPermission.Status = permissionDbDto.Status== "Activo" ? 1 : 0;
                myPermission.TenantId = Guid.Parse("A4C482BF-1468-4460-BE9B-2C325926230D");
                // save 
               var create=_permissionService.Create(myPermission);
               if(create)
               {
                   return Ok();
               }
               else
               {
                   return BadRequest(new { message = "Error" });
               }
                
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _permissionService.Delete(id);
            return Ok();
        }
    }
}
