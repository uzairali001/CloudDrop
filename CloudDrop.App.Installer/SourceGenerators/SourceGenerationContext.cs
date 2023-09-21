using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using System.Text.Json.Serialization;

namespace CloudDrop.App.Installer.SourceGenerators;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(AuthenticationResponse))]
[JsonSerializable(typeof(AuthenticationRequest))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}