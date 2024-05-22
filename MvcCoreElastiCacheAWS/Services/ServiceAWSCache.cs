using MvcCoreElastiCacheAWS.Helpers;
using MvcCoreElastiCacheAWS.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Services
{
    public class ServiceAWSCache
    {
        private IDatabase cache;

        public ServiceAWSCache()
        { 
            this.cache = HelperCacheRedis.Connection.GetDatabase();
        }

        public async Task<List<Coche>> GetCochesFavoritosAsync()
        {
            string jsonCoches = await this.cache.StringGetAsync("cochesfavoritos");
            if (jsonCoches == null)
            {
                return null;
            }
            else 
            {
                List<Coche> cars = JsonConvert.DeserializeObject<List<Coche>>(jsonCoches);
                return cars;
            }

        }


        public async Task AddCocheFavoritoAsync(Coche car)
        { 
            List<Coche> coches = await this.GetCochesFavoritosAsync();
            if (coches == null)
            {
                coches = new List<Coche>();
            }

            coches.Add(car);
            string jsonCoches = JsonConvert.SerializeObject(coches);
            await this.cache.StringSetAsync("cochesfavoritos", jsonCoches, TimeSpan.FromMinutes(30));

        }

        public async Task DeleteCocheFavoritoAsync(int idcoche)
        { 
            List<Coche> cars = await this.GetCochesFavoritosAsync();
            if (cars != null)
            {
                Coche cocheEliminar = cars.FirstOrDefault(x => x.IdCoche == idcoche);
                cars.Remove(cocheEliminar);
                if (cars.Count == 0)
                {
                    await this.cache.KeyDeleteAsync("cochesfavoritos");
                }
                else 
                {
                    string jsonCoches = JsonConvert.SerializeObject(cars);
                    await this.cache.StringSetAsync("cochesfavoritos", jsonCoches, TimeSpan.FromMinutes(30));
                }
            
            }
        }



    }
}
