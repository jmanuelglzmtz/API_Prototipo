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
    public class RolesController : ControllerBase
    {
        private IRoleService _roleService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public RolesController(
            IRoleService roleService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _roleService = roleService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles =  _roleService.GetAll();
            //var userDtos = _mapper.Map<IList<UserDto>>(users);
            //return Ok(userDtos);
            return Ok(roles);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]RoleDto rolDto)
        {
            
            // map dto to entity and set id
            /*
            var user = _mapper.Map<Rol>(rolDto);
            user.Id = id;
            */
            var myRol= new Role();
            myRol.Id=rolDto.Id;
            myRol.Name=rolDto.Name;
            myRol.Status=rolDto.Estatus== "Activo" ? 1 : 0;
            try 
            {
                // save 
                
                _roleService.Update(myRol);
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
        public IActionResult Register([FromBody]RoleDto rolDto)
        {
            // map dto to entity
            

            try 
            {
                var myRol= new Role();
                myRol.Id=rolDto.Id;
                myRol.Name=rolDto.Name;
                myRol.Status=rolDto.Estatus== "Activo" ? 1 : 0;
                myRol.TenantId = Guid.Parse("A4C482BF-1468-4460-BE9B-2C325926230D");
                // save 
               var create=_roleService.Create(myRol);
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
            _roleService.Delete(id);
            return Ok();
        }
    }
}
