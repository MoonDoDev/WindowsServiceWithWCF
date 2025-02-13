using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.Extensions.Options;
using WindowsServiceBase.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using WindowsServiceBase.WebServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using NReco.Logging.File;

namespace WindowsServiceBase;

/// <summary>
/// 
/// </summary>
public sealed class ServiceProcessBase
{
	#region private data

	/// <summary>
	/// 
	/// </summary>
	private readonly ServiceSettings? _appSettings;

	/// <summary>
	/// 
	/// </summary>
	private readonly ILogger<ServiceProcessBase> _logger;

	/// <summary>
	/// 
	/// </summary>
	private readonly EventId _trceEventId;
	private readonly EventId _infoEventId;
	private readonly EventId _failEventId;

	#endregion
	#region constructor

	/// <summary>
	/// 
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="appSettings"></param>
	public ServiceProcessBase( ILogger<ServiceProcessBase> logger, IOptionsMonitor<ServiceSettings> appSettings )
	{
		_trceEventId = new EventId( EventLogIds.TraceServiceReport );
		_failEventId = new EventId( EventLogIds.GeneralErrorException );
		_infoEventId = new EventId( EventLogIds.InfoServiceReport );

		_logger = logger;
		_logger.LogInformation( _infoEventId, "WinService Created" );

		_appSettings = appSettings.CurrentValue;
		ArgumentNullException.ThrowIfNull( _appSettings, nameof( _appSettings ) );

		CreateWCFServiceInstance();
	}

	#endregion
	#region internal processes

	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	internal async Task ExecuteServiceAsync( CancellationToken cancellationToken )
	{
		_logger.LogTrace( _trceEventId, "Executing WinService ..." );
		await Task.Delay( TimeSpan.FromSeconds( 60 ), cancellationToken );
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	internal async Task StopServiceAsync( CancellationToken cancellationToken )
	{
		_logger.LogTrace( _trceEventId, "Stopping WinService ..." );
		await Task.CompletedTask;
	}

	/// <summary>
	/// 
	/// </summary>
	private void CreateWCFServiceInstance()
	{
		var builder = WebApplication.CreateBuilder();

		builder.Logging.AddEventLog( elSettings =>
		{
			elSettings.SourceName = ServiceDefinitions.EventLogSource;
			elSettings.LogName = ServiceDefinitions.EventLogName;
		} );

		builder.WebHost.ConfigureKestrel( ( context, options ) =>
		{
			options.AllowSynchronousIO = true;
			if ( !Int32.TryParse( _appSettings!.WebServicePort, out int servicePort ) )
				servicePort = 5001;

			options.ListenAnyIP( servicePort, listenOptions =>
			{
				listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
				listenOptions.UseHttps(
					_appSettings!.ServerCertificatePfxFile!,
					_appSettings!.ServerCertificatePasskey );
			} );
		} );

		// Add WSDL support
		builder.Services.AddServiceModelServices()
			.AddServiceModelConfigurationManagerFile( "WebService.config" )
			.AddServiceModelMetadata()
			.AddSingleton( provider =>
			{
				var loggerService = provider.GetRequiredService<ILogger<ServiceTemplate>>();
				return new ServiceTemplate( loggerService );
			} );

		if ( _appSettings!.CreateLogFile )
		{
			builder.Logging.AddFile( "{0}jubatusproject-webservice-{1:dd}-{1:MM}-{1:yyyy}.log", fileLoggerOpts =>
				fileLoggerOpts.FormatLogFileName = fName => string.Format( fName, _appSettings.LogPathName, DateTime.Now ) );
		}

		var app = builder.Build();

		( app as IApplicationBuilder ).UseServiceModel( builder =>
			builder.AddService<ServiceTemplate>( serviceOptions =>
				serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true )
			.ConfigureServiceHostBase<ServiceTemplate>( serviceHost => { } ) );

		var serviceUrl = string.Concat( _appSettings!.WebServiceUrl, ":",
			_appSettings!.WebServicePort, _appSettings!.WebServiceEndpoint );

		var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
		serviceMetadataBehavior.HttpsGetEnabled = true;
		serviceMetadataBehavior.HttpsGetUrl = new Uri( serviceUrl );

		app.RunAsync();

		_logger.LogTrace( _trceEventId, "WebService is listening on {Url}", serviceUrl );
	}

	#endregion
}
