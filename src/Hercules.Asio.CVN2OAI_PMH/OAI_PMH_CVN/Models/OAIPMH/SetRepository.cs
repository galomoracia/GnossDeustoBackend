﻿// Copyright (c) UTE GNOSS - UNIVERSIDAD DE DEUSTO
// Licenciado bajo la licencia GPL 3. Ver https://www.gnu.org/licenses/gpl-3.0.html
// Proyecto Hércules ASIO Backend SGI. Ver https://www.um.es/web/hercules/proyectos/asio
// Implementación de ISetRepository
using System.Collections.Generic;
using System.Linq;

namespace OaiPmhNet.Models.OAIPMH
{
    /// <summary>
    /// Implementación de ISetRepository
    /// </summary>
    public class SetRepository : ISetRepository
    {
        private readonly IOaiConfiguration _configuration;
        private readonly IList<Set> _sets;

        /// <summary>
        /// Cinstructor
        /// </summary>
        /// <param name="configuration">Configuración OAI-PMH</param>
        /// <param name="sets">Lista de sets disponibles</param>
        public SetRepository(IOaiConfiguration configuration)
        {
            _configuration = configuration;

            Set set = new Set();
            set.Spec = "cvn";
            set.Name = "Currículum Vítae Normalizado";
            set.Description = "Currículum Vítae Normalizado";

            _sets = new List<Set>();
            _sets.Add(set);
        }

        /// <summary>
        /// Obtiene los sets del repositorio en función de los argumentos pasados
        /// </summary>
        /// <param name="arguments">Parámetros de la consulta</param>        
        /// <param name="resumptionToken">Token de reanudación</param>
        /// <returns></returns>
        public SetContainer GetSets(ArgumentContainer arguments, IResumptionToken resumptionToken = null)
        {
            SetContainer container = new SetContainer();
            IQueryable<Set> sets = _sets.AsQueryable().OrderBy(s => s.Name);
            int totalCount = sets.Count();
            container.Sets = sets.Take(_configuration.PageSize);
            return container;
        }
    }
}
