namespace helloAPI.Models;

public record Products{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; internal set; }

    public string? Guid {get;set;}

    public string? Name {get;set;}

    public string? Description {get; set;}

    public int Qty {get;set;}

    [DataType(DataType.Date)]
    public DateTime Date {get;set;}

    [Precision(10,2)]
    public decimal Price {get;set;}

    [ForeignKey("productsCategory")]
    public int CategoryId {get;set;}
    public ProductsCategory productsCategory {get;set;} = null!;

    [ForeignKey("userDetails")]
    public int UserId {get;set;}
    public UserDetails userDetails {get;set;} = null!;


}