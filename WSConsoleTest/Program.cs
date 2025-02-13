// See https://aka.ms/new-console-template for more information

using EchoWebService;

using var echoServive = new ServiceTemplateClient(
	ServiceTemplateClient.EndpointConfiguration.basicHttpBindingConfiguration_IServiceTemplate,
	"https://jubatusproject.services.com:5001/EchoService" );

var result = echoServive.GetServiceStatusAsync();
Console.WriteLine( "Hello, WebService => ( GetServiceStatus = {0}, {1} )",
	result.Result.StatusCode, result.Result.StatusMessage );

result = echoServive.GetEchoMessageAsync( "Hello from Windows Service" );
Console.WriteLine( "Hello, WebService => ( GetEchoMessage = {0}, {1} )", 
	result.Result.StatusCode, result.Result.StatusMessage );

echoServive.Close();
Console.ReadKey();
