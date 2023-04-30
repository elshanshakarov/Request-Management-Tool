namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public string Message { get; }
        public bool Success { get; }

        //get -> readonly-dir. Readonly-ler constractor-da set edile biler
        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public Result(bool success)
        {
            Success = success;
        }

    }
}
