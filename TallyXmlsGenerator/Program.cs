// See https://aka.ms/new-console-template for more information
using TallyXmlsGenerator.Services;

Console.WriteLine("Creating XmlGenerator Instance ....");
XmlGenerator XmlGenerator = new();
XmlGenerator.GenerateXmlsOnly();

