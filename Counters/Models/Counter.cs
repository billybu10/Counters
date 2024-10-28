namespace Counters.Models;

internal class Counter
{
    public string Filename { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }

    public Counter()
    {
        Filename = $"{Path.GetRandomFileName()}.counters.txt";
        Value = 0;
        Name = "";
    }

    public void Save()
    {
        if (Name == "" || Name == String.Empty || String.IsNullOrWhiteSpace(Name)) 
        {
            Name = "Default Name";
        }
        File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Name + "\n" + Value);
    }
    

    public void Delete() =>
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));

    public static Counter Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new()
            {
                Filename = Path.GetFileName(filename),
                Name = File.ReadLines(filename).First(),
                Value = Convert.ToInt32(File.ReadLines(filename).Skip(1).First())
            };
    }

    public static IEnumerable<Counter> LoadAll()
    {
        string appDataPath = FileSystem.AppDataDirectory;

        return Directory

                .EnumerateFiles(appDataPath, "*.counters.txt")

                .Select(filename => Counter.Load(Path.GetFileName(filename)))

                .OrderByDescending(counter => counter.Value);
    }
}