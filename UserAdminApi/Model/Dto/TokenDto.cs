using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserAdminApi.Model.Dto
{
    public class TokenDto
    {
        [Required]
        public string Role { get; set; }
        [Required]
        public String Token { get; set; }

    }
}
