using Spectre.Console;
using System.ComponentModel.Design;
using System.Net;
using System.Reflection;
using System.Threading.Channels;

class ProjektKveten
{
    public static void Main()
    {
        try
        {
            var moznost = new List<string>(); //Vytvoří list který vypíše možnosti co chce uživatel dělat
            var praceSKnihou = new List<string>(); //Vytvoří list kde budou možnosti jestli knihu přidat a nebo jít zpět
            //Přidá: Přidat a zpět
            praceSKnihou.Add("Přidat knihu");
            praceSKnihou.Add("Zpět");
            //Přidá možnosti co chceme dělat s knihovnou
            moznost.Add("Vytvoření nové knihy");
            moznost.Add("Výpis všech knih");
            moznost.Add("Vyhledání knihy podle autora");
            moznost.Add("Vyhledání knihy podle roku vydání");
            bool opakovani = true;
            while (opakovani)
            {
            Zpet:
                bool pridatKnihu = true;
                Console.Clear();
                //Vypíše možnosti
                Console.WriteLine("Vyber možnost");
                var vyber = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices(moznost));
                //Vytvoření nové knihy
                if (vyber == "Vytvoření nové knihy")
                {
                    bool opakovaniZadani = true; //Nastaveno opakování praceSKnihou
                    while (opakovaniZadani)
                    {
                        Console.Clear();
                        var vyberKnihy = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Přidej Knihu")
                                .PageSize(10)
                                .MoreChoicesText("[grey](Vyber šipkami)[/]")
                                .AddChoices(praceSKnihou));
                        if (vyberKnihy == "Zpět") //Volba zpět
                        {
                            opakovaniZadani = false; //Vypne opakování
                            goto Zpet; //Přesune se na výpis možností
                        }
                        if (vyberKnihy == "Přidat knihu") //Volba Přidat Knihu
                        {
                            Console.WriteLine("Zadej informace:");
                            Console.WriteLine("----------------------");
                            var nazev = AnsiConsole.Ask<string>("Název: "); //Zeptá se na název a uživatel ho zadá
                            var autor = AnsiConsole.Ask<string>("Autor: "); //Zeptá se na jméno autora a uživatel ho zadá
                            var rok = AnsiConsole.Ask<string>("Rok: "); //Zeptá se na rok a uživatel ho zadá
                            File.WriteAllText(nazev + ".txt", "Název: " + nazev + ", Autor: " + autor + ", Rok: " + rok); //Zapíšou se informace do textového souboru

                        }
                    }
                }
                //Výpis všech knih
                if (vyber == "Výpis všech knih")
                {
                    praceSKnihou.Remove("Zpět"); //Odebere možnost Zpět aby se nepočítala v listu jako kniha a nevypsalo se že kniha není zadaná
                    Console.Clear();
                    //Pro každou knihu v listu praceSKnihou
                    foreach (var nazev in praceSKnihou)
                    {
                        try
                        {
                            string obsahSouboru = File.ReadAllText(nazev + ".txt"); //Do proměnné obsahSouboru se napíšou informace z textového souboru který je uložen v počítači
                            Console.WriteLine(obsahSouboru); //Vypíše obsah souboru
                        }
                        catch //Kdyby v souboru nebyli zapsané informace, vypíše -KNIHA NEZADÁNA-
                        {
                            Console.WriteLine("");
                            Console.WriteLine("-KNIHA NEZADÁNA-");
                        }
                    }
                    Console.ReadLine();
                    praceSKnihou.Add("Zpět"); //Vrátí zpět možnost zpět
                }
                //Vyhledání knihy podle autora
                if (vyber == "Vyhledání knihy podle autora")
                {
                    Console.Clear();
                    var hledanyAutor = AnsiConsole.Ask<string>("Zadejte jméno autora knihy: "); //Zeptá se na jméno autora kterého chce najít a uživatel ho zadá
                    Console.Clear();
                    Console.WriteLine("Knihy od autora " + hledanyAutor + ":"); //Vypíše knihy od autora

                    bool nalezenaKniha = false; //Vytvoří se proměnná které určuje jestli byla alespoň 1 kniha nalezena (false = nebyla nalezena)
                    //Pro každou knihu v listu praceSKnihou
                    foreach (var vyberKnihy in praceSKnihou)
                    {
                        try
                        {
                            string obsahSouboru = File.ReadAllText(vyberKnihy + ".txt"); //Do proměnné obsahSouboru se vypíšou informace z textového souboru který je uložen v počítači se zadaným autorem
                            if (obsahSouboru.Contains(hledanyAutor)) //Je-li v informacích zapsáno jméno autora pokračuje se do if
                            {
                                nalezenaKniha = true; //Kniha byla nalezena
                                Console.WriteLine(obsahSouboru); //Vypíše obsah souboru
                            }
                        }
                        catch
                        {
                            //Když v obsahu souboru není zapsán autor, nevypíše se nic
                        }
                    }

                    if (!nalezenaKniha) //Nebyla nalezena žádná kniha
                    {
                        Console.Clear();
                        Console.WriteLine("Od autora " + hledanyAutor + " nebyla nalezena žádná kniha."); //Vypíše: Od autora nebyla nalezena žádná kniha.
                    }

                    Console.ReadLine();
                }
                //Vyhledání knihy podle roku
                if (vyber == "Vyhledání knihy podle roku vydání")
                {
                    Console.Clear();
                    var hledanyRok = AnsiConsole.Ask<string>("Zadejte rok vydání knihy: "); //Zeptá se na rok z kterého chce knihu najít a uživatel ho zadá
                    Console.Clear();
                    Console.WriteLine("Knihy z roku " + hledanyRok + ":"); //Vypíše knihy z daného roku

                    bool nalezenaKniha = false; //Vytvoří se proměnná které určuje jestli byla alespoň 1 kniha nalezena (false = nebyla nalezena)
                    //Pro každou knihu v listu praceSKnihou
                    foreach (var rokKnihy in praceSKnihou)
                    {
                        try
                        {
                            string obsahSouboru = File.ReadAllText(rokKnihy + ".txt"); //Do proměnné obsahSouboru se vypíšou informace z textového souboru který je uložen v počítači se zadaným rokem
                            if (obsahSouboru.Contains(hledanyRok)) //Je-li v informacích zapsán rok pokračuje se do if
                            {
                                nalezenaKniha = true; //Kniha byla nalezena
                                Console.WriteLine(obsahSouboru); //Vypíše obsah souboru
                            }
                        }
                        catch
                        {
                            //Když v obsahu souboru není zapsán rok, nevypíše se nic
                        }
                    }

                    if (!nalezenaKniha) //Nebyla nalezena žádná kniha
                    {
                        Console.Clear();
                        Console.WriteLine("Z roku " + hledanyRok + " nebyla nalezena žádná kniha."); //Vypíše: Z roku nebyla nalezena žádná kniha.
                    }

                    Console.ReadLine();
                }
            }
        }
        catch
        {
            Console.WriteLine("Něco se pokazilo!");
        }
    }
}