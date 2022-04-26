namespace helloAPI.DTO;

public class RegisterUser{

    [EmailAddress,Required]
    public string? Email {get;set;}

    [Required]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^\+(?:[0-9]●?){6,14}[0-9]$", ErrorMessage = "Invalid Phone format")]
    public string? Phone {get;set;}

    public string Firstname { get; set; } = null!;
    public string Lastname {get;set;} = null!;

    [Required,DataType(DataType.Password)]
    public string? Password {get;set;}

    [Required,DataType(DataType.Date)]
    public DateTime Birthdate {get;set;}

}