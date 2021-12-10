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
    public class MarchamoManager
    {
        string UrlBase = "http://localhost:49220/api/Marchamos/";

        /// <summary>
        /// Metodo que inicializa el objeto HttpClient
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
        /// <returns>Objeto Marchamo</returns>
        public async Task<Marchamo> ObtenerMarchamo(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Marchamo>(response);
        }

        /// <summary>
        /// Este metodo obtiene la lista de marchamos del API
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Lista IEnumerable de objetos Marchamo</returns>
        public async Task<IEnumerable<Marchamo>> ObtenerMarchamos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Marchamo>>(response);
        }


        public async Task<Marchamo> Ingresar(Marchamo servicio, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(servicio), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Marchamo>(await response.Content.ReadAsStringAsync());
        }


        public async Task<Marchamo> Actualizar(Marchamo servicio, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(servicio), 
                Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Marchamo>(await response.
                Content.ReadAsStringAsync());
        }


        public async Task<Marchamo> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Marchamo>(await response.Content.ReadAsStringAsync());
        }
    }
}