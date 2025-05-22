namespace NextERP.ModelBase.PagingResult
{
    public class PagingResult<T> : PagingBaseResult
    {
        public List<T>? Items { get; set; }
    }
}
