using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarciaApi.Domain.Models;

public class Location
{
    [Key]
    public string? LocationId { get; set; }
    [ForeignKey("OrderId")]
    public string? OrderId { get; set; }
    public string? CEP { get; set; }
    public string? Neighborhood { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? AdditionalAdressInfo { get; set; }
}