namespace WindowsServiceBase.Settings;

/// <summary>
/// Estructura de la sesión que contiene los parámetros de configuración en el archivo "appsettings.json"
/// </summary>
public record ServiceSettings
{
	/// <summary>
	/// Flag (true/false) que nos indica si se debe crear el archivo log
	/// </summary>
	public bool CreateLogFile { get; init; }

	/// <summary>
	/// Ruta donde se debe crear el arhivo log del Servicio
	/// - Ejemplo: "C:\\Temp\\"
	/// </summary>
	public string? LogPathName { get; init; }

	/// <summary>
	/// Url/Dominio donde se va hospedar el WebService.
	/// - Ejemplo: "https://jubatusproject.services.com"
	/// </summary>
	public string? WebServiceUrl { get; init; }

	/// <summary>
	/// Puerto que se usará para hospedar el WebService.
	/// - Ejemplo: "5001"
	/// </summary>
	public string? WebServicePort { get; init; }

	/// <summary>
	/// Endpoint que se usará para hospedar el WebService.
	/// - Ejemplo: "/EchoService"
	/// </summary>
	public string? WebServiceEndpoint { get; init; }

	/// <summary>
	/// Ruta y nombre del archivo *.PFX que contiene el Certificado firmado para las conexiones 
	/// seguras SSL/HTTPS con el dominio donde se está hospedando el WebService.
	/// - Ejemplo: "C:\\Projects\\SSL-Certificates\\server.pfx"
	/// </summary>
	public string? ServerCertificatePfxFile { get; init; }

	/// <summary>
	/// Contraseña/Passkey del archivo *.PFX.
	/// Ejemplo: "1234"
	/// </summary>
	public string? ServerCertificatePasskey { get; init; }

	/// <summary>
	/// Método que usaremos para permitir la validación de los parámetros del archivo "appsettings.json"
	/// </summary>
	/// <returns></returns>
	public bool AllDataIsValid()
	{
		return !string.IsNullOrEmpty( this.LogPathName ) &&
			!string.IsNullOrEmpty( this.WebServiceUrl ) &&
			!string.IsNullOrEmpty( this.WebServicePort ) &&
			!string.IsNullOrEmpty( this.WebServiceEndpoint ) &&
			!string.IsNullOrEmpty( this.ServerCertificatePfxFile ) &&
			!string.IsNullOrEmpty( this.ServerCertificatePasskey );
	}
}
