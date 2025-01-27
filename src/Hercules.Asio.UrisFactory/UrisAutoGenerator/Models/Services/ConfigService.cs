﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hercules.Asio.UrisFactory.Models.Services
{
    /// <summary>
    /// ConfigService
    /// </summary>
    public class ConfigService
    {
        private IConfiguration _configuration { get; set; }
        private string Base { get; set; }

        /// <summary>
        /// ConfigService
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigService(IConfiguration configuration)
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

        ///<summary>
        ///Método que obtiene la ConfigUrl configurada
        ///</summary>
        public string GetBase()
        {
            if (string.IsNullOrEmpty(Base))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                string baseUrl = "";
                if (environmentVariables.Contains("Base"))
                {
                    baseUrl = environmentVariables["Base"] as string;
                }
                else
                {
                    baseUrl = _configuration["Base"];
                }
                Base = baseUrl;
            }
            return Base;
        }
    }
}
