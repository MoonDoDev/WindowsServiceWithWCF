namespace WindowsServiceBase.WebServices;

/// <summary>
/// 
/// </summary>
public class ServiceTemplate : IServiceTemplate
{
	/// <summary>
	/// 
	/// </summary>
	private readonly ILogger<ServiceTemplate> _logger;

	/// <summary>
	/// 
	/// </summary>
	private readonly EventId _trceEventId;
	private readonly EventId _infoEventId;
	private readonly EventId _failEventId;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="logger"></param>
	public ServiceTemplate( ILogger<ServiceTemplate> logger )
	{
		_trceEventId = new EventId( EventLogIds.TraceServiceReport );
		_failEventId = new EventId( EventLogIds.GeneralErrorException );
		_infoEventId = new EventId( EventLogIds.InfoServiceReport );
		_logger = logger;

		_logger.LogInformation( _infoEventId, "WebService Created" );
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	public ServiceStatus GetEchoMessage( string message )
	{
		_logger.LogTrace( _trceEventId, "Executing GetEchoMessage( Params => {Msg} )", message );

		return new ServiceStatus
		{
			StatusCode = 200,
			StatusMessage = message
		};
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public ServiceStatus GetServiceStatus()
	{
		_logger.LogTrace( _trceEventId, "Executing GetServiceStatus()" );

		return new ServiceStatus
		{
			StatusCode = 200,
			StatusMessage = "Successful"
		};
	}
}
