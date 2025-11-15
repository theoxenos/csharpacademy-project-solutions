namespace ShoppingList.Server.Models;

public class ShoppingListModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ItemModel> Items { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}