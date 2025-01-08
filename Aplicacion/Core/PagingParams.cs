namespace Aplicacion.Core;
public abstract class PagingParams
{
    public int PageNumber { get; set; } = 1;

    private const int MaxPageSize = 250;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        // set => _pageSize = value;
    }
    public string? OrderBy { get; set; }

    public bool? OrderAsc { get; set; } = true;
}
