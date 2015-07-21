using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace StrubT.PlayGround.CSharpConsole {

	public class WebServiceTest : IRunnable {

		public bool IsActive() => false;

		public void Run() {

			using (var soap = new ServiceHost(typeof(WebService), new Uri("http://localhost:8079/soapService")))
			using (var rest = new WebServiceHost(typeof(WebService), new Uri("http://localhost:8079/restService"))) {
				var meta = soap.Description.Behaviors.Find<ServiceMetadataBehavior>() ?? new ServiceMetadataBehavior();
				meta.HttpGetEnabled = true;
				soap.Description.Behaviors.Add(meta);
				soap.Open();

				meta = rest.Description.Behaviors.Find<ServiceMetadataBehavior>() ?? new ServiceMetadataBehavior();
				meta.HttpGetEnabled = true;
				rest.Description.Behaviors.Add(meta);
				rest.Open();

				Console.Write("press enter to stop web services");
				Console.ReadLine();
			}
		}
	}

	[ServiceContract]
	public interface IWebService {

		[OperationContract]
		[WebGet(UriTemplate = "dateTime", ResponseFormat = WebMessageFormat.Json)]
		DateTime GetDateTime();

		[OperationContract]
		[WebGet(UriTemplate = "hostName", ResponseFormat = WebMessageFormat.Json)]
		string GetHostName();

		[OperationContract]
		[WebGet(UriTemplate = "userName", ResponseFormat = WebMessageFormat.Json)]
		string GetUserName();

		[OperationContract]
		[WebGet(UriTemplate = "specialFolders", ResponseFormat = WebMessageFormat.Json)]
		Dictionary<string, string> GetSpecialFolders();
	}

	[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
	public class WebService : IWebService {

		public DateTime GetDateTime() => DateTime.Now;

		public string GetHostName() => Environment.MachineName;

		public string GetUserName() => Environment.UserName;

		public Dictionary<string, string> GetSpecialFolders() => Enum.GetValues(typeof(Environment.SpecialFolder)).Cast<Environment.SpecialFolder>().Distinct().ToDictionary(f => f.ToString(), Environment.GetFolderPath);
	}
}
