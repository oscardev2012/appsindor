using Microsoft.AspNetCore.Mvc.RazorPages;
using ManuelaRuiz.Web.Services;

namespace ManuelaRuiz.Web.Pages;

public class OpsModel : PageModel
{
    private readonly LeadStore _store;

    public OpsModel(LeadStore store) => _store = store;

    public IReadOnlyList<Lead> Leads { get; private set; } = [];

    public void OnGet() => Leads = _store.All;
}
