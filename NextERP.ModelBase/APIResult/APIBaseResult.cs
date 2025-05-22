namespace NextERP.ModelBase.APIResult
{
    public class APIBaseResult<T>
    {
        public string? Message { get; set; }

        public bool IsSuccess { get; set; }

        public T? Result { get; set; }
    }
}
