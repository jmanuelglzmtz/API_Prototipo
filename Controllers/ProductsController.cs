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
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ProductsController(
            IProductService productService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _productService = productService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        

        [HttpGet]
        public IActionResult GetAll()
        {
            var products =  _productService.GetAll();
            var productDtos = _mapper.Map<IList<ProductDto>>(products);
            return Ok(productDtos);
        }              
    }
}
