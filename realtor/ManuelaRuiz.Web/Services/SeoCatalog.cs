namespace ManuelaRuiz.Web.Services;

public sealed record SeoFaq(string Question, string Answer);

public sealed record SeoPage(
    string Path,
    string TitleEs,
    string TitleEn,
    string DescriptionEs,
    string DescriptionEn,
    string OgType,
    IReadOnlyList<string> BreadcrumbEs,
    IReadOnlyList<string> BreadcrumbEn,
    IReadOnlyList<SeoFaq> FaqsEs,
    IReadOnlyList<SeoFaq> FaqsEn,
    bool Index,
    double Priority);

public static class SeoCatalog
{
    public static readonly IReadOnlyList<SeoPage> Pages = Build();

    private static IReadOnlyList<SeoPage> Build()
    {
        var pages = new List<SeoPage>
        {
            P("/", "Agente de bienes raíces en Charlotte, NC | Manuela Ruiz",
                "Realtor in Charlotte, NC | Manuela Ruiz, bilingual agent",
                "Realtor bilingüe en Charlotte, NC: compra, venta, ITIN, relocation y programas de enganche. Manuela Ruiz, NC Broker #350810.",
                "Bilingual Realtor in Charlotte, NC for buyers, sellers, ITIN conversations, relocation and down-payment programs. Manuela Ruiz, NC Broker #350810.",
                "website", ["Inicio"], ["Home"], 1.0),
            P("/search", "Casas en venta cerca de Charlotte NC | Búsqueda personalizada",
                "Homes for Sale Near Charlotte | Custom home search",
                "Dile a Manuela qué casa buscas en Charlotte, Concord, Matthews o Gastonia: precio, habitaciones y tipo. Ella revisa inventario actual.",
                "Tell Manuela what home you need in Charlotte, Concord, Matthews or Gastonia: price, beds and type. She reviews current inventory.",
                "website", ["Inicio", "Buscar casas"], ["Home", "Search homes"], 0.9),
            P("/qualify", "¿Cuánta casa puedo comprar en Charlotte? | Calculadora",
                "How much house can I afford in Charlotte? | Calculator",
                "Estimador educativo de poder de compra en Charlotte: ingresos, deudas, fondos y DTI. Sin credit pull. No es aprobación de préstamo.",
                "Educational buying-power estimator for Charlotte: income, debts, funds and DTI. No credit pull. This is not loan approval.",
                "website", ["Inicio", "¿Cuánto puedo comprar?"], ["Home", "How much can I buy?"], 0.9),
            P("/buyers", "Comprar casa en Charlotte y North Carolina | Manuela Ruiz",
                "Buy a Home in Charlotte and North Carolina | Manuela Ruiz",
                "Guía para comprar en Charlotte: presupuesto, prestamista, oferta, inspección y cierre. Incluye ITIN y primera vivienda. NC Broker #350810.",
                "Buy a home in Charlotte with a bilingual buyer agent: budget, lender, offer, inspection and closing. Includes ITIN conversations.",
                "article", ["Inicio", "Compradores"], ["Home", "Buyers"], 0.9),
            P("/sellers", "Venda su casa con una agente local | Charlotte NC",
                "Sell Your Home With a Local Listing Agent | Charlotte NC",
                "Vende en Charlotte con preparación, precio, mercadeo y revisión de ofertas. Manuela Ruiz, NC Broker #350810.",
                "Sell in Charlotte with preparation, pricing, marketing and offer review. Manuela Ruiz, NC Broker #350810.",
                "article", ["Inicio", "Vendedores"], ["Home", "Sellers"], 0.85),
            P("/home-valuation", "¿Cuánto vale mi casa en el área de Charlotte? | Manuela Ruiz",
                "What Is My Charlotte-Area Home Worth? | Manuela Ruiz",
                "Solicita un análisis comparativo de tu casa cerca de Charlotte. Es una opinión de precio, no un appraisal ni una garantía de venta.",
                "Request a comparative market analysis for your Charlotte-area home. It is a pricing opinion, not an appraisal or a sale-price guarantee.",
                "article", ["Inicio", "Valor de casa"], ["Home", "Home valuation"], 0.9),
            P("/relocation", "Mudarse a Charlotte y North Carolina | Realtor bilingüe",
                "Relocating to Charlotte and North Carolina | Bilingual Realtor",
                "Mudanza a Charlotte desde otro estado o país: zonas, recorridos remotos y logística. Manuela compara comunidades y coordina profesionales.",
                "Moving to Charlotte from another state or country: areas, remote tours and logistics. Manuela compares communities and coordinates local professionals.",
                "article", ["Inicio", "Relocation"], ["Home", "Relocation"], 0.8),
            P("/areas", "Comunidades del área de Charlotte | Concord, Huntersville, Matthews",
                "Charlotte-Area Communities | Concord, Huntersville, Matthews",
                "Áreas de servicio de Manuela Ruiz: Charlotte, Concord, Huntersville, Gastonia, Matthews y Statesville. SC se coordina con un profesional licenciado.",
                "Manuela Ruiz service areas: Charlotte, Concord, Huntersville, Gastonia, Matthews and Statesville. South Carolina is coordinated with a licensed professional.",
                "article", ["Inicio", "Ciudades"], ["Home", "Cities"], 0.8),
            P("/service-area", "Área de servicio de bienes raíces alrededor de Charlotte | Manuela Ruiz",
                "Real Estate Service Area Around Charlotte | Manuela Ruiz",
                "Manuela evalúa oportunidades aproximadamente a dos horas de Charlotte. Confirma inventario, commute y representación antes de agendar.",
                "Manuela evaluates opportunities about two hours from Charlotte. She confirms inventory, commute and representation before scheduling.",
                "article", ["Inicio", "Área de servicio"], ["Home", "Service area"], 0.8),
            P("/sold", "Casas vendidas y compradores representados | Portafolio Charlotte",
                "Sold homes and buyer representation | Charlotte portfolio",
                "Portafolio documentado de Manuela Ruiz: compradores representados, vendedores y un listado activo. Sin enviar a portales de terceros.",
                "Documented portfolio for Manuela Ruiz: buyer representation, sellers and an active listing. We do not send you to third-party portals.",
                "article", ["Inicio", "Propiedades"], ["Home", "Properties"], 0.85),
            P("/programs", "Ayuda para el enganche en North Carolina | Programas de compra",
                "Down Payment Assistance in North Carolina | Homebuyer programs",
                "Compara USDA, VA, NACA, HouseCharlotte, NC Home Advantage y $15,000 para first-time buyers. 0% de enganche no siempre es $0 al cerrar.",
                "Compare USDA, VA, NACA, HouseCharlotte, NC Home Advantage and $15,000 for first-time buyers. 0% down is not always $0 to close.",
                "article", ["Inicio", "Programas"], ["Home", "Programs"], 0.9),
            P("/itin-home-buyers", "Comprar casa con ITIN en Charlotte NC | Manuela Ruiz",
                "Buying a Home With an ITIN | Charlotte NC Realtor",
                "Ser dueño y obtener hipoteca son cosas distintas. Algunos prestamistas consideran ITIN; Manuela organiza la conversación inmobiliaria, no aprueba préstamos.",
                "Owning property and getting a mortgage are different. Some lenders consider ITIN buyers; Manuela organizes the real-estate conversation and does not approve loans.",
                "article", ["Inicio", "Comprar con ITIN"], ["Home", "ITIN home buyers"], 0.9),
            P("/investors", "Propiedades de inversión cerca de Charlotte | Manuela Ruiz",
                "Investment Properties Near Charlotte | Manuela Ruiz",
                "Inversión residencial cerca de Charlotte: renta, reparaciones, números y salida. Manuela ayuda a evaluar la propiedad antes de comprometer capital.",
                "Residential investing near Charlotte: rent, repairs, numbers and exit. Manuela helps evaluate the property before you commit capital.",
                "article", ["Inicio", "Inversionistas"], ["Home", "Investors"], 0.7),
            P("/first-time-buyers", "Guía para comprar casa por primera vez | Charlotte NC",
                "First-Time Home Buyer Guide | Charlotte NC",
                "Plan claro para tu primera casa en Charlotte: documentos, asistencia de enganche y decisiones sin presión. Manuela explica el proceso en español o inglés.",
                "A clear plan for your first home in Charlotte: documents, down-payment assistance and decisions without pressure. Manuela explains the process in English or Spanish.",
                "article", ["Inicio", "Primera vivienda"], ["Home", "First-time buyers"], 0.85),
            P("/new-construction", "Casas nuevas cerca de Charlotte | Agente del comprador",
                "New Construction Homes Near Charlotte | Buyer agent",
                "Casa nueva cerca de Charlotte: comunidades, incentivos, contrato e inspecciones. El representante del builder trabaja para el builder; Manuela te representa a ti.",
                "New construction near Charlotte: communities, incentives, contract and inspections. The builder representative works for the builder; Manuela represents you.",
                "article", ["Inicio", "Nueva construcción"], ["Home", "New construction"], 0.75),
            P("/faq", "Preguntas de bienes raíces en Charlotte NC | Manuela Ruiz",
                "Charlotte NC Real Estate FAQ | Manuela Ruiz",
                "Respuestas directas: cuánto de enganche, cómo comprar o vender, ITIN, due diligence en North Carolina y realtor bilingüe en Charlotte. Información educativa; no es asesoría legal ni crediticia.",
                "Direct answers: how much down payment, how to buy or sell, ITIN, North Carolina due diligence, and a bilingual realtor in Charlotte. Educational information; not legal or credit advice.",
                "article", ["Inicio", "Preguntas frecuentes"], ["Home", "FAQ"], 0.85),
            P("/partners", "Servicios inmobiliarios especializados | Red local Charlotte",
                "Specialty Real Estate Services | Charlotte local network",
                "Red local independiente: HVAC, techo y seguro de vivienda. Tú decides a quién contratar. Manuela coordina preguntas técnicas durante due diligence.",
                "Independent local network: HVAC, roofing and homeowners insurance. You choose whom to hire. Manuela coordinates technical questions during due diligence.",
                "article", ["Inicio", "Recursos locales"], ["Home", "Local resources"], 0.6),
            P("/resources", "Centro de conocimiento inmobiliario bilingüe | Charlotte NC",
                "Bilingual real estate knowledge center | Charlotte NC",
                "Respuestas claras sobre comprar en Charlotte: ITIN, inspección, precio y proceso. Contenido bilingüe de Manuela Ruiz.",
                "Clear answers about buying in Charlotte: ITIN, inspection, price and process. Bilingual content from Manuela Ruiz.",
                "article", ["Inicio", "Centro de conocimiento"], ["Home", "Knowledge center"], 0.7)
        };

        foreach (var city in SeoGuide.Cities)
        {
            pages.Add(P(
                "/areas/" + city.Slug,
                city.TitleEs,
                city.TitleEn,
                city.LeadEs,
                city.LeadEn,
                "article",
                ["Inicio", "Ciudades", city.Name],
                ["Home", "Cities", city.Name],
                city.Slug == "charlotte-nc" ? 0.85 : 0.7));
        }

        return pages;
    }

    private static SeoPage P(
        string path, string titleEs, string titleEn, string descEs, string descEn,
        string og, string[] crumbsEs, string[] crumbsEn, double priority) =>
        new(path, titleEs, titleEn, descEs, descEn, og, crumbsEs, crumbsEn, [], [], true, priority);

    public static SeoPage? Find(string path)
    {
        path = SiteLocale.Normalize(SiteLocale.StripEnglishPrefix(path));
        var page = Pages.FirstOrDefault(p => string.Equals(p.Path, path, StringComparison.OrdinalIgnoreCase));
        if (page is null) return null;
        var faqsEs = SeoGuide.Faqs(path, false);
        var faqsEn = SeoGuide.Faqs(path, true);
        if (faqsEs.Count == 0 && faqsEn.Count == 0) return page;
        return page with { FaqsEs = faqsEs, FaqsEn = faqsEn };
    }
}
