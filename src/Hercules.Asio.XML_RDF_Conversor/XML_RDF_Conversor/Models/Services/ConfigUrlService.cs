﻿// Copyright (c) UTE GNOSS - UNIVERSIDAD DE DEUSTO
// Licenciado bajo la licencia GPL 3. Ver https://www.gnu.org/licenses/gpl-3.0.html
// Proyecto Hércules ASIO Backend SGI. Ver https://www.um.es/web/hercules/proyectos/asio
// Servicio para obtener las variables de configuración de urls
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.IO;

namespace Hercules.Asio.XML_RDF_Conversor.Models.Services
{
    /// <summary>
    /// Servicio para obtener las variables de configuración de urls
    /// </summary>
    public class ConfigUrlService
    {
        /// <summary>
        /// UrlUrisFactory
        /// </summary>
        public string UrlUrisFactory { get; set; }

        private IConfiguration _configuration { get; set; }

        /// <summary>
        /// Servicio.
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigUrlService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// GetBuildConfiguration
        /// </summary>
        /// <returns></returns>
        public static IConfiguration GetBuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            return builder.Build();
        }
        /// <summary>
        /// Obtiene la url del api de uris factory que ha sido configurada
        /// </summary>
        /// <returns>uri del api uris factory</returns>
        public string GetUrlUrisFactory()
        {
            if (string.IsNullOrEmpty(UrlUrisFactory))
            {
                string connectionString = "";
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlUrisFactory"))
                {
                    connectionString = environmentVariables["UrlUrisFactory"] as string;
                }
                else
                {
                    connectionString = _configuration["UrlUrisFactory"];
                }

                UrlUrisFactory = connectionString;
            }
            return UrlUrisFactory;
        }
    }
}
