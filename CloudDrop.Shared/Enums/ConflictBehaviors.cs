using System.Text.Json.Serialization;

namespace CloudDrop.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<ConflictBehaviors>))]
public enum ConflictBehaviors
{
    Rename,
    Fail,
    Replace,
}
