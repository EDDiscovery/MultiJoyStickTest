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
}
