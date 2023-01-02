using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace VRChatPictureTagger.Models.Bootstrap
{
	public static class ModelsDependencyInjectionExtensions
	{
		public static IServiceCollection AddVRCPT_Models(this IServiceCollection services)
		{

			return services;
		}
	}
}
