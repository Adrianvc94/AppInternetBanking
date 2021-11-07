using AppWebInternetBanking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppWebInternetBanking.Controllers
{
    public class DepositoPlazoManager
    {

        string UrlBase = "http://localhost:49220/api/DepositoPlazos/";

        /// <summary>
        /// Metodo que inicializa el objeto HttpClientDepositoPlazo
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Objeto HttpClient con los headers inicializados</returns>
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        /// <summary>
        /// Este metodo obtiene un servicio proveniente del API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="codigo"></param>
        /// <returns>Objeto DepositoPlazo</returns>
        public async Task<DepositoPlazo> ObtenerDepositoPlazo(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<DepositoPlazo>(response);
        }

        /// <summary>
        /// Este metodo obtiene la lista de depositos a plazo del API
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Lista IEnumerable de objetos DepositoPlazo</returns>
        public async Task<IEnumerable<DepositoPlazo>> ObtenerDepositoPlazos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<DepositoPlazo>>(response);
        }


        public async Task<DepositoPlazo> Ingresar(DepositoPlazo servicio, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(servicio), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<DepositoPlazo>(await response.Content.ReadAsStringAsync());
        }


        public async Task<DepositoPlazo> Actualizar(DepositoPlazo servicio, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(servicio), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<DepositoPlazo>(await response.Content.ReadAsStringAsync());
        }


        public async Task<DepositoPlazo> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<DepositoPlazo>(await response.Content.ReadAsStringAsync());
        }



    }
}