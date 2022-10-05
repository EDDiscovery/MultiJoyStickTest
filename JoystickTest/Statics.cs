using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Statics
{
    public static string RemoveNuls( this string s)
    {
        int index = s.IndexOf('\0');
        if (index > 0)
            s = s.Substring(0, index);
        return s;
    }

    public static string ToStringInvariant(this int v)
    {
        return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    static public string GetVersionString(this System.Reflection.Assembly aw)
    {
        System.Reflection.AssemblyName an = new System.Reflection.AssemblyName(aw.FullName);
        return an.Version.Major.ToStringInvariant() + "." + an.Version.Minor.ToStringInvariant() + "." + an.Version.Build.ToStringInvariant() + "." + an.Version.Revision.ToStringInvariant();
    }
}
