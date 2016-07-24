using ConfigManager;
using NUnit.Framework;
using Shouldly;
using NSubstitute;
using System.IO;
using System.Reflection;

namespace QuickSilverSpecyfication
{
    [TestFixture]
    class ConfigurationReaderSpecification
    {
        private string path = Path.Combine(Path.GetTempPath(), "test");

        [Test, Category("Unit")]
        public void ShouldReturnConfigFile()
        {
            //Given
            IConfigurationReader reader = Substitute.For<IConfigurationReader>();
            reader.LoadConfiguration(Arg.Any<string>()).Returns(new Configuration
            {
                DirectoryPath = path,
                Filters = new string[] { "js", "html" },
                Arguments = "app.js"
            });
            //When
            Configuration configurartion =  reader.LoadConfiguration("path to json file");
            //Then
            configurartion.Arguments.ShouldBe("app.js");
            configurartion.DirectoryPath.ShouldBe(path);
            configurartion.Filters.ShouldBe(new string[] { "js", "html"});
        }

        [Test, Category("Box")]
        public void ShouldReadJsonFile()
        {
            //Given
            IConfigurationReader reader = new JSONReader();
            Assembly ass = Assembly.GetExecutingAssembly();
            string configFilePath = Path.GetDirectoryName(ass.Location);
            //When
            Configuration configuration = reader.LoadConfiguration(configFilePath);
            //Then
            configuration.Arguments.ShouldBe("app.js");
            configuration.DirectoryPath.ShouldBe(path);
            configuration.Filters.ShouldBe(new string[] { "js", "html"});
        }
    }
}
