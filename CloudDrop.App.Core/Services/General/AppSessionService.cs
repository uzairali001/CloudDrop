using CloudDrop.App.Core.Models.Dtos;

namespace CloudDrop.App.Core.Services.General;
public class AppSessionService
{
    public bool IsAuthenticated { get => Session is not null; }
    public AuthenticationDto? Session { get; private set; }

    public void SetSession(AuthenticationDto dto)
    {
        Session = dto;
    }
}
