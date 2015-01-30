using System;
using System.Linq;
using Microsoft.Office365.SharePoint.CoreServices;
using Microsoft.Office365.SharePoint.FileServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using XAuth;

namespace SharePointManagedOMTests
{
    [TestClass]
    public class ExchangeTests
    {
        private SharePointClient client;

        [TestInitialize]
        public void Init()
        {
            var settings = Settings.SharePointPrd;

            var auth = new XAuth.Auth(settings.Auth);

            client = new SharePointClient(settings.Environment.EndpointUri, () => auth.GetAccessToken(settings.Environment.ResourceId));
        }


        [TestMethod]
        public async Task GetAllFiles()
        {
            var filesQuery = await client.Files.ExecuteAsync();
            var files = filesQuery.CurrentPage.ToArray();
            Assert.IsTrue(files.Length > 0, "We have no files");
        }

        [TestMethod]
        public async Task CreateFolder()
        {
            try
            {
                var precondition = await client.Files["TestFolder"].ToFolder().ExecuteAsync();

                // If we reached this line then the folder exists
                await precondition.DeleteAsync();
                precondition = await client.Files["TestFolder"].ToFolder().ExecuteAsync();

                Assert.Fail("Failed to delete folder");
            }
            catch(Exception ex)
            {
                ex.ToString();
            }

            var folder = new Folder { Name= "TestFolder" };

            client.Context.AddObject("files", folder);

            var requery = await client.Files["TestFolder"].ToFolder().ExecuteAsync();
            Assert.IsNotNull(requery.Id, "Creation Failed");
        }

        [TestMethod]
        public async Task CreateFolders()
        {
            try
            {
                var precondition = await client.Files["TestFolder"].ToFolder().ExecuteAsync();

                // If we reached this line then the folder exists
                await precondition.DeleteAsync();
                precondition = await client.Files["TestFolder"].ToFolder().ExecuteAsync();

                Assert.Fail("Failed to delete folder");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            var folder = new Folder { Name = "TestFolder"};
            client.Context.AddObject("files", folder);

            var requery = await client.Files["TestFolder"].ToFolder().ExecuteAsync();
            Assert.IsNotNull(requery.Id, "Creation Failed");
        }

        [TestMethod]
        public async Task AddFile()
        {

            var name = Guid.NewGuid().ToString() + ".jpg";

            var file = new File {Name = name};

            await file.UpdateAsync();

            await file.UploadAsync(Resources.ProfilePhoto);

            Assert.IsNotNull(file.Id, "Creation Failed");
        }


    }
}