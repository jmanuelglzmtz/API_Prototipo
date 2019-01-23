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

        void Update(Role rol);   

        Boolean Create(Role rol);     

        Boolean Delete(Guid Id);
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
                                Id = rol.Id,
                                Name = rol.Name,
                                Status = rol.Status == 1 ? "Activo" : "Inactivo"
                            }).ToList()
            ;

            return myRoles;
        }

        public void Update(Role role)
        {
            AppUtilities.tenantChange(_context); 
            var rol = _context.Role.Find(role.Id);

            if (rol == null)
                throw new AppException("Role not found");            

            // update user properties
            rol.Name = role.Name;
            rol.Status = role.Status;

            _context.Role.Update(rol);
            _context.SaveChanges();
        }

        public Boolean Create(Role role)
        {
            try
            {
                AppUtilities.tenantChange(_context);             
                _context.Role.Add(role);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }                        
        }

        public Boolean Delete(Guid id)
        {
            try
            {
                AppUtilities.tenantChange(_context);
                var rol = _context.Role.Find(id);
                if (rol != null)
                {
                    _context.Role.Remove(rol);
                    _context.SaveChanges();
                }
                return true;
            }
            catch 
            {
                return false;
            }            
        }
    }
}