namespace Cancun.Booking.Tests
{
    internal static class EnvironmentVariablesForTesting
    {
        internal static void Configure()
        {
            Environment.SetEnvironmentVariable("MINDAYSBOOKINGADVANCE", "1");
            Environment.SetEnvironmentVariable("MAXSTAYDAYS", "3");
            Environment.SetEnvironmentVariable("MAXDAYSBOOKINGADVANCE", "30");
        }
    }
}
