using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Pages;

public class Oops : PageModel
{
    public string Text { get; set; } = "Oops!";

    public void OnGet()
    {
        throw new Exception("muh exception");
    }
}
