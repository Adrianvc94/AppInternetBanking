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
    public class LicenciaManager
    {
        string UrlBase = "http://localhost:49220/api/Licencias/";

        /// <summary>
        /// Metodo que inicializa el objeto HttpClient
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Objeto HttpClient con los headers inicializados</returns>
        /// 
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
        /// <returns>Objeto Servicio</returns>
        /// 
        public async Task<Licencia> ObtenerLicencia(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Licencia>(response);
        }

        /// <summary>
        /// Este metodo obtiene la lista de servicios del API
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Lista IEnumerable de objetos Servicio</returns>
        public async Task<IEnumerable<Licencia>> ObtenerLicencias(string token)
        {
            HttpClient httpClient = GetClient(token);
            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Licencia>>(response);
        }

        public async Task<Licencia> Ingresar(Licencia licencia, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(licencia), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Licencia>(await response.Content.ReadAsStringAsync());
        }


        public async Task<Licencia> Actualizar(Licencia licencia, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(licencia), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Licencia>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Licencia> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);
            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Licencia>(await response.Content.ReadAsStringAsync());
        }


    }
}