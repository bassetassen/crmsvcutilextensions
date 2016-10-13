CRMSvcUtilExtensions
====================

A library with extensions to CRMSvcUtil.

Copy crmsvcutilextensions.dll to same folder as CrmSvcUtil. Call CrmSvcUtil with /codefilter parameter

Available code filters:
* VanillaFilter - Filters out any custom entity, attribute and relationships
* SolutionFilter - Generates only entities that is incuded in the solution that is specified

VanillaFilter:
```
CrmSvcUtil /codewriterfilter:"CRMSvcUtilExtensions.VanillaFilter,CRMSvcUtilExtensions"
/url:... /out:D:\Entities.cs /username:... /password:... /namespace:...
```

SolutionFilter:
Solution argument is unique solution name or a comma-separated list of solutions
```
CrmSvcUtil /codewriterfilter:"CRMSvcUtilExtensions.SolutionFilter,CRMSvcUtilExtensions"
/solution:uniquename /url:... /out:D:\Entities.cs /username:... /password:... /namespace:...
```
