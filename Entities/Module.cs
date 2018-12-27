using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    [Table("Module", Schema = "security")]
    public class Module
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Compenent { get; set; }
        public string Icon { get; set; }
        public int Status { get; set; }
    }
}