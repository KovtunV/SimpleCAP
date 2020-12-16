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
            
            // if you don't need a result
            // await _bus.SendAsync(model);

            // if you need a result
            // var directRes = await _bus.SendAsync<MyRequestModel, MyResponseModel>(model);

            // If you don't want to write a request type in generic method
            // var res = await _bus.Request(model).SendAsync<MyResponseModel>();

            var res = await _bus.Request(model).SendAsync<MyResponseModel>();
            return res;
        }
    }
}
