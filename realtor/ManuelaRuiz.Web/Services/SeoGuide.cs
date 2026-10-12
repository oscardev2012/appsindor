namespace ManuelaRuiz.Web.Services;

public sealed record SeoCity(
    string Slug,
    string Name,
    string County,
    string TitleEs,
    string TitleEn,
    string H1Es,
    string H1En,
    string LeadEs,
    string LeadEn,
    string ContextEs,
    string ContextEn,
    IReadOnlyList<string> BulletsEs,
    IReadOnlyList<string> BulletsEn);

public static class SeoGuide
{
    public static readonly Dictionary<string, string> Aliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["/buy-a-home"] = "/buyers",
        ["/sell-your-home"] = "/sellers",
        ["/home-selling-guide"] = "/sellers",
        ["/sell-your-home/special-situations"] = "/sellers",
        ["/buy-a-home/offer-and-closing"] = "/buyers",
        ["/first-time-home-buyers"] = "/first-time-buyers",
        ["/down-payment-assistance"] = "/programs",
        ["/realtor-charlotte-nc"] = "/",
        ["/investment-properties"] = "/investors",
        ["/homes-for-sale"] = "/search",
        ["/reubicacion"] = "/relocation",
        ["/casas-nuevas"] = "/new-construction",
        ["/comprar-casa"] = "/buyers",
        ["/vender-casa"] = "/sellers",
        ["/comprador-primera-casa"] = "/first-time-buyers",
        ["/ayuda-enganche"] = "/programs",
        ["/agente-bienes-raices-charlotte-nc"] = "/",
        ["/valor-de-casa"] = "/home-valuation",
        ["/preguntas-frecuentes"] = "/faq",
        ["/faqs"] = "/faq",
        ["/preguntas"] = "/faq",
        ["/comprar-casa-con-itin"] = "/itin-home-buyers",
        ["/area-de-servicio"] = "/service-area",
        ["/propiedades-inversion"] = "/investors",
        ["/casas-en-venta"] = "/search",
        ["/guia-vender-casa"] = "/sellers",
        ["/specialty-real-estate"] = "/partners",
        ["/bienes-raices-especializados"] = "/partners",
        ["/fair-housing"] = "/faq"
    };

    public static readonly Dictionary<string, string> EsSlugs = new(StringComparer.OrdinalIgnoreCase)
    {
        ["comprar-casa"] = "/buyers",
        ["vender-casa"] = "/sellers",
        ["valor-de-casa"] = "/home-valuation",
        ["comprador-primera-casa"] = "/first-time-buyers",
        ["ayuda-enganche"] = "/programs",
        ["comprar-casa-con-itin"] = "/itin-home-buyers",
        ["reubicacion"] = "/relocation",
        ["casas-nuevas"] = "/new-construction",
        ["propiedades-inversion"] = "/investors",
        ["guia-vender-casa"] = "/sellers",
        ["preguntas-frecuentes"] = "/faq",
        ["faqs"] = "/faq",
        ["preguntas"] = "/faq",
        ["areas"] = "/areas",
        ["casas-en-venta"] = "/search",
        ["area-de-servicio"] = "/service-area",
        ["agente-bienes-raices-charlotte-nc"] = "/",
        ["bienes-raices-especializados"] = "/partners",
        ["vender-casa/situaciones-especiales"] = "/sellers"
    };

    public static readonly IReadOnlyList<SeoCity> Cities =
    [
        new("charlotte-nc", "Charlotte", "Mecklenburg County",
            "Agente de bienes raíces en Charlotte NC | Manuela Ruiz",
            "Real Estate Agent in Charlotte NC | Manuela Ruiz",
            "Agente de bienes raíces en Charlotte NC",
            "Real Estate Agent in Charlotte NC",
            "Charlotte es la base de Manuela Ruiz, NC Broker #350810. Compra, venta, primera vivienda, ITIN y programas de enganche con guía bilingüe.",
            "Charlotte is Manuela Ruiz's home base, NC Broker #350810. Buy, sell, first-time buyers, ITIN conversations and down-payment programs with bilingual guidance.",
            "Manuela trabaja desde Charlotte y compara inventario, commute e impuestos a nivel de propiedad, no con un promedio del condado. Rock Hill y Fort Mill se coordinan con un profesional licenciado en South Carolina.",
            "Manuela works from Charlotte and compares inventory, commute and taxes at the property level, not a county average. Rock Hill and Fort Mill are coordinated with a South Carolina-licensed professional.",
            ["Núcleo urbano y financiero de la región", "Resale y nueva construcción en el mismo radio", "Consulta bilingüe desde la primera llamada", "NC Broker #350810 · Guerra & Associates Realty LLC"],
            ["Urban and financial core of the region", "Resale and new construction in the same radius", "Bilingual consult from the first call", "NC Broker #350810 · Guerra & Associates Realty LLC"]),
        new("concord-nc", "Concord", "Cabarrus County",
            "Realtor en Concord NC | Agente bilingüe Manuela Ruiz",
            "Real Estate Agent in Concord NC | Manuela Ruiz",
            "Agente de bienes raíces en Concord NC",
            "Real Estate Agent in Concord NC",
            "Concord combina acceso a Charlotte con inventario de resale y nueva construcción. Manuela compara commute, impuestos y costo mensual total.",
            "Concord combines Charlotte access with resale and new-construction inventory. Manuela compares commute, taxes and total monthly cost.",
            "Cabarrus County no es un atajo automático de precio. Hay que revisar trayecto a la hora real, HOA, impuestos y condición de cada casa antes de ofertar.",
            "Cabarrus County is not an automatic price shortcut. Review commute at the hour you actually travel, HOA, taxes and condition before you offer.",
            ["Acceso a I-85 y al aeropuerto regional", "Mezcla de resale y builders", "Impuestos y seguros distintos a Mecklenburg", "Búsqueda personalizada, no un portal genérico"],
            ["Access to I-85 and the regional airport", "Mix of resale and builders", "Taxes and insurance unlike Mecklenburg", "Custom search, not a generic portal"]),
        new("gastonia-nc", "Gastonia", "Gaston County",
            "Realtor en Gastonia NC | Manuela Ruiz",
            "Real Estate Agent in Gastonia NC | Manuela Ruiz",
            "Agente de bienes raíces en Gastonia NC",
            "Real Estate Agent in Gastonia NC",
            "Gastonia ofrece distancias y precios distintos al centro de Charlotte. Revisamos inventario actual, condición y traslado antes de ofertar.",
            "Gastonia offers different distances and prices than central Charlotte. We review current inventory, condition and commute before you offer.",
            "Gaston County cambia el cálculo de commute, impuestos y tipo de vivienda. Manuela organiza la comparación con datos de la propiedad, no con un titular regional.",
            "Gaston County changes commute, taxes and housing type. Manuela organizes the comparison with property-level data, not a regional headline.",
            ["Oeste de Charlotte, I-85 y US-321", "Inventario y condición muy variables", "Belmont y Mount Holly en el mismo radio", "Representación bilingüe para compra o venta"],
            ["West of Charlotte, I-85 and US-321", "Inventory and condition vary widely", "Belmont and Mount Holly in the same radius", "Bilingual representation to buy or sell"]),
        new("huntersville-nc", "Huntersville", "Mecklenburg County",
            "Realtor en Huntersville NC | Lake Norman",
            "Real Estate Agent in Huntersville NC | Lake Norman",
            "Agente de bienes raíces en Huntersville NC",
            "Real Estate Agent in Huntersville NC",
            "Huntersville y Lake Norman: compara lago, commute, HOA y escuelas dentro de tu presupuesto real, no un promedio del condado.",
            "Huntersville and Lake Norman: compare the lake, commute, HOA and schools inside your real budget, not a county average.",
            "Vivir cerca del lago no significa el mismo pago mensual. HOA, seguro, trayecto a Uptown y tipo de casa cambian el número final.",
            "Living near the lake does not mean the same monthly payment. HOA, insurance, the Uptown commute and home type change the final number.",
            ["Lake Norman, Cornelius y Davidson cerca", "HOA y amenidades hay que presupuestar", "Commute I-77 en hora pico", "Guía en español o inglés"],
            ["Lake Norman, Cornelius and Davidson nearby", "HOA and amenities belong in the budget", "I-77 commute at peak hours", "Guidance in English or Spanish"]),
        new("matthews-nc", "Matthews", "Mecklenburg County",
            "Realtor en Matthews NC | Manuela Ruiz",
            "Real Estate Agent in Matthews NC | Manuela Ruiz",
            "Agente de bienes raíces en Matthews NC",
            "Real Estate Agent in Matthews NC",
            "Matthews está al sureste de Charlotte. Manuela ayuda a comparar inventario, trayecto y costo total con un plan bilingüe.",
            "Matthews sits southeast of Charlotte. Manuela helps compare inventory, commute and total cost with a bilingual plan.",
            "Matthews, Mint Hill y Union County se sienten distintos en commute, impuestos y estilo. El plan empieza por el presupuesto mensual y la rutina real de la familia.",
            "Matthews, Mint Hill and Union County feel different in commute, taxes and lifestyle. The plan starts with monthly budget and the family's real routine.",
            ["Sureste de Charlotte y acceso a Independence Blvd", "Resale establecido y pockets de obra nueva", "Comparar con Mint Hill e Indian Trail", "Consulta bilingüe y búsqueda a medida"],
            ["Southeast Charlotte and Independence Blvd access", "Established resale and pockets of new construction", "Compare with Mint Hill and Indian Trail", "Bilingual consult and custom search"]),
        new("statesville-nc", "Statesville", "Iredell County",
            "Realtor en Statesville NC | Manuela Ruiz",
            "Real Estate Agent in Statesville NC | Manuela Ruiz",
            "Agente de bienes raíces en Statesville NC",
            "Real Estate Agent in Statesville NC",
            "Statesville está dentro del radio práctico desde Charlotte. Confirmamos inventario, traslado y representación antes de agendar.",
            "Statesville is inside the practical radius from Charlotte. We confirm inventory, commute and representation before scheduling.",
            "Iredell County puede encajar si el commute y el presupuesto cuadran. Manuela confirma alcance y profesionales antes de avanzar, sin asumir que cada ZIP está a dos horas reales.",
            "Iredell County can fit when commute and budget line up. Manuela confirms coverage and professionals before moving forward, without assuming every ZIP is a true two-hour drive.",
            ["I-40 e I-77 como ejes de traslado", "Mezcla de pueblo, rural y resale", "Confirmar inventario antes de viajar", "Coordinación bilingüe desde Charlotte"],
            ["I-40 and I-77 as commute axes", "Mix of town, rural and resale", "Confirm inventory before you travel", "Bilingual coordination from Charlotte"])
    ];

    static SeoGuide()
    {
        foreach (var city in Cities)
        {
            Aliases["/homes-for-sale/" + city.Slug] = "/areas/" + city.Slug;
            EsSlugs["areas/" + city.Slug] = "/areas/" + city.Slug;
            EsSlugs["casas-en-venta/" + city.Slug] = "/areas/" + city.Slug;
        }
    }

    public static string? ResolveAlias(string path)
    {
        path = SiteLocale.Normalize(path);
        return Aliases.TryGetValue(path, out var dest) ? dest : null;
    }

    public static string ResolveSpanishSlug(string slug)
    {
        slug = (slug ?? "").Trim('/');
        if (string.IsNullOrEmpty(slug)) return "/";
        if (EsSlugs.TryGetValue(slug, out var dest)) return dest;
        var asPath = "/" + slug;
        var alias = ResolveAlias(asPath);
        return alias ?? asPath;
    }

    public static IReadOnlyList<SeoFaq> Faqs(string path, bool english)
    {
        path = SiteLocale.Normalize(SiteLocale.StripEnglishPrefix(path));
        if (string.Equals(path, "/faq", StringComparison.OrdinalIgnoreCase))
        {
            return english
                ? HubFaqs.Select(f => new SeoFaq(f.QEn, f.AEn)).ToArray()
                : HubFaqs.Select(f => new SeoFaq(f.QEs, f.AEs)).ToArray();
        }

        var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { path };
        if (path.StartsWith("/areas/", StringComparison.OrdinalIgnoreCase) && path.Length > 7)
            keys.Add("/areas");
        if (string.Equals(path, "/service-area", StringComparison.OrdinalIgnoreCase))
            keys.Add("/areas");
        var rows = AllFaqs.Where(f => keys.Contains(f.Path)).ToList();
        return english
            ? rows.Select(f => new SeoFaq(f.QEn, f.AEn)).ToArray()
            : rows.Select(f => new SeoFaq(f.QEs, f.AEs)).ToArray();
    }

    public static IReadOnlyList<FaqRow> HubFaqs => AllFaqs
        .Where(f => f.Path != "/qualify")
        .GroupBy(f => f.QEn)
        .Select(g => g.First())
        .ToArray();

    public static readonly string[] HubBucketOrder =
        ["Cost", "How-to", "Comparison", "Local", "Problems", "Policy"];

    public static string HubBucket(string category) => category switch
    {
        "Financing" or "NC assistance" or "FHA" or "VA" or "USDA" or "Costs" or "Budget" or "Credit" or "Closing" or "Cost"
            => "Cost",
        "First-time buyer" or "Preapproval" or "Inspection" or "Offers" or "Seller" or "New construction"
            or "Relocation" or "ITIN" or "How-to" => "How-to",
        "Investment" or "Agent" or "Comparison" => "Comparison",
        "Market" or "Local" => "Local",
        "Problems" => "Problems",
        "Buyer agreement" or "Open house" or "Fair housing" or "Security" or "NC process" or "Policy"
            => "Policy",
        _ => "How-to"
    };

    public static string HubCategoryLabel(string bucket, bool english) => bucket switch
    {
        "Cost" => english ? "Cost and money" : "Costo y dinero",
        "How-to" => english ? "How to buy or sell" : "Cómo comprar o vender",
        "Comparison" => english ? "Comparisons" : "Comparaciones",
        "Local" => english ? "Charlotte and nearby" : "Charlotte y alrededores",
        "Problems" => english ? "Problems and what to do" : "Problemas y qué hacer",
        "Policy" => english ? "Agreements, rights and safety" : "Acuerdos, derechos y seguridad",
        _ => bucket
    };

    public static string GuideCta(string path, bool english) => path switch
    {
        "/" => english ? "About Manuela Ruiz →" : "Conoce a Manuela Ruiz →",
        "/buyers" => english ? "Buyer guide →" : "Guía de compradores →",
        "/sellers" => english ? "Seller guide →" : "Guía de vendedores →",
        "/first-time-buyers" => english ? "First-time buyer guide →" : "Guía de primera vivienda →",
        "/programs" => english ? "Down-payment programs →" : "Programas de enganche →",
        "/itin-home-buyers" => english ? "Buying with an ITIN →" : "Comprar con ITIN →",
        "/home-valuation" => english ? "Home valuation →" : "Valor de la casa →",
        "/new-construction" => english ? "New construction guide →" : "Guía de casa nueva →",
        "/relocation" => english ? "Relocation guide →" : "Guía de mudanza →",
        "/investors" => english ? "Investor guide →" : "Guía de inversionistas →",
        "/areas" or "/service-area" => english ? "Cities and service area →" : "Ciudades y área de servicio →",
        "/search" => english ? "Custom home search →" : "Búsqueda personalizada →",
        "/qualify" => english ? "Buying-power estimator →" : "Estimador de poder de compra →",
        "/partners" => english ? "Local specialists →" : "Especialistas locales →",
        _ => ""
    };

    public sealed record FaqRow(string Path, string Category, string QEn, string QEs, string AEn, string AEs);

    private static readonly FaqRow[] AllFaqs =
    [        new("/buyers", "Agent", "Do I need a real estate agent to buy a house?", "¿Necesito un agente para comprar una casa?",
            "You can buy without an agent, but a buyer representative can help with property searches, tours, offers, inspections, negotiations and closing coordination. Review the written representation agreement and compensation terms before touring homes.",
            "Puede comprar sin agente, pero un representante del comprador puede ayudar con búsquedas, visitas, ofertas, inspecciones, negociaciones y coordinación del cierre. Revise el acuerdo escrito de representación y los términos de compensación antes de visitar propiedades."),
        new("/faq", "Agent", "What is the difference between a Realtor and a real estate agent?", "¿Cuál es la diferencia entre Realtor y agente de bienes raíces?",
            "A real estate agent is licensed by the state. REALTOR® is a membership mark used by members of the National Association of REALTORS®. Verify the professional's active license and the services offered.",
            "Un agente de bienes raíces tiene licencia estatal. REALTOR® es una marca de membresía utilizada por miembros de la National Association of REALTORS®. Verifique la licencia activa y los servicios ofrecidos."),
        new("/faq", "Buyer agreement", "Why am I being asked to sign a buyer agreement?", "¿Por qué me piden firmar un acuerdo de comprador?",
            "Written buyer agreements define the services, duration and compensation terms. Many MLS participants require an agreement before an in-person or live virtual tour. Terms are negotiable; read and understand them before signing.",
            "Los acuerdos escritos definen los servicios, duración y compensación. Muchos participantes de MLS requieren un acuerdo antes de una visita presencial o virtual en vivo. Los términos son negociables; léalos y entiéndalos antes de firmar."),
        new("/first-time-buyers", "First-time buyer", "What should I do first when buying a home?", "¿Qué debo hacer primero para comprar una casa?",
            "Start with a realistic budget, review your credit and speak with qualified lenders about preapproval. Then choose an agent, define your needs and begin the property search.",
            "Comience con un presupuesto realista, revise su crédito y hable con prestamistas calificados sobre preaprobación. Después elija agente, defina sus necesidades y comience la búsqueda."),
        new("/programs", "Financing", "How much down payment do I need?", "¿Cuánto necesito de enganche?",
            "The amount depends on the loan and your qualifications. Some conventional programs allow as little as 3%, FHA may allow 3.5%, and qualifying VA or USDA buyers may have no-down-payment options. A lender must confirm eligibility and total cash to close.",
            "La cantidad depende del préstamo y sus requisitos. Algunos programas convencionales permiten desde 3%, FHA puede permitir 3.5% y algunos compradores que califican para VA o USDA pueden tener opciones sin enganche. Un prestamista debe confirmar elegibilidad y efectivo total para el cierre."),
        new("/programs", "NC assistance", "What down payment assistance is available in North Carolina?", "¿Qué ayuda para enganche existe en North Carolina?",
            "NCHFA offers programs that may include assistance up to 3% of the loan amount and a separate $15,000 option for qualifying first-time buyers and military veterans. Program rules, limits and availability can change, so verify with NCHFA and a participating lender.",
            "NCHFA ofrece programas que pueden incluir ayuda de hasta 3% del préstamo y una opción separada de $15,000 para ciertos primeros compradores y veteranos. Las reglas, límites y disponibilidad pueden cambiar; verifique con NCHFA y un prestamista participante."),
        new("/programs", "FHA", "Can I buy a house with an FHA loan?", "¿Puedo comprar una casa con préstamo FHA?",
            "FHA-insured loans can be an option for qualified buyers and may allow a down payment as low as 3.5%. The lender evaluates credit, income, debt and the property.",
            "Los préstamos asegurados por FHA pueden ser opción para compradores que califican y pueden permitir un enganche desde 3.5%. El prestamista evalúa crédito, ingresos, deudas y la propiedad."),
        new("/programs", "VA", "Can a veteran buy with no down payment?", "¿Un veterano puede comprar sin enganche?",
            "Many VA-backed purchase loans close with no down payment, but eligibility, entitlement, credit, income, occupancy and lender requirements still apply. Obtain a Certificate of Eligibility and lender preapproval.",
            "Muchos préstamos de compra respaldados por VA cierran sin enganche, pero aplican requisitos de elegibilidad, beneficio, crédito, ingresos, ocupación y prestamista. Obtenga el Certificate of Eligibility y preaprobación."),
        new("/programs", "USDA", "What homes qualify for a USDA loan?", "¿Qué casas califican para préstamo USDA?",
            "USDA programs focus on eligible rural areas and qualified households. The property location, household income, occupancy and lender rules determine eligibility; verify the exact address with USDA or an approved lender.",
            "Los programas USDA se enfocan en áreas rurales elegibles y hogares que califican. La ubicación, ingresos del hogar, ocupación y reglas del prestamista determinan elegibilidad; verifique la dirección con USDA o prestamista aprobado."),
        new("/itin-home-buyers", "ITIN", "Can I buy a house with an ITIN?", "¿Puedo comprar una casa con ITIN?",
            "Property ownership is different from mortgage approval. Some lenders offer ITIN or alternative-documentation loans, but down payment, income, reserves, pricing and documentation vary. A licensed lender should evaluate your specific case.",
            "Ser dueño de propiedad es distinto de obtener una hipoteca. Algunos prestamistas ofrecen préstamos con ITIN o documentación alternativa, pero el enganche, ingresos, reservas, precio y documentos varían. Un prestamista con licencia debe evaluar su caso."),
        new("/first-time-buyers", "Preapproval", "Should I get preapproved before touring homes?", "¿Debo obtener preaprobación antes de visitar casas?",
            "Preapproval helps define a realistic price range and can strengthen an offer. It is not final loan approval and may expire or change if your finances or market conditions change.",
            "La preaprobación ayuda a definir un precio realista y puede fortalecer una oferta. No es aprobación final y puede vencer o cambiar si cambian sus finanzas o el mercado."),
        new("/first-time-buyers", "Costs", "What are closing costs when buying a home?", "¿Qué son los costos de cierre al comprar casa?",
            "Closing costs can include lender charges, appraisal, title services, taxes, insurance, prepaid items and other settlement costs. Compare the Loan Estimate and Closing Disclosure and ask about every change.",
            "Los costos pueden incluir cargos del prestamista, appraisal, título, impuestos, seguros, pagos anticipados y otros costos. Compare el Loan Estimate y Closing Disclosure y pregunte por cada cambio."),
        new("/faq", "NC process", "What is due diligence in North Carolina?", "¿Qué es due diligence en North Carolina?",
            "North Carolina contracts can include a negotiated due diligence period and fee. The exact rights, deadlines and refundability depend on the signed contract. Ask your agent and closing attorney to explain the current form before signing.",
            "Los contratos de North Carolina pueden incluir un periodo y tarifa de due diligence negociados. Los derechos, fechas y reembolsos dependen del contrato firmado. Pida a su agente y abogado de cierre que expliquen el formulario vigente antes de firmar."),
        new("/buyers", "Inspection", "Should I get a home inspection?", "¿Debo hacer una inspección de la casa?",
            "An inspection can identify visible conditions and help you make informed decisions, but it is not a warranty and may not find every problem. Review scope, findings, deadlines and specialist recommendations.",
            "Una inspección puede identificar condiciones visibles y ayudar a decidir, pero no es garantía y puede no encontrar todos los problemas. Revise alcance, hallazgos, fechas y recomendaciones de especialistas."),
        new("/buyers", "Appraisal", "What happens if the appraisal is low?", "¿Qué pasa si el appraisal sale bajo?",
            "Options can include renegotiating, challenging factual errors, changing financing terms, paying a difference or using contract rights. The best path depends on the contract, lender and available funds.",
            "Las opciones pueden incluir renegociar, corregir errores, cambiar financiamiento, pagar una diferencia o usar derechos del contrato. La mejor opción depende del contrato, prestamista y fondos disponibles."),
        new("/home-valuation", "Seller", "How do I know what my house is worth?", "¿Cómo sé cuánto vale mi casa?",
            "A comparative market analysis reviews recent comparable sales, active competition, condition, location and market timing. It is a pricing opinion, not an appraisal or guarantee of the final sale price.",
            "Un análisis comparativo revisa ventas similares, competencia activa, condición, ubicación y momento del mercado. Es una opinión de precio, no un appraisal ni garantía del precio final."),
        new("/sellers", "Seller", "What should I repair before selling?", "¿Qué debo reparar antes de vender?",
            "Prioritize safety, active leaks, obvious defects and items likely to affect financing or inspections. The right plan depends on expected return, timing and whether selling as-is is more practical.",
            "Priorice seguridad, fugas activas, defectos visibles y asuntos que puedan afectar financiamiento o inspección. El plan depende del retorno, tiempo y si conviene vender como está."),
        new("/sellers", "Seller", "How long does it take to sell a home?", "¿Cuánto tarda vender una casa?",
            "Timing depends on price, condition, location, competition, marketing, buyer financing and closing. Use current local data rather than a national average to set expectations.",
            "El tiempo depende de precio, condición, ubicación, competencia, marketing, financiamiento y cierre. Use datos locales actuales en vez de un promedio nacional."),
        new("/sellers", "Seller", "How much does it cost to sell a house?", "¿Cuánto cuesta vender una casa?",
            "Possible costs include negotiated brokerage compensation, seller concessions, repairs, title or attorney charges, taxes, mortgage payoff and moving expenses. Request a seller net sheet based on your property and proposed offer.",
            "Los costos pueden incluir compensación de corretaje negociada, concesiones, reparaciones, título o abogado, impuestos, pago de hipoteca y mudanza. Pida una hoja de dinero neto basada en su propiedad y oferta."),
        new("/new-construction", "New construction", "Do I need my own agent when buying from a builder?", "¿Necesito mi propio agente al comprar con constructor?",
            "The builder's sales representative works for the builder. Your own agent can help compare communities, review incentives, coordinate inspections and negotiate within the builder's process. Confirm representation terms before visiting or registering.",
            "El representante de ventas trabaja para el constructor. Su agente puede comparar comunidades, revisar incentivos, coordinar inspecciones y negociar dentro del proceso. Confirme representación antes de visitar o registrarse."),
        new("/relocation", "Relocation", "Can I buy a home remotely?", "¿Puedo comprar una casa a distancia?",
            "Many steps can be handled remotely through video tours, electronic documents and coordinated inspections, but lender, attorney, notarization and closing requirements vary. Build a written timeline with your team.",
            "Muchos pasos se pueden manejar a distancia con recorridos en video, documentos electrónicos e inspecciones coordinadas, pero los requisitos varían. Haga un calendario escrito con su equipo."),
        new("/faq", "Fair housing", "What are my fair housing rights?", "¿Cuáles son mis derechos de vivienda justa?",
            "Federal law prohibits housing discrimination based on race, color, national origin, religion, sex, familial status and disability. Other state or local protections may also apply. HUD explains rights and complaint options.",
            "La ley federal prohíbe discriminación por raza, color, origen nacional, religión, sexo, estado familiar y discapacidad. También pueden aplicar protecciones estatales o locales. HUD explica derechos y opciones de queja."),
        new("/", "Agent", "How do I choose the right Realtor?", "¿Cómo elijo al Realtor correcto?",
            "Interview agents about local experience, communication, availability, strategy, representation, compensation and references. Verify the active state license and request a clear written explanation of services.",
            "Entreviste agentes sobre experiencia local, comunicación, disponibilidad, estrategia, representación, compensación y referencias. Verifique la licencia estatal activa y pida una explicación escrita de los servicios."),
        new("/faq", "Agent", "Can the same firm represent the buyer and seller?", "¿La misma firma puede representar al comprador y vendedor?",
            "Dual or designated agency may be permitted when properly disclosed and agreed to in writing, subject to state law and brokerage policy. Ask what information the agent can and cannot share before consenting.",
            "La agencia dual o designada puede permitirse cuando se divulga y acepta por escrito, sujeto a la ley estatal y política de la firma. Pregunte qué información puede o no compartir el agente antes de aceptar."),
        new("/faq", "Agent", "Can I cancel a buyer agency agreement?", "¿Puedo cancelar un acuerdo con agente de comprador?",
            "The agreement should state its duration and termination terms. The client and brokerage may mutually agree to change or end it. Read the contract and communicate in writing before working with another agent.",
            "El acuerdo debe indicar duración y condiciones de terminación. Cliente y firma pueden acordar cambiarlo o terminarlo. Lea el contrato y comuníquese por escrito antes de trabajar con otro agente."),
        new("/faq", "Open house", "Do I need a written buyer agreement to attend an open house?", "¿Necesito acuerdo de comprador para asistir a un open house?",
            "NAR guidance distinguishes simply attending an open house on your own from working with an MLS participant to tour a home. If you request representation or a private tour, an agreement may be required. Ask before the visit.",
            "La guía de NAR distingue entre asistir por cuenta propia a un open house y trabajar con un participante de MLS para visitar una casa. Si pide representación o visita privada, puede requerirse acuerdo. Pregunte antes."),
        new("/first-time-buyers", "Budget", "How much house can I afford?", "¿Cuánta casa puedo pagar?",
            "Affordability depends on income, debts, down payment, interest rate, taxes, insurance, HOA and reserves. Use a complete monthly budget and compare official Loan Estimates; a lender determines approval.",
            "La capacidad depende de ingresos, deudas, enganche, tasa, impuestos, seguro, HOA y reservas. Use un presupuesto mensual completo y compare Loan Estimates oficiales; el prestamista determina aprobación."),
        new("/first-time-buyers", "Credit", "What credit score do I need to buy a house?", "¿Qué puntaje de crédito necesito para comprar casa?",
            "There is no single score for every mortgage. Loan type, lender rules, debt, income, down payment and credit history all matter. Ask several licensed lenders for options without assuming approval.",
            "No existe un solo puntaje para todas las hipotecas. Importan el tipo de préstamo, reglas del prestamista, deudas, ingresos, enganche e historial. Consulte varios prestamistas con licencia sin asumir aprobación."),
        new("/buyers", "Offers", "How much earnest money should I offer?", "¿Cuánto earnest money debo ofrecer?",
            "Earnest money is negotiated and local practice varies. The amount should reflect the contract, competition and your risk tolerance. Understand deadlines, escrow handling and refund conditions before signing.",
            "El earnest money se negocia y la práctica local varía. La cantidad debe considerar contrato, competencia y tolerancia al riesgo. Entienda fechas, custodia y condiciones de reembolso antes de firmar."),
        new("/buyers", "Offers", "Should I waive the home inspection?", "¿Debo renunciar a la inspección de la casa?",
            "Waiving an inspection can create significant risk. Competitive strategies should be evaluated with your agent, lender and appropriate professionals; understand contract rights and possible defects before deciding.",
            "Renunciar a la inspección puede crear riesgo importante. Evalúe estrategias competitivas con su agente, prestamista y profesionales; entienda derechos del contrato y posibles defectos antes de decidir."),
        new("/programs", "Offers", "Can the seller pay my closing costs?", "¿El vendedor puede pagar mis costos de cierre?",
            "Seller concessions can be negotiated, but loan-program limits, appraisal and seller acceptance apply. Your lender should confirm the maximum allowed and how it affects cash to close.",
            "Las concesiones del vendedor pueden negociarse, pero aplican límites del préstamo, appraisal y aceptación del vendedor. El prestamista debe confirmar el máximo permitido y efecto en el efectivo para cierre."),
        new("/first-time-buyers", "Closing", "When do I receive the Closing Disclosure?", "¿Cuándo recibo el Closing Disclosure?",
            "For most covered mortgages, the lender must provide the Closing Disclosure at least three business days before closing. Compare it with the Loan Estimate and ask about unexpected changes immediately.",
            "Para la mayoría de hipotecas cubiertas, el prestamista debe entregar el Closing Disclosure al menos tres días hábiles antes del cierre. Compárelo con el Loan Estimate y pregunte de inmediato por cambios inesperados."),
        new("/first-time-buyers", "Closing", "What is cash to close?", "¿Qué significa cash to close?",
            "Cash to close is the amount you must bring or wire for the transaction after accounting for down payment, closing costs, credits, deposits and adjustments. Use the final Closing Disclosure and verified wiring instructions.",
            "Cash to close es la cantidad que debe llevar o transferir después de considerar enganche, costos, créditos, depósitos y ajustes. Use el Closing Disclosure final e instrucciones de transferencia verificadas."),
        new("/faq", "Security", "How do I avoid wire fraud at closing?", "¿Cómo evito fraude de transferencia en el cierre?",
            "Do not rely on emailed changes to wiring instructions. Independently call the closing attorney or settlement company using a verified number, confirm details verbally and report suspicious messages immediately.",
            "No confíe en cambios enviados por correo a instrucciones de transferencia. Llame independientemente al abogado o compañía de cierre usando un número verificado, confirme verbalmente y reporte mensajes sospechosos."),
        new("/sellers", "Seller", "Should I sell my house as-is?", "¿Debo vender mi casa como está?",
            "An as-is sale may reduce upfront work but can affect price, buyer pool, inspections and financing. Compare expected net proceeds and timeline for as-is versus targeted repairs before choosing.",
            "Vender como está puede reducir trabajo inicial, pero afectar precio, compradores, inspecciones y financiamiento. Compare dinero neto y tiempo contra hacer reparaciones específicas."),
        new("/sellers", "Seller", "Do I need professional photos to sell my house?", "¿Necesito fotos profesionales para vender mi casa?",
            "Strong photos help buyers understand the property online and support the listing launch. Photography should accurately represent the home and comply with MLS and brokerage rules.",
            "Buenas fotos ayudan a compradores a entender la propiedad en internet y apoyan el lanzamiento. Deben representar la casa con precisión y cumplir reglas de MLS y de la firma."),
        new("/home-valuation", "Seller", "What is a seller net sheet?", "¿Qué es una hoja de dinero neto del vendedor?",
            "A seller net sheet estimates proceeds after the expected sale price, mortgage payoff, negotiated compensation, concessions, taxes, closing charges and other costs. It is an estimate and should be updated for each offer.",
            "La hoja estima lo que queda después del precio, pago de hipoteca, compensación negociada, concesiones, impuestos, cargos de cierre y otros costos. Es un estimado y debe actualizarse con cada oferta."),
        new("/sellers", "Seller", "Can I sell a home with a tenant in it?", "¿Puedo vender una casa con inquilino?",
            "Often yes, but leases, notice requirements, access, deposits and buyer plans matter. Have the lease and applicable landlord-tenant obligations reviewed before marketing or promising vacancy.",
            "Con frecuencia sí, pero importan contrato de renta, avisos, acceso, depósitos y planes del comprador. Revise el contrato y obligaciones antes de anunciar o prometer desocupación."),
        new("/sellers", "Seller", "Can I sell an inherited home before probate is finished?", "¿Puedo vender una casa heredada antes de terminar probate?",
            "Authority to sell depends on title, estate administration, court orders and state law. Consult the estate attorney and closing attorney before listing or signing a contract.",
            "La autoridad para vender depende del título, administración de la sucesión, órdenes judiciales y ley estatal. Consulte al abogado de la sucesión y del cierre antes de anunciar o firmar."),
        new("/sellers", "Seller", "What happens if my listing expires?", "¿Qué pasa si vence mi contrato de venta?",
            "Review why the property did not sell: price, condition, access, marketing, feedback and market changes. Confirm any continuing obligations before hiring another brokerage, then create a new strategy.",
            "Revise por qué no se vendió: precio, condición, acceso, marketing, comentarios y cambios del mercado. Confirme obligaciones vigentes antes de contratar otra firma y cree una estrategia nueva."),
        new("/new-construction", "New construction", "Will using a Realtor cost more on a new construction home?", "¿Usar Realtor cuesta más al comprar casa nueva?",
            "Representation and compensation should be discussed in writing before visiting or registering with a builder. Builder policies and available compensation vary; do not assume it is free or that the price changes.",
            "La representación y compensación deben hablarse por escrito antes de visitar o registrarse con constructor. Las políticas y compensación varían; no asuma que es gratis ni que cambia el precio."),
        new("/relocation", "Relocation", "How early should I contact a relocation Realtor?", "¿Con cuánto tiempo debo contactar a un Realtor de reubicación?",
            "Contacting an agent several months before the move can help align financing, sale timing, travel, tours and closing. A shorter timeline is possible, but requires clear priorities and backup plans.",
            "Contactar varios meses antes puede coordinar financiamiento, venta, viajes, visitas y cierre. Un plazo corto es posible, pero requiere prioridades y planes alternativos claros."),
        new("/investors", "Investment", "What is cap rate?", "¿Qué es cap rate?",
            "Capitalization rate generally compares a property's annual net operating income with its price or value. It does not include every financing or tax effect and should be evaluated with cash flow, condition and risk.",
            "La tasa de capitalización compara generalmente el ingreso operativo neto anual con el precio o valor. No incluye todos los efectos de financiamiento o impuestos y debe evaluarse con flujo, condición y riesgo."),
        new("/investors", "Investment", "Can I use an FHA loan for a duplex?", "¿Puedo usar préstamo FHA para un dúplex?",
            "FHA can insure qualifying owner-occupied one-to-four-unit properties, subject to borrower, lender and property rules. The buyer must verify occupancy, appraisal and underwriting requirements with an FHA-approved lender.",
            "FHA puede asegurar propiedades elegibles de una a cuatro unidades ocupadas por el dueño, sujeto a reglas. Verifique ocupación, appraisal y requisitos con prestamista aprobado por FHA."),
        new("/areas", "Market", "Which North Carolina area near Charlotte fits my budget and commute?", "¿Qué área de North Carolina cerca de Charlotte se ajusta a mi presupuesto y traslado?",
            "Compare the total monthly cost, property taxes, commute, insurance, services and lifestyle for each specific property and community. County averages do not replace a property-level budget and local advice.",
            "Compare el costo mensual total, impuestos, traslado, seguro, servicios y estilo de vida para cada propiedad y comunidad. Los promedios por condado no sustituyen un presupuesto por propiedad."),
        new("/home-valuation", "Market", "Are online home-value estimates accurate?", "¿Son exactos los estimados de valor en internet?",
            "Automated estimates can be a starting point but may miss condition, renovations, lot differences and very recent local activity. Compare them with a current local market analysis and, when required, an appraisal.",
            "Los estimados automáticos son un punto de inicio, pero pueden omitir condición, mejoras, terreno y actividad reciente. Compárelos con análisis local actual y, cuando corresponda, appraisal."),
        new("/itin-home-buyers", "ITIN", "Are ITIN mortgage terms the same as conventional loans?", "¿Los préstamos con ITIN tienen los mismos términos que los convencionales?",
            "Not necessarily. Product availability, rates, fees, down payment, reserves and documentation can differ by lender and borrower profile. Compare written offers from licensed lenders and review total cost.",
            "No necesariamente. Disponibilidad, tasas, cargos, enganche, reservas y documentos pueden variar según prestamista y perfil. Compare ofertas escritas de prestamistas con licencia y el costo total."),
        new("/qualify", "Budget", "How much house can I afford?", "¿Cuánta casa puedo pagar?",
            "Affordability depends on income, debts, down payment, interest rate, taxes, insurance, HOA and reserves. Use a complete monthly budget and compare official Loan Estimates; a lender determines approval. Manuela's estimator is educational and does not pull credit or approve a loan.",
            "La capacidad depende de ingresos, deudas, enganche, tasa, impuestos, seguro, HOA y reservas. Use un presupuesto mensual completo y compare Loan Estimates oficiales; el prestamista determina aprobación. El estimador de Manuela es educativo: no revisa crédito ni aprueba un préstamo."),
        new("/service-area", "Market", "Which North Carolina area near Charlotte fits my budget and commute?", "¿Qué área de North Carolina cerca de Charlotte se ajusta a mi presupuesto y traslado?",
            "Compare the total monthly cost, property taxes, commute, insurance, services and lifestyle for each specific property and community. County averages do not replace a property-level budget and local advice.",
            "Compare el costo mensual total, impuestos, traslado, seguro, servicios y estilo de vida para cada propiedad y comunidad. Los promedios por condado no sustituyen un presupuesto por propiedad."),
        new("/", "Local", "Who is Manuela Ruiz?", "¿Quién es Manuela Ruiz?",
            "Manuela Ruiz is a bilingual North Carolina REALTOR, NC Broker #350810, with Guerra & Associates Realty LLC in Charlotte. She helps buyers, sellers, first-time buyers, ITIN conversations, relocation and new-construction clients in Spanish or English and does not pull credit or take loan applications.",
            "Manuela Ruiz es REALTOR bilingüe de North Carolina, NC Broker #350810, con Guerra & Associates Realty LLC en Charlotte. Ayuda a compradores, vendedores, primera vivienda, conversaciones ITIN, relocation y casa nueva en español o inglés, y no revisa crédito ni toma solicitudes de préstamo."),
        new("/", "Local", "Is there a bilingual realtor in Charlotte who speaks Spanish?", "¿Hay un realtor bilingüe en Charlotte que hable español?",
            "Yes. Manuela Ruiz works in Spanish and English from Charlotte, NC. Office: 6230 Fairview Rd Suite 105, Charlotte, NC 28210. Phone 704-830-8486. Email manuelaruiz.realty@gmail.com.",
            "Sí. Manuela Ruiz trabaja en español e inglés desde Charlotte, NC. Oficina: 6230 Fairview Rd Suite 105, Charlotte, NC 28210. Teléfono 704-830-8486. Email manuelaruiz.realty@gmail.com."),
        new("/", "Local", "How do I contact Manuela Ruiz?", "¿Cómo contacto a Manuela Ruiz?",
            "Call 704-830-8486, email manuelaruiz.realty@gmail.com, or complete the buyer or seller questionnaire on this site. The office is Guerra & Associates Realty LLC, 6230 Fairview Rd Suite 105, Charlotte, NC 28210.",
            "Llame al 704-830-8486, escriba a manuelaruiz.realty@gmail.com o complete el cuestionario de comprador o vendedor en este sitio. La oficina es Guerra & Associates Realty LLC, 6230 Fairview Rd Suite 105, Charlotte, NC 28210."),
        new("/service-area", "Local", "What cities near Charlotte does Manuela Ruiz serve?", "¿Qué ciudades cerca de Charlotte atiende Manuela Ruiz?",
            "Manuela works from Charlotte and evaluates North Carolina cities such as Concord, Gastonia, Huntersville, Matthews and Statesville, about a two-hour practical radius. She confirms inventory, commute and representation before scheduling.",
            "Manuela trabaja desde Charlotte y evalúa ciudades de North Carolina como Concord, Gastonia, Huntersville, Matthews y Statesville, en un radio práctico de unas dos horas. Confirma inventario, traslado y representación antes de agendar."),
        new("/service-area", "Local", "Does Manuela Ruiz work in South Carolina?", "¿Manuela Ruiz trabaja en South Carolina?",
            "Rock Hill, Fort Mill and other South Carolina locations are coordinated with a professional licensed in South Carolina. Manuela does not treat an SC ZIP as automatic North Carolina coverage.",
            "Rock Hill, Fort Mill y otras ubicaciones de South Carolina se coordinan con un profesional con licencia en South Carolina. Manuela no trata un ZIP de SC como cobertura automática de North Carolina."),
        new("/search", "How-to", "How do I search homes for sale near Charlotte?", "¿Cómo busco casas en venta cerca de Charlotte?",
            "Tell Manuela the city or ZIP, price range, beds, baths and must-haves. She reviews current inventory for a bilingual search; this site does not send you to a third-party listing portal.",
            "Dile a Manuela la ciudad o ZIP, rango de precio, habitaciones, baños e indispensables. Ella revisa inventario actual para una búsqueda bilingüe; este sitio no te envía a un portal de terceros."),
        new("/", "How-to", "How do I start working with Manuela Ruiz?", "¿Cómo empiezo a trabajar con Manuela Ruiz?",
            "Complete the buyer or seller questionnaire on this site, or call 704-830-8486. The form is a real-estate consultation, not a loan application, and it does not pull credit.",
            "Complete el cuestionario de comprador o vendedor en este sitio, o llame al 704-830-8486. El formulario es una consulta inmobiliaria, no una solicitud de préstamo, y no revisa crédito."),
        new("/faq", "Policy", "Does Manuela Ruiz pull credit or take loan applications?", "¿Manuela Ruiz revisa crédito o toma solicitudes de préstamo?",
            "No. Manuela does not pull credit or take loan applications. A licensed lender evaluates financing; she organizes the real-estate process in Spanish or English.",
            "No. Manuela no revisa crédito ni toma solicitudes de préstamo. Un prestamista con licencia evalúa el financiamiento; ella organiza el proceso inmobiliario en español o inglés."),
        new("/sellers", "Cost", "How much are realtor commissions in North Carolina?", "¿Cuánto son las comisiones de realtor en North Carolina?",
            "There is no fixed commission rate. Brokerage compensation is negotiated in writing and can vary by listing, buyer representation and the specific offer. Ask for a seller net sheet or a buyer-agreement explanation before you sign.",
            "No hay una tarifa fija. La compensación de corretaje se negocia por escrito y puede variar según el listing, la representación del comprador y la oferta. Pida una hoja de dinero neto o la explicación del acuerdo de comprador antes de firmar."),
        new("/faq", "Cost", "How much is the due diligence fee in North Carolina?", "¿Cuánto es la tarifa de due diligence en North Carolina?",
            "There is no statewide fixed amount. The due diligence fee and period are negotiated in the contract. Ask your agent and closing attorney to explain refundability and deadlines on the current North Carolina form before you sign.",
            "No hay un monto fijo estatal. La tarifa y el periodo de due diligence se negocian en el contrato. Pida a su agente y abogado de cierre que expliquen reembolsos y fechas en el formulario vigente de North Carolina antes de firmar."),
        new("/home-valuation", "Cost", "Are there public prices for Manuela Ruiz's services?", "¿Hay precios públicos de los servicios de Manuela Ruiz?",
            "This site does not publish a fee menu. Compensation is discussed in a written listing or buyer agreement. Online home-value widgets are not her fee and are not a sale-price guarantee.",
            "Este sitio no publica un menú de honorarios. La compensación se habla en un acuerdo escrito de listing o de comprador. Los widgets de valor en internet no son su tarifa ni una garantía de precio de venta."),
        new("/buyers", "Problems", "What should I do if the home inspection finds problems?", "¿Qué debo hacer si la inspección encuentra problemas?",
            "Review the report, deadlines and contract rights with your agent before the due-diligence period ends. Options can include requesting repairs, a credit, more investigation, or walking away if the contract allows. An inspection is not a warranty.",
            "Revise el informe, las fechas y los derechos del contrato con su agente antes de que termine el due diligence. Las opciones pueden incluir pedir reparaciones, un crédito, más investigación o retirarse si el contrato lo permite. La inspección no es una garantía."),
        new("/buyers", "Problems", "What should I do if my offer is not accepted?", "¿Qué debo hacer si no aceptan mi oferta?",
            "Ask why it lost: price, dates, due diligence, financing or competition. Then decide whether to improve terms, wait, or search again. Manuela compares the next inventory against your written budget, not against the rejected house alone.",
            "Pregunte por qué perdió: precio, fechas, due diligence, financiamiento o competencia. Luego decida si mejorar términos, esperar o buscar de nuevo. Manuela compara el siguiente inventario con su presupuesto escrito, no solo con la casa rechazada."),
        new("/buyers", "Problems", "What should I do if my loan is denied after I am under contract?", "¿Qué debo hacer si niegan el préstamo después de estar bajo contrato?",
            "Read the financing and due-diligence terms in the signed contract immediately and tell your agent and lender. Options depend on those dates, the denial reason and whether another licensed lender can still perform. Manuela does not reverse a lender decision.",
            "Lea de inmediato las cláusulas de financiamiento y due diligence del contrato firmado y avise a su agente y prestamista. Las opciones dependen de esas fechas, del motivo de la negación y de si otro prestamista con licencia aún puede cumplir. Manuela no revierte la decisión de un lender."),
        new("/new-construction", "Comparison", "Should I buy new construction or a resale home in Charlotte?", "¿Debo comprar casa nueva o de reventa en Charlotte?",
            "Neither is automatically better. New construction can offer warranties and incentives; resale can offer location, lot and a known neighborhood. Compare total monthly cost, timeline, inspections and who the sales representative works for.",
            "Ninguna es automáticamente mejor. La casa nueva puede ofrecer garantías e incentivos; la reventa puede ofrecer ubicación, lote y vecindario conocido. Compare el costo mensual total, plazos, inspecciones y para quién trabaja el representante de ventas."),
        new("/programs", "Comparison", "Which is better, FHA or conventional, in Charlotte?", "¿Qué es mejor en Charlotte, FHA o convencional?",
            "It depends on credit, down payment, debt, occupancy and the property. FHA can allow a lower down payment for some buyers; conventional can have different mortgage-insurance rules. Only a licensed lender can say which product you may use.",
            "Depende del crédito, enganche, deudas, ocupación y la propiedad. FHA puede permitir menos enganche para algunos compradores; el convencional puede tener otras reglas de seguro hipotecario. Solo un prestamista con licencia puede decir qué producto puede usar."),
        new("/first-time-buyers", "Comparison", "Should I buy or keep renting in Charlotte?", "¿Debo comprar o seguir rentando en Charlotte?",
            "Buying is not automatically cheaper than renting. Compare your cash to close, monthly payment with taxes, insurance and HOA, how long you expect to stay, and whether a licensed lender would approve a loan. Manuela can map the housing steps; she does not give tax or investment advice.",
            "Comprar no es automáticamente más barato que rentar. Compare el efectivo para cerrar, el pago mensual con impuestos, seguro y HOA, cuánto tiempo planea quedarse y si un prestamista con licencia aprobaría un préstamo. Manuela puede mapear los pasos inmobiliarios; no da asesoría fiscal ni de inversión.")
    ];
}