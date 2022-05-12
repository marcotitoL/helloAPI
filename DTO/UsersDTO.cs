namespace helloAPI.DTO;

public class UsersDTO{
    public string? Id {get;set;}

    public string? Email {get;set;}

    public string? Firstname {get;set;}

    public string? Lastname {get;set;}

    public string? Birthdate {get;set;}

    public string? ProfileImage {get;set;}

    public Decimal Balance {get;set;}

    public string? Roles {get;set;}

    public string? PhoneNumber {get;set;} 

    public Boolean? EmailConfirmed {get;set;}
    public Boolean? PhoneNumberConfirmed {get;set;}
}