using XTI_Credentials;

namespace XTI_ScheduledJobIntegrationTests;

internal sealed class AdminUserCredentials
{
    public AdminUserCredentials()
    {
        Value = new CredentialValue("schdjobadmin", Guid.NewGuid().ToString());
    }

    public CredentialValue Value { get; }
}
