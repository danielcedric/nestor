

/// <summary>
/// Source : http://blog.soat.fr/2012/02/simplifier-lecriture-de-inotifypropertychanged-en-c/
/// </summary>
namespace Nestor.Tools.Domain.DataAnnotations
{
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //public class NotifyAttribute : Attribute
    //{
    //    /// <summary>
    //    /// Affecte ou obtient le nom de la propriété à notifier
    //    /// </summary>
    //    public string Property { get; set; }

    //    public NotifyAttribute()
    //    {

    //    }

    //    public NotifyAttribute(string propertyName)
    //    {
    //        this.Property = propertyName;
    //    }
    //}

    /// <summary>
    /// Classe qui gère automatiquement les notifications de changement de valeurs sur les propriétés marquées par l'attribut Notify
    /// </summary>
    //public class NotifyPropertyChanged : INotifyPropertyChanged
    //{
    //    #region Properties
    //    /// <summary>
    //    /// Affecte ou obtient le dictionnaire qui stocke les valeurs à surveiller
    //    /// </summary>
    //    private Dictionary<string, dynamic> Values { get; set; } = new Dictionary<string, dynamic>();
    //    /// <summary>
    //    /// Affecte ou obtient un dictionnaires de propriétés à surveiller
    //    /// </summary>
    //    private Dictionary<string, IEnumerable<string>> Properties { get; set; }
    //    /// <summary>
    //    /// Affecte ou obtient une valeur booléenne qui indique si l'initialisation a déjà eu lieu
    //    /// </summary>
    //    private bool Initialized { get; set; } = false;

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    #endregion

    //    #region Constructors

    //    #endregion

    //    #region Methods
    //    private void InitializeNotifyAttributes()
    //    {
    //        if (Initialized)
    //            return;

    //        var prop = from p in this.GetType().GetProperties()
    //                   where p.GetCustomAttributes(typeof(NotifyAttribute), true).Any()
    //                   select new
    //                   {
    //                       p.Name,
    //                       Attributes = p.GetCustomAttributes(typeof(NotifyAttribute), true)
    //                       .Cast<NotifyAttribute>()
    //                       .Select(a => string.IsNullOrEmpty(a.Property) ? p.Name : a.Property)
    //                   };

    //        Properties = new Dictionary<string, IEnumerable<string>>();
    //        prop.ToList().ForEach(p => p.Attributes.ToList());
    //        Initialized = true;
    //    }

    //    /// <summary>
    //    /// Affectation de la valeur
    //    /// </summary>
    //    /// <typeparam name="TObject"></typeparam>
    //    /// <param name="expression"></param>
    //    /// <param name="value"></param>
    //    public void SetValue<TObject>(Expression<Func<TObject>> expression, dynamic value)
    //    {
    //        InitializeNotifyAttributes();
    //        SetValue(value, GetPropertyName(expression));
    //    }

    //    public void SetValue(dynamic value, [CallerMemberName]string propertyName = null)
    //    {
    //        InitializeNotifyAttributes();
    //        if (!Values.ContainsKey(propertyName))
    //        {
    //            Values.Add(propertyName, value);
    //            NotifyPropertyChanged(propertyName);
    //        }
    //        else
    //        {
    //            if (Values[propertyName] != value)
    //            {
    //                Values[propertyName] = value;
    //                NotifyPropertyChanged(propertyName);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Obtient la valeur
    //    /// </summary>
    //    /// <typeparam name="T">Type de la donnée</typeparam>
    //    /// <param name="defaultValue">Valeur par défaut</param>
    //    /// <param name="propertyName">Nom de la propriété</param>
    //    /// <returns></returns>
    //    public T GetValue<T>(T defaultValue = default(T), [CallerMemberName] string propertyName = null)
    //    {
    //        InitializeNotifyAttributes();
    //        if (Values.ContainsKey(propertyName))
    //            return (T)Values[propertyName];

    //        return defaultValue;
    //    }

    //    public void NotifyPropertyChanged<T>(Expression<Func<T>> action)
    //    {
    //        NotifyPropertyChanged(GetPropertyName(action));
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="_strPropertyName"></param>
    //    public void NotifyPropertyChanged(string _strPropertyName)
    //    {
    //        var action = (Action<string>)((propertyName) =>
    //        {
    //            PropertyChangedEventHandler handler = PropertyChanged;
    //            if (handler != null)
    //            {
    //                handler(this, new PropertyChangedEventArgs(propertyName));
    //            }
    //        });

    //        if (DispatcherHelper.UIDispatcher.CheckAccess())
    //        {   // Fait dans le thread principal
    //            if (Properties.ContainsKey(_strPropertyName))
    //            {
    //                Properties[_strPropertyName].ForEach(propertyName =>
    //                {
    //                    DispatcherHelper.UIDispatcher.Invoke(DispatcherPriority.Render, (Action)(() => { action(propertyName); }));
    //                });
    //            }
    //            else
    //            {
    //                DispatcherHelper.UIDispatcher.Invoke(DispatcherPriority.Render, (Action)(() => { action(_strPropertyName); }));
    //            }
    //        }
    //        else
    //        {
    //            if (Properties.ContainsKey(_strPropertyName))
    //            {
    //                Properties[_strPropertyName].ForEach(propertyName =>
    //                {
    //                    DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() => { action(propertyName); }));
    //                });
    //            }
    //            else
    //            {
    //                DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() => { action(_strPropertyName); }));
    //            }
    //        }
    //    }

    //    private static string GetPropertyName<T>(Expression<Func<T>> _action)
    //    {
    //        var expression = (MemberExpression)_action.Body;
    //        var propertyName = expression.Member.Name;
    //        return propertyName;
    //    }

    //    private static PropertyInfo GetProperty<T>(Expression<Func<T>> _action)
    //    {
    //        return typeof(T).GetProperty(GetPropertyName(_action));
    //    }
    //    #endregion
    //}
}
