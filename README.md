SparkIODotNet
=============
This project is a set of c# class libraries for interacting with a Spark Core.
The Spark Core is an IoT device that is similar to a an Arduino or Netduino.  It
is different in that it has built in WiFi and connects to the cloud for connected
functionality.

Before you use this library, you need to get and understand the sparkCore and its
environment.  Goto www.Spark.io for all of the information you could ever use and
for purchasing options.

The project is a Visual Studio 2012 Solution with several projects:

- BaseAPI : A base Class for use by the specific classes in the library
- CoreAPI : A class for accessing functions and variables on your Spark Core
- EventsAPI : A class for accessing events published by the SparkCors as native .Net events
- TinkerAPI : A class for accessing the functionality of the Tinker Firmware on your device.

The Solution's projects can be referenced from your application by referencing th projects or 
including a built DLL.  It does not make use of the App.COnfig file.  All configuration occurs 
in the constructors.
