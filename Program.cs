using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;

public class Part
{
    public string name { get; private set; }
    public float weight { get; private set; }
    public Vector3 dimensions { get; private set; }

    public Material material { get; private set; }

    public Part() { }

    public Part(string name, float weight, Vector3 dimensions, Material material)
    {
        this.name = name;
        this.weight = weight;
        this.dimensions = dimensions;
        this.material = material;
    }
}

public class Material
{
    public string name { get; private set; } 
    public float density { get; private set; }
    public float cost { get; private set; }

    public Material() { }

    public Material(string name, float density, float cost)
    {
        this.name = name;
        this.density = density;
        this.cost = cost;
    }
}



public class JsonHandler<T> where T : class
{
    private string fileName;
    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };


    public JsonHandler() { }

    public JsonHandler(string fileName)
    {
        this.fileName = fileName;
    }


    public void SetFileName(string fileName)
    {
        this.fileName = fileName;
    }

    public void Write(List<T> list)
    {
        string jsonString = JsonSerializer.Serialize(list, options);

        if (new FileInfo(fileName).Length == 0)
        {
            File.WriteAllText(fileName, jsonString);
        }
        else
        {
            Console.WriteLine("Specified path file is not empty");
        }
    }

    public void Delete()
    {
        File.WriteAllText(fileName, string.Empty);
    }

    public void Rewrite(List<T> list)
    {
        string jsonString = JsonSerializer.Serialize(list, options);

        File.WriteAllText(fileName, jsonString);
    }

    public void Read(ref List<T> list)
    {
        if (File.Exists(fileName))
        {
            if (new FileInfo(fileName).Length != 0)
            {
                string jsonString = File.ReadAllText(fileName);
                list = JsonSerializer.Deserialize<List<T>>(jsonString);
            }
            else
            {
                Console.WriteLine("Specified path file is empty");
            }
        }
    }

    public void OutputJsonContents()
    {
        string jsonString = File.ReadAllText(fileName);

        Console.WriteLine(jsonString);
    }

    public void OutputSerializedList(List<T> list)
    {
        Console.WriteLine(JsonSerializer.Serialize(list, options));
    }
}



class Program
{
    static void Main(string[] args)
    {
        List<Part> partsList = new List<Part>();

        JsonHandler<Part> partsHandler = new JsonHandler<Part>("partsFile.json");

        partsList.Add(new Part("Shaft", 0.5f, new Vector3(30, 40, 50), new Material("440C", 7.8f, 8f)));
        partsList.Add(new Part("Hub", 0.5f, new Vector3(50, 20, 60), new Material("AISI321", 7.8f, 10f)));
        partsList.Add(new Part("Impeller", 0.5f, new Vector3(60, 60, 30), new Material("AISI304", 7.8f, 12f)));

        partsHandler.Rewrite(partsList);
        partsHandler.OutputJsonContents();
    }
}
