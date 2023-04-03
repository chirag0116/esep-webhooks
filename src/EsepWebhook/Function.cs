Skip to content
Search or jump to…
Pull requests
Issues
Codespaces
Marketplace
Explore
 
@chirag0116 
jcutrono
/
esep-webhooks
Public
Fork your own copy of jcutrono/esep-webhooks
Code
Issues
1
Pull requests
Actions
Projects
Security
Insights
esep-webhooks/EsepWebhook/src/EsepWebhook/Function.cs /
@jcutrono
jcutrono using environment variables with underscore...
Latest commit d07f621 on Feb 23
 History
 1 contributor
36 lines (28 sloc)  1.21 KB

using System.Text;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EsepWebhook;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(object input, ILambdaContext context)
    {
        dynamic json = JsonConvert.DeserializeObject<dynamic>(input.ToString());
        
        string payload = $"{{'text':'Issue Created: {json.issue.html_url}'}}";
        
        var client = new HttpClient();
        var webRequest = new HttpRequestMessage(HttpMethod.Post, Environment.GetEnvironmentVariable("SLACK_URL"))
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };

        var response = client.Send(webRequest);
        using var reader = new StreamReader(response.Content.ReadAsStream());
            
        return reader.ReadToEnd();
    }
}