using System.ComponentModel.DataAnnotations;

namespace MessaginApp.API.Dtos
{
    public class UserForRegister
    {
        [Required]
        public string Username {get; set;}
        [Required]
        [StringLength(8,MinimumLength=4, ErrorMessage="You Must specity password between 4 and 8 charachters")]

        public string Password {get; set;}

        
    }
}