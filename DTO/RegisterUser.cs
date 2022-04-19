namespace helloAPI.DTO;

public class RegisterUser{

    [EmailAddress,Required]
    public string? Email {get;set;}

    public string Firstname { get; set; } = null!;
    public string Lastname {get;set;} = null!;

    [Required,DataType(DataType.Password)]
    public string? Password {get;set;}

    [Required,DataType(DataType.Date)]
    public DateTime Birthdate {get;set;}

}