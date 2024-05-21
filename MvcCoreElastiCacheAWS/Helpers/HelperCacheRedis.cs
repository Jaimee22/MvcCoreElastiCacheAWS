using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Helpers
{
    public class HelperCacheRedis
    {
        private static Lazy<ConnectionMultiplexer> CreateConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("");
        });


        public ConnectionMultiplexer Connection 
        {
            get 
            {
                return CreateConnection.Value;
            }
        }



    }
}
