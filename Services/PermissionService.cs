using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Helpers;


namespace WebApi.Services
{
    public interface IPermissionService
    {        
        IEnumerable<PermissionDbDto> GetAll();

        void Update(Permission rol);   

        Boolean Create(Permission rol);     

        Boolean Delete(Guid Id);
    }

    public class PermissionService : IPermissionService
    {
        private DataContext _context;

        public PermissionService(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<PermissionDbDto> GetAll()
        {
            AppUtilities.tenantChange(_context);  

            var myPermissions = (from permission in _context.Permission
                            select new PermissionDbDto{
                                Id = permission.Id,
                                Type = permission.Type,
                                Status = permission.Status == 1 ? "Activo" : "Inactivo"
                            }).ToList()
            ;

            return myPermissions;
        }

        public void Update(Permission permission)
        {
            AppUtilities.tenantChange(_context); 
            var myPermission = _context.Permission.Find(permission.Id);

            if (myPermission == null)
                throw new AppException("Permission not found");            

            // update user properties
            myPermission.Type = permission.Type;
            myPermission.Status = permission.Status;

            _context.Permission.Update(myPermission);
            _context.SaveChanges();
        }

        public Boolean Create(Permission permission)
        {
            try
            {
                AppUtilities.tenantChange(_context);             
                _context.Permission.Add(permission);
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
                var myPermission = _context.Permission.Find(id);
                if (myPermission != null)
                {
                    _context.Permission.Remove(myPermission);
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