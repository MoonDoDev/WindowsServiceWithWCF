namespace WindowsServiceBase;

/// <summary>
/// 
/// </summary>
public sealed class ServiceBackWorker : BackgroundService
{
	/// <summary>
	/// 
	/// </summary>
	private readonly ServiceProcessBase _serviceProcess;
	private readonly ILogger<ServiceBackWorker> _logger;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="serviceProcess"></param>
	/// <param name="logger"></param>
	public ServiceBackWorker( ServiceProcessBase serviceProcess, ILogger<ServiceBackWorker> logger )
	{
		logger.LogInformation( new EventId( EventLogIds.InfoStartService ), "WkrService Created" );
		_serviceProcess = serviceProcess;
		_logger = logger;
	}

	/// <summary>
	/// => Para instalar el Servicio en la Consola de Servicios Windows:
	///    => sc.exe create "Windows Service Template" binpath= "C:\..\WindowsServiceTemplate.exe"
	/// => Para configurar la recuperación del Servicio:
	///    => sc.exe failure "Windows Service Template" reset= 0 actions= restart/60000/restart/60000/run/1000
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public override async Task StartAsync( CancellationToken cancellationToken )
	{
		_logger.LogInformation( new EventId( EventLogIds.InfoStartService ), "WkrService Started" );
		await base.StartAsync( cancellationToken );
	}

	/// <summary>
	/// => Para desinstalar el Servicio de la Consola de Servicios Windows:
	///    => sc.exe delete "Windows Service Template"
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public override async Task StopAsync( CancellationToken cancellationToken )
	{
		_logger.LogInformation( new EventId( EventLogIds.InfoStopService ), "WkrService Stopped" );
		await _serviceProcess.StopServiceAsync( cancellationToken );
		await base.StopAsync( cancellationToken );
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="stoppingToken"></param>
	/// <returns></returns>
	protected override async Task ExecuteAsync( CancellationToken stoppingToken )
	{
		while ( !stoppingToken.IsCancellationRequested )
		{
			try
			{
				await _serviceProcess.ExecuteServiceAsync( stoppingToken );
			}
			catch ( TaskCanceledException )
			{
				break;
			}
			catch ( Exception ex )
			{
				_logger.LogError( new EventId( EventLogIds.GeneralErrorException ), ex,
					"ExecuteAsync( Throws an exception: {Message})", ex.Message );

				Environment.Exit( 1 );
			}
		}
	}
}
