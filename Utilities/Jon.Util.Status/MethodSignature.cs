using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Jon.Util.Status
{
    public static class MethodSignature
    {
        public static string GetMethodSignature(MethodBase methodBase)
        {
            ParameterInfo[] parameterInfos = methodBase.GetParameters();
            StringBuilder sbArgs = new StringBuilder();
            for (int idx = 0; idx < parameterInfos.Length; idx++)
            {
                ParameterInfo pi = parameterInfos[idx];
                if (pi.IsOut)
                {
                    sbArgs.Append(" out ");
                }
                sbArgs.Append(pi.ParameterType.FullName + " " + pi.Name);
                if (idx + 1 < parameterInfos.Length)
                {
                    sbArgs.Append(", ");
                }
            }
            return(methodBase.Name + "(" + sbArgs.ToString() + ")");
        }
    }
}
