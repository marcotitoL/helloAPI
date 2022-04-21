namespace helloAPI.Models;

public record ProductsCategory{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;internal set;}

    public string? Guid {get;set;}

    public string? CategoryName {get;set;}

}