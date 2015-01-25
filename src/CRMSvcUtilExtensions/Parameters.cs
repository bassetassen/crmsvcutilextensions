using System;
using System.Linq;

namespace CRMSvcUtilExtensions
{
    public static class Parameters
    {
        public static string GetParameter(string key)
        {
            string[] args = Environment.GetCommandLineArgs();
            var arg = args.FirstOrDefault(a => a.ToLowerInvariant().StartsWith("/" + key.ToLowerInvariant()));
            if (!string.IsNullOrEmpty(arg))
            {
                var firstColonPosition = arg.IndexOf(':');
                var value = arg.Substring(firstColonPosition + 1).Trim(new[] { '"' });
                
                return value;
            }

            return null;
        }
    }
}