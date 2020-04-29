using System.Collections.Generic;
using System.Configuration;

namespace Nestor.Tools.Infrastructure.FluentConfig
{
    public class FluentConfigurationAssemblyCollection : ConfigurationElementCollection, IList<FluentConfigurationAssemblyElement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FluentConfigurationAssemblyElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as FluentConfigurationAssemblyElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                    base.BaseRemoveAt(index);

                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FluentConfigurationAssemblyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FluentConfigurationAssemblyElement)element).Key;
        }

        #region IList<FluentConfigurationAssemblyElement> Membres

        public int IndexOf(FluentConfigurationAssemblyElement item)
        {
            return BaseIndexOf(item);
        }

        public void Insert(int index, FluentConfigurationAssemblyElement item)
        {
            BaseAdd(index, item);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        #endregion

        #region ICollection<FluentConfigurationAssemblyElement> Membres

        public void Add(FluentConfigurationAssemblyElement item)
        {
            BaseAdd(item);
        }

        public void Clear()
        {
            BaseClear();
        }

        public bool Contains(FluentConfigurationAssemblyElement item)
        {
            return !(IndexOf(item) < 0);
        }

        public void CopyTo(FluentConfigurationAssemblyElement[] array, int arrayIndex)
        {
            base.CopyTo(array, arrayIndex);
        }

        public new bool IsReadOnly
        {
            get { return IsReadOnly; }
        }

        public bool Remove(FluentConfigurationAssemblyElement item)
        {
            BaseRemove(GetElementKey(item));
            return true;
        }

        #endregion

        #region IEnumerable<FluentConfigurationAssemblyElement> Membres

        public new IEnumerator<FluentConfigurationAssemblyElement> GetEnumerator()
        {
            for (int i = 0; i < base.Count; i++)
            {
                yield return base.BaseGet(i) as FluentConfigurationAssemblyElement;
            }

            yield break;
        }

        #endregion
    }
}
