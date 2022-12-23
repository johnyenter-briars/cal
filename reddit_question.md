# Best Way to Call API on a Scheduled Cadence?
I'm hoping the answer to my question is relatively simple and obvious to someone more knowledgeable about android development than me.

### Context

I'm building an app in [Xamarin Forms](https://learn.microsoft.com/en-us/xamarin/get-started/what-is-xamarin) and targeting for android version: 12.0.

My app needs to periodically make async HTTPS web requests to my API to fetch fresh data. Based on the state of the data, the app may send a push notification to the user.

These requests need to be on a period of every 15 minutes or so. But more importantly, these requests need to happen even when the app is **not** running in the foreground. (Or the app is killed and not running)

### What I've Tried:

#### Background Work

I took a look at [Background Work](https://developer.android.com/guide/background) and it didn't seem to be quite what I was looking for. It looked like it was more for doing *one* long running job away from the main UI thread - as opposed to actually scheduling something.

#### Broadcast Receiver

I read that Broadcast Receiver is no [longer guarenteed](https://github.com/xamarin/xamarin-android/issues/5105) to trigger on exact times. Which doesn't really matter for my purpose, but seemed to me to be an indication that this wasn't quite what I was looking for.


#### JobService

I read somewhere that using a [Job Service](https://developer.android.com/reference/android/app/job/JobService) was the recommended way to handle background tasks.

However, I schedule these, via [code](https://github.com/johnyenter-briars/cal/blob/51c9ee008f7768bc91cd70af570a22fca6d76113/CAL/CAL.Android/MainActivity.cs#L30) however, after the initial instance of the job, after the app is killed, no subsequent requests come through. (Even after waiting 15 minutes)

### Ultimate Objective

Does anybody have any experience scheduling a *simple* network request in Android? Is one of the above methods the "right" way? Or is there a different API that I should be using? Thank you!



