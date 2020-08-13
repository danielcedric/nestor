using System.Collections.Generic;
using System.Configuration;

namespace Nestor.Tools.Infrastructure.FluentNHibernate.Config
{
    [ConfigurationCollection(typeof(FluentConfigurationDatabaseDispatcherElement))]
    public class FluentConfigurationDatabaseDispatcherCollection : ConfigurationElementCollection,
        IList<FluentConfigurationDatabaseDispatcherElement>
    {
        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FluentConfigurationDatabaseDispatcherElement this[int index]
        {
            get => BaseGet(index) as FluentConfigurationDatabaseDispatcherElement;
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        #region IEnumerable<FluentConfigurationDatabaseDispatcherElement> Membres

        public new IEnumerator<FluentConfigurationDatabaseDispatcherElement> GetEnumerator()
        {
            for (var i = 0; i < Count; i++) yield return BaseGet(i) as FluentConfigurationDatabaseDispatcherElement;
        }

        #endregion

        protected override ConfigurationElement CreateNewElement()
        {
            return new FluentConfigurationDatabaseDispatcherElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FluentConfigurationDatabaseDispatcherElement) element).ConnectionStringName;
        }

        #region IList<FluentConfigurationDatabaseDispatcherElement> Membres

        public int IndexOf(FluentConfigurationDatabaseDispatcherElement item)
        {
            return BaseIndexOf(item);
        }

        public void Insert(int index, FluentConfigurationDatabaseDispatcherElement item)
        {
            BaseAdd(index, item);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        #endregion

        #region ICollection<FluentConfigurationDatabaseDispatcherElement> Membres

        public void Add(FluentConfigurationDatabaseDispatcherElement item)
        {
            BaseAdd(item);
        }

        public void Clear()
        {
            BaseClear();
        }

        public bool Contains(FluentConfigurationDatabaseDispatcherElement item)
        {
            return !(IndexOf(item) < 0);
        }

        public void CopyTo(FluentConfigurationDatabaseDispatcherElement[] array, int arrayIndex)
        {
            base.CopyTo(array, arrayIndex);
        }

        public new bool IsReadOnly => IsReadOnly;

        public bool Remove(FluentConfigurationDatabaseDispatcherElement item)
        {
            BaseRemove(GetElementKey(item));
            return true;
        }

        #endregion
    }
}