using NK_ITMedia_License;
using System;
using System.Reflection;
using static NK_ITMedia_License.License;

namespace TestLicense
{
    class Program
    {

        public static DateTime UnixTimeStampToDateTime(Int32 unixTimeStamp)
        {
            DateTime DtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DtDateTime = DtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return DtDateTime;
        }

        public static void Error(string text)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ReadKey();
            System.Diagnostics.Process.Start(Assembly.GetExecutingAssembly().Location);
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            License Lic = new License("roflpeter", "hansenkopter");

            string HWID;

            Console.WriteLine("Wie lange soll die Lizenz gültig sein?");
            if(!int.TryParse(Console.ReadLine(), out int Days))
            {
                Error("ACHTUNG: Die Anzahl der Tage ist kein Integer!");
            }
            Console.WriteLine("Für welche Produkt ID soll die Lizenz gültig sein?");
            if (!int.TryParse(Console.ReadLine(), out int Pid))
            {
                Error("ACHTUNG: Die Produkt ID ist nicht gültig!");
            }
            Console.WriteLine(string.Format("Generiere eine Serial mit der eigenen HWID für die Produkt ID \"{0}\" die für {1} Tage gültig ist.", Pid, Days));
            Console.WriteLine("Für welche HWID soll die Lizenz gültig sein? - Bei keiner Eingabe wird die von deinem System genommen.");
            HWID = Console.ReadLine();

            if(string.IsNullOrEmpty(HWID) || string.IsNullOrWhiteSpace(HWID))
            {
                HWID = Lic.GetHWID();
            }

            string Serial = Lic.GenerateLicense(Days, false, HWID, Pid, "Nils", "Kleinert", "NK ITMedia");

            try
            {
                GetLicenseInformations TmpCheck = new GetLicenseInformations(Serial);
            }
            catch (Exception Ex)
            {
                Error("ACHTUNG: Die Seriennummer ist nicht gültig (Zum Beispiel inkorrekte HWID)!");
            }
            GetLicenseInformations Check = new GetLicenseInformations(Serial);

            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("========================== SERIAL GENERIERT ==========================");
            Console.WriteLine(Serial);
            Console.WriteLine("========================== SERIAL GENERIERT ==========================");
            Console.WriteLine("");
            Console.WriteLine("Gegenprüfung:");


            Console.WriteLine("Ist die Serial gültig:\t" + Lic.ValidLicense(Serial));
            Console.WriteLine("Timestamp:\t\t" + Check.Timestamp().ToString() + " (" + UnixTimeStampToDateTime(Check.Timestamp()).ToShortDateString() + " " + UnixTimeStampToDateTime(Check.Timestamp()).ToShortTimeString() + ")");
            Console.WriteLine("Trial:\t\t\t" + Check.IsTrial().ToString());
            Console.WriteLine("HWID:\t\t\t" + Check.HWID());
            Console.WriteLine("ProductID:\t\t" + Check.ProductID());
            Console.WriteLine("Host manipuliert:\t" + Lic.IsHostManipulated());
            Console.WriteLine("IsHWIDBannedForPID:\t" + Lic.IsHWIDBannedForPID(Check.HWID(), Check.ProductID()));

            Console.ReadKey();
        }
    }
}
