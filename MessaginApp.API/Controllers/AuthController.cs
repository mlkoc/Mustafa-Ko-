using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MessaginApp.API.Data;
using MessaginApp.API.Dtos;
using MessaginApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace MessaginApp.API.Controllers
{
    //talepler yapılırken kimlik doğrulma yapılır
    [Route("api/[controller]")]
    [ApiController]
    //validationların kullanılabilmesi için ve dtos la bağlantı kurmak için
    public class AuthController:ControllerBase
    {  
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config){
            _repo=repo;
            _config=config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegister userForRegister){
            
           if (!ModelState.IsValid){
               return BadRequest(ModelState);
           }
           
            userForRegister.Username =userForRegister.Username.ToLower();

            if (await _repo.UserExists(userForRegister.Username)){
                return BadRequest("Username already exists");
            }

            var userToCreate=new User(){
                    Username=userForRegister.Username
            };

            var createdUsed = _repo.Register(userToCreate,userForRegister.Password);

            return StatusCode(201);

            
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login (UserForLoginDto userForLoginDto){

            var UserFromRepo = await _repo.Login(userForLoginDto.Username,userForLoginDto.Password);

            if(UserFromRepo==null)
                return Unauthorized();
                 //nameIden. veritabanındaki kullanıcın id si
                //claims gereksiz bilgiler
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,UserFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,UserFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:token").Value));
            var creds = new  SigningCredentials(key,SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor(){
                    Subject=new ClaimsIdentity(claims),
                    Expires=DateTime.Now.AddDays(1),
                    SigningCredentials = creds
            };
            //token oluşturucu
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
           return Ok(new {
                token = tokenHandler.WriteToken(token),
           }); 
        }
    
        
    }
}