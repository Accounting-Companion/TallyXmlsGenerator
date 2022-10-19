using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TallyXmlsGenerator.Models;
// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class Auth
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class Body
{
    [JsonPropertyName("mode")]
    public string Mode { get; set; }

    [JsonPropertyName("raw")]
    public string Raw { get; set; }

    [JsonPropertyName("options")]
    public Options Options { get; set; }
}

public class DisabledSystemHeaders
{
    [JsonPropertyName("accept-encoding")]
    public bool AcceptEncoding { get; set; }

    [JsonPropertyName("content-type")]
    public bool? ContentType { get; set; }
}

public class Event
{
    [JsonPropertyName("listen")]
    public string Listen { get; set; }

    [JsonPropertyName("script")]
    public Script Script { get; set; }
}

public class Header
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("disabled")]
    public bool Disabled { get; set; }
}

public class Info
{
    public Info(string id, string name, string description, string schema, string exporter_id)
    {
        PostmanId = id;
        Name = name;
        Description = description;
        Schema = schema;
        ExporterId = exporter_id;
    }

    [JsonPropertyName("_postman_id")]
    public string PostmanId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("schema")]
    public string Schema { get; set; }

    [JsonPropertyName("_exporter_id")]
    public string ExporterId { get; set; }
}

public class Item
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("item")]
    public List<Item> ChildItems { get; set; }

    [JsonPropertyName("protocolProfileBehavior")]
    public ProtocolProfileBehavior ProtocolProfileBehavior { get; set; }

    [JsonPropertyName("request")]
    public Request Request { get; set; }

    [JsonPropertyName("response")]
    public List<object> Response { get; set; }

    [JsonPropertyName("event")]
    public List<Event> Event { get; set; }
}

public class Options
{
    [JsonPropertyName("raw")]
    public Raw Raw { get; set; }
}

public class ProtocolProfileBehavior
{
    [JsonPropertyName("disabledSystemHeaders")]
    public DisabledSystemHeaders DisabledSystemHeaders { get; set; }

    [JsonPropertyName("disableBodyPruning")]
    public bool? DisableBodyPruning { get; set; }
}

public class Raw
{
    [JsonPropertyName("language")]
    public string Language { get; set; }
}

public class Request
{
    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("header")]
    public List<Header> Header { get; set; }

    [JsonPropertyName("body")]
    public Body Body { get; set; }

    [JsonPropertyName("url")]
    public Url Url { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("auth")]
    public Auth Auth { get; set; }
}

public class PostManCollection
{
    public PostManCollection(string id,string name,string description,string schema,string exporter_id)
    {
        Info = new(id, name, description, schema, exporter_id);
    }

    [JsonPropertyName("info")]
    public Info Info { get; set; }

    [JsonPropertyName("item")]
    public List<Item> Item { get; set; }

    [JsonPropertyName("event")]
    public List<Event> Event { get; set; }

    [JsonPropertyName("variable")]
    public List<Variable> Variable { get; set; }
}

public class Script
{
    [JsonPropertyName("exec")]
    public List<string> Exec { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class Url
{
    [JsonPropertyName("raw")]
    public string Raw { get; set; }

    [JsonPropertyName("host")]
    public List<string> Host { get; set; }

    [JsonPropertyName("port")]
    public string Port { get; set; }
}

public class Variable
{
    public Variable(string key, string value)
    {
        Key = key;
        Value = value;
    }

    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}


