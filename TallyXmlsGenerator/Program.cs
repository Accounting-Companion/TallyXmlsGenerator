// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using System.Reflection;
using TallyXmlsGenerator.Services;
var configuration = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();
Console.WriteLine("Creating XmlGenerator Instance ....");
XmlGenerator XmlGenerator = new();
XmlGenerator.GenerateXmlsOnly();
//XmlGenerator.GeneratePostManCollectionJson();
string collectionid = configuration["collectionid"];
string postmantoken = configuration["postmantoken"];
if (!string.IsNullOrEmpty(collectionid) && !string.IsNullOrEmpty(postmantoken))
{
    await XmlGenerator.UpdateCollection(collectionid, postmantoken);
}
else
{
    Console.WriteLine("No Token and CollectionId found");
}

