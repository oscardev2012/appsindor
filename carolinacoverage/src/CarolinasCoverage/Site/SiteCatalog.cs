namespace CarolinasCoverage.Site;

public sealed record ProductCard(string Id, string Icon, string Es, string En, string DescEs, string DescEn);

public sealed record GuideLocale(
    string Label,
    string Title,
    string Intro,
    string Story,
    string[] Common,
    string[] Claim,
    string[] Ask,
    string Note,
    string[]? Related = null);

public sealed record GuideProduct(string Icon, string[] Carriers, GuideLocale Es, GuideLocale En);

public sealed record ServiceLocale(
    string Icon,
    string Title,
    string Intro,
    string Safety,
    string[] Steps,
    string[] Choices,
    string[] Fields,
    string Summary,
    string Complete,
    string Review);

public sealed record ServiceFlow(ServiceLocale Es, ServiceLocale En);

public sealed class SiteCatalog
{
    public IReadOnlyList<string> Carriers { get; } =
    [
        "Progressive", "Nationwide", "Safeco / Liberty Mutual", "Dairyland",
        "National General", "Travelers", "Assurant America", "JSA", "Berkshire Hathaway"
    ];

    public IReadOnlyList<ProductCard> Products { get; } =
    [
        new("auto", "🚗", "Seguro de auto", "Auto insurance",
            "Vehículos, conductores y la protección que deseas explorar.",
            "Vehicles, drivers, and the protection you want to explore."),
        new("home", "🏠", "Seguro de hogar", "Home insurance",
            "Tu propiedad, historial y la ubicación de tu hogar.",
            "Your property, history, and the location of your home."),
        new("renters", "🔑", "Seguro para inquilinos", "Renters insurance",
            "Tus pertenencias y los requisitos de tu renta.",
            "Your belongings and rental requirements."),
        new("pet", "🐾", "Seguro para mascotas", "Pet insurance",
            "Elige el tipo de plan y comparte los datos de tu mascota.",
            "Choose a plan type and share your pet’s details."),
        new("business", "🏢", "Seguro para negocios", "Business insurance",
            "Responsabilidad civil y compensación laboral para tu operación.",
            "General liability and workers’ compensation for your operation."),
        new("commercial-auto", "🚚", "Auto comercial", "Commercial auto",
            "Vehículos de trabajo, conductores, carga y DOT cuando aplique.",
            "Work vehicles, drivers, cargo, and DOT when applicable."),
        new("life", "🤍", "Seguro de vida", "Life insurance",
            "Término, vida entera o gastos finales. No incluye salud, Medicare ni disability.",
            "Term, whole life, or final expense. Health, Medicare, and disability are not offered.")
    ];

    public IReadOnlyDictionary<string, GuideProduct> Guides { get; } = GuideSeed.All;

    public IReadOnlyDictionary<string, ServiceFlow> Services { get; } = ServiceSeed.All;

    public IReadOnlyList<string[]> GlossaryEs { get; } =
    [
        ["Deducible", "La cantidad que una persona puede pagar de su bolsillo antes de que aplique una cobertura elegible."],
        ["Límite", "El máximo que una aseguradora podría pagar por una categoría de reclamo cubierta, según la póliza."],
        ["Prima", "El costo de la póliza. Puede cambiar por riesgo, coberturas, ubicación y elegibilidad."],
        ["Exclusión", "Una situación, bien, servicio o causa que la póliza no cubre o trata de manera limitada."],
        ["Página de declaraciones", "Resumen de personas o bienes asegurados, fechas, coberturas, límites y deducibles."]
    ];

    public IReadOnlyList<string[]> GlossaryEn { get; } =
    [
        ["Deductible", "The amount a person may pay out of pocket before eligible coverage applies."],
        ["Limit", "The most an insurer could pay for a category of covered claim, subject to the policy."],
        ["Premium", "The cost of the policy. It can change with risk, coverage, location, and eligibility."],
        ["Exclusion", "A situation, property, service, or cause the policy does not cover or treats in a limited way."],
        ["Declarations page", "A summary of insured people or property, dates, coverages, limits, and deductibles."]
    ];
}
