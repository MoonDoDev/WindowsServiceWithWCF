using Microsoft.Extensions.Options;
using WindowsServiceBase.Settings;

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
	private readonly EventId _traceEventId;
	private readonly EventId _exceptionEventId;

	#endregion
	#region constructor

	/// <summary>
	/// 
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="appSettings"></param>
	public ServiceProcessBase( ILogger<ServiceProcessBase> logger, IOptionsMonitor<ServiceSettings> appSettings )
	{
		_traceEventId = new EventId( EventLogIds.TraceServiceReport );
		_exceptionEventId = new EventId( EventLogIds.GeneralErrorException );

		_logger = logger;
		_logger.LogTrace( _traceEventId, "ServiceProcessBase Constructor" );

		_appSettings = appSettings.CurrentValue;
		ArgumentNullException.ThrowIfNull( _appSettings, nameof( _appSettings ) );
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
		while ( !cancellationToken.IsCancellationRequested )
		{
			try
			{
				_logger.LogTrace( _traceEventId, "Checking ( LogPathName: {LogName} )", _appSettings!.LogPathName );
				await Task.Delay( TimeSpan.FromSeconds( 10 ), cancellationToken );
			}
			catch ( TaskCanceledException )
			{
				break;
			}
			catch ( Exception ex )
			{
				_logger.LogError( _exceptionEventId, ex,
					"ExecuteServiceAsync( Throws an exception => {Message} )", ex.Message );
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	internal async Task StopServiceAsync( CancellationToken cancellationToken )
	{
		_logger.LogTrace( _traceEventId, "Cleaning ..." );
		await Task.CompletedTask;
	}

	#endregion
}
