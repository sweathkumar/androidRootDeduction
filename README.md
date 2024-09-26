# Android Root Detection

This project implements a comprehensive root detection mechanism for Android applications using Xamarin. It helps identify whether a device is rooted, which is crucial for applications that require a secure environment.

## Features

- **Root Detection**: 
  - `IsJailBreaked()`: Checks for root binaries and management applications to determine if the device is rooted.
  
- **Emulator Detection**: 
  - `IsInEmulator()`: Identifies if the application is running on an emulator by examining the device's fingerprint.

- **Security Checks**: 
  - `IstestKeys()`: Detects the presence of test keys, often found in rooted environments.
  - `IsDebuggable()`: Determines if the app is built with the debuggable flag, indicating development mode.
  - `IsDebuggerConnected()`: Checks if a debugger is attached to the application.

- **Hook Detection**: 
  - `IsHooked()`: Scans for known rooting or hooking applications installed on the device.

- **Magisk Detection**: 
  - `IsMagisk()`: Looks for the presence of Magisk by checking for the `libstub.so` library in native directories.

- **File Path Management**: 
  - `GetFilePath(string filename)`: Returns the full file path for accessing files within the application's external storage directory.

## Usage

To integrate this root detection mechanism into your Xamarin Android application, simply implement the `IHardwareSecurity` interface and utilize the provided methods to assess the security status of the device.

## License

This project is licensed under the MIT License.
