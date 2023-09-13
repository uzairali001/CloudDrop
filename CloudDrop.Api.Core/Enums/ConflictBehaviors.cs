using System.Text.Json.Serialization;

namespace CloudDrop.Api.Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ConflictBehaviors
{
    Rename,
    Fail,
    Replace,
}
