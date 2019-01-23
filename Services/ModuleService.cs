using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Helpers;


namespace WebApi.Services
{
    public interface IModuleService
    {        
        IEnumerable<ModuleDbDto> GetAll();

        void Update(Module rol);   

        Boolean Create(Module rol);     

        Boolean Delete(Guid Id);
    }

    public class ModuleService : IModuleService
    {
        private DataContext _context;

        public ModuleService(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<ModuleDbDto> GetAll()
        {
            AppUtilities.tenantChange(_context);  

            var myModules = (from module in _context.Module
                            select new ModuleDbDto{
                                Id = module.Id,
                                Name = module.Name,
                                Component = module.Component,
                                Icon = module.Icon,
                                Status = module.Status == 1 ? "Activo" : "Inactivo"
                            }).ToList()
            ;

            return myModules;
        }

        public void Update(Module module)
        {
            AppUtilities.tenantChange(_context); 
            var mol = _context.Module.Find(module.Id);

            if (mol == null)
                throw new AppException("Module not found");            

            // update user properties
            mol.Name = module.Name;
            mol.Component = module.Component;
            mol.Status = module.Status;
            mol.Icon = module.Icon;

            _context.Module.Update(mol);
            _context.SaveChanges();
        }

        public Boolean Create(Module module)
        {
            try
            {
                AppUtilities.tenantChange(_context);             
                _context.Module.Add(module);
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
                var module = _context.Module.Find(id);
                if (module != null)
                {
                    _context.Module.Remove(module);
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