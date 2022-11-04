using System.ComponentModel.DataAnnotations;

namespace Task11.Dtos;

public class LoginDto
{
    [Required]
    public string? Login { get; set; }
   
    [Required]
    public string? Password { get; set; }
}
