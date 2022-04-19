namespace helloAPI.DTO;

public class UpdateUser{
    public string? Firstname {get;set;}

    public string? Lastname {get;set;}

    [DataType(DataType.Date)]
    public DateTime Birthdate {get;set;}

}