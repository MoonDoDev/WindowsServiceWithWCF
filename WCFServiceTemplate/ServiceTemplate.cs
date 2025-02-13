using FluentResults;

namespace WCFServiceTemplate;

/// <summary>
/// 
/// </summary>
public class ServiceTemplate : IServiceTemplate
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	public Result<EchoMessage> GetEchoMessage( string message )
	{
		return Result.Ok( new EchoMessage
		{
			Message = message,
		} );
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public Result<ServiceStatus> GetServiceStatus()
	{
		return Result.Ok( new ServiceStatus
		{
			StatusCode = 200,
			StatusMessage = "Successful"
		} );
	}
}
