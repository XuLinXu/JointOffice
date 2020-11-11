using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace JointOffice.WorkFlow
{
    [Serializable]
    public class WF_Expression
    {
        public static bool ExecuteBoolenExpression(string pythonScript)
        {
            ScriptEngine engin = Python.CreateEngine();
            var sourcCode = engin.CreateScriptSourceFromString(pythonScript.Trim());
            var result = sourcCode.Execute<bool>();
            return result;
        }
    }
}
