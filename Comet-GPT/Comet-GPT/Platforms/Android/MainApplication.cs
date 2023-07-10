﻿using Android.App;
using Android.Runtime;
using Microsoft.Maui.Hosting;

namespace Comet_GPT;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)	: base(handle, ownership)
	{
	}
	protected override MauiApp CreateMauiApp() => App.CreateMauiApp();
}
