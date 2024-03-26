using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_carrinho_api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<HealthCheckController> _logger;

    public HealthCheckController(ILogger<HealthCheckController> logger)
    {
        _logger = logger;
    }

}
