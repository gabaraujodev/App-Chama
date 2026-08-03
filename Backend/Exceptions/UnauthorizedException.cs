namespace Backend.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message): base(message) 
        {
            
        }
    }
}
//401