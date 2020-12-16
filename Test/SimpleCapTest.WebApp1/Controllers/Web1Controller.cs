using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SimpleCAP.Contracts.Bus;
using SimpleCapTest.WebApp1.TestData;

namespace SimpleCapTest.WebApp1.Controllers
{
    [ApiController]
    public class Web1Controller : ControllerBase
    {
        private readonly ISimpleCapBus _bus;

        public Web1Controller(ISimpleCapBus bus)
        {
            _bus = bus;
        }

        [HttpGet("/api/send")]
        public async Task<MyResponseModel> Get()
        {
            var model = new MyRequestModel
            {
                Name = "Vitaly",
                Age = 26.256m
            };

            var res = await _bus.Request(model).SendAsync<MyResponseModel>();
            return res;
        }
    }
}
