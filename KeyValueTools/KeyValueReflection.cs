using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GL.KeyValueTools
{
    public class KeyValueReflection
    {
        public int SetValues<T>(IKeyValueBag bag, T obj) => SetValues(bag, obj, DefaultAdapter);
        public int SetValues<T>(IKeyValueBag bag, T obj, Func<PropertyInfo, KeyValuePair<string, object>, object> setValueAdapter)
        {
            var type = typeof (T);
            int misses = 0;
            var props = type.GetProperties();
            foreach (var pair in bag)
            {
                var p = props.FirstOrDefault(x => x.Name == pair.Key);
                if (p != null)
                {
                    if (setValueAdapter == null)
                    {
                        // Just ram it in there
                        p.SetValue(obj, pair.Value);
                    }
                    else
                    {
                        var adapt = setValueAdapter(p, pair);
                        if (adapt == Skip)
                        {
                            misses++;
                        }
                        else
                        {
                            p.SetValue(obj, adapt);
                        }
                    }
                }
                else
                {
                    misses++;
                }
                
            }
            return misses;
        }

        protected static readonly object Skip = new object();

        public static object DefaultAdapter(PropertyInfo prop, KeyValuePair<string, object> pair)
        {
            if (pair.Value == null) return null;
            if (prop.PropertyType == typeof (int))
            {
                if (!(pair.Value is int))
                {
                    int ii;
                    if (int.TryParse(pair.Value.ToString(), out ii))
                    {
                        return ii;
                    }
                    return Skip;
                }
                return pair.Value;
            }
            else if (prop.PropertyType == typeof (DateTime))
            {
                if (!(pair.Value is DateTime))
                {
                    DateTime ii;
                    if (DateTime.TryParse(pair.Value.ToString(), out ii))
                    {
                        return ii;
                    }
                    return Skip;
                }
                return pair.Value;
            }
            else if (prop.PropertyType == typeof(bool))
            {
                if (!(pair.Value is Boolean))
                {
                    Boolean ii;
                    if (Boolean.TryParse(pair.Value.ToString(), out ii))
                    {
                        return ii;
                    }
                    return Skip;
                }
                return pair.Value;
            }
            else if (prop.PropertyType == typeof(string))
            {
                return pair.Value.ToString();
            }
            return pair.Value;
        }
    }
}