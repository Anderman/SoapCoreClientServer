using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using SoapCore.SoapServices;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Hosting
{
	public static class ApplicationBuilderExtensions
	{
		public static Dictionary<string, OperationDescription> Services = new Dictionary<string, OperationDescription>();
		public static IApplicationBuilder UseSoapEndpoints<T>(this IApplicationBuilder builder) where T : class
		{
			AddAllAssemblyContracts<T>();
			return builder.UseMiddleware<SoapEndpointMiddleware>(Services);
		}
		
		private static void AddAllAssemblyContracts<T>() where T : class
		{
			Services = (
				from serviceType in GetServiceContractTypes<T>()
				from serviceContract in serviceType.GetTypeInfo().GetCustomAttributes<ServiceContractAttribute>()
				from operationMethodInfo in serviceType.GetTypeInfo().DeclaredMethods
				from operationContract in operationMethodInfo.GetCustomAttributes<OperationContractAttribute>()
				select new OperationDescription
				{
					ServiceType = serviceType,                                  //Used to create a instance by Core DI
					DispatchMethod = operationMethodInfo,                       //the service to execute
					SoapAction = operationContract.Action,                      //used in the soap request to select the service that should be executed
					Namespace = serviceContract.Namespace,                      //Namespace used for the action, request, response and result xmlElements
					Name = operationContract.Name ?? operationMethodInfo.Name   //Name that is used to construct action, request, response and result xmlElements
				}).ToDictionary(x => x.SoapAction);
		}

		private static IEnumerable<Type> GetServiceContractTypes<T>() where T : class
		{
			return typeof(T).Assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ServiceContractAttribute), true).Length > 0);
		}
	}
}