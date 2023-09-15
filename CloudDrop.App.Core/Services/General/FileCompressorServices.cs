using System.IO.Compression;

namespace CloudDrop.App.Core.Services.General;
public class FileCompressorServices
{
    public static byte[] Compress(byte[] bytes)
    {
        using var memoryStream = new MemoryStream();
        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
        {
            gZipStream.Write(bytes, 0, bytes.Length);
        }
        return memoryStream.ToArray();
    }

    public static byte[] Decompress(byte[] bytes)
    {
        using var memoryStream = new MemoryStream(bytes);
        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        {
            var decompressedBytes = new byte[memoryStream.Length];
            gZipStream.Read(decompressedBytes, 0, decompressedBytes.Length);
            return decompressedBytes;
        }
    }

    //public static async Task<Memory<byte>> DecompressAsync(byte[] bytes, CancellationToken cancellationToken = default)
    //{
    //    using var memoryStream = new MemoryStream(bytes);
    //    using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
    //    {
    //        Memory<byte> buffer = new(bytes);
    //        await gZipStream.ReadAsync(buffer, cancellationToken);

    //        return buffer;
    //    }
    //}
}
