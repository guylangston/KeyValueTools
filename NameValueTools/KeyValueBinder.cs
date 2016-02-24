using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace GL.NameValueTools
{
    public static class KeyValueBinder
    {
        public static IKeyValueBag FromCommandLine(string[] args)
        {
            var bag = new KeyValueBag();
            foreach (var arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    var idx = arg.IndexOf(':');
                    if (idx > 0)
                    {
                        bag[arg.Substring(1, idx - 1)] = arg.Substring(idx + 1, arg.Length - idx - 1);
                    }
                    else
                    {
                        bag[arg.Remove(0, 1)] = null;
                    }
                }
            
            }
            return bag;
        }

        public static IKeyValueBag FromURL(string urlQueryString)
        {
            return new KeyValueBag(HttpUtility.ParseQueryString(urlQueryString));
        }

        public static IKeyValueBag FromJSON(string jsonString)
        {
            var jobj = JObject.Parse(jsonString);
            var vals = jobj.Descendants()
                .Where(x => x is JProperty)
                .Cast<JProperty>()
                .Select(x => new KeyValuePair<string, object>(x.Name, x.Value.ToString()));
            return new KeyValueBag(vals);
            
        }

        public static IKeyValueBag FromCSV(string csvString)
        {
            throw new NotImplementedException();
        }

        public static IKeyValueBag FromObject(object obj)
        {
            var type = obj.GetType();
            var bag = new KeyValueBag();
            foreach (var prop in type.GetProperties())
            {
                var value = ReduceNullable(prop.GetValue(obj, null));
                if (value != null)
                {
                    if (IsComplex(value))
                    {
                        bag[prop.Name] = value.ToString();
                    }
                    else
                    {
                        bag[prop.Name] = value;
                    }
                }
            }
            return bag;
        }

        public static bool IsComplex(object obj)
        {
            obj = ReduceNullable(obj);
            if (obj is string) return false;
            if (obj is bool) return false;
            if (obj is int) return false;
            if (obj is DateTime) return false;
            if (obj is decimal) return false;
            if (obj is double) return false;
            return true;
        }

        private static object ReduceNullable(object obj)
        {
            if (obj == null) return null;
            if (obj.GetType().IsValueType) return obj;
            if (obj is string) return obj;
            if (obj is int?) return (int) obj;
            if (obj is DateTime?) return (DateTime)obj;
            if (obj is bool?) return (bool)obj;
            if (obj is decimal?) return (decimal)obj;
            if (obj is double?) return (double)obj;
            
            return obj;
        }
    }
}