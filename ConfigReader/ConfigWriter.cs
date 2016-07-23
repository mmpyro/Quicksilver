namespace ConfigManager
{
    public interface IConfigurationWriter
    {
        void Save(string path, Configuration configuration);
    }
}
