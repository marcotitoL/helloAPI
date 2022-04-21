namespace helloAPI.DTO;

public class ViewProducts{

    public string? Id {get; internal set;}

    public string? Name {get;set;}

    public string? Description {get; set;}

    public int Qty {get;set;}

    [Precision(10,2)]
    public decimal Price {get;set;}

    public idDTO Category {get;set;} = null!;

    public idDTO Seller {get;set;} = null!;

}