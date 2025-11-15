namespace ShoppingList.Server.Models;

public class ShoppingList
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Item> Items { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}