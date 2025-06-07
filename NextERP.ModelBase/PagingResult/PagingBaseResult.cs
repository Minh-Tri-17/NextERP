namespace NextERP.ModelBase.PagingResult
{
    public class PagingBaseResult : PagingBase
    {
        public int TotalRecord { get; set; }
        public int PageRecord { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecord / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }
}
