using XTI_Credentials;
using XTI_WebAppClient;

namespace XTI_ScheduledJobIntegrationTests;

internal sealed class AdminUserXtiToken : AuthenticatorXtiToken
{
    public AdminUserXtiToken(IAuthClient authClient, AdminUserCredentials credentials) 
        : base(authClient, new SimpleCredentials(credentials.Value))
    {
    }
}
