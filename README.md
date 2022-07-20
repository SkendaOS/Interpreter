# Custom C++ Interpreter for SkendaOS System

SkendaOS is an experimental operating system build with Cosmos Kernel using C# language. In case to make the system can run customizable software that developers build, we create a custom C++ based interpreter for SkendaOS. This interpreter can run C++ code in runtime environment of SkendaOS.

## Disclaimer

SkendaOS doesn't support multi-thread. `System.Threads` on Visual Studio won't work. All program must run in synchronus, so it must follow the SkendaOS Architecture to handle error or throws an exception to make sure the OS not crashing because the running program. System can handle error with custom exception handling that build in kernel. When the running program crash or stopped working, system can handle the error even it's not multi-threading.

## Understanding The Code Structure
![Code Structures](/Resources/Code_Structures.jpg)

## Current Software Architecture
![Software Architecture](/Resources/Sofrware_Architecture.jpg)

## Authors
- [@fahlisaputra](https://www.github.com/fahlisaputra)