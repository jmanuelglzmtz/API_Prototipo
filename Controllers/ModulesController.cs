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
    public class ModulesController : ControllerBase
    {
        private IModuleService _moduleService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ModulesController(
            IModuleService moduleService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _moduleService = moduleService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var permissions =  _moduleService.GetAll();
            //var userDtos = _mapper.Map<IList<UserDto>>(users);
            //return Ok(userDtos);
            return Ok(permissions);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]ModuleDbDto moduleDbDto)
        {
            
            // map dto to entity and set id
            /*
            var user = _mapper.Map<Rol>(rolDto);
            user.Id = id;
            */
            var myModule = new Module();
            myModule.Id = moduleDbDto.Id;
            myModule.Name = moduleDbDto.Name;
            myModule.Component = moduleDbDto.Component;
            myModule.Icon = moduleDbDto.Icon;
            myModule.Status = moduleDbDto.Status== "Activo" ? 1 : 0;
            
            try 
            {
                // save 
                
                _moduleService.Update(myModule);
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
        public IActionResult Register([FromBody]ModuleDbDto moduleDbDto)
        {
            // map dto to entity
            

            try 
            {
                var myModule = new Module();
                myModule.Id = moduleDbDto.Id;
                myModule.Name = moduleDbDto.Name;
                myModule.Component = moduleDbDto.Component;
                myModule.Icon = moduleDbDto.Icon;
                myModule.Status = moduleDbDto.Status== "Activo" ? 1 : 0;
                myModule.TenantId = Guid.Parse("A4C482BF-1468-4460-BE9B-2C325926230D");
                // save 
               var create=_moduleService.Create(myModule);
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
            _moduleService.Delete(id);
            return Ok();
        }
    }
}
