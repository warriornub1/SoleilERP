namespace SERP.Application.Common
{
    public class PagingUtilities
    {
        public static readonly int DEFAULT_PAGE = 1;
        public static readonly int DEFAULT_PAGING_SIZE = 10;

        public int Page { get; set; }
        public int Size { get; set; }

        public PagingUtilities(int page, int size)
        {
            Page = page;
            Size = size;
        }

        public static int GetPageNumber(int? page)
        {
            if (page == null || page < 1)
            {
                page = DEFAULT_PAGE;
            }
            return (int)page;
        }

        public static int GetPageSize(int? size)
        {
            if (size == null || size < 1)
            {
                size = DEFAULT_PAGING_SIZE;
            }
            return (int)size;
        }

        public static int GetSkipRow(int page, int size)
        {
            return (page - 1) * size;
        }

        public static PagingUtilities GetPageable(int? page, int? size)
        {
            PagingUtilities pageable = new PagingUtilities(GetPageNumber(page), GetPageSize(size));
            return pageable;
        }
    }
}
