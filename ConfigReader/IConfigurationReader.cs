namespace ConfigReader
{
    public interface IConfigurationReader
    {
        Configuration LoadConfiguration(string configurationFilePath);
        Configuration LoadConfiguration();
    }
}