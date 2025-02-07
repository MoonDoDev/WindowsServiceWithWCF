namespace WindowsServiceBase.Settings;

/// <summary>
/// 
/// </summary>
public record ServiceSettings
{
	public string? LogPathName { get; set; }

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public bool AllDataIsValid()
	{
		return !string.IsNullOrEmpty( this.LogPathName );
	}
}
