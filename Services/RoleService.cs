using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Helpers;


namespace WebApi.Services
{
    public interface IRoleService
    {        
        IEnumerable<RoleDto> GetAll();        
    }

    public class RoleService : IRoleService
    {
        private DataContext _context;

        public RoleService(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<RoleDto> GetAll()
        {
            AppUtilities.tenantChange(_context);  

            var myRoles = (from rol in _context.Role
                            select new RoleDto{
                                Name = rol.Name,
                                Estatus = rol.Status == 1 ? "Activo" : "Inactivo"
                            }).ToList()
            ;

            return myRoles;
        }


    }
}