using System.ComponentModel.DataAnnotations;

namespace ShoppingList.Server.Dtos;

public class CreateItemRequest
{
    [Required]
    public int ShoppingListId { get; set; }
    public string Name { get; set; } = string.Empty;
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}