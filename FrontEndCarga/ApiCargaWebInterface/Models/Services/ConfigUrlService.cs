﻿// Copyright (c) UTE GNOSS - UNIVERSIDAD DE DEUSTO
// Licenciado bajo la licencia GPL 3. Ver https://www.gnu.org/licenses/gpl-3.0.html
// Proyecto Hércules ASIO Backend SGI. Ver https://www.um.es/web/hercules/proyectos/asio
// Servicio para obtener las variables de configuración de urls
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.IO;

namespace ApiCargaWebInterface.Models.Services
{
    /// <summary>
    /// Servicio para obtener las variables de configuración de urls
    /// </summary>
    public class ConfigUrlService
    {
        public IConfigurationRoot Configuration { get; set; }
        public string Url { get; set; }
        public string UrlUris { get; set; }
        public string UrlDocumentacion { get; set; }
        /// <summary>
        /// Obtiene la url del api de carga que ha sido configurada
        /// </summary>
        /// <returns>uri del api carga</returns>
        public string GetUrl()
        {
            if (string.IsNullOrEmpty(Url))
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();
                string connectionString = "";
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("ConfigUrl"))
                {
                    connectionString = environmentVariables["ConfigUrl"] as string;
                }
                else
                {
                    connectionString = Configuration["ConfigUrl"];
                }
                
                Url = connectionString;
            }
            return Url;
        }
        /// <summary>
        /// Obtiene la url del api de uris factory que ha sido configurada
        /// </summary>
        /// <returns>uri del api uris factory</returns>
        public string GetUrlUrisFactory()
        {
            if (string.IsNullOrEmpty(UrlUris))
            {

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();
                string connectionString = "";
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("ConfigUrlUrisFactory"))
                {
                    connectionString = environmentVariables["ConfigUrlUrisFactory"] as string;
                }
                else
                {
                    connectionString = Configuration["ConfigUrlUrisFactory"];
                }

                UrlUris = connectionString;
            }
            return UrlUris;
        }

        /// <summary>
        /// Obtiene la url del api de documentación que ha sido configurada
        /// </summary>
        /// <returns>uri del api uris factory</returns>
        public string GetUrlDocumentacion()
        {
            if (string.IsNullOrEmpty(UrlDocumentacion))
            {

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();
                string connectionString = "";
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("ConfigUrlDocumentacion"))
                {
                    connectionString = environmentVariables["ConfigUrlDocumentacion"] as string;
                }
                else
                {
                    connectionString = Configuration["ConfigUrlDocumentacion"];
                }

                UrlDocumentacion = connectionString;
            }
            return UrlDocumentacion;
        }
    }
}
