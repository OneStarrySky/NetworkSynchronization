﻿#if UNITY_ANDROID
using System;
using UnityEngine;

namespace Reign.Plugin
{
	public class DFP_InterstitialAdPlugin_Android : IInterstitialAdPlugin
	{
		private string id;
		private AndroidJavaClass native;
		private InterstitialAdEventCallbackMethod eventCallback;
		private InterstitialAdCreatedCallbackMethod createdCallback;

		public DFP_InterstitialAdPlugin_Android (InterstitialAdDesc desc, InterstitialAdCreatedCallbackMethod createdCallback)
		{
			this.createdCallback = createdCallback;
			try
			{
				eventCallback = desc.EventCallback;
				native = new AndroidJavaClass("com.reignstudios.reignnative.DFP_InterstitialAdNative");
				id = Guid.NewGuid().ToString();
				native.CallStatic("CreateAd", desc.Android_DFP_UnitID, desc.Testing, id);
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
				if (createdCallback != null) createdCallback(false);
			}
		}

		~DFP_InterstitialAdPlugin_Android()
		{
			if (native != null)
			{
				native.Dispose();
				native = null;
			}
		}

		public void Dispose()
		{
			if (native != null)
			{
				native.CallStatic("Dispose", id);
				native.Dispose();
				native = null;
			}
		}

		public void Cache()
		{
			native.CallStatic("Cache", id);
		}

		public void Show ()
		{
			native.CallStatic("Show", id);
		}

		public void Update ()
		{
			if (eventCallback != null && native.CallStatic<bool>("HasEvents"))
			{
				string eventName = native.CallStatic<string>("GetNextEvent");
				var eventValues = eventName.Split(':');
				switch (eventValues[0])
				{
					case "Created": if (createdCallback != null) createdCallback(eventValues[1] != "Failed"); break;
					case "Cached": eventCallback(InterstitialAdEvents.Cached, null); break;
					case "Shown": eventCallback(InterstitialAdEvents.Shown, null); break;
					case "Canceled": eventCallback(InterstitialAdEvents.Canceled, null); break;
					case "Clicked": eventCallback(InterstitialAdEvents.Clicked, null); break;
					case "Error": eventCallback(InterstitialAdEvents.Error, eventValues[1]); break;
				}
			}
		}

		public void OnGUI()
		{
			// do nothing...
		}

		public void OverrideOnGUI()
		{
			// do nothing...
		}
	}
}
#endif