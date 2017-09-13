# SquidListManagerWCF
Domain List manager for squid for windows. Uses two isolated components.
For those who haven't been familiar with squid - it is a proxy server, you can get it from
[here.](http://wiki.squid-cache.org/SquidFaq/BinaryPackages)

1. Windows Service hosted WCF microservice,
2. Desktop client.


**_Squid configuration:_**
```text
acl allowed_https_access dstdomain "c:/squid/etc/trusted-sites.txt"
acl blocked_url_access dstdomain "c:/squid/etc/blacklist.txt"
http_access deny blocked_url_access
http_access deny CONNECT !allowed_https_access
```

For now c:\squid is hardcoded path for installed squid, but will be configurable soon.
I've just finished testing basic functionality, documentation will follow.

Best Regards,
Omkar.


**_Installation:_**
From administrative (possibly developer) command prompt,
1. Install the service
```text
installutil "SquidListManagerWCF/Squid Manager/SquidManager_Host/bin/Release/SquidManager_Host.exe"
```
2. Grant access to reserve URL
```text
netsh http add urlacl url=http://127.0.0.1:5656/ sddl="D:(A;;GX;;;LS)" listen=yes
```
3. Allow the program (to listen on 127.0.0.1:5656) if blocked for local access by a firewall

done!


**_Use:_**
1. Start SquidManager service from "View local services" (or Service Manager)
2. Launch SquidMonitor desktop application


**_Screenshots:_**
Squid Manager WCF service hosted inside windows service host
![WCF Microservice](/Screenshots/SquidManager_WCF_Service_InAction.png)

Squid Monitor Windows Forms simple desktop application
Simple dragging domain name to active/inactive section of a list reflects immediately on the text files and reconfigures squid to load these changes.

If you get access denied on browser, just ALT-TAB to Squid Monitor desktop application and it'll (on-focus) reload new-domains that were denied of access. Press "+" to trust the new domain. ALT-TAB back to your browser and you shall now be able to access the website.

Doing this trust/block is an intense process at first, but once set up with your commonly accessed websites, you may even forget Squid Monitor. Base domains list may be downloaded from [Domains List folder](/DomainLists).

1. Lazy loading with wait for service

![Squid Monitor](/Screenshots/SquidMonitor_WaitingToConnect.png)

2. Loads lists as soon as WCF service is up -- no need to relaunch the application

![Squid Monitor](/Screenshots/SquidMonitor_InAction.png)

3. Reviewing domain list

![Squid Monitor](/Screenshots/SquidMonitor_InAction2.png)
