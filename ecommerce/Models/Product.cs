using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ecommerce.Models;

public class Product
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
}

public class CreateUpdateProductRequest
{
    [Required]
    public string? Name { get; set; }

    public string? Description { get; set; }

}


public class ProductRequestDto
{
    public string?  Name { get; set; }
}