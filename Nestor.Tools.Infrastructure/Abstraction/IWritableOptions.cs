using Microsoft.Extensions.Options;
using System;

namespace Nestor.Tools.Infrastructure.Abstraction
{
    public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }

}
