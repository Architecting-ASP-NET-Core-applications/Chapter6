using Microsoft.Extensions.Options;

namespace Chapter6CustomMiddleware.Models;

///
/// Examples for page 22
///
public class MyService
{
    private readonly MyServiceConfig settings;
    public MyService(IOptions<MyServiceConfig> settings)
        => this.settings = settings.Value;
}

