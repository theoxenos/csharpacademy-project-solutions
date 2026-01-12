// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitLoggerMvc.Models;

public class ErrorPageModel : PageModel
{
    public string? ErrorMessage { get; set; }
}
