﻿// Copyright (c) UTE GNOSS - UNIVERSIDAD DE DEUSTO
// Licenciado bajo la licencia GPL 3. Ver https://www.gnu.org/licenses/gpl-3.0.html
// Proyecto Hércules ASIO Backend SGI. Ver https://www.um.es/web/hercules/proyectos/asio
// Clase para crear una sincronización 
using API_CARGA.Extras.Excepciones;
using API_CARGA.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace API_CARGA.Models.Services
{
    ///<summary>
    ///Clase para crear una sincronización
    ///</summary>
    public class OaiPublishRDFService
    {
        readonly EntityContext _context;
        readonly ICallNeedPublishData _publishData;
        readonly TokenBearer _token;
        readonly ConfigUrlService _configUrlService;

        public OaiPublishRDFService(EntityContext context, ICallNeedPublishData publishData, CallTokenService tokenService, ConfigUrlService configUrlService)
        {
            if (tokenService != null)
            {
                _token = tokenService.CallTokenCarga();
            }
            _context = context;
            _publishData = publishData;
            _configUrlService = configUrlService;
        }

        /// <summary>
        /// Hace la sincronización del repositorio
        /// </summary>
        /// <param name="identifier">fIdentificador del repositorio</param>
        /// <param name="fechaFrom">fecha a partir de la cual se debe actualizar</param>
        /// <param name="set">tipo del objeto, usado para filtrar por agrupaciones</param>
        /// <param name="codigoObjeto">codigo del objeto a sincronizar, es necesario pasar el parametro set si se quiere pasar este parámetro</param>
        /// <param name="jobId">En el caso de que haya sido una tarea la que ha lanzado la acción representa el identificador de la tarea</param>
        ///<param name="jobCreatedDate">En el caso de que haya sido una tarea la que ha lanzado la acción representa la fecha de creación de dicha tarea</param>
        public void PublishRepositories(Guid identifier, DateTime? fechaFrom = null, string set = null, string codigoObjeto = null, string jobId = null, DateTime? jobCreatedDate = null)
        {
            bool validationException = false;
            StringBuilder exception = new StringBuilder();

            IdentifierOAIPMH lastSyncro = null;
            try
            {
                //Obtenemos los metadataformat del repositorio
                List<string> metadataformats = CallListMetadataFormats(identifier);
                string granularity = CallGranularity(identifier);

                //SI los metadataformat contienen "rdf" trtabajamos con el
                string metadataformat = "";
                if (metadataformats.Contains("rdf"))
                {
                    metadataformat = "rdf";
                }
                else
                {
                    //Si no, hacemos una peticion al servicio conversor para que nos liste los tipos disponible
                    List<string> types = JsonConvert.DeserializeObject<List<string>>(CallGetConfigurationsFiles());
                    bool ismetadata = false;
                    foreach (string format in metadataformats)
                    {
                        if (types.Contains(format))
                        {
                            //En caso de que si haya alguno procedemos a llamar al metodo de conversion con ese formato
                            metadataformat = format;
                            ismetadata = true;
                        }
                    }
                    if (!ismetadata)
                    {
                        //Si en el conversor no hay ningún tipo de los que vienen del OAI-PMH lanzamos exception
                        throw new Exception("Los metadataformat " + string.Join(",", metadataformats) + " no son válidos.");
                    }
                }
                if (codigoObjeto == null)
                {
                    string fechaFromString = null;
                    if (fechaFrom.HasValue)
                    {
                        if (granularity.ToUpper() == "YYYY-MM-DD")
                        {
                            fechaFromString = $"{fechaFrom.Value.ToString("yyyy-MM-dd")}";
                        }
                        else
                        {
                            fechaFromString = $"{fechaFrom.Value.ToString("u", CultureInfo.InvariantCulture).Replace(" ", "T")}";
                        }
                    }

                    List<IdentifierOAIPMH> listIdentifier = CallListIdentifier(identifier, metadataformat, set, fechaFromString);
                    int totalCount = listIdentifier.Count();
                    foreach (IdentifierOAIPMH identifierOAIPMH in listIdentifier)
                    {
                        if (!string.IsNullOrEmpty(jobId))
                        {
                            AddProcessingState(identifierOAIPMH.Identifier, identifier, jobId, listIdentifier.IndexOf(identifierOAIPMH), totalCount);
                        }
                        if (set == "openaire_cris_orgunits")
                        {
                            identifierOAIPMH.Identifier = identifierOAIPMH.Identifier.Replace("oai:metis.ru.nl:", "oai:metis.ru.nl:OrgUnits/");
                        }
                        else if (set == "openaire_cris_publications")
                        {
                            identifierOAIPMH.Identifier = identifierOAIPMH.Identifier.Replace("oai:metis.ru.nl:", "oai:metis.ru.nl:Publications/");
                        }
                        else if (set == "openaire_cris_projects")
                        {
                            identifierOAIPMH.Identifier=identifierOAIPMH.Identifier.Replace("oai:metis.ru.nl:", "oai:metis.ru.nl:Projects/");
                        }
                        else if (set == "openaire_cris_products")
                        {
                            identifierOAIPMH.Identifier = identifierOAIPMH.Identifier.Replace("oai:metis.ru.nl:", "oai:metis.ru.nl:Products/");
                        }
                        else if (set == "openaire_cris_persons")
                        {
                            identifierOAIPMH.Identifier = identifierOAIPMH.Identifier.Replace("oai:metis.ru.nl:", "oai:metis.ru.nl:Persons/");
                        }

                        string record = CallGetRecord(identifier, metadataformat, identifierOAIPMH.Identifier);

                        string rdf = "";

                        if (metadataformat == "rdf")
                        {
                            rdf = record;
                        }
                        else
                        {
                            rdf = CallXMLConverter(record, metadataformat);
                        }
                        try
                        {
                            _publishData.CallDataValidate(rdf, identifier, _token);
                            _publishData.CallDataPublish(rdf, jobId, true, _token);
                        }
                        catch (Exception ex)
                        {
                            validationException = true;
                            exception.AppendLine(ex.Message);
                            throw ex;
                        }
                        lastSyncro = identifierOAIPMH;
                        if (lastSyncro != null)
                        {
                            AddSyncro(lastSyncro, set, identifier);
                        }
                    }
                }
                else
                {
                    string record = CallGetRecord(identifier, metadataformat, codigoObjeto);
                    string rdf = "";
                    if (metadataformat == "rdf")
                    {
                        rdf = record;
                    }
                    else
                    {
                        rdf = CallXMLConverter(record, metadataformat);
                    }
                    _publishData.CallDataValidate(rdf, identifier, _token);
                    _publishData.CallDataPublish(rdf, jobId, false, _token);
                }
                if (validationException)
                {
                    throw new Exception(exception.ToString());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Añade un objeto de sincronización en base de datos
        /// </summary>
        /// <param name="lastSyncro">Objeto identificador de OAIPMH que contiene la fecha</param>
        /// <param name="repositoryId">Identificador del repositorio</param>
        /// <param name="set">tipo del objeto, usado para filtrar por agrupaciones</param>
        private void AddSyncro(IdentifierOAIPMH lastSyncro, string set, Guid repositoryId)
        {
            //Actualizamos la fecha de la última sincronización           
            RepositoryConfig repositoryConfig = _context.RepositoryConfig.Include(item => item.RepositoryConfigSet).FirstOrDefault(x => x.RepositoryConfigID == repositoryId);
            if (set != null)
            {
                //En caso de que venga el set
                if (repositoryConfig.RepositoryConfigSet.FirstOrDefault(x => x.Set == set) != null)
                {
                    //actualizamos la fila de la tabla RepositoryConfigSet con la fecha
                    repositoryConfig.RepositoryConfigSet.FirstOrDefault(x => x.Set == set).LastUpdate = lastSyncro.Fecha;
                }
                else
                {
                    //Creamos la fila de la tabla RepositoryConfigSet con la fecha
                    _context.RepositoryConfigSet.Add(new RepositoryConfigSet()
                    {
                        RepositoryConfigSetID = Guid.NewGuid(),
                        LastUpdate = lastSyncro.Fecha,
                        RepositoryID = repositoryId,
                        Set = set
                    });
                }
            }
            else
            {
                //En caso de que no venga el set
                if (repositoryConfig.RepositoryConfigSet.FirstOrDefault(x => x.Set == "-") != null)
                {
                    //Actualizamos la fila de la tabla RepositoryConfigSet(con el campo set '-') con la fecha
                    repositoryConfig.RepositoryConfigSet.FirstOrDefault(x => x.Set == "-").LastUpdate = lastSyncro.Fecha;
                }
                else
                {
                    //Creamos la fila de la tabla RepositoryConfigSet con la fecha
                    _context.RepositoryConfigSet.Add(new RepositoryConfigSet()
                    {
                        RepositoryConfigSetID = Guid.NewGuid(),
                        LastUpdate = lastSyncro.Fecha,
                        RepositoryID = repositoryId,
                        Set = "-"
                    });
                }
            }


            if (_context.RepositorySync.Any(item => item.RepositoryId.Equals(repositoryId)))
            {
                if (!string.IsNullOrEmpty(set))
                {
                    RepositorySync repoSync = _context.RepositorySync.FirstOrDefault(item => item.RepositoryId.Equals(repositoryId) && item.Set == null);
                    repoSync.UltimaFechaDeSincronizacion = lastSyncro.Fecha;

                }
                else
                {
                    RepositorySync repoSync = _context.RepositorySync.FirstOrDefault(item => item.RepositoryId.Equals(repositoryId) && item.Set.Equals(set));
                    if (repoSync == null)
                    {
                        repoSync.UltimaFechaDeSincronizacion = lastSyncro.Fecha;
                    }
                    else
                    {
                        RepositorySync repoSyncAdd = new RepositorySync()
                        {
                            Id = Guid.NewGuid(),
                            RepositoryId = repositoryId,
                            Set = set,
                            UltimaFechaDeSincronizacion = lastSyncro.Fecha
                        };
                        _context.RepositorySync.Add(repoSyncAdd);
                    }
                }
            }
            else
            {
                RepositorySync repoSyncAdd = new RepositorySync()
                {
                    Id = Guid.NewGuid(),
                    RepositoryId = repositoryId,
                    Set = null,
                    UltimaFechaDeSincronizacion = lastSyncro.Fecha
                };
                _context.RepositorySync.Add(repoSyncAdd);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Añade o actualiza a la tabla ProcessingJobState una entrada para saber en que estado se encuentra la sincronización
        /// </summary>
        /// <param name="identifierOAIPMH">Objeto identificador de OAIPMH </param>
        /// <param name="repositoryId">Identificador del repositorio</param>
        /// <param name="jobId">Identificador de la tarea</param>
        /// <param name="index">Número del elemento que se está procesando</param>
        /// <param name="totalOfElements">Elementos a procesar</param>
        public void AddProcessingState(string identifierOAIPMH, Guid repositoryId, string jobId, int index, int totalOfElements)
        {
            var processingJobState = _context.ProcessingJobState.FirstOrDefault(item => item.JobId.Equals(jobId));
            if (processingJobState == null)
            {
                ProcessingJobState processingJobStateNew = new ProcessingJobState()
                {
                    Id = Guid.NewGuid(),
                    JobId = jobId,
                    LastIdentifierOAIPMH = identifierOAIPMH,
                    ProcessNumIdentifierOAIPMH = index,
                    RepositoryId = repositoryId,
                    TotalNumIdentifierOAIPMH = totalOfElements
                };
                _context.ProcessingJobState.Add(processingJobStateNew);
            }
            else
            {
                processingJobState.LastIdentifierOAIPMH = identifierOAIPMH;
                processingJobState.ProcessNumIdentifierOAIPMH = index;
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Obtiene una lista de los metadataformats disponibles
        /// </summary>
        /// <param name="identifierRepo">Identificador del repositorio</param>
        /// <returns>Lista de identificadores</returns>
        public List<string> CallListMetadataFormats(Guid identifierRepo)
        {
            string uri = $"etl/ListMetadataFormats/{identifierRepo}";
            List<string> listMetadataFormats = new List<string>();
            string xml = _publishData.CallGetApi(uri, _token);
            XDocument respuestaXML = XDocument.Load(new StringReader(xml));
            XNamespace nameSpace = respuestaXML.Root.GetDefaultNamespace();
            XElement listIdentifierElement = respuestaXML.Root.Element(nameSpace + "ListMetadataFormats");
            IEnumerable<XElement> listMetadataFormat = listIdentifierElement.Descendants(nameSpace + "metadataFormat");
            foreach (var metadataFormat in listMetadataFormat)
            {
                listMetadataFormats.Add(metadataFormat.Element(nameSpace + "metadataPrefix").Value);
            }
            return listMetadataFormats;
        }

        /// <summary>
        /// Obtiene la granularidad
        /// </summary>
        /// <param name="identifierRepo">Identificador del repositorio</param>
        /// <returns>Lista de identificadores</returns>
        public string CallGranularity(Guid identifierRepo)
        {
            string uri = $"etl/Identify/{identifierRepo}";
            List<string> listMetadataFormats = new List<string>();
            string xml = _publishData.CallGetApi(uri, _token);
            XDocument respuestaXML = XDocument.Load(new StringReader(xml));
            XNamespace nameSpace = respuestaXML.Root.GetDefaultNamespace();
            XElement identify = respuestaXML.Root.Element(nameSpace + "Identify");
            return identify.Element(nameSpace + "granularity").Value;
        }

        /// <summary>
        /// Obtiene una lista de identificadores
        /// </summary>
        /// <param name="identifierRepo">Identificador del repositorio</param>
        /// <param name="metadataPrefix">metadataPrefix</param>
        /// <param name="fechaFrom">fecha a partir de la cual se debe actualizar</param>
        /// <param name="set">tipo del objeto, usado para filtrar por agrupaciones</param>
        /// <returns>Lista de identificadores</returns>
        public List<IdentifierOAIPMH> CallListIdentifier(Guid identifierRepo, string metadataPrefix, string set = null, string fechaFrom = null)
        {
            string uri = $"etl/ListIdentifiers/{identifierRepo}?metadataPrefix={metadataPrefix}";
            if (set != null)
            {
                uri += $"&set={set}";
            }
            if (fechaFrom != null)
            {
                //uri += $"&from={fechaFrom.Value.ToString("u",CultureInfo.InvariantCulture)}";
                uri += $"&from={fechaFrom}";
            }
            List<IdentifierOAIPMH> listIdentifier = new List<IdentifierOAIPMH>();

            string resumptionToken = "";
            while (resumptionToken != null)
            {
                string xml;
                if (resumptionToken == "")
                {
                    xml = _publishData.CallGetApi(uri, _token);
                }
                else
                {
                    xml = _publishData.CallGetApi($"etl/ListIdentifiers/{identifierRepo}?resumptionToken={resumptionToken}", _token);
                }
                XDocument respuestaXML = XDocument.Load(new StringReader(xml));
                XNamespace nameSpace = respuestaXML.Root.GetDefaultNamespace();
                XElement listIdentifierElement = respuestaXML.Root.Element(nameSpace + "ListIdentifiers");
                IEnumerable<XElement> listHeader = listIdentifierElement.Descendants(nameSpace + "header");
                foreach (var header in listHeader)
                {
                    string identifier = header.Element(nameSpace + "identifier").Value;
                    string fecha = header.Element(nameSpace + "datestamp").Value;
                    DateTime fechaSincro = DateTime.Parse(fecha);
                    IdentifierOAIPMH identifierOAIPMH = new IdentifierOAIPMH()
                    {
                        Fecha = fechaSincro,
                        Identifier = identifier
                    };
                    listIdentifier.Add(identifierOAIPMH);
                }
                resumptionToken = null;
                XElement resumptionTokenElement = listIdentifierElement.Element(nameSpace + "resumptionToken");
                if (resumptionTokenElement != null && resumptionTokenElement.Value != "")
                {
                    resumptionToken = resumptionTokenElement.Value;
                }
            }
            listIdentifier = listIdentifier.OrderBy(x => x.Fecha).ToList();

            return listIdentifier;
        }

        /// <summary>
        /// Obtiene el rdf del identificador en el repositorio
        /// </summary>
        /// <param name="repoIdentifier">Identificador del repositorio</param>
        /// <param name="metadataPrefix">metadataPrefix</param>
        /// <param name="identifier">Identificador del elemento</param>
        /// <returns>RDF</returns>
        public string CallGetRecord(Guid repoIdentifier, string metadataPrefix, string identifier)
        {
            string respuesta = _publishData.CallGetApi($"etl/GetRecord/{repoIdentifier}?identifier={System.Web.HttpUtility.UrlEncode(identifier)}&metadataPrefix={metadataPrefix}", _token);
            XDocument respuestaXML = XDocument.Parse(respuesta);
            XNamespace nameSpace = respuestaXML.Root.GetDefaultNamespace();
            string rdf = respuestaXML.Root.Element(nameSpace + "GetRecord").Descendants(nameSpace + "metadata").First().FirstNode.ToString();
            rdf = rdf.Replace("xmlns=\"" + nameSpace + "\"", "");
            return rdf;
        }

        /// <summary>
        /// Obtiene el rdf 
        /// </summary>
        /// <param name="record">fichero XML</param>
        /// <param name="type">configuración del xml</param>
        /// <returns></returns>
        public string CallXMLConverter(string record, string type)
        {
            byte[] bytedata = Encoding.UTF8.GetBytes(record);
            var webClient = new WebClient();
            string boundary = "------------------------" + DateTime.Now.Ticks.ToString("x");
            webClient.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);
            var fileData = webClient.Encoding.GetString(bytedata);
            var package = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n{3}\r\n--{0}--\r\n", boundary, "pXmlFile", "text/xml", fileData);

            var nfile = webClient.Encoding.GetBytes(package);

            byte[] resp = webClient.UploadData(_configUrlService.GetUrlXmlConverter() + "Conversor/Convert?pType=" + type, "POST", nfile);


            string respuesta = System.Text.Encoding.UTF8.GetString(resp);
            return respuesta;
        }

        /// <summary>
        /// Llama al método ConfigurationsFilesList del servicio Conversor_XML_RDF
        /// </summary>
        /// <returns>Lista de configuraciones</returns>
        public string CallGetConfigurationsFiles()
        {
            HttpClient client = new HttpClient();
            var response = client.PostAsync($"{_configUrlService.GetUrlXmlConverter()}conversor/ConfigurationFilesList", null).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}