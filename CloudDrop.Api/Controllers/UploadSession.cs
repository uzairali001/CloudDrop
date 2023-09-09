using Microsoft.AspNetCore.Mvc;

namespace CloudDrop.Api.Controllers;
public class UploadSession : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
