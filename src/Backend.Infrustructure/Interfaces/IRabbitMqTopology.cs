namespace Backend.Infrustructure.Interfaces
{
    public interface IRabbitMqTopology
    {
        Task EnsureTopologyAsync(CancellationToken ct);
    }
}
