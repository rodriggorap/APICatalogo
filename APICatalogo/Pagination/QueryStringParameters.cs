using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Pagination;

public abstract class QueryStringParameters
{
    const int maxPageSize = 50;
    [Required]
    public int PageNumber { get; set; } = 1;
    private int _pageSize = maxPageSize;
    [Required]
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
