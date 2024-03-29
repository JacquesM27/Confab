﻿using System.ComponentModel.DataAnnotations;

namespace Confab.Modules.Users.Core.DTO;

public class SignInDto
{
    [EmailAddress]
    [Required] 
    public string Email { get; set; } = string.Empty;
    
    [Required] 
    public string Password { get; set; } = string.Empty;
}