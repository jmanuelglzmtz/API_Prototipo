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
    //[Authorize]
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

        
    }
}
