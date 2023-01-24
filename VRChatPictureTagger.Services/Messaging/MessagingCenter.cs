using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Options;

using VRChatPictureTagger.Core.Enums;
using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.Services.Messaging
{
	public class MessagingCenter : IMessagingCenter
	{
		private readonly Dictionary<MessagingEvents, List<SubscriptionBase>> _subscriptions = new();
		private readonly IOptions<WindowAndNavigationOptions> _options;

		public MessagingCenter(IOptions<WindowAndNavigationOptions> options)
		{
			MessagingEvents[] events = (MessagingEvents[])Enum.GetValues(typeof(MessagingEvents));
			foreach (var @event in events)
				_subscriptions[@event] = new List<SubscriptionBase>();
			_options = options;
		}

		/// <summary>
		/// Subscribe to be notified that a specific event occured.
		/// </summary>
		/// <param name="events">The event that you want to be notified for.</param>
		/// <param name="callback">The action that is invoked when the subscribed event occurs. This is guaranteed to be run on the UI thread.</param>
		/// <returns></returns>
		public void SubscribeTo(MessagingEvents events, Action callback)
		{
			NotificationSubscription not = new(events, callback, callback.Target);
			List<SubscriptionBase> lstSubs = _subscriptions[events];
			lock (lstSubs)
				lstSubs.Add(not);
		}

		/// <summary>
		/// Subscribe to receive a message when a specific event occured
		/// </summary>
		/// <typeparam name="T">The type of data being sent</typeparam>
		/// <param name="events">The event that occured</param>
		/// <param name="callback">The action that the data is being handed to and invoked when the subscribed event occurs. This is guaranteed to be run on the UI Thread.</param>
		/// <returns></returns>
		public void SubscribeTo<T>(MessagingEvents events, Action<T> callback)
		{
			MessageSubscription<T> msg = new(events, callback, callback.Target);
			List<SubscriptionBase> lstSubs = _subscriptions[events];
			lock (lstSubs)
				lstSubs.Add(msg);
		}

		/// <summary>
		/// Unsibscribe from a subscribed Notification or Message
		/// </summary>
		/// <param name="owner">The object that subscribed to a notification or message</param>
		/// <param name="events">The event that was subscribed to</param>
		public void UnsubscribeFrom(object owner, MessagingEvents events)
		{
			List<SubscriptionBase> lstSubs = _subscriptions[events];
			lock (lstSubs)
				lstSubs.RemoveAll(x => x.Owner == owner);
		}

		/// <summary>
		/// Notifies all subscribers of an event on the main thread.
		/// </summary>
		/// <param name="events">The event that happened</param>
		public void Send(MessagingEvents events)
		{
			List<SubscriptionBase> lstSubs = _subscriptions[events].ToList();
			foreach (var item in lstSubs.OfType<NotificationSubscription>())
				_options.Value.UIDispatcher.TryEnqueue(() => { item.Notify(); });
		}
		/// <summary>
		/// Messages an object to every subscriber of an event on the main thread.
		/// </summary>
		/// <typeparam name="T">The type of the object to be sent to the subscribers</typeparam>
		/// <param name="events">The event that happened</param>
		/// <param name="message">The object to be sent to the subscribers</param>
		/// <param name="asNotification">Handle this also as a notification, so that the message also triggers notification subscriptions</param>
		public void Send<T>(MessagingEvents events, T message, bool asNotification = true)
		{
			List<SubscriptionBase> lstSubs = _subscriptions[events].ToList();
			foreach (var item in lstSubs)
			{
				if (item is MessageSubscription<T> msg)
					_options.Value.UIDispatcher.TryEnqueue(() => { msg.Send(message); });
				else if (asNotification && item is NotificationSubscription not)
					_options.Value.UIDispatcher.TryEnqueue(() => { not.Notify(); });
			}
		}
	}
}
