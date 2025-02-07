namespace WindowsServiceBase;

/// <summary>
/// 
/// </summary>
/// <param name="serviceProcess"></param>
/// <param name="logger"></param>
public sealed class ServiceBackWorker(
	ServiceProcessBase serviceProcess,
	ILogger<ServiceBackWorker> logger ) : BackgroundService
{
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
		logger.LogInformation( new EventId( EventLogIds.InfoStartService ), "Service Started" );
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
		logger.LogInformation( new EventId( EventLogIds.InfoStopService ), "Service Stopped" );
		await serviceProcess.StopServiceAsync( cancellationToken );
		await base.StopAsync( cancellationToken );
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="stoppingToken"></param>
	/// <returns></returns>
	protected override async Task ExecuteAsync( CancellationToken stoppingToken )
	{
		try
		{
			await serviceProcess.ExecuteServiceAsync( stoppingToken );
		}
		catch ( Exception ex )
		{
			logger.LogError( new EventId( EventLogIds.GeneralErrorException ), ex,
				"ExecuteAsync( Throws an exception: {Message})", ex.Message );

			Environment.Exit( 1 );
		}
	}
}
