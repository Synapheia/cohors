using System.Text.Json.Serialization;

namespace Cohors.Contracts;

public sealed class Queries<T> where T : class
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; set; } = default(T);
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public string SortBy { get; set; } = "Id";
    public string SortDirection { get; set; } = "asc";
    public string? SearchTerm { get; set; }
    public string? SearchBy { get; set; }
    public string? FilterBy { get; set; }
    public string? FilterValue { get; set; }
}