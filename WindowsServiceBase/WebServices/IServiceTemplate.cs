using CoreWCF;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace WindowsServiceBase.WebServices;

/// <summary>
/// 
/// </summary>
[DataContract]
public record ServiceStatus
{
	[DataMember]
	public int StatusCode { get; set; }

	[DataMember]
	[AllowNull]
	public string? StatusMessage { get; set; }
}

/// <summary>
/// 
/// </summary>
[ServiceContract]
public interface IServiceTemplate
{
	[OperationContract]
	ServiceStatus GetServiceStatus();

	[OperationContract]
	ServiceStatus GetEchoMessage( string message );
}
