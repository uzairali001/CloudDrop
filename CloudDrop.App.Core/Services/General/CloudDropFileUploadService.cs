using CloudDrop.App.Core.Contracts.Services.General;

namespace CloudDrop.App.Core.Services.General;
internal class CloudDropFileUploadService : FileUploadBaseServices, IFileUploadService
{
    //public async Task UploadAsync()
    //{
    //    HttpClient httpClient = new HttpClient();
    //    var content = new ByteArrayContent(new byte[] { });

    //    content.Headers.

    //    await httpClient.PutAsync("", content);







    //    var request = new HttpRequestMessage(HttpMethod.Put, apiUrl);

    //    // Set the content length of the request.
    //    request.ContentLength = chunk.Length;

    //    // Set the content type of the request.
    //    request.Headers.Add("Content-Type", "application/octet-stream");

    //    // Set the content range of the request.
    //    request.Headers.Add("Content-Range", $"bytes {0}-{chunk.Length - 1}/{chunk.Length}");

    //    // Set the request body.
    //    request.Content.Write(chunk);

    //    // Execute the request and get the response.
    //    HttpResponseMessage response = await httpClient.SendAsync(request);



    //    // Check the response status code.
    //    if (response.StatusCode != HttpStatusCode.OK)
    //    {
    //        throw new Exception("Failed to upload file chunk.");
    //    }
    //}
    public Task UploadAsync()
    {
        throw new NotImplementedException();
    }
}
