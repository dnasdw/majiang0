using System.Collections.Generic;

using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public class mapp<TKey, TValue> : SortedDictionary<TKey, TValue>, IContainer<KeyValuePair<TKey, TValue>>
        {
            public new TValue this[TKey a_Key]
            {
                get
                {
                    TValue value = default(TValue);
                    if (base.TryGetValue(a_Key, out value))
                    {
                        return value;
                    }
                    value = CDefault.Instance.GetDefaultValue<TValue>();
                    base[a_Key] = value;
                    return value;
                }
                set
                {
                    base[a_Key] = value;
                }
            }

            public bool empty()
            {
                return base.Count == 0;
            }

            public int size()
            {
                return base.Count;
            }

            public pairp<object, bool> insert(pairp<TKey, TValue> a_pValue)
            {
                pairp<object, bool> pResult = new pairp<object, bool>(null, this.TryAdd(a_pValue.first, a_pValue.second));
                return pResult;
            }
        }
    }
}
