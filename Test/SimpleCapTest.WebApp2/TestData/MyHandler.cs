using System.Threading.Tasks;
using SimpleCAP.Attributes;
using SimpleCAP.Handlers;

namespace SimpleCapTest.WebApp2.TestData
{
    // Optional
    // [OverrideSubscribe(Name = "my topic name", Group = "my group name")]
    public class MyHandler : ICapEventHandler<MyRequestModel, MyResponseModel>
    {
        public Task<MyResponseModel> HandleAsync(MyRequestModel model)
        {
            var res = new MyResponseModel {Message = $"Hi {model.Name}! Are you {model.Age} years old?" };
            return Task.FromResult(res);
        }
    }
}
