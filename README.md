# SpookFlare


<p align="center"><img src="https://2.bp.blogspot.com/-C3sO1hv77RI/WuyTTuKUqaI/AAAAAAAALEE/DfOeVy73FiEn4o6NTJj3RP6fWbvjVbxtgCLcBGAs/s1600/SpookFlare_1.png" alt="SpookFlare" width="300" height="200"></p>

SpookFlare has a different perspective to bypass security measures and it gives you the opportunity to bypass the endpoint countermeasures at the client-side detection and network-side detection. SpookFlare is a loader/dropper generator for Meterpreter, Empire, Koadic etc. SpookFlare has obfuscation, encoding, run-time code compilation and character substitution features. So you can bypass the countermeasures of the target systems like a boss until they "learn" the technique and behavior of SpookFlare payloads.

* Obfuscation
* Encoding
* Run-time Code Compiling
* Character Substitution
* Patched Meterpreter Stage Support
* Blocked powershell.exe Bypass

```
     ___ ___  ___   ___  _  _____ _      _   ___ ___ 
    / __| _ \/ _ \ / _ \| |/ / __| |    /_\ | _ \ __|
    \__ \  _/ (_) | (_) | ' <| _|| |__ / _ \|   / _| 
    |___/_|  \___/ \___/|_|\_\_| |____/_/ \_\_|_\___|

            Version    : 2.0
            Author     : Halil Dalabasmaz
            WWW        : artofpwn.com, spookflare.com
            Twitter    : @hlldz
            Github     : @hlldz
            Licence    : Apache License 2.0
            Note       : Stay in shadows!

 [*] You can use "help" command for access help section.

SpookFlare > list

 ID | Payload                | Description                                                
----+------------------------+------------------------------------------------------------
 1  | meterpreter/binary     | .EXE Meterpreter Reverse HTTP and HTTPS loader             
 2  | meterpreter/powershell | PowerShell based Meterpreter Reverse HTTP and HTTPS loader 
 3  | javascript/hta         | .HTA loader with .HTML extension for specific command      
 4  | vba/macro              | Office Macro loader for specific command                   

```

## Installation
```
# git clone https://github.com/hlldz/SpookFlare.git
# cd SpookFlare
# pip install -r requirements.txt
```

## Technical Details
https://artofpwn.com/spookflare.html

## Usage Videos and Tutorials
* SpookFlare HTA Loader for Koadic: https://youtu.be/6OyZuyIbRLU
* SpookFlare PowerShell/VBA Loaders for Meterpreter: https://youtu.be/xFBRZz78U_M
* v1.0 Usage Video: https://www.youtube.com/watch?v=p_eKKVoEl0o

### Note
I developed the SpookFlare and technique for use in penetration tests, red team engagements and it is purely educational. Please use with responsibility and stay in shadows!

### Acknowledgements and References
Special thanks to the following projects and contributors.
* https://github.com/rapid7/metasploit-framework
* https://github.com/zerosum0x0/koadic
* https://github.com/EmpireProject/Empire
* https://github.com/Veil-Framework/Veil
* https://github.com/nccgroup/demiguise
