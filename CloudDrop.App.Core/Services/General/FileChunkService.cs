using System.Runtime.CompilerServices;

namespace CloudDrop.App.Core.Services.General;
public static class FileChunkService
{
    public static async IAsyncEnumerable<Memory<byte>> ChunkAsync(string filePath, int chunkSize = 320 * 1024,
        [EnumeratorCancellation] CancellationToken cancellationToken = default) // 320 KiB
    {
        // Create a new file stream to read the file from.
        using FileStream fileStream = File.OpenRead(filePath);
        // Calculate the number of chunks in the file.
        int numChunks = (int)Math.Ceiling((fileStream.Length / (decimal)chunkSize));

        // Create a list to store the chunks.
        //List<byte[]> chunks = new List<byte[]>();

        // Iterate over the chunks and read them from the file.
        for (int i = 0; i < numChunks; i++)
        {
            // Get the start and end offsets of the current chunk.
            int startOffset = i * chunkSize;
            int endOffset = (int)Math.Min((i + 1) * chunkSize, fileStream.Length);

            // Create a new Memory<T> of byte to store the current chunk.
            Memory<byte> chunk = new(new byte[endOffset - startOffset]);

            // Read the current chunk from the file asynchronously.
            await fileStream.ReadAsync(chunk, cancellationToken);

            // Yield return the current chunk.
            yield return chunk;
        }
    }
}
