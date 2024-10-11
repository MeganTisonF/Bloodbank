namespace BloodBanken;

using System;
using System.Collections.Generic;

class Program
{
    // Skapar en instans av klassen BloodBank, används för att hantera blodgivare och donationer.
    static BloodBank bloodBank = new BloodBank(); 

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Välkommen till blodgivningscentralen!");
            Console.WriteLine("Är du en blodgivare eller personal? eller skriv 'avsluta' för att avsluta");
            string userType = Console.ReadLine().ToLower();

            if (userType == "blodgivare")
            { 
                //Metod
                HandleBloodDonor();
            }
            else if (userType == "personal")
            {
                //Metod
                HandleStaff();
            }
            else if (userType == "avsluta")
            {
                Console.WriteLine("Tack för att du använde programmet. Hejdå!");
                break;
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
            }
        }
    }

    static void HandleBloodDonor()
    {
        while (true)
        {
            Console.WriteLine("Ange ditt namn:");
            string name = Console.ReadLine();
            Console.WriteLine("Ange din e-post:");
            string email = Console.ReadLine();
            Console.WriteLine("Ange din blodgrupp (t.ex. A+, O-):");
            string bloodType = Console.ReadLine();
            
            Console.WriteLine("Har du några medicinska tillstånd? (ja/nej)");
            // Ternär operator
            string healthHistory = Console.ReadLine().ToLower() == "ja" ? "Ja" : "Nej";

            BloodDonor donor = new BloodDonor(name, email, bloodType, healthHistory);
            bloodBank.AddBloodDonor(donor);

            Console.WriteLine("Vill du anmäla dig för att donera blod? (ja/nej)");
            string wantsToDonate = Console.ReadLine().ToLower();
            if (wantsToDonate == "ja")
            {
                donor.RequestToDonate();
                bloodBank.RegisterDonation(donor); // Direkt registrering av donationen
            }
            else
            {
                Console.WriteLine("Tack för din registrering!");
            }

            Console.WriteLine("Vill du gå tillbaka till huvudmenyn eller avsluta? (ange 'meny' för huvudmenyn eller 'avsluta' för att avsluta)");
            string choice = Console.ReadLine().ToLower();
            if (choice == "avsluta")
            {
                Console.WriteLine("Tack för att du använde programmet. Hejdå!");
                Environment.Exit(0); // Avslutar programmet
            }
            else if (choice == "meny")
            {
                break;
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Återgår till huvudmenyn.");
                break;
            }
        }
    }

    static void HandleStaff()
    {
        while (true)
        {
            Console.WriteLine("Välkommen personal!");
            Console.WriteLine("1. Visa blodlager");
            Console.WriteLine("2. Visa alla blodgivare som är villiga att donera");
            Console.WriteLine("3. Registrera en blodgivning");
            Console.WriteLine("4. Skicka förfrågan till givare baserat på blodgrupp");
            Console.WriteLine("Ange ditt val (1-4):");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    bloodBank.ShowBloodInventory();
                    break;
                case "2":
                    bloodBank.ShowWillingDonors();
                    break;
                case "3":
                    Console.WriteLine("Ange blodgivarens namn:");
                    string donorName = Console.ReadLine();
                    bloodBank.RegisterDonation(donorName);
                    break;
                case "4":
                    Console.WriteLine("Ange blodgruppen du vill skicka en förfrågan till:");
                    string bloodType = Console.ReadLine();
                    bloodBank.SendRequestToDonors(bloodType);
                    break;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }

            Console.WriteLine("Vill du gå tillbaka till huvudmenyn eller avsluta? (ange 'meny' för huvudmenyn eller 'avsluta' för att avsluta)");
            string userChoice = Console.ReadLine().ToLower();
            if (userChoice == "avsluta")
            {
                Console.WriteLine("Tack för att du använde programmet. Hejdå!");
                Environment.Exit(0); // Avslutar programmet
            }
            else if (userChoice == "meny")
            {
                break;
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Återgår till huvudmenyn.");
                break;
            }
        }
    }
}

class BloodDonor
{
    public string Name { get; }
    public string Email { get; }
    public string BloodType { get; }
    public string HealthHistory { get; }
    public bool IsWillingToDonate { get; private set; }

    public BloodDonor(string name, string email, string bloodType, string healthHistory)
    {
        Name = name;
        Email = email;
        BloodType = bloodType;
        HealthHistory = healthHistory;
        IsWillingToDonate = false;
    }

    public void RequestToDonate()
    {
        IsWillingToDonate = true;
        Console.WriteLine($"{Name} har anmält sig för att donera blod.");
    }
}

class Staff
{
    public string Name { get; }
    public string Email { get; }

    public Staff(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public void RegisterDonation(BloodDonor donor)
    {
        Console.WriteLine($"Blodgivning registrerad för {donor.Name}.");
    }
}

class BloodBank
{
    private List<BloodDonor> donors = new List<BloodDonor>();
    private Dictionary<string, int> bloodInventory = new Dictionary<string, int>()
    {
        { "A+", 0 }, { "A-", 0 }, { "B+", 0 }, { "B-", 0 }, { "O+", 0 }, { "O-", 0 }, { "AB+", 0 }, { "AB-", 0 }
    };

    public void AddBloodDonor(BloodDonor donor)
    {
        donors.Add(donor);
        Console.WriteLine($"{donor.Name} har lagts till i blodbanken.");
    }

    public void ShowBloodInventory()
    {
        Console.WriteLine("Blodbankens lager:");
        foreach (var bloodType in bloodInventory)
        {
            Console.WriteLine($"{bloodType.Key}: {bloodType.Value} donationer");
        }
    }

    public void RegisterDonation(BloodDonor donor)
    {
        if (donor.IsWillingToDonate)
        {
            bloodInventory[donor.BloodType]++;
            Console.WriteLine($"Donation från {donor.Name} med blodtyp {donor.BloodType} registrerad. Blodlagret har uppdaterats.");
        }
        else
        {
            Console.WriteLine("Blodgivare har inte anmält sig för att donera.");
        }
    }

    public void RegisterDonation(string donorName)
    {
        BloodDonor donor = donors.Find(d => d.Name == donorName);
        if (donor != null && donor.IsWillingToDonate)
        {
            RegisterDonation(donor);
        }
        else
        {
            Console.WriteLine("Blodgivare hittades inte eller har inte anmält sig för att donera.");
        }
    }

    public void SendRequestToDonors(string bloodType)
    {
        List<BloodDonor> eligibleDonors = donors.FindAll(d => d.BloodType == bloodType && d.IsWillingToDonate);
        if (eligibleDonors.Count > 0)
        {
            foreach (var donor in eligibleDonors)
            {
                Console.WriteLine($"Skickar förfrågan till {donor.Name} ({donor.Email}) för blodtyp {bloodType}.");
            }
        }
        else
        {
            Console.WriteLine($"Inga givare hittades för blodtyp {bloodType}.");
        }
    }

    public void ShowWillingDonors()
    {
        List<BloodDonor> willingDonors = donors.FindAll(d => d.IsWillingToDonate);
        if (willingDonors.Count > 0)
        {
            Console.WriteLine("Blodgivare som är villiga att donera:");
            foreach (var donor in willingDonors)
            {
                Console.WriteLine($"{donor.Name} - Blodtyp: {donor.BloodType}");
            }
        }
        else
        {
            Console.WriteLine("Inga blodgivare har anmält sig för att donera.");
        }
    }
}




