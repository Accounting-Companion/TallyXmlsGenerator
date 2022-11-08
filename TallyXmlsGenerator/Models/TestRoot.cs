using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TallyXmlsGenerator.Models;
internal class TestRoot
{
    public TestRoot(string name)
    {
        Name = name;
    }

    public string Name { get; }

    [JsonExtensionData]
    public Dictionary<string, object> JsonExtendedData { get; set; } = new();
}
