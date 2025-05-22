namespace NextERP.ModelBase.APIResult
{
    public class APISuccessResult<T> : APIBaseResult<T>
    {
        public APISuccessResult()
        {
            IsSuccess = true;
        }

        public APISuccessResult(string message, T result)
        {
            IsSuccess = true;
            Message = message;
            Result = result;
        }
    }
}
