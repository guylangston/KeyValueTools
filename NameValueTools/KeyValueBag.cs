using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace GL.KeyValueTools
{
    /// <summary>
    /// Does not allow many-keys
    /// Does not allow null keys
    /// Allows nulls as values
    /// </summary>
    public interface IKeyValueBag : IEnumerable<KeyValuePair<string, object>>
    {
        object this[string name] { get; set; }
        bool ContainsKey(string name);

        string ToString(string fmt);
    }

    
    /// <summary>
    /// Does not allow many-keys
    /// Does not allow null keys
    /// Allows nulls as values
    /// <see cref="Check"/>
    /// </summary>
    public class KeyValueBag : IKeyValueBag
    {
        // This is a thoughtful choice as we expect bags to have relatively few members and don't want the overhead
        private readonly List<KeyValuePair<string, object>> inner; 

        public KeyValueBag()
        {
            inner = new List<KeyValuePair<string, object>>();
        }

        public KeyValueBag(IEnumerable<KeyValuePair<string, object>> items)
        {
            inner = new List<KeyValuePair<string, object>>(items);
            Check();
        }

        

        public KeyValueBag(IEnumerable<Tuple<string, object>> items)
        {
            inner = new List<KeyValuePair<string, object>>(items.Select( x=>new KeyValuePair<string, object>(x.Item1, x.Item2)));
            Check();
        }

        public KeyValueBag(IReadOnlyDictionary<string, object> dict)
        {
            inner = new List<KeyValuePair<string, object>>(dict);
            Check();
        }

        /// <summary>
        /// This is a strange class internally
        /// </summary>
        /// <param name="nameValues"></param>
        public KeyValueBag(NameValueCollection nameValues)
        {
            inner = new List<KeyValuePair<string, object>>(nameValues.AllKeys.Select(x=>new KeyValuePair<string, object>(x, nameValues[x])));
            Check();
        }

        /// <summary>
        /// Does not allow many-keys
        /// Does not allow null keys
        /// Allows nulls as values
        /// </summary>
        private void Check()
        {
            // TODO: 
        }

        public object this[string name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return inner.FirstOrDefault(x=>string.Equals(name, x.Key, StringComparison.InvariantCultureIgnoreCase)).Value;
            }
            set
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                inner.RemoveAll(x=> string.Equals(name, x.Key, StringComparison.InvariantCultureIgnoreCase));
                inner.Add(new KeyValuePair<string, object>(name, value));
            }
        }

        public bool ContainsKey(string name) => inner.Any(x => string.Equals(name, x.Key, StringComparison.InvariantCultureIgnoreCase));
      

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => inner.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var sb = new StringBuilder();
            bool first = true;
            foreach (var pair  in inner)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(",");
                }
                sb.Append(pair.ToString());
            }

            return sb.ToString();
        }

        public string ToString(string fmt)
        {
            throw new NotImplementedException();
        }
    }
}
