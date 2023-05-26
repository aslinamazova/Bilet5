using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Anyar.Areas.Admin.ViewModels
{
    public class UpdateTeamVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(255)]
        public string Position { get; set; }
        [Required]
        [MaxLength(255)]
        public string JobDescription { get; set; }
        public IFormFile Photo { get; set; }
    }
}
