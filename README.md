# SquidListManagerWCF
Domain List manager for squid for windows. Uses two isolated components.
(For those who haven't been familiar with squid - it is a proxy server, you can get it from ![Download Squid](http://wiki.squid-cache.org/SquidFaq/BinaryPackages) here.)
1. Windows Service hosted WCF microservice,
2. Desktop client.

Squid configuration:
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

Screenshots:
Squid Manager WCF service hosted inside windows service host
![WCF Microservice](/Screenshots/SquidManager_WCF_Service_InAction.png)

Squid Monitor Windows Forms simple desktop application
Simple dragging domain name to active/inactive section of a list reflects immediately on the text files and reconfigures squid to load these changes.
![Squid Monitor](/Screenshots/SquidMonitor_InAction.png)
