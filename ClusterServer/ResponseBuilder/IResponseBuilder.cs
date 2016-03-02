namespace ClusterServer.ResponseBuilder
{
    public interface IResponseBuilder
    {
        byte[] Build(string content);
    }
}