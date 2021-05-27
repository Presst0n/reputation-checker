using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RepChecker.Extensions
{
    public static class SimpleConverter
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return new ObservableCollection<T>(input);
        }
    }
}
