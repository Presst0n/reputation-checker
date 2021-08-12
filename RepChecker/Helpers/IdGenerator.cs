using System;

namespace RepChecker.Helpers
{
    public static class IdGenerator
    {
        public static int GenerateId()
        {
            var now = DateTime.Now;
            var zeroDate = DateTime.MinValue.AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second).AddMilliseconds(now.Millisecond);
            int uniqueId = (int)(zeroDate.Ticks / 10000);

            return uniqueId;
        }
    }
}
