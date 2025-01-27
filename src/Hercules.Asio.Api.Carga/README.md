![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 20/06/20201                                                  |
| ------------- | ------------------------------------------------------------ |
|Titulo|API CARGA readme| 
|Descripción|Manual del servicio API CARGA|
|Versión|1.2|
|Módulo|API CARGA|
|Tipo|Manual|
|Cambios de la Versión|Cambiado el enlace a las pruebas unitarias|

## Sobre API CARGA
[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/dashboard?id=API_CARGA)

![](https://github.com/HerculesCRUE/GnossDeustoBackend/workflows/Build%20and%20test%20Hercules.Asio.Api.Carga/badge.svg)
[![codecov](https://codecov.io/gh/HerculesCRUE/GnossDeustoBackend/branch/master/graph/badge.svg?token=4SONQMD1TI&flag=carga)](https://codecov.io/gh/HerculesCRUE/GnossDeustoBackend)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=API_CARGA&metric=bugs)](https://sonarcloud.io/dashboard?id=API_CARGA)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=API_CARGA&metric=security_rating)](https://sonarcloud.io/dashboard?id=API_CARGA)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=API_CARGA&metric=ncloc)](https://sonarcloud.io/dashboard?id=API_CARGA)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=API_CARGA&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=API_CARGA)

[<img align="right" width="100px" src="https://dotnetfoundation.org/img/logo_big.svg" />](https://dotnetfoundation.org/projects?searchquery=IdentityServer&type=project)

Accesible en el entorno de desarrollo en esta dirección a través de swagger: http://herc-as-front-desa.atica.um.es/carga/swagger/index.html.

La documentación de la librería está disponible en: 
http://herc-as-front-desa.atica.um.es/api-carga/library/api/API_CARGA.Controllers.html

API CARGA es un servicio web que contienen 4 controladores, utilizados cada uno de ellos para su propio propósito:
 - etlController: Contiene los procesos ETL (Extract, Transform and Load) necesarios para la carga de datos.
 - repositoryController: Contiene los procesos necesarios para la gestión de los repositorios OAI-PMH (creación, modificación, eliminación...).
 - syncController: Contiene los procesos necesarios para la ejecución de las sincronizaciones.
 - ValidationController: Contiene los procesos necesarios para la gestión de las validaciones  (creación, modificación, eliminación...). La carpeta [Validaciones](https://github.com/HerculesCRUE/GnossDeustoBackend/tree/master/src/Hercules.Asio.Api.Carga/Validaciones) contiene información sobre los [shapes SHACL](https://www.w3.org/TR/shacl/) definidos para validar.
 
Para una especificación más detallada del servicio se puede consultar la siguiente documentación: [Hercules-ASIO-Especificacion-de-funciones-de-Carga.md](../../Docs/Hercules-ASIO-Especificacion-de-funciones-de-Carga.md)
 
Esta aplicación se encarga de sincronizar los datos de un repositorio OAI-PMH con el RDF Store. Obtiene todas las entidades actualizadas desde la última sincronización, solicita al repositorio OAI-PMH todos sus datos y los inserta en el RDF Store.

Los resultados de las pruebas unitarias se pueden consultar en [CodeCov](https://codecov.io/gh/HerculesCRUE/GnossDeustoBackend).

Las librerías compiladas se encuentran en la carpeta [librerías](https://github.com/HerculesCRUE/GnossDeustoBackend/tree/master/libraries).

*Obtención del Token*
-------------------------
Este api esta protegida mediante tokens, por ello para poder usar la interfaz swagger hay que obtener un token, el cual se puede obtener desde https://herc-as-front-desa.atica.um.es/carga-web/Token

*Conexión a Triple Store*
-------------------------

Como no es necesario ningún conector específico para actualizar un RDF Store ya que, por definición, deben tener un SPARQL Endpoint, no se ha creado ninguna librería específica de conexión al RDF Store. Las actualizaciones se realizan vía peticiones HTTP al SPARQL Endpoint.

El SPARQL Endpoint provisional se encuentra disponible en un servidor de la Universidad de Murcia, con acceso protegido por una VPN en la siguiente URL:

http://155.54.239.204:8890/sparql

Hay ejemplos de consultas en el documento [20200325 Hércules ASIO Ejemplos de consultas](../../Docs/SPARQL/Hercules-ASIO-Ejemplos-de-consultas-SPARQL.md)

Los datos cargados se pueden consultar en una versión preliminar del servidor Linked Data, soportado por el servidor [Linked Data desarrollado](https://github.com/HerculesCRUE/GnossDeustoBackend/tree/master/src/Hercules.Asio.LinkedDataServer), desplegado en los servidores de la Universidad de Murcia. Por ejemplo:

http://graph.um.es/res/project/RADBOUDUMC

## Configuración en el appsettings.json

    { 
		"ConnectionStrings": {
			"PostgreConnectionmigration": ""
		},
		"Logging": {
			"LogLevel": {
				"Default": "Information",
				"Microsoft": "Warning",
				"Microsoft.Hosting.Lifetime": "Information"
			}
		},
		"AllowedHosts": "*",
		"Urls": "http://0.0.0.0:5100",
		"ConfigUrl": "http://herc-as-front-desa.atica.um.es/carga/",
		"Sparql": {
			"Graph": "http://data.um.es/graph/sgi",
			"Endpoint": "http://155.54.239.204:8890/sparql"
			"QueryParam": "query",
			"GraphRoh": "http://graph.um.es/graph/research/roh",
			"GraphRohes": "http://graph.um.es/graph/research/rohes",
			"GraphRohum": "http://graph.um.es/graph/research/rohum"
		},
		"RabbitMQ": {
			"usernameRabbitMq": "user",
			"passwordRabbitMq": "pass",
			"hostnameRabbitMq": "hercules",
			"uriRabbitMq": "amqp://user:pass@ip:puerto/hercules",
			"virtualhostRabbitMq": "hercules"
		},
		"RabbitQueueName": "HerculesDemoQueue",
		"RabbitQueueNameVirtuoso": "HerculesQueueVirtuoso",
		"RabbitQueueNameDelete": "HerculesDemoQueueDelete",
		"Authority": "http://herc-as-front-desa.atica.um.es:5108",
		"ConfigUrlXmlConverter": "http://herc-as-front-desa.atica.um.es/conversor_xml_rdf/"
    }
 - PostgreConnectionmigration: Cadena de conexión a la base de datos PostgreSQL
 - LogLevel.Default: Nivel de error por defecto
 - LogLevel.Microsoft: Nivel de error para los errores propios de Microsoft
 - LogLevel.Microsoft.Hosting.Lifetime: Nivel de error para los errores de host
 - Urls: Url en la que se va a lanzar la aplicación
 - ConfigUrl: URL donde está lanzada esta aplicación
 - Sparql.Graph: Grafo en el que se van a almacenar los triples
 - Sparql.Endpoint: URL del Endpoint Sparql
 - Sparql.QueryParam: Parámetro para la query en el Endpoint Sparql
 - Sparql.GraphRoh: gráfo de la ontologia roh
 - Sparql.GraphRohes: gráfo de la ontologia rohes
 - Sparql.GraphRohum: gráfo de la ontologia rohum
 - Sparql.Username: Usuario para el Endpoint Sparql
 - Sparql.Password: Password para el Endpoint Sparql 
 - RabbitMQ.usernameRabbitMq: usuario para acceder a Rabbit
 - RabbitMQ.passwordRabbitMq: contraseña del usuario para acceder a Rabbit
 - RabbitMQ.hostnameRabbitMq: host de Rabbit
 - RabbitMQ.uriRabbitMq: cadena de conexión para acceder a Rabbit
 - RabbitMQ.virtualhostRabbitMq: host virtual configurado en Rabbit
 - RabbitQueueName: Nombre de la cola de Rabbit para la carga de RDFs
 - RabbitQueueNameVirtuoso: Nombre de la cola de Rabbit para la replicación de la BBDD RDF
 - RabbitQueueNameDelete: Nombre de la cola de Rabbit para el borrado de entidades
 - Authority: Url de la servicio de identidades
 - ConfigUrlXmlConverter: URL donse está lanzada la aplicación CONVERSOR_XML_RDF

Se puede encontrar un el appsettings usado para este servicio sin datos sensibles en: https://github.com/HerculesCRUE/GnossDeustoBackend/blob/master/src/Hercules.Asio.Api.Carga/API_CARGA/appsettings.json
## Dependencias

- **dotNetRDF**: versión 2.5.1
- **IdentityServer4**: versión 3.1.2
- **IdentityServer4.EntityFramework**: versión 3.1.2
- **Microsoft.AspNetCore.Mvc.Formatters.Json**: versión 2.2.0
- **Microsoft.AspNetCore.Mvc.NewtonsoftJson**: versión 3.0.0
- **Microsoft.EntityFrameworkCore**: versión 3.1.4
- **Microsoft.EntityFrameworkCore.SqlServer**: versión 3.1.2
- **Microsoft.EntityFrameworkCore.Tools**: versión 3.1.2
- **Microsoft.Extensions.Logging.Debug**: versión 3.0.0
- **Microsoft.VisualStudio.Web.CodeGeneration.Design**: versión 3.0.0
- **Npgsql.EntityFrameworkCore.PostgreSQL**: versión 3.1.2
- **OaiPmhNet**: versión 0.4.1
- **Serilog.AspNetCore**: versión 3.2.0
- **Swashbuckle.AspNetCore**: versión 5.0.0
- **Swashbuckle.AspNetCore.Annotations**: versión 5.0.0
- **Swashbuckle.AspNetCore.Filters**: versión 5.0.2
- **Swashbuckle.AspNetCore.SwaggerGen**: versión 5.0.0
- **Swashbuckle.AspNetCore.SwaggerUI**: versión 5.0.0
