using System;
using Xunit;
using API_DISCOVER.Utility;
using VDS.RDF;
using VDS.RDF.Query.Inference;
using VDS.RDF.Parsing;
using System.Collections.Generic;
using API_DISCOVER.Models.Entities;
using VDS.RDF.Query;
using API_DISCOVER.Models.Services;

namespace XUnitTestProjectAPI_DISCOVER
{
    public class UnitTestDiscover
    {
        [Fact]
        public void TestPrepareData()
        {
			RohGraph dataGraph = new RohGraph();
			dataGraph.LoadFromString(rdfFile, new RdfXmlParser());

			RohRdfsReasoner reasoner = new RohRdfsReasoner();
			RohGraph ontologyGraph = new RohGraph();

			ontologyGraph.LoadFromFile("Ontology/roh-v2.owl");
			reasoner.Initialise(ontologyGraph);

			RohGraph dataInferenceGraph = dataGraph.Clone();
			reasoner.Apply(dataInferenceGraph);

			Dictionary<string, HashSet<string>> entitiesRdfTypes;
			Dictionary<string, string> entitiesRdfType;
			Dictionary<string, List<DisambiguationData>> disambiguationDataRdf;

			Discover.PrepareData(dataGraph, reasoner, out dataInferenceGraph, out entitiesRdfTypes, out entitiesRdfType, out disambiguationDataRdf, false);
			string query = @"select distinct ?s where{?s a ?rdftype. FILTER(!isBlank(?s))}";
			SparqlResultSet sparqlResultSet = (SparqlResultSet)dataGraph.ExecuteQuery(query.ToString());
			//TODO verificar
			Assert.True((dataInferenceGraph != null) && (entitiesRdfTypes.Count == sparqlResultSet.Count) && (entitiesRdfType.Count == sparqlResultSet.Count) && (disambiguationDataRdf.Count == sparqlResultSet.Count));
		}

		[Fact]
		public void TestReconciliateRDF()
        {
			bool hasChanges = false;

			Dictionary<string, string> discoveredEntityList = new Dictionary<string, string>();

			RohGraph dataGraph = new RohGraph();
			dataGraph.LoadFromString(rdfFile, new RdfXmlParser());

			RohRdfsReasoner reasoner = new RohRdfsReasoner();

			Dictionary<string, HashSet<string>> discardDissambiguations = new Dictionary<string, HashSet<string>>();

			DiscoverCache discoverCache = new DiscoverCache();

			Discover.ReconciliateRDF(ref hasChanges, ref discoveredEntityList, ref dataGraph, reasoner, discardDissambiguations, discoverCache);
			Assert.True(true);
		}

		[Fact]
		public void TestExternalIntegration()
        {
			bool hasChanges = false;
			Dictionary<string, string> discoveredEntityList = new Dictionary<string, string>();

			Dictionary<string, Dictionary<string, float>> discoveredEntitiesProbability = new Dictionary<string, Dictionary<string, float>>();

			RohGraph dataGraph = new RohGraph();
			dataGraph.LoadFromString(rdfFile, new RdfXmlParser());

			RohRdfsReasoner reasoner = new RohRdfsReasoner();

			Dictionary<string, Dictionary<string, float>> namesScore = new Dictionary<string, Dictionary<string, float>>();

			RohGraph ontologyGraph = new RohGraph();
			ontologyGraph.LoadFromFile("Ontology/roh-v2.owl");
			reasoner.Initialise(ontologyGraph);

			Dictionary<string, KeyValuePair<string, float>> entidadesReconciliadasConIntegracionExternaAux;

			Dictionary<string, HashSet<string>> discardDissambiguations = new Dictionary<string, HashSet<string>>();

			DiscoverCache discoverCache = new DiscoverCache();
			Discover.mSparqlUtility = new SparqlUtilityMock();
			Discover.ExternalIntegration(ref hasChanges, ref discoveredEntityList, ref discoveredEntitiesProbability, ref dataGraph, reasoner, namesScore, ontologyGraph, out entidadesReconciliadasConIntegracionExternaAux, discardDissambiguations, discoverCache);
		}

