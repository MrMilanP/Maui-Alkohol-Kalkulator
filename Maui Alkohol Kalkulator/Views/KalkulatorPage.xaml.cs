namespace Maui_Alkohol_Kalkulator.Views
{
    public partial class KalkulatorPage : ContentPage
    {
        // Korekcijske vrednosti za proračun korekcije jačine alkohola.
        private readonly double[] korekcijskeVrednosti =
        {
            0.169, 0.177, 0.192, 0.213, 0.207, 0.217, 0.227, 0.233, 0.243, 0.250,
            0.257, 0.260, 0.267, 0.273, 0.277, 0.283, 0.287, 0.293, 0.293, 0.297,
            0.300, 0.303, 0.303, 0.310, 0.310, 0.310, 0.317, 0.317, 0.320, 0.323,
            0.323, 0.327, 0.327, 0.327, 0.330, 0.330, 0.337, 0.337, 0.340, 0.343,
            0.343, 0.350, 0.350, 0.350, 0.357, 0.357, 0.360, 0.367, 0.367, 0.370,
            0.373, 0.373, 0.380, 0.380, 0.383, 0.387, 0.390, 0.390, 0.393, 0.397,
            0.397, 0.400, 0.400, 0.400, 0.397, 0.400, 0.400, 0.397, 0.397, 0.393,
            0.390, 0.383, 0.377, 0.373, 0.363, 0.353, 0.343, 0.333, 0.317, 0.307,
            0.287, 0.267, 0.247, 0.230, 0.210, 0.190, 0.173, 0.157, 0.140, 0.130,
            0.113, 0.103, 0.097, 0.087, 0.083, 0.077, 0.077, 0.073, 0.072, 0.048
        };

        public KalkulatorPage()
        {
            InitializeComponent(); // Inicijalizuje vizuelne komponente.

            spinnerReferentnaTemperatura.SelectedIndex = 0; // Postavlja podrazumevani indeks spinner-a.

        }

        // Ažurira vidljivost dugmeta za brisanje na osnovu unosa u odgovarajuće polje.
        private void UpdateClearButtonVisibility(Entry entry, ImageButton clearButton)
        {
            clearButton.IsVisible = !string.IsNullOrEmpty(entry.Text);
        }


        // Obradjuje klik na dugme za brisanje.
        private void OnClearBtnClicked(object sender, EventArgs e)
        {
            if (sender == clearBtnOcitanaVrednost)
            {
                etOcitanaVrednost.Text = string.Empty;
            }
            else if (sender == clearBtnTemperatura)
            {
                etTemperatura.Text = string.Empty;
            }

            // Dugme postaje nevidljivo nakon brisanja.
            if(sender is ImageButton button)
                button.IsVisible = false;
        }


        // Obradjuje klik na dugme za izračunavanje.
        private void OnCalculateClicked(object sender, EventArgs e)
        {
            IzracunajPravuJacinu(); // Poziva metodu za izračunavanje jačine.
        }

        // Izračunava pravu jačinu alkohola na osnovu unetih vrednosti.
        private void IzracunajPravuJacinu()
        {
            try
            {
                // Provera da li su unete vrednosti validni brojevi.
                if (int.TryParse(etOcitanaVrednost.Text, out int ocitanaVrednost) &&
                    int.TryParse(etTemperatura.Text, out int temperatura))
                {
                    // Validacija da li je temperatura u dozvoljenom opsegu.
                    if (temperatura < 1 || temperatura > 30)
                    {
                        tvRezultat.Text = "Temperatura mora biti između 1 i 30.";
                        return;
                    }

                    // Bezbedno parsiranje referentne temperature.
                    if (spinnerReferentnaTemperatura.SelectedItem?.ToString() is string selectedItem &&
                        int.TryParse(selectedItem, out int referentnaTemperatura))
                    {
                        double pravaJacina = IzracunajKorekciju(ocitanaVrednost, temperatura, referentnaTemperatura);

                        // Prikaz rezultata u procentima i gradima.
                        tvRezultat.Text = $"Prava jačina alkohola: {pravaJacina:F2}%";
                        tvRezultatGradi.Text = $"Prava jačina u gradima: {pravaJacina / 2.46:F2}";
                    }
                    else
                    {
                        tvRezultat.Text = "Izaberite validnu referentnu temperaturu.";
                    }
                }
                else
                {
                    tvRezultat.Text = "Unesite važeće brojeve.";
                }
            }
            catch (ArgumentException ex)
            {
                // Prikazuje poruku o grešci ako proračun baci izuzetak.
                tvRezultat.Text = ex.Message;
            }
        }

        // Proračun korekcije jačine alkohola na osnovu unetih parametara.
        private double IzracunajKorekciju(int ocitanaVrednost, int temperatura, int referentnaTemperatura)
        {
            // Provera da li je očitana vrednost u dozvoljenom opsegu.
            if (ocitanaVrednost < 1 || ocitanaVrednost > 100)
                throw new ArgumentException("Očitana vrednost mora biti između 1 i 100.");

            // Dohvata korekciju po stepenu iz niza.
            double korekcijaPoStepeni = korekcijskeVrednosti[100 - ocitanaVrednost];
            int razlikaUTemperaturi = temperatura - referentnaTemperatura;

            // Izračunavanje korekcije na osnovu razlike u temperaturi.
            double korekcija = razlikaUTemperaturi > 0
                ? ocitanaVrednost - razlikaUTemperaturi * korekcijaPoStepeni
                : ocitanaVrednost + Math.Abs(razlikaUTemperaturi) * korekcijaPoStepeni;

            // Ograničava rezultat na opseg od 0 do 100.
            return Math.Clamp(korekcija, 0, 100);
        }
    }
}
