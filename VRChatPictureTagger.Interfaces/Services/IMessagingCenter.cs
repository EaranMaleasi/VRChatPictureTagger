using System;

using VRChatPictureTagger.Core.Enums;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface IMessagingCenter
	{
		/// <summary>
		/// Notifies all subscribers of an event on the main thread.
		/// </summary>
		/// <param name="events">The event that happened</param>
		void Send(MessagingEvents events);
		/// <summary>
		/// Messages an object to every subscriber of an event on the main thread.
		/// </summary>
		/// <typeparam name="T">The type of the object to be sent to the subscribers</typeparam>
		/// <param name="events">The event that happened</param>
		/// <param name="message">The object to be sent to the subscribers</param>
		/// <param name="asNotification">Handle this also as a notification, so that the message also triggers notification subscriptions</param>
		void Send<T>(MessagingEvents events, T message, bool asNotification = true);
		/// <summary>
		/// Subscribe to be notified that a specific event occured.
		/// </summary>
		/// <param name="events">The event that you want to be notified for.</param>
		/// <param name="callback">The action that is invoked when the subscribed event occurs. This is guaranteed to be run on the UI Thread.</param>
		/// <returns></returns>
		void SubscribeTo(MessagingEvents events, Action callback);
		/// <summary>
		/// Subscribe to receive a message when a specific event occured
		/// </summary>
		/// <typeparam name="T">The type of data being sent</typeparam>
		/// <param name="events">The event that occured</param>
		/// <param name="callback">The action that the data is being handed to and invoked when the subscribed event occurs. This is guaranteed to be run on the UI Thread.</param>
		/// <returns></returns>
		void SubscribeTo<T>(MessagingEvents events, Action<T> callback);
		/// <summary>
		/// Unsibscribe from a subscribed Notification or Message
		/// </summary>
		/// <param name="owner">The object that subscribed to a notification or message</param>
		/// <param name="events">The event that was subscribed to</param>
		void UnsubscribeFrom(object owner, MessagingEvents events);
	}
}