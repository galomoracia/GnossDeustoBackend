package es.gnossdeusto.backend;

import java.util.List;
import java.util.ArrayList;
import java.util.Map;

import org.json.JSONObject;

import com.hp.hpl.jena.query.Query;
import com.hp.hpl.jena.query.QueryExecution;
import com.hp.hpl.jena.query.QueryExecutionFactory;
import com.hp.hpl.jena.query.QueryFactory;
import com.hp.hpl.jena.query.QuerySolution;
import com.hp.hpl.jena.query.ResultSet;
import com.hp.hpl.jena.rdf.model.Model;
import com.hp.hpl.jena.rdf.model.RDFNode;

import java.util.HashMap;


public class QueryExecutor {
    
    public static JSONObject execute(String queryString, Model data) {
        Query query = QueryFactory.create(queryString) ;
        JSONObject result = new JSONObject();
		//List<Map<String, String>> result = new ArrayList<Map<String, String>>();
        try(QueryExecution qexec = QueryExecutionFactory.create(query, data)) {
            ResultSet results = qexec.execSelect();
            List<String> resultVars = results.getResultVars();
            for ( ; results.hasNext() ; )
			    {
			      QuerySolution soln = results.nextSolution() ;
			      JSONObject partialResult = new JSONObject();
                  // Map<String, String> partialResult = new HashMap<String, String>();
                  for(String var : resultVars) {
                    RDFNode node = soln.get(var);
                    partialResult.put(var, node.toString());
                  }
                  // result.add(partialResult);
                  result.append("result", partialResult);
			    }
        }
        return result;
    }
}
