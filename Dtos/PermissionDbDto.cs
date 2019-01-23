using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class PermissionDbDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}