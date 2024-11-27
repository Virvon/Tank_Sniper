namespace Assets.Sources.Services.StaticDataService
{
    public interface IConfig<TKey>
    {
        public TKey Key { get; }
    }
}