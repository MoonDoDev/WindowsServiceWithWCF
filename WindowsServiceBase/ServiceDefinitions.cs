namespace WindowsServiceBase;

/// <summary>
/// 
/// </summary>
public static class ServiceDefinitions
{
	public const string ServiceName = "Jubatus Project WinService";
	public const string EventLogSource = "Windows Service Base";
	public const string EventLogName = "Windows Service Base Events";
}

/// <summary>
/// 
/// </summary>
public static class EventLogIds
{
	public const int InfoStartService = 1000;
	public const int InfoStopService = 1001;
	public const int InfoServiceReport = 1002;

	public const int GeneralErrorException = 1100;
	public const int CanceledErrorException = 1101;

	public const int TraceServiceReport = 1200;
}
