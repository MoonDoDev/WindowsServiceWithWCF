using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using WCFServiceTemplate;

var builder = WebApplication.CreateBuilder( args );
builder.WebHost.ConfigureKestrel( ( context, options ) =>
{
	options.AllowSynchronousIO = true;
} );

// Add WSDL support
builder.Services.AddServiceModelServices().AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();
app.UseServiceModel( builder =>
{
	builder.AddService<ServiceTemplate>( ( serviceOptions ) =>
	{
		serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
	} )
	// Add a BasicHttpBinding at a specific endpoint
	.AddServiceEndpoint<ServiceTemplate, IServiceTemplate>( new BasicHttpBinding(), "/EchoService/basichttp" )
	// Add a WSHttpBinding with Transport Security for TLS
	.AddServiceEndpoint<ServiceTemplate, IServiceTemplate>( new WSHttpBinding( SecurityMode.Transport ), "/EchoService/WSHttps" );
} );

var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
serviceMetadataBehavior.HttpGetEnabled = true;

app.Run();
