using System.Collections.Generic;

namespace WebApi.Dtos
{
    public class UserRolModulePermissionDto
    {   
        public string UserName { get; set; }
        public string ImageProfile { get; set; }
        public string FirstName { get; set; }
        public List<RolDto>  Roles { get; set; }
    }
    public class RolDto
    {
        public string RolName { get; set; }
        public List<ModuleDto>  Modules { get; set; }
    }
    public class ModuleDto
    {
        public string ModuleName { get; set; }
        public string IconName { get; set; }
        public TypeDto Types { get; set; }
    } 
    public class TypeDto
    {
        public List<string> TypeName{ get; set; }
    }
}