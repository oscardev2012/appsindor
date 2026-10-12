using System.Text.Json.Nodes;

namespace SafeManagement.Data;

/* Catálogo de reparaciones: repair-catalog.json + cambios y reparaciones nuevas de Management en dbo.SafeOpsCatalogItems. */
public static class SafeCatalog
{
    public static JsonArray BaseItems(string basePath) => JsonNode.Parse(File.ReadAllText(basePath))!.AsArray();

    public static List<JsonNode> All(SafeSql sql, string basePath)
    {
        var saved = new Dictionary<string, (string Kind, JsonNode Data)>();
        using (var db = sql.Open())
        using (var cmd = db.Command("SELECT ItemId, Kind, Data, UpdatedAt FROM dbo.SafeOpsCatalogItems ORDER BY Seq"))
        using (var r = cmd.ExecuteReader())
        {
            while (r.Read())
            {
                var node = JsonNode.Parse(r.GetString(2))!;
                node["origin"] = r.GetString(1);
                node["updatedAt"] = r.Iso(3);
                saved[r.GetString(0)] = (r.GetString(1), node);
            }
        }
        var items = BaseItems(basePath).Select(n =>
        {
            var id = (string)n!["id"]!;
            return saved.TryGetValue(id, out var s) ? s.Data : n.DeepClone();
        }).ToList();
        items.AddRange(saved.Values.Where(s => s.Kind == "custom").Select(s => s.Data));
        return items;
    }
}
