global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // IdentityUser

global using Microsoft.AspNetCore.Mvc; // BaseController

global using Microsoft.AspNetCore.Identity; // UserManager

global using Microsoft.AspNetCore.Authorization; // [Authorize]

global using System.Security.Claims; //Claims

global using System.IdentityModel.Tokens.Jwt; // JwtRegisteredClaimNames

global using System.Security.Cryptography; //RandomNumberGenerator


global using Microsoft.AspNetCore.Hosting;


global using Microsoft.AspNetCore.Authentication.JwtBearer; // JwtBearerDefaults
global using Microsoft.IdentityModel.Tokens; //TokenValidationParameters,SymmetricSecurityKey

global using Microsoft.OpenApi.Models;

global using System.Text; //Encoding

global using System;
global using System.Collections.Generic;
global using System.Linq;

global using System.Reflection;

global using helloAPI.Models;
//global using helloAPI.Data;

global using helloAPI.Controllers;
global using helloAPI.DTO;