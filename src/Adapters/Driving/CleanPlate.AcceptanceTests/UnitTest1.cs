using CleanPlate.WebAPI.Controllers;
using System;
using System.Linq;
using Xunit;

namespace CleanPlate.AcceptanceTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var controller = new WeatherForecastController();
            var result = controller.Get();

            Assert.Equal(5, result.Count());
        }
    }
}
