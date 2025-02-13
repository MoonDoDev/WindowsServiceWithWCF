﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EchoWebService
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceStatus", Namespace="http://schemas.datacontract.org/2004/07/WindowsServiceBase.WebServices")]
    public partial class ServiceStatus : object
    {
        
        private int StatusCodeField;
        
        private string StatusMessageField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StatusCode
        {
            get
            {
                return this.StatusCodeField;
            }
            set
            {
                this.StatusCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StatusMessage
        {
            get
            {
                return this.StatusMessageField;
            }
            set
            {
                this.StatusMessageField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EchoWebService.IServiceTemplate")]
    public interface IServiceTemplate
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTemplate/GetServiceStatus", ReplyAction="http://tempuri.org/IServiceTemplate/GetServiceStatusResponse")]
        System.Threading.Tasks.Task<EchoWebService.ServiceStatus> GetServiceStatusAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTemplate/GetEchoMessage", ReplyAction="http://tempuri.org/IServiceTemplate/GetEchoMessageResponse")]
        System.Threading.Tasks.Task<EchoWebService.ServiceStatus> GetEchoMessageAsync(string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public interface IServiceTemplateChannel : EchoWebService.IServiceTemplate, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public partial class ServiceTemplateClient : System.ServiceModel.ClientBase<EchoWebService.IServiceTemplate>, EchoWebService.IServiceTemplate
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ServiceTemplateClient() : 
                base(ServiceTemplateClient.GetDefaultBinding(), ServiceTemplateClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.basicHttpBindingConfiguration_IServiceTemplate.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceTemplateClient(EndpointConfiguration endpointConfiguration) : 
                base(ServiceTemplateClient.GetBindingForEndpoint(endpointConfiguration), ServiceTemplateClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceTemplateClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ServiceTemplateClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceTemplateClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ServiceTemplateClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceTemplateClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<EchoWebService.ServiceStatus> GetServiceStatusAsync()
        {
            return base.Channel.GetServiceStatusAsync();
        }
        
        public System.Threading.Tasks.Task<EchoWebService.ServiceStatus> GetEchoMessageAsync(string message)
        {
            return base.Channel.GetEchoMessageAsync(message);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.basicHttpBindingConfiguration_IServiceTemplate))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.basicHttpBindingConfiguration_IServiceTemplate))
            {
                return new System.ServiceModel.EndpointAddress("https://localhost:5001/EchoService");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return ServiceTemplateClient.GetBindingForEndpoint(EndpointConfiguration.basicHttpBindingConfiguration_IServiceTemplate);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return ServiceTemplateClient.GetEndpointAddress(EndpointConfiguration.basicHttpBindingConfiguration_IServiceTemplate);
        }
        
        public enum EndpointConfiguration
        {
            
            basicHttpBindingConfiguration_IServiceTemplate,
        }
    }
}
