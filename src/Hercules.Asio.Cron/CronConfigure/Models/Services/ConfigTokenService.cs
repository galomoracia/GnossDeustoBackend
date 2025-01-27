﻿// Copyright (c) UTE GNOSS - UNIVERSIDAD DE DEUSTO
// Licenciado bajo la licencia GPL 3. Ver https://www.gnu.org/licenses/gpl-3.0.html
// Proyecto Hércules ASIO Backend SGI. Ver https://www.um.es/web/hercules/proyectos/asio
// Clase para la obtención de los datos necesarios para obtener el token de acceso al apiCarga
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CronConfigure.Models.Services
{
    /// <summary>
    /// Clase para la obtención de los datos necesarios para obtener el token de acceso al apiCarga
    /// </summary> 
    [ExcludeFromCodeCoverage]
    public class ConfigTokenService
    {
        /// <summary>
        /// Authority
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// GrantType
        /// </summary>
        public string GrantType { get; set; }
        /// <summary>
        /// Scope
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// ScopeCron
        /// </summary>
        public string ScopeCron { get; set; }
        /// <summary>
        /// ClientId
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// ClientSecret
        /// </summary>
        public string ClientSecret { get; set; }
        private IConfiguration _configuration { get; set; }
        /// <summary>
        /// ConfigTokenService
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// obtiene el endpoint para la llamada de obtención del token
        /// </summary> 
        public string GetAuthority()
        {
            if (string.IsNullOrEmpty(Authority))
            {
                string authority = "";
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("Authority"))
                {
                    authority = environmentVariables["Authority"] as string;
                }
                else
                {
                    authority = _configuration["Authority"];
                }

                Authority = authority;
            }
            return Authority;
        }
    }
}
