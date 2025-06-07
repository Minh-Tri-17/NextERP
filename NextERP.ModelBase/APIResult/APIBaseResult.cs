namespace NextERP.ModelBase.APIResult
{
    public class APIBaseResult<T>
    {
        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public T? Result { get; set; }
    }
}
