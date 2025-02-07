using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using NReco.Logging.File;
using WindowsServiceBase;
using WindowsServiceBase.Settings;
using System.Diagnostics;

var builder = Host.CreateApplicationBuilder( args );

builder.Services.AddWindowsService( options => options.ServiceName = ServiceDefinitions.ServiceName );

Directory.SetCurrentDirectory( AppDomain.CurrentDomain.BaseDirectory );
builder.Logging.ClearProviders();

if ( !EventLog.SourceExists( ServiceDefinitions.EventLogSource ) )
{
	EventLog.CreateEventSource( ServiceDefinitions.EventLogSource, ServiceDefinitions.EventLogName );
}

builder.Logging.AddEventLog( eventLogSettings =>
{
	eventLogSettings.SourceName = ServiceDefinitions.EventLogSource;
	eventLogSettings.LogName = ServiceDefinitions.EventLogName;
} );

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>( builder.Services );

builder.Services.AddSingleton<ServiceProcessBase>()
	.AddHostedService<ServiceBackWorker>()
	.AddOptions<ServiceSettings>()
	.BindConfiguration( nameof( ServiceSettings ) )
	.Validate( serviceSettings => serviceSettings.AllDataIsValid() )
	.ValidateOnStart();

var appSettings = builder.Configuration.GetSection( nameof( ServiceSettings ) ).Get<ServiceSettings>();
ArgumentNullException.ThrowIfNull( appSettings, nameof( appSettings ) );

builder.Logging.AddFile( "{0}win-service-base({1:dd}-{1:MM}-{1:yyyy}).log", fileLoggerOpts =>
	fileLoggerOpts.FormatLogFileName = fName => string.Format( fName, appSettings.LogPathName, DateTime.Now ) );

var host = builder.Build();
host.Run();
