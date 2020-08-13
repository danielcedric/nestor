using System;

namespace Nestor.Tools.Helpers
{
    public static class UniqueIdHelper
    {
        public static string GenerateUniqueString()
        {
            long i = 1;
            foreach (var b in Guid.NewGuid().ToByteArray()) i *= b + 1;
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public static string GenerateUniqueId()
        {
            long i = 1;
            foreach (var b in Guid.NewGuid().ToByteArray()) i *= b + 1;
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
    }
}