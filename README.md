# SquidListManagerWCF
Domain List manager for squid for windows. Uses two isolated components.

1. Windows Service hosted WCF microservice,
2. Desktop client.

Squid configuration:
acl allowed_https_access dstdomain "c:/squid/etc/trusted-sites.txt"
acl blocked_url_access dstdomain "c:/squid/etc/blacklist.txt"
http_access deny blocked_url_access
http_access deny CONNECT !allowed_https_access


For now c:\squid is hardcoded path for installed squid, but will be configurable soon.
I've just finished testing basic functionality, documentation will follow.

Best Regards,
Omkar.
