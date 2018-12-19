using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    [Table("Permission", Schema = "security")]
    public class Permission
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
    }
}