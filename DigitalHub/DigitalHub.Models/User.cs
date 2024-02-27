using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalHub.Models
{
    public class User : IdentityUser
    {
        [Required]
        public int Name { get; set; }
    }
}
