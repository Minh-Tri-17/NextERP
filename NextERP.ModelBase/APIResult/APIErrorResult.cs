namespace NextERP.ModelBase.APIResult
{
    public class APIErrorResult<T> : APIBaseResult<T>
    {
        public APIErrorResult()
        {
            IsSuccess = false;
        }

        public APIErrorResult(string message)
        {
            IsSuccess = false;
            Message = message;
        }
    }
}
