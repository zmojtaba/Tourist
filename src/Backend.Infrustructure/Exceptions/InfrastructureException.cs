namespace Backend.Infrustructure.Exceptions
{
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string message)  : base($"\n Infrastructure Exception: {message} \n \n")
        { }
    }
}
