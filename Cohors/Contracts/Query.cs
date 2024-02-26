using System.Text.Json.Serialization;

namespace Cohors.Contracts;

public sealed class Query<T> where T : class
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; set; } = default(T);
    
    // The PageSize specifies the number of items to display per page.
    public int PageSize { get; set; } = 10;
    // The PageNumber specifies the current page number.
    public int PageNumber { get; set; } = 1;
    // The TotalPages specifies the total number of pages in the database.
    public int  TotalPages { get; set; } = 1;
    
    // The SortBy specifies the column to sort by.
    public string SortBy { get; set; } = "Id";
    // The SortDirection specifies the direction to sort by. Usually asc or desc
    public string SortDirection { get; set; } = "asc";
    
    
    // The SearchTerm specifies the string to search for in the database.
    public string? SearchTerm { get; set; }
    // The SearchBy specifies the column to search for the SearchTerm. Usually the name or title column
    public string? SearchBy { get; set; }
    
    
    // The FilterBy specifies the column to filter by.
    public string? FilterBy { get; set; }
    // The FilterValue specifies the value to filter by.
    public string? FilterValue { get; set; }
    
}