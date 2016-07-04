# Android ePUB reader for Xamarin
An ePUB reader made in Xamarin.Android

This is a Xamarin.Android ePUB viewer based on this code: http://www.codeproject.com/Articles/592909/EPUB-Viewer-for-Android-with-Text-to-Speech

Although it works, it is a very old implementation of ePUB rendering. Since my programming skills are pretty lame, it has a lot of errors, it doesn't follow any C# code pattern or best practices and a lot of ugly code-related stuffs either.

But if you have to write an app for Android using Xamarin that can open an ePUB file (and you don't have much time to implement from scratch or money to get better frameworks), this can speed up some of your development.

* A little difference from the originalversion: the former code used two main viewers, one for Android 2.3 and one for Android 3.0. The previous one were removed, since almost every Android device now supports it. As a result, the HTML parsers are gone as well.

* Although the license is GLP v3, use the way you want it. If there are any restrictions in the license, just ignore it.
