using System.ComponentModel.DataAnnotations;

namespace TodoAppI.DTOs;

public record PaginationRequest(
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be positive")]
    int PageNumber = 1,
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    int PageSize = 5
);