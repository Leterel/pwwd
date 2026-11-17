# pwwd

A lightweight, open-source, cryptographically secure password generator.

Available as a PowerShell script, PowerShell module, standalone Windows executable, and Windows MSI installer.

## Features

- Cryptographically secure random password generation
- Configurable password length (1-1024 characters)
- Uppercase and lowercase letters, numbers, and special characters
- Fully offline
- No accounts, telemetry, or network requests
- No password storage
- MIT licensed

## Downloads

Prebuilt Windows packages are available from GitHub Releases.

- Portable ZIP: `pwwd-v0.1.0-win-x64.zip`
- Windows installer: `pwwd-v0.1.0-win-x64.msi`

## Windows executable

Extract the portable ZIP and run:

```powershell
.\pwwd.exe
.\pwwd.exe --length 32
.\pwwd.exe --help
.\pwwd.exe --version
```

The standalone Windows executable includes the required .NET runtime.

## PowerShell script

Requires PowerShell 7 or later.

```powershell
.\pwwd.ps1
.\pwwd.ps1 -Length 32
```

## PowerShell module

```powershell
Import-Module .\pwwd.psm1
New-SecurePassword -Length 32
```

## Build from source

Requires .NET 10 SDK.

```powershell
dotnet publish .\src\pwwd-cli\pwwd-cli.csproj --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true
```

The MSI installer is built using WiX Toolset 7 and the source file `installer/Package.wxs`.

## Security

pwwd uses `System.Security.Cryptography.RandomNumberGenerator.GetInt32()` to select characters using a cryptographically secure random number generator.

Each character is selected independently. The generator does not guarantee that every character category appears in every password.

Passwords are generated locally and printed to standard output. They are not transmitted or intentionally saved by the application.

Be aware that generated passwords may remain visible in terminal output or logs.

## License

Licensed under the MIT License. See [LICENSE](LICENSE).
