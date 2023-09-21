namespace CloudDrop.Api.Core.Models.Dtos;
public class AccessTokenDto
{
    public required string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
}
