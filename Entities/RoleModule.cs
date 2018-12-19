using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{ 
    [Table("RoleModule", Schema = "security")]
    public class RoleModule
    {   
        public Guid Id { get; set; }     
        public Guid RoleId { get; set; }        
        public Guid ModuleId { get; set; }
        
    }
}