# pwwd

A lightweight, cryptographically secure password generator written in PowerShell 7.

## Features

- Cryptographically secure random password generation
- Configurable password length (1–1024 characters)
- Uppercase and lowercase letters
- Numbers and special characters
- Fully offline
- No external dependencies
- Compatible with PowerShell 7 on Windows, Linux, and macOS

## Requirements

- PowerShell 7 or later

## Usage

Generate a 24-character password:

```powershell
./pwwd.ps1
```

Generate a password with a custom length:

```powershell
./pwwd.ps1 -Length 32
```

## Security

pwwd uses the .NET RandomNumberGenerator.GetInt32() method to generate cryptographically secure random character selections.

Passwords are generated locally and are not transmitted or saved by the script.

Each character is selected independently. The generator does not guarantee that every character category appears in every password.

## License

Licensed under the MIT License. See [LICENSE](LICENSE).
