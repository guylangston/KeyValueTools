using System.Linq;

namespace GL.KeyValueTools
{
    public class KeyValueReflection
    {
        public int SetValues<T>(IKeyValueBag bag, T obj)
        {
            var type = typeof (T);
            int misses = 0;
            var props = type.GetProperties();
            foreach (var pair in bag)
            {
                var p = props.FirstOrDefault(x => x.Name == pair.Key);
                if (p != null)
                {
                    p.SetValue(obj, pair.Value); 
                }
                else
                {
                    misses++;
                }
                
            }
            return misses;
        }
    }
}