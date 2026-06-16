
using Google.Apis.Gmail.v1;
namespace Service.Helper;
public class GmailServiceHelper
{
    static string[] Scopes = { GmailService.Scope.GmailSend };

    private GmailService _service;

    public GmailServiceHelper(string credential, string tokenPath, GmailService service)
    {
        _service = service;
    }
}