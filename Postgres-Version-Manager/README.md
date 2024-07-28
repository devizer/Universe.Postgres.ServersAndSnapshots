## PostgreSQL Version Manager for windows
The intended use of this project is for Continuous Integration (CI) scenarios, where:
- The PostgreSQL needs to be installed without user interaction and without admin rights.
- The PostgreSQL installation doesn't need to persist across multiple CI runs.

### Why
- Supports Windows 7 ... Windows 11, and Windows Server 2008 R2+ ... Windows Server 2025. Windows on ARM64 is also supported.
- Does not need additional dependencies. Supports Powershell 2.0+ or any Powershell Core (pwsh)
- Optionally installs corresponding MS VC Rintime depending on version if not preinstalled.
- Two distributions: full and tiny. Tiny is 20x smaller download size
- Two Configuration options: as user process, or as windows ervice. The first does not need additional priviedges
- Client side download failover using to CDNs: official and mirror on sourceforge.
- Reuses the force of aria2c multithreaded download speed. For very old windows without TLS 1.2 support aria2c is required.

### Full Options List:
```powershell
[string] $Command = "Install", # Install
[string] $Mode = "Process", # Process|Service
[string] $Version = "16.3-x64",
[string] $Admin = "postgres",
[string] $Password = "Meaga`$str0ng",
[string] $Locale = "en-US",
[string] $OnlyLocalhost = "False",
[int]    $Port = 5432,
[string] $DownloadType = "tiny", # Tiny|Full
[string] $ServiceId = "", # Empty means do not install windows service
[string] $BinFolder = "",
[string] $DataFolder = "",
[string] $LogFolder = "",
[string] $VcRedistMode = "Auto" # Audo|Skip|Force
```

### List of tested PostgresSQL Versions:
 - 16.3, 16.0,
 - 15.7, 15.4, 15.1,
 - 14.12, 14.9, 14.6,
 - 13.15, 13.12, 13.9,
 - 12.19, 12.16, 12.13,
 - 11.21, 11.18,
 - 10.23, 10.23 x86,
 - 9.6.24, 9.6.24 x86

### macos and linux
Already have PostgreSQL version managers.
- On linux: official repo
- Mac OS: both macports and brew support multiple instances
