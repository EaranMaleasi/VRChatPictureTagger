using System;

using VRChatPictureTagger.Core.Enums;

namespace VRChatPictureTagger.Services.Messaging
{
	internal class NotificationSubscription : SubscriptionBase
	{
		private readonly Action _callback;

		public NotificationSubscription(MessagingEvents eventMassage, Action callback, object _owner) : base(eventMassage, _owner)
			=> _callback = callback;

		/// <summary>
		/// Send a notification to the subscriber
		/// </summary>
		public void Notify()
			=> _callback();
	}
}
