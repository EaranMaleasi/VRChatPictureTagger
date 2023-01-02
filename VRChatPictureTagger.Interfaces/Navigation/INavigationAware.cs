using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChatPictureTagger.Interfaces.Navigation
{
	public interface INavigationAware
	{
		void NavigatedTo();
		void NavigatedFrom();
	}
}
