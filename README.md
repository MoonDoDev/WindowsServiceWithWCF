
# Servicio Windows que hospeda un WCF usando HTTPS

Este es un proyecto de ejemplo que muestra cómo construir un Servicio Windows, basándonos en un proyecto .NET de tipo `worker`, y adicional cómo hospedar en él un Web Service de tipo WCF, para exponerlo de forma segura, usando el protocolo SSL.

Actualmente en .NET Core no existe un proyecto propiamente para crear un Servicio Windows, y es por esto que nos tenemos que apoyar del tipo de proyecto `worker` y modificarlo para que opere como un Servicio Windows.

Otro tema que nos proponemos cubrir en este proyecto de ejemplo, es mostrar cómo hospedar un Servicio Web WCF, sin necesidad de usar el IIS de Windows.

El proyecto está construido en Visual Studio 2022, con .NET Core 8 para Windows, y los pasos son los siguientes:

### Creamos el Servicio Windows:
- [x]  Creamos el proyecto **worker** desde una Consola de Windows o directamente desde Visual Studio, seleccionando el tipo de proyecto **Worker Service**:

```
dotnet new worker --name "WindowsServiceBase"
```
- [x]  Instalamos el paquete NuGet requerido:

```
dotnet add package Microsoft.Extensions.Hosting.WindowsServices
```
- [x]  Chequeamos el archivo del proyecto **WindowsServiceBase.csproj**, el cual se debe ver de la siguiente manera:
```
<Project Sdk="Microsoft.NET.Sdk.Worker">

   <PropertyGroup>
      <TargetFramework>net8.0-windows</TargetFramework>
      <Nullable>enable</Nullable>
      <ImplicitUsings>enable</ImplicitUsings>
      <UserSecretsId>dotnet-WindowsServiceBase-b7c3169e-baa4-49cc-821a-292307198e46</UserSecretsId>
   </PropertyGroup>

   <ItemGroup>
      <PackageReference Include="Microsoft.Extensions.Hosting" Version="9.0.1" />
      <PackageReference Include="Microsoft.Extensions.Hosting.WindowsServices" Version="9.0.1" />
   </ItemGroup>
   
</Project>
```
- [x]  Creamos la clase **ServiceProcessBase**, la cual contendrá el código que ejecutará nuestro Servicio Windows:
```
namespace WindowsServiceBase;

public sealed class ServiceProcessBase
{
   private readonly ServiceSettings? _appSettings;
   private readonly ILogger<ServiceProcessBase> _logger;

   public ServiceProcessBase( ... )
   {
      ...
   }
}
```
- [x]  Renombramos la clase **Worker** y el archivo *.cs, por **ServiceBackWorker**:
```
namespace WindowsServiceBase;

public sealed class ServiceBackWorker : BackgroundService
{
   private readonly ServiceProcessBase _serviceProcess;
   private readonly ILogger<ServiceBackWorker> _logger;

   public ServiceBackWorker( ... )
   {
      ...
   }
}
```
- [x]  Actualizamos el archivo de configuración **appsettings.json**, agregando los parámetros requeridos para el manejo del **EventLog** en el Servicio Windows:
```
{
   "Logging": {
      "LogLevel": {
         "Default": "Warning",
         "WindowsServiceBase": "Trace"
      },
      "EventLog": {
         "SourceName": "Windows Service Base",
         "LogName": "Windows Service Base Events",
         "LogLevel": {
            "Default": "Warning",
            "WindowsServiceBase": "Trace"
         }
      }
   }
}
```
- [x]  Actualizamos el archivo **Program.cs**:
```
var builder = Host.CreateApplicationBuilder( args );

builder.Services.AddWindowsService( options => options.ServiceName = ServiceDefinitions.ServiceName );

Directory.SetCurrentDirectory( AppDomain.CurrentDomain.BaseDirectory );
builder.Logging.ClearProviders();

[...]

var host = builder.Build();
host.Run();
```
### Creamos el Servicio WCF:
- [x]  Agregamos al proyecto los paquetes requeridos para hospedar el Servicio WCF:

