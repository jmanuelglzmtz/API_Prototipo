using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
        public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Estatus { get; set; }
    }
}