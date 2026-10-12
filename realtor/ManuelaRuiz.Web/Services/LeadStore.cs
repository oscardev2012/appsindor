namespace ManuelaRuiz.Web.Services;

public sealed record Lead(
    DateTimeOffset CreatedAt,
    string Kind,
    string Name,
    string Email,
    string Phone,
    IReadOnlyDictionary<string, string> Fields);

public sealed class LeadStore
{
    private readonly List<Lead> _leads = [];
    private readonly object _gate = new();

    public IReadOnlyList<Lead> All
    {
        get
        {
            lock (_gate)
            {
                return _leads.ToArray();
            }
        }
    }

    public void Add(Lead lead)
    {
        lock (_gate)
        {
            _leads.Insert(0, lead);
        }
    }
}
