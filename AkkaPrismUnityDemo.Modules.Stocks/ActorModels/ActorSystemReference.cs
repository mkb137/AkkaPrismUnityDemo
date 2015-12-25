using System;
using Akka.Actor;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels {

	internal static class ActorSystemReference {

		/// <summary>
		/// The static constructor.
		/// </summary>
		static ActorSystemReference() {
			CreateActorSystem();
		}

		/// <summary>
		/// Singleton instance of our actor system.
		/// </summary>
		public static ActorSystem ActorSystem { get; private set; }

		/// <summary>
		/// Creates the singleton actor system.
		/// </summary>
		private static void CreateActorSystem() {
			// Get a reference to the Prism container
		}

	}

}
