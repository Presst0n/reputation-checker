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

        public static string ToLevel(this int tier)
        {
            var output = "";

            switch (tier)
            {
                case 0:
                    output = "Hated";
                    break;
                case 1:
                    output = "Hostile";
                    break;
                case 2:
                    output = "Unfriendly";
                    break;
                case 3:
                    output = "Neutral";
                    break;
                case 4:
                    output = "Friendly";
                    break;
                case 5:
                    output = "Honored";
                    break;
                case 6:
                    output = "Revered";
                    break;
                case 7:
                    output = "Exalted";
                    break;
            }

            return output;
        }
    }
}
