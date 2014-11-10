CRMSvcUtilExtensions
====================

An extension to only generate standard entities and attributes.

Copy crmsvcutilextensions.dll to same folder as CrmSvcUtil. Call CrmSvcUtil with /codefilter parameter

```
CrmSvcUtil /codewriterfilter:"CRMSvcUtilExtensions.VanillaFilter,CRMSvcUtilExtensions" /url:... /out:D:\Entities.cs /username:... /password:... /namespace:...
```