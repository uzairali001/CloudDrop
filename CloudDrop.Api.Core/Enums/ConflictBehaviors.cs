using System.Text.Json.Serialization;

namespace CloudDrop.Api.Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<ConflictBehaviors>))]
public enum ConflictBehaviors
{
    Rename,
    Fail,
    Replace,
}
