# pwwd

**A lightweight, open-source password generator built for local use.**

pwwd generates random passwords locally using the cryptographic random-number generator provided by .NET. It is available as a PowerShell script, a PowerShell module, a standalone Windows x64 command-line application, and a Windows MSI installer.

- **Current release:** [v0.1.0](https://github.com/Leterel/pwwd/releases/tag/v0.1.0)
- **License:** MIT
- **Project:** [github.com/Leterel/pwwd](https://github.com/Leterel/pwwd)

## Why this project exists

pwwd started as a personal tool. I wanted a password generator that I could inspect, run locally, and control myself instead of depending on a browser-based password-generation workflow.

My experience with online password tools had become frustrating: I wanted a simple utility without a web page, account, advertising experience, or unnecessary services around the act of generating a password. That is a statement about my own preferences and experience, not a claim that another provider or product is inherently insecure.

The goal is deliberately narrow: generate a random password, print it locally, and get out of the way.

## Project status

**v0.1.0 is the first public release.** The command-line application, PowerShell script, PowerShell module, portable ZIP, and Windows MSI installer have been built and manually tested on Windows.

The project is still young. The current release is a password generator, not a password manager. It does not store, synchronize, organize, autofill, or recover passwords.

## Features

- Cryptographically secure random character selection using `.NET RandomNumberGenerator.GetInt32()`
- Default password length: 24 characters
- Configurable length: 1–1024 characters
- Lowercase letters, uppercase letters, digits, and the supported special characters
- Local generation; no network connection is needed
- No account, cloud service, telemetry, or password database
- PowerShell script and importable PowerShell module
- Standalone Windows x64 command-line executable
- Windows MSI installer
- MIT license

### Character set

The current generator selects each character independently from this alphabet:

```text
abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&*-_?
```

The generator does not currently guarantee that every character category appears in every generated password. If a particular website requires at least one uppercase letter, digit, or special character, inspect the generated password before using it.

## Downloads

Download the current release from:

**[pwwd v0.1.0 on GitHub Releases](https://github.com/Leterel/pwwd/releases/tag/v0.1.0)**

| File | Purpose |
|---|---|
| `pwwd-v0.1.0-win-x64.zip` | Portable Windows x64 package |
| `pwwd-v0.1.0-win-x64.msi` | Windows Installer package |
| `pwwd.exe` | Standalone Windows x64 executable |
| `SHA256SUMS.txt` | SHA-256 checksums for the release files |

The standalone executable is published as a self-contained .NET application. The .NET runtime does not need to be installed separately to run that executable.

### Verify a download

After downloading a release file, calculate its SHA-256 hash in PowerShell:

```powershell
Get-FileHash "$HOME\Downloads\pwwd-v0.1.0-win-x64.msi" -Algorithm SHA256
```

Compare the result with the corresponding entry in `SHA256SUMS.txt` from the same release.

## Usage

### Windows executable

For the portable version, extract the ZIP archive and open PowerShell in the extracted directory:

```powershell
.\pwwd.exe
.\pwwd.exe 32
.\pwwd.exe --length 32
.\pwwd.exe -l 32
.\pwwd.exe --help
.\pwwd.exe --version
```

The default length is 24 characters. The accepted length range is 1–1024 characters.

If you installed the MSI, the executable is installed by default at:

```text
C:\Program Files\pwwd\pwwd.exe
```

Example:

```powershell
& 'C:\Program Files\pwwd\pwwd.exe' --length 32
```

### PowerShell script

The script requires PowerShell 7 or later:

```powershell
.\pwwd.ps1
.\pwwd.ps1 -Length 32
```

### PowerShell module

Import the module and call its exported function:

```powershell
Import-Module .\pwwd.psm1
New-SecurePassword
New-SecurePassword -Length 32
```

The module exports the function `New-SecurePassword`. Remove it from the current session with:

```powershell
Remove-Module pwwd
```

## How it works

1. The application reads the requested password length, or uses the default of 24.
2. It allocates a character array of that length.
3. For every character position, it calls `RandomNumberGenerator.GetInt32(chars.Length)`.
4. The returned index selects one character from the supported character set.
5. The completed password is written to standard output.

The selection is performed independently for each character. The implementation uses a cryptographic random-number generator rather than a general-purpose pseudo-random generator.

## Security notes and limitations

pwwd is intentionally small, but a cryptographic random-number generator does not make every aspect of password handling automatically secure.

- Generated passwords are printed to the terminal. They may remain visible in terminal scrollback, screenshots, copied command output, or logs.
- pwwd does not save or manage generated passwords.
- pwwd does not assess password strength or check passwords against known-breach databases.
- The current implementation does not guarantee inclusion of every character category.
- The application does not protect a password after it has been copied into another application.
- The project has not undergone an independent security audit.

Use a unique password for each service. Consider the security of the destination application and the method used to store the generated password. Do not treat pwwd as a replacement for a password manager.

## Development environment

The following versions describe the environment used to build and test the first public release. They are development-environment details, not all runtime requirements for end users.

| Component | Version / configuration |
|---|---|
| Operating system | Windows, x64 |
| Shell | PowerShell 7.6.10 |
| .NET SDK | 10.0.404 |
| .NET target framework | `net10.0` |
| Published runtime identifier | `win-x64` |
| Publish mode | Self-contained, single-file |
| WiX Toolset | 7.0.1 |
| Git for Windows | 2.56.0.windows.4 |
| GitHub CLI | 2.103.3 |
| Source hosting | GitHub |

The PowerShell script and module use APIs available in PowerShell 7 and .NET. Windows is the tested release environment; other operating systems have not yet been validated as release targets.

## Repository structure

```text
pwwd/
├── .gitignore
├── LICENSE
├── README.md
├── pwwd.ps1
├── pwwd.psm1
├── installer/
│   └── Package.wxs
└── src/
    └── pwwd-cli/
        ├── Program.cs
        └── pwwd-cli.csproj
```

Release build output is written to `dist/` and excluded from Git.

## Build from source

### Requirements

- Windows x64 for reproducing the published Windows packages
- PowerShell 7
- .NET 10 SDK
- Git
- WiX Toolset 7 for building the MSI

### Build the standalone executable

From the repository root:

```powershell
dotnet publish .\src\pwwd-cli\pwwd-cli.csproj `
    --configuration Release `
    --runtime win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:DebugType=none `
    -p:DebugSymbols=false `
    --output .\dist\win-x64
```

Create the shorter executable filename used in the release:

```powershell
Copy-Item .\dist\win-x64\pwwd-cli.exe .\dist\win-x64\pwwd.exe
```

Run a local smoke test:

```powershell
.\dist\win-x64\pwwd.exe --version
.\dist\win-x64\pwwd.exe --length 32
.\dist\win-x64\pwwd.exe --help
```

### Build the MSI installer

Install WiX Toolset 7 as a .NET global tool:

```powershell
dotnet tool install --global wix
wix --version
```

WiX Toolset 7 requires acceptance of its applicable EULA before use. Review the [WiX Open Source Maintenance Fee information](https://wixtoolset.org/osmf/) and the applicable terms. For an interactive per-user acceptance, the documented command is:

```powershell
wix eula accept wix7
```

Build the MSI from the repository root after publishing the executable:

```powershell
wix build .\installer\Package.wxs `
    -arch x64 `
    -b . `
    -o .\dist\pwwd-v0.1.0-win-x64.msi
```

### Create the portable ZIP

```powershell
Compress-Archive `
    -Path .\dist\win-x64\pwwd.exe, .\README.md, .\LICENSE `
    -DestinationPath .\dist\pwwd-v0.1.0-win-x64.zip
```

### Generate SHA-256 checksums

```powershell
$files = @(
    '.\dist\win-x64\pwwd.exe',
    '.\dist\pwwd-v0.1.0-win-x64.zip',
    '.\dist\pwwd-v0.1.0-win-x64.msi'
)

$lines = foreach ($file in $files) {
    $hash = Get-FileHash $file -Algorithm SHA256
    "$($hash.Hash)  $([System.IO.Path]::GetFileName($file))"
}

$lines | Set-Content '.\dist\SHA256SUMS.txt' -Encoding ascii
```

Recalculate and verify the hashes after building. Do not assume that a newly built artifact is identical to a previously published release.

## Testing performed for v0.1.0

The first release was manually tested on Windows x64.

- [x] PowerShell script generates default-length and custom-length passwords.
- [x] PowerShell module imports and exports `New-SecurePassword`.
- [x] Standalone executable prints its version and help text.
- [x] Standalone executable generates passwords with custom lengths.
- [x] Portable ZIP downloaded from GitHub, extracted, and executed.
- [x] MSI downloaded from GitHub and its SHA-256 hash matched the locally recorded build hash.
- [x] MSI installed successfully and the installed executable ran.
- [x] Invalid CLI inputs (zero, 1025, non-numeric input, unknown option, and extra arguments) returned a non-zero exit code.

These are manual functional checks, not a formal security audit or a comprehensive automated test suite.

## Roadmap

The following items are ideas for future development. They are not implemented in v0.1.0.

### Planned: graphical user interface

A native GUI is a planned direction for a future version. Possible controls and features include:

- Password length control
- Toggles for uppercase letters, lowercase letters, digits, and special characters
- Generate and copy buttons
- Clear indication of the selected character groups
- Optional password-strength estimation, with its method and limitations documented
- User-configurable character sets and exclusions
- Accessible keyboard navigation and a clean Windows interface

The GUI framework and exact design have not yet been selected. No GUI is included in the current release.

### Other possible improvements

- Guarantee at least one character from every selected category
- More precise CLI argument parsing and error messages
- Automated unit and integration tests
- CI builds and release automation
- Additional platform-specific builds after testing
- Code signing and documented release provenance

Roadmap items are proposals, not delivery commitments.

## Project history

- **17 November 2026:** Initial PowerShell password generator created; Git repository initialized and published.
- **18 November 2026:** PowerShell module and .NET 10 CLI added; Windows x64 executable, ZIP archive, and MSI installer built.
- **18 November 2026:** Public release `v0.1.0` published and download/install paths manually tested.

## Contributing and feedback

Bug reports, suggestions, and contributions are welcome. Please include the pwwd version, operating system, exact command used, and the error message. Never include real passwords, secrets, tokens, or other credentials in an issue.

## License

pwwd is licensed under the [MIT License](LICENSE).

Copyright (c) 2026 Merlin Erdogmus.
