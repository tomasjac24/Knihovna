using Spectre.Console;

class ProjektKveten
{

    private class Kniha 
    {

        public Kniha(string nazev, string autor, int year) {
            Nazev = nazev;
            Autor = autor;
            Rok = year;
        }

        public string Nazev { get; set; }
        public string Autor { get; set; }
        public int Rok { get; set; }
    }

    private static class Program 
    {

        public static List<string> moznost = new() { "Vytvoření nové knihy", "Výpis všech knih", "Vyhledání knihy podle autora", "Vyhledání knihy podle roku vydání", "[red]KONEC[/]" }; //Vytvoří list který vypíše možnosti co chce uživatel dělat
        public static List<string> praceSKnihou = new() { "Přidat knihu", "Zpět" }; //Vytvoří list kde budou možnosti jestli knihu přidat a nebo jít zpět
        
        public static void VytvorKnihu() {
            
            while (true) {
                
                Console.Clear();

                var vyberKnihy = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Přidej Knihu")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Vyber šipkami)[/]")
                        .AddChoices(praceSKnihou)
                );


                if (vyberKnihy == "Zpět") break;//Volba zpět

                if (vyberKnihy == "Přidat knihu") { //Volba Přidat Knihu
                    Console.WriteLine("Zadej informace:");
                    Console.WriteLine("----------------------");
                    var nazev = AnsiConsole.Ask<string>("Název: "); //Zeptá se na název a uživatel ho zadá
                    var autor = AnsiConsole.Ask<string>("Autor: "); //Zeptá se na jméno autora a uživatel ho zadá
                    var rok = AnsiConsole.Ask<int>("Rok: "); //Zeptá se na rok a uživatel ho zadá
                    File.AppendAllText("knihovna.txt", $"{nazev},{autor},{rok}\n"); //Zapíšou se informace do textového souboru
                }
            }
        }
        public static void VypisVsechnyKnihy() {
            Console.Clear();

            string[] řádky = File.ReadAllLines("knihovna.txt");
            var vsechnyKnihy = new List<Kniha>();

            foreach(var řádek in řádky) {
                string[] data = řádek.Split(',');
                string jmeno = data[0];
                string autor = data[1];
                int rok = int.Parse(data[2]);

                vsechnyKnihy.Add(new Kniha(jmeno, autor, rok));
            }

            Console.WriteLine("List všech knih:\n");
            foreach (var kniha in vsechnyKnihy) {
                Console.WriteLine();
                Console.WriteLine($"Název: {kniha.Nazev}");
                Console.WriteLine($"Autor: {kniha.Autor}");
                Console.WriteLine($"Rok: {kniha.Rok}");
                Console.WriteLine();
            }

            Console.ReadKey();
        }
        public static void VyhledejKnihyPodle(string typ) {

            Console.Clear();
            bool hledaSePodleRoku = false; // defaultně je nastaveno, že se nehledá podle roku
            if (typ == "roku") hledaSePodleRoku = true;
            
            var vsechnyKnihy = new List<Kniha>(); // vytvoří se list se všehmi knihami
            string[] řádky = File.ReadAllLines("knihovna.txt"); // přečtou se všechny řádky
            foreach (var řádek in řádky) 
            {
                string[] informace = řádek.Split(',');
                vsechnyKnihy.Add(new Kniha(informace[0], informace[1], int.Parse(informace[2]))); // do listu s knihama se přidá kniha
            }

            string input = "";
            int inputToNumber = 0;
            if (hledaSePodleRoku == false) input = AnsiConsole.Ask<string>("Zadejte jméno autora knihy: "); //Zeptá se na jméno autora kterého chce najít a uživatel ho zadá
            if (hledaSePodleRoku == true) inputToNumber = AnsiConsole.Ask<int>("Zadejte rok vydání knihy: "); //Zeptá se na rok vydání knihy

            var nalezeneKnihy = new List<Kniha>(); // vytvoří se list, kam se dají nalezené knihy
            Console.Clear();


            if(hledaSePodleRoku == true) nalezeneKnihy = vsechnyKnihy.FindAll(kniha => kniha.Rok == inputToNumber);
            if(hledaSePodleRoku == false) nalezeneKnihy = vsechnyKnihy.FindAll(kniha => kniha.Autor == input);


            // pokud se něco našlo
            if(nalezeneKnihy.Count > 0) 
            {
                Console.Clear();
                foreach (var kniha in nalezeneKnihy) {
                    Console.WriteLine();
                    Console.WriteLine($"Název: {kniha.Nazev}");
                    Console.WriteLine($"Autor: {kniha.Autor}");
                    Console.WriteLine($"Rok: {kniha.Rok}");
                    Console.WriteLine();
                }

                Console.ReadKey();
            } else {
                Console.Clear();
                AnsiConsole.MarkupLine("[red]Nebyla nalezena žádná kniha![/]");
                Console.ReadKey();
            }
        }
    }

    public static void Main()
    {
        while (true)
        {
            Console.Clear();

            //Vypíše možnosti
            var vyber = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Vyber možnost")
                .PageSize(10)
                .AddChoices(Program.moznost)
            );


            //Vytvoření nové knihy
            if (vyber == "Vytvoření nové knihy") Program.VytvorKnihu();
            //Výpis všech knih
            if (vyber == "Výpis všech knih") Program.VypisVsechnyKnihy();
            //Vyhledání knihy podle roku
            if (vyber == "Vyhledání knihy podle roku vydání") Program.VyhledejKnihyPodle("roku");
            //Vyhledání knihy podle autora
            if (vyber == "Vyhledání knihy podle autora") Program.VyhledejKnihyPodle("autora");
            if (vyber == "[red]KONEC[/]") break;
        }
    }
}