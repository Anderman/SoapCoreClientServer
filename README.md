# Soap client and server for net core based on xml specification

## Reason for this project.
Currently net core does not support soap12 addressing for clients, no support for soap services and to complex for most of my projects.
This implementation allow me to use the same controller for soap and json requests

## nuget

   `PM> Install-Package Medella.SoapCore`

## Soap service 
To Use soap services add the following line to your config in the `startup.cs`.

```csharp
public void Configure(IApplicationBuilder app)
{
	app.UseSoapServices<Startup>();
}
```
Now you have to create your soap service interface with the request and response object

```csharp
public class IsAliveResponse : object
{
	[XmlElement(Namespace = "urn:www-vecozo-nl:messages:isalive:v1")]
	public bool Resultaat { get; set; }
}

public class IsAliveRequest : object
{
}

[ServiceContract(Namespace = "urn:www-vecozo-nl:isalive:v1")]
public class AliveService
{
	[OperationContract(Action = "urn:www-vecozo-nl:v1:isalive", ReplyAction = "urn:www-vecozo-nl:v1:isaliveresponse")]
	public IsAliveResponse IsAlive(IsAliveRequest request)
	{
		return new IsAliveResponse { Resultaat = true };
	}
}
```

## Soap Client
The soap client can be used when it added to your services in the startup.cs

```csharp
public void ConfigureServices(IServiceCollection services)
{
	services.AddSoapClients();
}
```

Use a client in a controller


```csharp
public class SoapController
{
	private readonly SoapClient<Config, DownloadPdfRequest, DownloadPdfResponse> _client;

	public SoapController(SoapClient<Config, DownloadPdfRequest, DownloadPdfResponse> client)
	{
		_client = client;
	}

	public async Task DownloadPdf(int pdfId)
	{
		var request = new DownloadPdfRequest { PdfId = pdfId };
		var result = await _client.PostAsync(request);
	}

	public class Config : ISoapConfig
	{
		public string Namespace => "urn:www-vecozo-nl:vsp:edp:declareren:downloaden:v1";
		public string SoapActionElementName => "DownloadPdf";
		public string SoapAction => $"{Namespace}:{SoapActionElementName}";
		public X509Certificate ClientCertificate => new SoapClientX509Certificate2(new OutgoingCertificateProvider());

		public string GetUrl(IHostingEnvironment env)
		{
			return "https://accedpwebservice.vecozo.nl/Router.V1.svc/DownloadenRetourinformatieV1";
		}
	}
}
```

## Backgroud
### Soap service implementation
The middleware will scan all soap request. Soap requests has an *action*. 
In the wsdl file these *actions* are specifoed. With a WCF tool the xsd can converted to C#.

The c# file is a good starting point but can't be used and should be converted to a less verbose version.
The generated file can't be used because the default xml serializer and the soap xml serializer use different attributes to specify namespaces.

Also the 4 reference to *system.servicemodelxxx* should be removed when the VS tool is used.

The implemented soap service read only attributes of the *service contract* and *operation contracts*.
all other attributes should be removed and new xmlElement attributes should be placed on the right properties 
A xml attribute is only needed to set the correct namespace and can be used to overrule a elementname

#### incomming call
When the middleware sees a soap request then the service that implements the '*action*' is started by DI.
The soap request is parsed and converted to the request object parameter of the *operation*
All namespaces are removed by the parser. **Be sure that your request object has no namespaces**. Just remove all soap attributes from the generated c# file should be fine.

#### outgoing response
The action is called and the response object is returned to the middeleware.
The middeware serialize this object with an xmlserialize to a soap response. **Be sure that you have set the correct namespaces on the properies of the response object**. 
NB: If the namespace is not changed on a deeper level then the namespace attribute can be skipped.

#### troubleshooting
Problems with soap(xml) is almost always namespaces. there should be no namespace placed on all request parameter objects.
Only on the response object there should be namespace on the right place. Soap set namespaces on classes you should set the namespace on properties that refer to the classes.

The soap request and response xml will be logged on debug level.

### Soap Client implementation
A soap client can be instantiated by the DI. The client is specified by a config type, response type and a request type.
The generated WCF file is only used for the response and request object. Attributes on service and operation are ignored.
There are 3 things that should be copied from the service/operation attributes to the config file
* the soapaction from *action* attribute on the operationcontract
* the ActionElementname. this the *methodename* without the async
* the namespace from the servicecontract.

The url and the clientcertificate are also in the config file. The url can depend on the enviroment.

