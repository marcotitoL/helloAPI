namespace helloAPI.DTO;

public class LoginUser{

    [Required,EmailAddress]
    public string? email { get; set; }

    [Required,DataType(DataType.Password)]
    public string? password { get; set; }
}