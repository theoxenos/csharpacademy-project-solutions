namespace ShoppingList.Server.Models;

public class ItemModel
{
    public int Id { get; set; }
    public int ShoppingListId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool IsChecked { get; set; }
    public DateTime CreatedAt { get; set; }
}