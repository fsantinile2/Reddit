namespace Reddit.Core.Interfaces
{
    public interface INewsService
    {
        public Task<int> Get(string token);

    }
}
