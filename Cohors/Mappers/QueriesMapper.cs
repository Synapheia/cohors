using Cohors.Contracts;
using Heracles.Contracts;

namespace Cohors.Mappers;

public static class QueriesMapper
{
    // This method maps a QueryRequestDto to a QueryResponseDto
    public static QueryResponseDto<T> MapToQueryResponseDto<T>(this QueryRequestDto queryRequestDto ) where T : class
    {
        return new QueryResponseDto<T>
        {
            PageSize = queryRequestDto.PageSize > 0 ? queryRequestDto.PageSize : new QueryResponseDto<T>().PageSize,
            PageNumber = queryRequestDto.PageNumber > 0 ? queryRequestDto.PageNumber : new QueryResponseDto<T>().PageNumber,
            SortBy = queryRequestDto.SortBy ?? new QueryResponseDto<T>().SortBy,
            SortDirection = queryRequestDto.SortDirection ?? new QueryResponseDto<T>().SortDirection,
            SearchTerm = queryRequestDto.SearchTerm,
            SearchBy = queryRequestDto.SearchBy,
            FilterBy = queryRequestDto.FilterBy,
            FilterValue = queryRequestDto.FilterValue
        };
    }
}