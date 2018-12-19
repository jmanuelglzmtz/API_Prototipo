using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{ 
    [Table("UserRole", Schema = "security")]
    public class UserRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }        
        public Guid RoleId { get; set; }        
    }
}