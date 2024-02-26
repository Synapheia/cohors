namespace Heracles.Contracts;

public class QueryRequestDto
{
    public int PageSize { get; set; }

    public int PageNumber { get; set; } 
    public string SortBy { get; set; } 

    public string SortDirection { get; set; }

    public string? SearchTerm { get; set; }

    public string? SearchBy { get; set; }

    public string? FilterBy { get; set; }

    public string? FilterValue { get; set; }
}