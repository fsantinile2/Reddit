namespace Reddit.Core.Interfaces
{
    public interface IAuthorizeService
    {
        public Task<string> Authorize();
    }
}
