7---
theme: gaia
class: invert
paginate: true
---
# EUMEL DJ Party Suite

### Open source on [github](https://github.com/EUMEL-Suite)

### Goal is learning new things

### And it solves my party music problem.

---
# Overview

* Music app for partys
* App plays random songs
* Guests can "vote" for songs
* DJ will prefer voted songs.

---
## User's Interface

* Desktop app as music player
* Mobile app for guests (iPhone, Android, UWP)
* Qrcode-based login.

---
## Features for guests

* Login using a qrcode
* Can view playlist
* Can up-vote for available songs
* Can send/receive chat messages
* "Admin" guests can control player.

---
## WPF desktop app features

* Use iTunes library as song provider
* Select available songs based on iTunes playlist
* Show player and song info
* Show chat messages to all guests.

---
## Demo.

---
## Primary Technologies

* WPF-based desktop app
* Xamarin-based client
* ASP.NET core-based REST backend
* Syslog logging server.

---
## Secondary Technologies

* Caliburn.Micro for MVVM in WPF
* TinyMessageBus for server-server communication
* SignalR for server-client communication
* QRCoder and ZXing for qrcode token "login"
* Serilog as logging backend provider
* StructureMap for dependency injection on server.

---
# Implementation details.

---
## TinyMessageBus Overview

* Message bus for communication between components
* `Hub` is a context for all-to-all communication.
* Uses a publish/subscribe pattern

---
## TinyMessageBus Implementation

* `TinyMessengerHub.DefaultHub` for system wide coms
* `IComponentMessageHub` for UI internal communication
* `Subscribe` in constructor
* `Unsubscribe` in `Dispose` (_not required_).

---
## Demo

* `CoreServicesRegistry`
* `WebServiceHost` _publish_
* `StatusViewModel.ServiceStatusChanged` _subscribe_.

---
## Jaunt: Strong vs. Weak Reference

* References to objects are counted
* Strong reference increases a reference counter
* Weak reference does not increate the reference counter
* If a reference counter is back to `0` the reference can be removed
* A weak reference can be collected by GC, so a reference becomes `null`
* Strong references can causes memory leaks (caching)
* TinyMessageBus uses WeakReference internally.

<!-- THOMAS, you are here with your documentation -->
---
## Syslog Overview

* TCP or UDP service to aggregate error message
* Network-wide single point of logging
...


---
## Syslog Implementation

* Desktop app uses serilog for logging
* Mobile app uses serilog client

---
## Demo

----
## Jaunt: Nuget commandline

* Install packages from packages.config
* Download packages into dedicated folder
* Get tools without need of .NET or Visual Studio
* `nuget install syslog-server`
* _Issue_: Dependencies

---
# Thank you for listening

* [Thomas Ley](https://www.linkedin.com/in/thomas-ley/)
* [codequalitycoach@outlook.de](mailto:codequalitycoach@outlook.de)
* [@CleanCodeCoach](https://twitter.com/CleanCodeCoach).


# Missing topics

* Additional swagger header for token
* TinyMessageBus to SignalR adapter
* Message Hub to Log Adapter
* Create a sink on serilog could be another option



# Missing features

* WPF ui design :-)
* docking support for WPF ui
* Logging in a proper way
* Test iPhone
* Fix issues UWP (e.g. theming)
* Push to 
	* Play Store
	* App Store
	* Microsoft Store
* Logging settings from serilog library settings file
* DjList should be smarter
* Configuration settings e.g. backend
* youtube playlist/music requests

* implementation resolver `ImplementationResolver<T>`

* C# features
	* Discrds (aka. underscore)
	* `FileSystemMp3Searcher` has a record `private record SongWrapper(Song Song, string Id, Uri Location)`