		private string rdfFile
        {
            get
            {
                string rdfFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!DOCTYPE rdf:RDF [
        	<!ENTITY rdf 'http://www.w3.org/1999/02/22-rdf-syntax-ns#'>
		<!ENTITY rdfs 'http://www.w3.org/2000/01/rdf-schema#'>
			<!ENTITY xsd 'http://www.w3.org/2001/XMLSchema#'>
				]>
				<rdf:RDF xmlns:rdfs=""http://www.w3.org/2000/01/rdf-schema#""
       					xmlns:xsd=""http://www.w3.org/2001/XMLSchema#""
       					xmlns:foaf=""http://purl.org/roh/mirror/foaf#""
       					xmlns:roh=""http://purl.org/roh#""
       					xmlns:bibo=""http://purl.org/roh/mirror/bibo#""
       					xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
					<foaf:Person rdf:about=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"">
						<foaf:name rdf:datatype=""&xsd;string"">Diego Casado-Mansilla</foaf:name>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/76c8f70c-50ea-4de0-b3b6-5bcf4fdb829e""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/eec217c9-44c4-480d-9653-6d84b711a9b2""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/990d9d14-4cbb-4387-8220-5a6d5a43f2a0""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/5d17fc89-8c73-4157-965c-bfacc6030171""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/584fc505-dbca-49f8-ac30-be7b44fc1957""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/3844c942-79ab-443c-bf59-4f74b2f1edeb""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/11826b3e-ce40-4097-a738-532561209b40""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/feb48b56-ac2a-4e52-aa81-c39f30dc18ae""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/a7db2203-49a1-4bb5-aac4-cda161c1468f""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/17d68fc4-0aa8-4ecd-a570-2946c42c5ade""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/86769078-ccc8-47dd-83aa-a472666b262f""/>
					</foaf:Person>
					<foaf:Person rdf:about=""http://graph.um.es/res/person/e12d2a31-e285-459a-9d71-2d4a45329532"">
						<foaf:name rdf:datatype=""&xsd;string"">D. L�pez-de-Ipi�a</foaf:name>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/76c8f70c-50ea-4de0-b3b6-5bcf4fdb829e""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/eec217c9-44c4-480d-9653-6d84b711a9b2""/>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/990d9d14-4cbb-4387-8220-5a6d5a43f2a0""/>
					</foaf:Person>
					<foaf:Person rdf:about=""http://graph.um.es/res/person/799860d8-b7c5-4229-a4c3-44533871493d"">
						<foaf:name rdf:datatype=""&xsd;string"">�lvaro Palacios Mariju�n</foaf:name>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/5d17fc89-8c73-4157-965c-bfacc6030171""/>
					</foaf:Person>
					<foaf:Person rdf:about=""http://graph.um.es/res/person/7a4898f2-195c-42d0-98c6-91ba121c14ea"">
						<foaf:name rdf:datatype=""&xsd;string"">Esteban Sota Leiva</foaf:name>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/584fc505-dbca-49f8-ac30-be7b44fc1957""/>
					</foaf:Person>
					<foaf:Person rdf:about=""http://graph.um.es/res/person/27b9fd2f-386c-4da6-bfe2-ce4fc6ace6a4"">
						<foaf:name rdf:datatype=""&xsd;string"">Oihane G�mez-Carmona</foaf:name>
						<roh:correspondingAuthorOf rdf:resource=""http://graph.um.es/res/document/3844c942-79ab-443c-bf59-4f74b2f1edeb""/>
					</foaf:Person>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/76c8f70c-50ea-4de0-b3b6-5bcf4fdb829e"">
						<roh:title rdf:datatype=""&xsd;string"" >Exploring the computational cost of machine learning at the edge for human-centric Internet of Things.</roh:title>
						<bibo:authorList rdf:nodeID=""N57c461361fc640adaac6bcb1c7f54d0d"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/eec217c9-44c4-480d-9653-6d84b711a9b2"">
						<roh:title rdf:datatype=""&xsd;string"" >User perspectives in the design of interactive everyday objects for sustainable behaviour.</roh:title>
						<bibo:authorList rdf:nodeID=""N59d40eb0e46c40d083cf88d019323568"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/990d9d14-4cbb-4387-8220-5a6d5a43f2a0"">
						<roh:title rdf:datatype=""&xsd;string"" >Exploring the Application of the FOX Model to Foster Pro-Environmental Behaviours in Smart Environments.</roh:title>
						<bibo:authorList rdf:nodeID=""N55f7938ae6494843a9622da4446bc608"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/5d17fc89-8c73-4157-965c-bfacc6030171"">
						<roh:title rdf:datatype=""&xsd;string"" >Documento de prueba de �lvaro y Diego</roh:title>
						<bibo:authorList rdf:nodeID=""N57c461361fc640adaac6bcb1c7f54d0c"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/584fc505-dbca-49f8-ac30-be7b44fc1957"">
						<roh:title rdf:datatype=""&xsd;string"" >Documento de prueba con t�tulo inventado</roh:title>
						<bibo:authorList rdf:nodeID=""N57c461361fc640adaac6bcb1c7f54d22"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/3844c942-79ab-443c-bf59-4f74b2f1edeb"">
						<roh:title rdf:datatype=""&xsd;string"" >SmartWorkplace: A Privacy-based Fog Computing Approach to Boost Energy Efficiency and Wellness in Digital Workspaces.</roh:title>
						<bibo:authorList rdf:nodeID=""N57c461361fc640adaac6bcb1c7f54d0e"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/11826b3e-ce40-4097-a738-532561209b40"">
						<roh:title rdf:datatype=""&xsd;string"" >The DiY Smart Experiences Project - A European Endeavour Removing Barriers for User-generated Internet of Things Applications.</roh:title>
						<bibo:authorList rdf:nodeID=""N58dbc92831344c0b812c8a7d202a2399"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/feb48b56-ac2a-4e52-aa81-c39f30dc18ae"">
						<roh:title rdf:datatype=""&xsd;string"" >'Close the Loop': An iBeacon App to Foster Recycling Through Just-in-Time Feedback.</roh:title>
						<bibo:authorList rdf:nodeID=""N5a2ca30985bd4c4786717981a299f788"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/a7db2203-49a1-4bb5-aac4-cda161c1468f"">
						<roh:title rdf:datatype=""&xsd;string"" >To switch off the coffee-maker or not: that is the question to be energy-efficient at work.</roh:title>
						<bibo:authorList rdf:nodeID=""N5680b4d50ff4415883a761cbba34d2f7"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/17d68fc4-0aa8-4ecd-a570-2946c42c5ade"">
						<roh:title rdf:datatype=""&xsd;string"" >Validation of a CoAP to IEC 61850 Mapping and Benchmarking vs HTTP-REST and WS-SOAP.</roh:title>
						<bibo:authorList rdf:nodeID=""N5549bfe4dfdc496fafd47f3e77f64d7a"" />
					</bibo:Document>
					<bibo:Document rdf:about=""http://graph.um.es/res/document/86769078-ccc8-47dd-83aa-a472666b262f"">
						<roh:title rdf:datatype=""&xsd;string"" >Design-insights for Devising Persuasive IoT Devices for Sustainability in the Workplace.</roh:title>
						<bibo:authorList rdf:nodeID=""N5c342df56893442da790bd591df1c245"" />
					</bibo:Document>
					<rdf:Seq rdf:nodeID=""N57c461361fc640adaac6bcb1c7f54d0d"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
						<rdf:_2 rdf:resource=""http://graph.um.es/res/person/e12d2a31-e285-459a-9d71-2d4a45329532"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N59d40eb0e46c40d083cf88d019323568"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
						<rdf:_2 rdf:resource=""http://graph.um.es/res/person/e12d2a31-e285-459a-9d71-2d4a45329532"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N55f7938ae6494843a9622da4446bc608"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
						<rdf:_2 rdf:resource=""http://graph.um.es/res/person/e12d2a31-e285-459a-9d71-2d4a45329532"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N57c461361fc640adaac6bcb1c7f54d0c"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
						<rdf:_2 rdf:resource=""http://graph.um.es/res/person/799860d8-b7c5-4229-a4c3-44533871493d"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N57c461361fc640adaac6bcb1c7f54d22"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
						<rdf:_2 rdf:resource=""http://graph.um.es/res/person/7a4898f2-195c-42d0-98c6-91ba121c14ea"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N57c461361fc640adaac6bcb1c7f54d0e"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
						<rdf:_2 rdf:resource=""http://graph.um.es/res/person/27b9fd2f-386c-4da6-bfe2-ce4fc6ace6a4"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N58dbc92831344c0b812c8a7d202a2399"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N5a2ca30985bd4c4786717981a299f788"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N5680b4d50ff4415883a761cbba34d2f7"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N5549bfe4dfdc496fafd47f3e77f64d7a"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
					</rdf:Seq>
					<rdf:Seq rdf:nodeID=""N5c342df56893442da790bd591df1c245"">
						<rdf:_1 rdf:resource=""http://graph.um.es/res/person/c8a16863-a606-48e3-a858-9def000380c0"" />
					</rdf:Seq>
				</rdf:RDF>
				";
                return rdfFile;
            }
        }
    }
}
