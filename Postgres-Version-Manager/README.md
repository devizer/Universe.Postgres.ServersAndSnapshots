## PostgreSQL Version Manager for windows
The intended use of this project is for Continuous Integration (CI) scenarios, where:
- The PostgreSQL needs to be installed without user interaction and without admin rights.
- The PostgreSQL installation doesn't need to persist across multiple CI runs.

### Why
- Supports Windows 7 ... Windows Server 2025 or Windows Server 2008 R2+ ... Windows 11
- Does not need additional dependencies. Supports Powershell 2.0+ or any Powershell Core (pwsh)
- Optionally installs corresponding MS VC Rintime depending on version if not preinstalled.
- Two distributions: full and tiny. Tiny is 20x smaller download size
- Two Configuration options: As process or as Service. The first does not need additional priviedges
- Client side download failover using to CDNs: official and mirror on sourceforge.
- Reuses the force of aria2c multithreaded download speed.

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

### macos and linux
Already have PostgreSQL version managers.
- On linux: official repo
- Mac OS: both macports and brew supports multiple instances
