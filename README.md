[![Build](https://github.com/VasilyBulygin/HaspSessionKiller/actions/workflows/dotnet.yml/badge.svg)](https://github.com/VasilyBulygin/HaspSessionKiller/actions/workflows/dotnet.yml) ![GitHub](https://img.shields.io/github/license/VasilyBulygin/HaspSessionKiller) ![GitHub top language](https://img.shields.io/github/languages/top/VasilyBulygin/HaspSessionKiller)
# HaspSessionKiller
Simple utility that kills all sessions connected to HASP service

## Building
For building application execute `dotnet build` command.

## Usage
Run `HaspSessionKiller.exe` to kill all connected to ypur HASP key sessions. By default, utility will try to connect to HASP service on port 1947. If you have different service port configuration then simply add specific port number as a parameter.
