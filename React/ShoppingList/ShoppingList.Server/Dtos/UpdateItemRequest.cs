using System.ComponentModel.DataAnnotations;

namespace ShoppingList.Server.Dtos;

public class UpdateItemRequest
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int ShoppingListId { get; set; }
    [Required, MinLength(1)]
    public string Name { get; set; } = string.Empty;
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    public bool IsChecked { get; set; }
}