```
dotnet add package CoreWCF.Http
dotnet add package CoreWCF.Primitives
dotnet add package CoreWCF.ConfigurationManager
```
- [x]  Creamos la Interface con los **Service Contract** y los **Data Contract** requeridos para el WCF:
```
[ServiceContract]
public interface IServiceTemplate
{
   [OperationContract]
   Result<ServiceStatus> GetServiceStatus();

   [...]
}

[DataContract]
public record ServiceStatus
{
   [DataMember]
   public int StatusCode { get; set; }

   [...]
}
```
- [x]  Creamos la clase que implementa la Interface creada, con la funcionalidad de los métodos que estamos exponiendo:
```
public class ServiceTemplate : IServiceTemplate
{
   [...]
}
```
- [x]  Creamos el archivo de configuración **WebService.config** para hospedar el Servicio WCF:
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
   <system.serviceModel>
      <behaviors>
         <serviceBehaviors>
            <behavior name="ServiceBehavior">
               <serviceMetadata httpGetEnabled="true" httpsGetEnabled="true"/>
               <serviceDebug includeExceptionDetailInFaults="true"/>
            </behavior>
         </serviceBehaviors>
      </behaviors>
      <services>
         [...]
      </services>
      <bindings>
         <basicHttpBinding>
            <binding name="basicHttpBindingConfiguration">
               <security mode="Transport">
                  <transport clientCredentialType="None"/>
               </security>
            </binding>
         </basicHttpBinding>
      </bindings>
   </system.serviceModel>
</configuration>
```
- [x]  Actualizamos la clase base del Servicio Windows, para crear el método que nos permitirá hospedar el Servicio WCF:
```
private void CreateWCFServiceInstance()
{
   var builder = WebApplication.CreateBuilder();

   builder.Logging.AddEventLog( elSettings =>
   {
      elSettings.SourceName = ServiceDefinitions.EventLogSource;
      elSettings.LogName = ServiceDefinitions.EventLogName;
   } );

   builder.WebHost.ConfigureKestrel( ( context, options ) =>
   {
      [...]
   }

   [...]
}
```
### Configuramos el Kestrel e iniciamos el Servicio:
- [x]  Configuramos el Web Server de ASP.NET Core para que use HTTPS para hospedar el Servicio WCF:
Para este punto es importante contar con el archivo (*.pfx) el cual corresponde al certificado que usaremos para la conexión segura SSL, teniendo en cuenta que dicho Certificado ya debería estar instalado en la máquina donde estamos hospedando el Servicio Windows y por ende el Servicio WCF. 
```
options.ListenAnyIP( 443, listenOptions =>
{
   listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
   listenOptions.UseHttps( "Certificado.pfx", "1234" );
} );

```
- [x]  Instalamos nuestro Servicio Windows en la Consola de Servicios:
Para instalar el Servicio Windows en la Consola, debemos abrir una ventana de línea de comandos con privilegios de Admin, y allí ejecutar el siguiente comando:
```
sc.exe create "Service Name" binpath= "C:\Projects\Published\WindowsServiceBase.exe"
```
Después de tener el Servicio Windows instalado, abrimos la Consola de Servicios Windows, lo ubicamos con su nombre y lo iniciamos. Si el Servicio Windows inicia sin problemas, el Servicio WCF ya debería estar disponible para probarlo en un navegador, digitando la dirección HTTPS que le configuramos.

> [!NOTE]
> - Para desplegar y probar este proyecto en mi máquina de trabajo, fue necesario generar un certificado SSL de pruebas con la ayuda de la herramienta [**OpenSSL**](https://www.openssl.org/), y con cierto proceso, el cual está disponible en Internet, generamos el archivo **Certificado.pfx**, teniendo como base los archivos **Certificado.crt** y **Certificado.key**.
> - Si tienes alguna inquietud al respecto, me puedes contactar, y con gusto te apoyo en tus proyectos.

---------

[**YouTube**](https://www.youtube.com/@hectorgomez-backend-dev/featured) - 
[**LinkedIn**](https://www.linkedin.com/in/hectorgomez-backend-dev/) -
[**GitHub**](https://github.com/MoonDoDev/Jubatus.WebApi.Extensions)
