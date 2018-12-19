using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    [Table("RolePermission", Schema = "security")]
    public class RolePermission
    {   
        public Guid Id { get; set; }     
        public Guid RoleId { get; set; }        
        public Guid PermissionId { get; set; }
        
    }
}