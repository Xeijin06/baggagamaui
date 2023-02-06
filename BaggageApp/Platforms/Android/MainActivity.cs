﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace BaggageApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, WindowSoftInputMode = Android.Views.SoftInput.AdjustPan, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
