using FluentResults;
using CoreWCF;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace WCFServiceTemplate;

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
[DataContract]
public record EchoMessage
{
	[DataMember]
	[AllowNull]
	public string? Message { get; set; }
}

/// <summary>
/// 
/// </summary>
[ServiceContract]
public interface IServiceTemplate
{
	[OperationContract]
	Result<ServiceStatus> GetServiceStatus();

	[OperationContract]
	Result<EchoMessage> GetEchoMessage( string message );
}
