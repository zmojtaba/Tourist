namespace Backend.Infrustructure
{
    internal class InfrustructureException : Exception
    {
        public InfrustructureException(string message) : base($"Infru Exception: \"{message}\" throws from infru Layer.")
        {
        }
    }
}
