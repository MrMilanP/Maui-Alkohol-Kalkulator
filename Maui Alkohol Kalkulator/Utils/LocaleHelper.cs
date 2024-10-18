using System.Globalization;

namespace Maui_Alkohol_Kalkulator.Utils
{
    internal class LocaleHelper
    {
        // Proverava da li je trenutni jezik engleski
        public static bool IsLocaleEnglish()
        {
            CultureInfo currentCulture = CultureInfo.CurrentUICulture;
            return currentCulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);
        }

        // Postavlja podrazumevani jezik aplikacije
        public static void SetDefaultLanguage()
        {
            string defaultLang = "def"; // Zamenite sa odgovarajućim kodom jezika, npr. "en" za engleski

            CultureInfo defaultCulture = new CultureInfo(defaultLang);
            CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

            // Ako želite da promenite kulturu i za UI elemenata (npr. vreme, valuta), koristite i sledeće
            CultureInfo.CurrentCulture = defaultCulture;
            CultureInfo.CurrentUICulture = defaultCulture;
        }
    }
}
