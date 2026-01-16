namespace TodoAppI.DTOs;

// Source: https://www.c-sharpcorner.com/article/implementing-pagination-and-filtering-in-asp-net-core-8-0-api/
public class PagedResponse<T>(List<T> data, int pageNumber, int pageSize, int totalRecords)
{
    public List<T> Data { get; set; } = data;
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
    public int TotalRecords { get; set; } = totalRecords;
}