using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StatusCodes.API.Models
{
    public class Token
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string Secret { get; set; }
    }
}
