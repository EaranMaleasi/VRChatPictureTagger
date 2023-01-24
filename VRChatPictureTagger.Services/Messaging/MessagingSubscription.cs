using System;

using VRChatPictureTagger.Core.Enums;

namespace VRChatPictureTagger.Services.Messaging
{
	internal class MessageSubscription<T> : SubscriptionBase
	{
		private readonly Action<T> _callback;

		public MessageSubscription(MessagingEvents events, Action<T> callback, object _owner) : base(events, _owner)
			=> _callback = callback;

		/// <summary>
		/// Send the mesasge to the subscriber
		/// </summary>
		/// <param name="Message"></param>
		public void Send(T Message)
			=> _callback(Message);
	}
}
