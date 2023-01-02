using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using VRChatPictureTagger.DbContexts.VRCPT;
using VRChatPictureTagger.DbContexts.VRCX;

namespace VRChatPictureTagger.DbContexts.Bootstrap
{
	public static class ContextDependencyInjectionExtensions
	{
		public static IServiceCollection AddVRCPT_Contexts(this IServiceCollection services)
		{
			services.AddDbContext<VrcxContext>();
			services.AddDbContext<VRCPTContext>();
			return services;
		}
	}
}
