namespace ExceptionsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                Console.WriteLine("=== Start av programmet ===");

                // Exempel 1: try-catch-finally
                try
                {
                    Console.WriteLine("Försöker läsa fil och räkna...");
                    var path = Path.Combine(AppContext.BaseDirectory, "numbers.txt");
                    var result = ProcessFile(path);

                    Console.WriteLine($"\nResultat: {result}");
                }
                catch (ArgumentNullException ex)
                {
                    // Specifikt fel om filnamnet är tomt eller null.
                    Console.WriteLine(ex.Message);
                }
                catch (FileNotFoundException ex)
                {
                    // Specifikt fel om filen inte kunde hittas.
                    Console.WriteLine(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    // Specifikt fel om det inte gick att processa filen.
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    // Specifikt fel om innehållet i filen inte är i rätt format.
                    Console.WriteLine(ex.Message);
                }
                catch (DivideByZeroException ex)
                {
                    // Specifikt fel om nolldivision.
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    // Fallback för alla övriga obekanta fel
                    Console.WriteLine($"Ett okänt fel har inträffat: {ex.Message}");
                }
                finally
                {
                    // Körs ALLTID, även om det blev undantag
                    Console.WriteLine("Cleanup: Logging avslutat anrop.");
                }

                Console.WriteLine("Programmet avslutas normalt.");
            }

            // Exempel på metod som själv kastar ett undantag (throw)
            static double ProcessFile(string fileName)
            {
                StreamReader? reader = null;

                try
                {
                    // Om filnamnet är tomt: logiskt fel vi vill signalera
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        throw new ArgumentNullException("Filnamnet är tomt eller null.", nameof(fileName));
                    }

                    reader = new StreamReader(fileName);

                    string? line = reader.ReadLine();

                    if (line == null)
                    {
                        throw new InvalidOperationException("Filen är tom.");
                    }

                    // Försöker omvandla text till tal
                    int number = int.Parse(line); // Kan ge FormatException

                    // Division: kan ge DivideByZeroException
                    return 100.0 / number;
                }
                catch (ArgumentNullException ex)
                {
                    // Vi vill ge en mer meningsfull feltyp till anroparen.
                    Console.WriteLine($"Filnamnet är tomt eller null i ProcessFile: {ex.Message}");
                    throw; // Kastar vidare samma undantag.
                }
                catch (FileNotFoundException ex)
                {
                    // Vi kan logga felet här om vi vill
                    Console.WriteLine($"Filen hittades inte i ProcessFile: {ex.Message}");
                    throw; // Kastar vidare samma undantag
                }
                catch (InvalidOperationException ex)
                {
                    // Vi kan logga felet här om vi vill
                    Console.WriteLine($"Ogiltig operation i ProcessFile: {ex.Message}");
                    throw; // Kastar vidare samma undantag
                }
                catch (FormatException ex)
                {
                    // Vi kan logga eller omformulera felet
                    Console.WriteLine($"Formatfel i ProcessFile: {ex.Message}");
                    // Vi kan välja att låta metoden "kasta upp" felet
                    throw; // När du i `catch` bara vill logga/analysera,
                           // men låta anroparen (t.ex. en högre nivå i applikationen)
                           // bestämma hur man ska återhämta sig. 
                }
                catch (DivideByZeroException ex)
                {
                    // Vi kan logga eller omformulera felet
                    Console.WriteLine($"Nolldivision i ProcessFile: {ex.Message}");
                    // Vi kan välja att låta metoden "kasta upp" felet
                    throw; // När du i `catch` bara vill logga/analysera,
                           // men låta anroparen (t.ex. en högre nivå i applikationen)
                           // bestämma hur man ska återhämta sig. 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett okänt fel har inträffat i ProcessFile: {ex.Message}");
                    // Om vi vill ge en mer meningsfull feltyp till anroparen
                    throw new Exception("Ett okänt fel har inträffat.", ex); // InnerException = ursprunglig fel
                }
                finally
                {
                    // Garanterad stängning av resurs
                    reader?.Close();
                    Console.WriteLine("finally i ProcessFile: StreamReader stängd.");
                }
            }
        }
    }
}

