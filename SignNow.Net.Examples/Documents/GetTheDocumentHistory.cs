using System.Collections.Generic;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Get Document History example
        /// </summary>
        /// <param name="documentId">Identity of the document</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns><see cref="DocumentHistoryResponse"/></returns>
        public static async Task<IReadOnlyList<DocumentHistoryResponse>>
            GetTheDocumentHistory(string documentId, SignNowContext signNowContext)
        {
            return await signNowContext.Documents
                .GetDocumentHistoryAsync(documentId)
                .ConfigureAwait(false);
        }
    }
}
