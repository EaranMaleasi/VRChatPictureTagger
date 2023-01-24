using System;
using System.Collections.Generic;

using VRChatPictureTagger.Core.Enums;

namespace VRChatPictureTagger.Services.Messaging
{
	internal abstract class SubscriptionBase : IEquatable<SubscriptionBase>
	{
		public object Owner { get; set; }

		public MessagingEvents Event { get; private set; }

		public SubscriptionBase(MessagingEvents eventMassage, object owner)
		{
			Event = eventMassage;
			Owner = owner;
		}

		public override string ToString()
			=> $"Event {Event} subscribed by {Owner}";

		public override bool Equals(object obj)
			=> Equals(obj as SubscriptionBase);

		public bool Equals(SubscriptionBase other)
			=> other != null &&
			   Owner.Equals(other.Owner) &&
			   Event == other.Event;

		public override int GetHashCode()
			=> HashCode.Combine(Owner, Event);

		public static bool operator ==(SubscriptionBase base1, SubscriptionBase base2)
			=> EqualityComparer<SubscriptionBase>.Default.Equals(base1, base2);

		public static bool operator !=(SubscriptionBase base1, SubscriptionBase base2)
			=> !(base1 == base2);
	}
}
