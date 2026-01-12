// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Diagnostics;

namespace HabitLoggerMvc.Middleware;

public class ExceptionHandler: IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {

        await httpContext.Response.WriteAsync($"An error occurred: {exception.Message}", cancellationToken: cancellationToken);

        return true;
    }
}
