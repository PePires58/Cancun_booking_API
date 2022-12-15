using Cancun.Booking.Application.Services;
using Cancun.Booking.Domain.Interfaces.Services;

namespace Cancun.Booking.Tests
{
    [TestClass]
    public class ParameterServiceTests
    {
        #region Properties
        IParameterService IParameterService { get; set; }
        #endregion

        #region Constructor
        public ParameterServiceTests()
        {
            IParameterService = new ParameterService();

            Environment.SetEnvironmentVariable("MINDAYSBOOKINGADVANCE", "1");
            Environment.SetEnvironmentVariable("MAXSTAYDAYS", "3");
            Environment.SetEnvironmentVariable("MAXDAYSBOOKINGADVANCE", "30");
        }
        #endregion

        [TestMethod]
        public void ShouldReturnANotNullObject()
        {
            Assert.IsNotNull(IParameterService.GetParameters());
        }

        [TestMethod]
        public void ShouldMaxStayDaysBeTheSameAsEnvironmentVariable()
        {
            Assert.AreEqual(IParameterService.GetParameters().MaxStayDays,
                Convert.ToInt32(Environment.GetEnvironmentVariable("MAXSTAYDAYS")));
        }

        [TestMethod]
        public void ShouldMaxDaysBookingAdvanceBeTheSameAsEnvironmentVariable()
        {
            Assert.AreEqual(IParameterService.GetParameters().MaxDaysBookingAdvance,
                Convert.ToInt32(Environment.GetEnvironmentVariable("MAXDAYSBOOKINGADVANCE")));
        }

        [TestMethod]
        public void ShouldMinDaysBookingAdvanceBeTheSameAsEnvironmentVariable()
        {
            Assert.AreEqual(IParameterService.GetParameters().MinDaysBookingAdvance,
                Convert.ToInt32(Environment.GetEnvironmentVariable("MINDAYSBOOKINGADVANCE")));
        }
    }
}
