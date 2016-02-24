using System.Web.UI.WebControls;

namespace GL.NameValueTools
{
    public class KeyValueReflection
    {
        public int SetValues<T>(IKeyValueBag bag, T obj)
        {
            var type = typeof (T);
            int misses = 0;
            foreach (var prop in type.GetProperties())
            {
                
            }
            return misses;
        }
    }
}