// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.Json;
using TallyXmlsGenerator.Models;
using TallyXmlsGenerator.Services;
var configuration = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();
//TestRoot _testRoot = new("sdfg");
//_testRoot.JsonExtendedData.Add("sdsfdg", "sdf");
//_testRoot.JsonExtendedData.Add("sdsfsdfdg", 1);
//_testRoot.JsonExtendedData.Add("sdg", true);
//var sdfg = JsonSerializer.Serialize(_testRoot);
Console.WriteLine("Creating XmlGenerator Instance ....");
XmlGenerator XmlGenerator = new();
XmlGenerator.GenerateXmlsOnly();
//XmlGenerator.GeneratePostManCollectionJson();
string collectionid = configuration["collectionid"];
string postmantoken = configuration["postmantoken"];
var s = XmlGenerator.GetLedgersXml();
if (!string.IsNullOrEmpty(collectionid) && !string.IsNullOrEmpty(postmantoken))
{
    await XmlGenerator.UpdateCollection(collectionid, postmantoken);
}
else
{
    Console.WriteLine("No Token and CollectionId found");
}

