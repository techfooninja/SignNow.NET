using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        [DataRow("test template name")]
        public async Task CreateTemplateFromDocumentSuccessfully(string templateName)
        {
            var response =
                await SignNowTestContext.Documents.CreateTemplateFromDocumentAsync(
                    new CreateTemplateFromDocumentRequest(templateName, TestPdfDocumentId));
            Assert.IsNotNull(response.Id);
            DisposableDocumentId = response.Id;
            var template = await SignNowTestContext.Documents.GetDocumentAsync(response.Id);
            Assert.AreEqual(templateName, template.Name);
        }
    }
}
