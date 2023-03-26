using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatusCodes.API.Models
{
    public class Token
    {
        public int Id { get; set; }
        [ForeignKey("Email")]
        public string Email { get; set; }
        public string? TokenStr { get; set; }
    }
}
