namespace CarolinasCoverage.Site;

public static class SeoHubCatalog
{
    private static readonly Dictionary<string, string> ProductMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["auto"] = "auto",
        ["home"] = "home-insurance",
        ["renters"] = "renters",
        ["pet"] = "pet",
        ["business"] = "business",
        ["commercial-auto"] = "commercial-auto",
        ["general-liability"] = "general-liability",
        ["workers-comp"] = "workers-comp",
        ["life"] = "life"
    };

    public static IReadOnlyList<HubDef> All { get; } = Build();

    public static bool TryGet(string id, out HubDef hub)
    {
        hub = All.FirstOrDefault(item => item.Id.Equals(id, StringComparison.OrdinalIgnoreCase))!;
        return hub is not null;
    }

    public static bool TryProduct(string productId, out HubDef hub)
    {
        hub = null!;
        return ProductMap.TryGetValue(productId, out var id) && TryGet(id, out hub);
    }

    public static bool HasProduct(string productId) => ProductMap.ContainsKey(productId);

    public static bool IsUnconfirmedSpecialty(string productId) =>
        productId is "umbrella" or "excess-liability" or "professional-liability" or "cyber-liability";

    public static bool TryGetByPath(string path, out HubDef hub, out bool english)
    {
        path = Lang.Normalize(path);
        foreach (var item in All)
        {
            if (path.Equals(item.SlugEn, StringComparison.OrdinalIgnoreCase))
            {
                hub = item;
                english = true;
                return true;
            }

            if (path.Equals(item.SlugEs, StringComparison.OrdinalIgnoreCase))
            {
                hub = item;
                english = false;
                return true;
            }
        }

        hub = null!;
        english = true;
        return false;
    }

    private static IReadOnlyList<HubDef> Build() =>
    [
        new()
        {
            Id = "spanish-agent",
            SlugEn = "/spanish-speaking-insurance-agent",
            SlugEs = "/es/agente-seguros",
            Icon = "💬",
            EyebrowEn = "Bilingual service",
            EyebrowEs = "Atención bilingüe",
            H1En = "Spanish-Speaking Insurance Agent",
            H1Es = "Agente de Seguros que Habla Español",
            TitleEn = "Spanish-Speaking Insurance Agent | Carolinas Coverage",
            TitleEs = "Agente de seguros en español | Carolinas Coverage",
            DescEn = "Bilingual insurance help in North Carolina and South Carolina. Auto, home, business, pet and life. Call 704-595-3429 to confirm Spanish availability.",
            DescEs = "Ayuda bilingüe de seguros en Carolina del Norte y Carolina del Sur. Auto, casa, negocio, mascotas y vida. Llame al 704-595-3429 para confirmar disponibilidad en español.",
            IntroEn = "Carolinas Coverage Insurance Agency offers bilingual contact and quote paths for people and businesses in North Carolina and South Carolina. Confirm staff availability and hours before expecting immediate Spanish-language service.",
            IntroEs = "Carolinas Coverage Insurance Agency ofrece contacto y cotización bilingüe para personas y negocios en Carolina del Norte y Carolina del Sur. Confirme disponibilidad y horario del personal antes de esperar atención inmediata en español.",
            CtaEn = "Talk with an agent",
            CtaEs = "Hablar con una agente",
            FaqKey = "spanish-agent",
            Schema = "InsuranceAgency",
            RelatedIds = ["north-carolina", "south-carolina", "contact"],
            Sections =
            [
                new("What we can discuss in Spanish", "Qué podemos conversar en español",
                    "Confirmed lines include auto, home and property, business liability, pet and life insurance. Health insurance, Medicare and disability insurance are not offered.",
                    "Las líneas confirmadas incluyen auto, casa y propiedad, responsabilidad de negocio, mascotas y vida. No se ofrece seguro de salud, Medicare ni disability."),
                new("How a bilingual quote works", "Cómo funciona una cotización bilingüe",
                    "Share the product, state, and details the worksheet asks for. An agent reviews available options after appointments and eligibility are confirmed. Price is never guaranteed as the lowest.",
                    "Comparta el producto, el estado y los datos que pide la hoja. Un agente revisa opciones disponibles después de confirmar nombramientos y elegibilidad. El precio nunca se garantiza como el más bajo.")
            ],
            NoteEn = "Do not promise same-day service or that a Spanish-speaking agent is always on the line until hours and staffing are verified.",
            NoteEs = "No se promete servicio el mismo día ni que siempre haya un agente en español en línea hasta verificar horario y personal."
        },
        new()
        {
            Id = "north-carolina",
            SlugEn = "/north-carolina",
            SlugEs = "/es/carolina-del-norte",
            Icon = "NC",
            EyebrowEn = "North Carolina",
            EyebrowEs = "Carolina del Norte",
            H1En = "North Carolina Insurance Agency",
            H1Es = "Agencia de Seguros en Carolina del Norte",
            TitleEn = "North Carolina Insurance Agency | Carolinas Coverage",
            TitleEs = "Agencia de seguros Carolina del Norte | Carolinas Coverage",
            DescEn = "Insurance agency helping North Carolina with auto, home, business, pet and life. Office in Charlotte. Licensed service; confirm product availability. Call 704-595-3429.",
            DescEs = "Agencia de seguros en Carolina del Norte: auto, casa, negocio, mascotas y vida. Oficina en Charlotte. Confirme disponibilidad. Llame al 704-595-3429.",
            IntroEn = "Carolinas Coverage Insurance Agency is based in Charlotte and helps North Carolina applicants review auto, home, renters, pet, business and life insurance. North Carolina rules are not the same as South Carolina rules.",
            IntroEs = "Carolinas Coverage Insurance Agency tiene oficina en Charlotte y ayuda a solicitantes de Carolina del Norte con auto, casa, inquilinos, mascotas, negocio y vida. Las reglas de Carolina del Norte no son las mismas que las de Carolina del Sur.",
            CtaEn = "Request an NC quote",
            CtaEs = "Solicitar cotización de NC",
            FaqKey = "north-carolina",
            Schema = "InsuranceAgency",
            RelatedIds = ["auto", "dl-123", "home-insurance", "workers-comp", "south-carolina"],
            Sections =
            [
                new("North Carolina auto liability minimums", "Límites mínimos de auto en Carolina del Norte",
                    "For new or renewed policies effective on or after July 1, 2025, the NC Department of Insurance states minimum auto liability limits of 50/100/50. Verify the current rule and effective date before relying on this figure.",
                    "Para pólizas nuevas o renovadas vigentes a partir del 1 de julio de 2025, el Departamento de Seguros de NC indica límites mínimos de auto 50/100/50. Verifique la regla y fecha de vigencia actuales antes de usar esta cifra."),
                new("DL-123 and license transactions", "DL-123 y trámites de licencia",
                    "A DL-123 is one form North Carolina may accept as proof of liability insurance for certain driver-license transactions. Confirm with NCDMV and the insurer which proof applies. This is a North Carolina topic; do not mix it with South Carolina road-test rules.",
                    "Un DL-123 es una forma que Carolina del Norte puede aceptar como comprobante de responsabilidad civil para ciertos trámites de licencia. Confirme con NCDMV y la aseguradora. Este tema es de Carolina del Norte; no lo mezcle con las reglas de examen de Carolina del Sur."),
                new("Workers’ compensation in North Carolina", "Compensación laboral en Carolina del Norte",
                    "North Carolina generally requires workers’ compensation for businesses with three or more employees, subject to statutory exceptions and classification questions.",
                    "Carolina del Norte generalmente exige compensación laboral para negocios con tres o más empleados, sujeto a excepciones legales y preguntas de clasificación.")
            ],
            NoteEn = "License numbers and NPN should be added after the agency supplies official evidence.",
            NoteEs = "Los números de licencia y NPN deben añadirse cuando la agencia entregue evidencia oficial."
        },
        new()
        {
            Id = "south-carolina",
            SlugEn = "/south-carolina",
            SlugEs = "/es/carolina-del-sur",
            Icon = "SC",
            EyebrowEn = "South Carolina",
            EyebrowEs = "Carolina del Sur",
            H1En = "South Carolina Insurance Agency",
            H1Es = "Agencia de Seguros en Carolina del Sur",
            TitleEn = "South Carolina Insurance Agency | Carolinas Coverage",
            TitleEs = "Agencia de seguros Carolina del Sur | Carolinas Coverage",
            DescEn = "Insurance help in South Carolina for auto, home, business, pet and life. Charlotte office serving SC clients. Confirm product and carrier availability. Call 704-595-3429.",
            DescEs = "Seguros en Carolina del Sur: auto, casa, negocio, mascotas y vida. Oficina en Charlotte. Confirme producto y compañía. Llame al 704-595-3429.",
            IntroEn = "The agency helps South Carolina applicants from its Charlotte office. South Carolina auto, road-test, and workers’ compensation rules differ from North Carolina and are described separately.",
            IntroEs = "La agencia ayuda a solicitantes de Carolina del Sur desde su oficina en Charlotte. Las reglas de auto, examen de manejo y compensación laboral de Carolina del Sur son distintas a las de Carolina del Norte y se describen por separado.",
            CtaEn = "Request an SC quote",
            CtaEs = "Solicitar cotización de SC",
            FaqKey = "south-carolina",
            Schema = "InsuranceAgency",
            RelatedIds = ["auto", "home-insurance", "workers-comp", "north-carolina"],
            Sections =
            [
                new("South Carolina auto minimums", "Límites mínimos de auto en Carolina del Sur",
                    "South Carolina lists minimum liability and uninsured motorist limits of 25/50/25. Underinsured motorist coverage must be offered but is not required.",
                    "Carolina del Sur indica límites mínimos de responsabilidad civil y conductor sin seguro de 25/50/25. La cobertura para conductor con seguro insuficiente debe ofrecerse, pero no es obligatoria."),
                new("South Carolina road-test proof", "Comprobante para el examen de manejo en Carolina del Sur",
                    "SCDMV says the test vehicle needs a valid plate, registration and insurance card. Rental vehicles have additional contract requirements. Do not use North Carolina DL-123 rules for a South Carolina road test.",
                    "SCDMV indica que el vehículo del examen necesita placa válida, registro y tarjeta de seguro. Los vehículos alquilados tienen requisitos adicionales. No use las reglas de DL-123 de Carolina del Norte para un examen en Carolina del Sur."),
                new("Workers’ compensation in South Carolina", "Compensación laboral en Carolina del Sur",
                    "South Carolina generally requires workers’ compensation at four or more employees; part-time and family employees may count. Confirm exceptions with the Commission.",
                    "Carolina del Sur generalmente la exige con cuatro o más empleados; empleados de medio tiempo y familiares pueden contar. Confirme excepciones con la Comisión.")
            ],
            NoteEn = "License numbers and NPN should be added after the agency supplies official evidence.",
            NoteEs = "Los números de licencia y NPN deben añadirse cuando la agencia entregue evidencia oficial."
        },
        new()
        {
            Id = "auto",
            SlugEn = "/auto-insurance",
            SlugEs = "/es/seguro-auto",
            Icon = "🚗",
            EyebrowEn = "Auto insurance",
            EyebrowEs = "Seguro de auto",
            H1En = "Auto Insurance in North Carolina & South Carolina",
            H1Es = "Seguro de Auto en Carolina del Norte y Carolina del Sur",
            TitleEn = "Auto Insurance in NC & SC | Carolinas Coverage",
            TitleEs = "Seguro de auto NC y SC | Carolinas Coverage",
            DescEn = "Auto insurance quotes in North Carolina and South Carolina. Liability, full coverage, new drivers, non-owner and SR-22 help. Bilingual agency in Charlotte. Call 704-595-3429.",
            DescEs = "Cotización de seguro de auto en Carolina del Norte y Carolina del Sur. Responsabilidad, cobertura completa, conductores nuevos, no propietario y SR-22. Agencia bilingüe en Charlotte. 704-595-3429.",
            IntroEn = "Personal auto insurance is required liability coverage plus optional coverages that vary. North Carolina and South Carolina use different minimums. An agent reviews available options; price and acceptance are never guaranteed.",
            IntroEs = "El seguro de auto personal es responsabilidad civil requerida más coberturas opcionales que varían. Carolina del Norte y Carolina del Sur usan mínimos distintos. Un agente revisa opciones disponibles; precio y aceptación nunca están garantizados.",
            QuoteProduct = "auto",
            CtaEn = "Get an auto quote",
            CtaEs = "Cotizar seguro de auto",
            FaqKey = "auto",
            RelatedIds = ["liability-full", "dl-123", "non-owner", "sr-22", "new-drivers", "north-carolina", "south-carolina"],
            Sections =
            [
                new("What personal auto insurance may cover", "Qué puede cubrir el seguro de auto personal",
                    "Liability generally pays covered injury or property damage you cause to others, up to limits. Collision and comprehensive may pay for damage to your vehicle when selected. Uninsured and underinsured motorist rules differ by state.",
                    "La responsabilidad civil generalmente paga lesiones o daños cubiertos que usted causa a otros, hasta los límites. Colisión y comprensiva pueden pagar daños a su vehículo cuando se eligen. Las reglas de conductor sin seguro o con seguro insuficiente difieren por estado."),
                new("North Carolina versus South Carolina", "Carolina del Norte versus Carolina del Sur",
                    "North Carolina minimum liability for new or renewed policies on or after July 1, 2025 is stated as 50/100/50. South Carolina lists 25/50/25 for liability and uninsured motorist; underinsured motorist must be offered but is not required. Do not mix the two states.",
                    "El mínimo de Carolina del Norte para pólizas nuevas o renovadas desde el 1 de julio de 2025 se indica como 50/100/50. Carolina del Sur lista 25/50/25 para responsabilidad y conductor sin seguro; el conductor con seguro insuficiente debe ofrecerse, pero no es obligatorio. No mezcle los dos estados."),
                new("How much does auto insurance cost?", "¿Cuánto cuesta el seguro de auto?",
                    "Cost depends on underwriting, coverage, deductibles, location, driving history, vehicle and carrier. No quote on this site guarantees the lowest price.",
                    "El costo depende de suscripción, cobertura, deducibles, ubicación, historial, vehículo y compañía. Ninguna cotización en este sitio garantiza el precio más bajo.")
            ],
            NoteEn = "Coverage, availability, price and eligibility depend on the final policy, state and insurer approval.",
            NoteEs = "La cobertura, disponibilidad, precio y elegibilidad dependen de la póliza final, el estado y la aprobación de la aseguradora."
        },
        new()
        {
            Id = "liability-full",
            SlugEn = "/auto-insurance/liability-vs-full-coverage",
            SlugEs = "/es/seguro-auto/responsabilidad-vs-cobertura-completa",
            Icon = "🚗",
            ParentId = "auto",
            EyebrowEn = "Liability vs full coverage",
            EyebrowEs = "Responsabilidad vs cobertura completa",
            H1En = "Liability vs Full Coverage Auto Insurance",
            H1Es = "Responsabilidad Civil vs Cobertura Completa",
            TitleEn = "Liability vs Full Coverage Auto Insurance | Carolinas Coverage",
            TitleEs = "Responsabilidad vs cobertura completa | Carolinas Coverage",
            DescEn = "Compare liability-only and full coverage auto insurance in NC and SC. Full coverage is not a standard legal term. Request available options. Call 704-595-3429.",
            DescEs = "Compare responsabilidad civil y cobertura completa de auto en NC y SC. Cobertura completa no es un término legal único. 704-595-3429.",
            IntroEn = "Liability-only and “full coverage” are common search phrases. Liability is the required third-party protection. “Full coverage” usually means liability plus collision and comprehensive, but limits and deductibles still vary.",
            IntroEs = "Responsabilidad civil sola y “cobertura completa” son búsquedas comunes. La responsabilidad es la protección requerida frente a terceros. “Cobertura completa” suele significar responsabilidad más colisión y comprensiva, pero límites y deducibles varían.",
            QuoteProduct = "auto",
            CtaEn = "Compare options",
            CtaEs = "Comparar opciones",
            FaqKey = "liability-full",
            RelatedIds = ["auto", "non-owner", "new-drivers"],
            Sections =
            [
                new("What liability-only covers", "Qué cubre solo responsabilidad civil",
                    "It generally pays covered bodily injury or property damage you cause to others, up to policy limits. It does not usually pay to repair your own vehicle after a collision.",
                    "Generalmente paga lesiones corporales o daños materiales cubiertos que usted causa a otros, hasta los límites. Normalmente no paga la reparación de su propio vehículo después de una colisión."),
                new("What people mean by full coverage", "Qué suele significar cobertura completa",
                    "“Full coverage” is not a single standardized policy term. People commonly mean liability plus collision and comprehensive. Theft or hail may be considered under comprehensive when that coverage is in force, subject to the deductible and exclusions.",
                    "“Cobertura completa” no es un término único estandarizado. Normalmente significa responsabilidad más colisión y comprensiva. Robo o granizo pueden considerarse bajo comprensiva cuando esa cobertura está vigente, sujeto a deducible y exclusiones."),
                new("Financed or leased vehicles", "Vehículos financiados o arrendados",
                    "A lender or leasing company commonly requires collision and comprehensive. Check the contract for exact limits and deductibles.",
                    "Un prestamista o arrendador comúnmente exige colisión y comprensiva. Revise el contrato para límites y deducibles exactos.")
            ],
            NoteEn = "Do not treat “full coverage” as a guarantee that every loss is paid.",
            NoteEs = "No trate “cobertura completa” como garantía de que toda pérdida se paga."
        },
        new()
        {
            Id = "dl-123",
            SlugEn = "/north-carolina/dl-123",
            SlugEs = "/es/carolina-del-norte/dl-123",
            Icon = "📄",
            ParentId = "north-carolina",
            EyebrowEn = "North Carolina only",
            EyebrowEs = "Solo Carolina del Norte",
            H1En = "DL-123 Insurance Proof in North Carolina",
            H1Es = "Comprobante de Seguro DL-123 en Carolina del Norte",
            TitleEn = "DL-123 Insurance Proof in North Carolina | Carolinas Coverage",
            TitleEs = "Seguro DL-123 Carolina del Norte | Carolinas Coverage",
            DescEn = "DL-123 is one North Carolina proof of liability insurance for certain license transactions. Confirm with NCDMV. Call Carolinas Coverage at 704-595-3429.",
            DescEs = "El DL-123 es un comprobante de responsabilidad civil en Carolina del Norte para ciertos trámites de licencia. Confirme con NCDMV. 704-595-3429.",
            IntroEn = "A DL-123 is one form North Carolina accepts as proof of liability insurance for certain driver-license transactions. It is a North Carolina topic. South Carolina road tests use different vehicle proof rules.",
            IntroEs = "Un DL-123 es una forma que Carolina del Norte acepta como comprobante de responsabilidad civil para ciertos trámites de licencia. Es un tema de Carolina del Norte. Los exámenes de Carolina del Sur usan otras reglas de comprobante.",
            QuoteProduct = "auto",
            CtaEn = "Ask about DL-123",
            CtaEs = "Consultar sobre DL-123",
            FaqKey = "dl-123",
            RelatedIds = ["north-carolina", "auto", "non-owner", "new-drivers"],
            Sections =
            [
                new("When a DL-123 may be discussed", "Cuándo se puede conversar un DL-123",
                    "NCDMV decides which proof applies to a license, road test, or related transaction. Non-owner coverage does not automatically satisfy every DMV need.",
                    "NCDMV decide qué comprobante aplica a una licencia, examen o trámite. El seguro de no propietario no cumple automáticamente todo requisito del DMV."),
                new("Road tests without owning a car", "Examen de manejo sin ser dueño del carro",
                    "You may need acceptable proof of liability insurance and an eligible vehicle. Confirm directly with NCDMV and the insurer.",
                    "Puede necesitar un comprobante aceptable de responsabilidad civil y un vehículo elegible. Confirme con NCDMV y la aseguradora.")
            ],
            NoteEn = "Verify current NCDMV wording before treating any form name as universal.",
            NoteEs = "Verifique el texto actual de NCDMV antes de tratar cualquier nombre de formulario como universal."
        },
        new()
        {
            Id = "non-owner",
            SlugEn = "/auto-insurance/non-owner",
            SlugEs = "/es/seguro-auto/no-propietario",
            Icon = "🚗",
            ParentId = "auto",
            EyebrowEn = "Non-owner auto",
            EyebrowEs = "Auto no propietario",
            H1En = "Non-Owner Car Insurance",
            H1Es = "Seguro de Auto para No Propietario",
            TitleEn = "Non-Owner Car Insurance | Carolinas Coverage",
            TitleEs = "Seguro de auto no propietario | Carolinas Coverage",
            DescEn = "Non-owner car insurance is liability-focused coverage for some licensed drivers who do not own a vehicle. It may not satisfy every DMV or filing need. Call 704-595-3429.",
            DescEs = "El seguro de auto para no propietario es cobertura de responsabilidad civil para ciertos conductores que no poseen vehículo. Puede no cumplir todo requisito del DMV. 704-595-3429.",
            IntroEn = "Non-owner car insurance is liability-focused coverage for some licensed drivers who do not own a vehicle. It does not insure a specific car.",
            IntroEs = "El seguro de auto para no propietario es cobertura principalmente de responsabilidad civil para ciertos conductores con licencia que no poseen vehículo. No asegura un carro específico.",
            QuoteProduct = "auto",
            FaqKey = "non-owner",
            RelatedIds = ["auto", "dl-123", "sr-22", "new-drivers"],
            Sections =
            [
                new("What it does and does not do", "Qué hace y qué no hace",
                    "It may help with liability while driving cars you do not own, subject to the policy. It may not satisfy every road-test, registration or SR-22 filing need.",
                    "Puede ayudar con responsabilidad civil al manejar carros que no son suyos, sujeto a la póliza. Puede no cumplir todo requisito de examen, registro o filing SR-22."),
                new("Do I need non-owner insurance?", "¿Necesito seguro de no propietario?",
                    "It depends on license status, how you drive other vehicles, and what the DMV or a court requires. An agent can review available options for your state.",
                    "Depende del estado de la licencia, cómo maneja otros vehículos y lo que exija el DMV o un tribunal. Un agente puede revisar opciones disponibles para su estado.")
            ],
            NoteEn = "Availability and accepted use vary by carrier and state.",
            NoteEs = "La disponibilidad y el uso aceptado varían por compañía y estado."
        },
        new()
        {
            Id = "sr-22",
            SlugEn = "/auto-insurance/sr-22",
            SlugEs = "/es/seguro-auto/sr-22",
            Icon = "📋",
            ParentId = "auto",
            EyebrowEn = "SR-22",
            EyebrowEs = "SR-22",
            H1En = "SR-22 Insurance Help",
            H1Es = "Ayuda con Seguro SR-22",
            TitleEn = "SR-22 Insurance Help | Carolinas Coverage",
            TitleEs = "Ayuda con seguro SR-22 | Carolinas Coverage",
            DescEn = "An SR-22 is a financial-responsibility filing some drivers must have after certain violations. It is not required for every driver. Call 704-595-3429.",
            DescEs = "Un SR-22 es un comprobante de responsabilidad financiera que algunos conductores deben tener después de ciertas infracciones. No se exige a todo conductor. 704-595-3429.",
            IntroEn = "An SR-22 is a financial-responsibility filing that a state may require after certain violations or suspensions. It is not required for every driver.",
            IntroEs = "Un SR-22 es un comprobante de responsabilidad financiera que un estado puede exigir después de ciertas infracciones o suspensiones. No se requiere para todo conductor.",
            QuoteProduct = "auto",
            CtaEn = "Ask about SR-22",
            CtaEs = "Consultar sobre SR-22",
            FaqKey = "sr-22",
            RelatedIds = ["auto", "non-owner", "north-carolina", "south-carolina"],
            Sections =
            [
                new("SR-22 is a filing, not a coverage type", "El SR-22 es un filing, no un tipo de cobertura",
                    "The filing is attached to a policy that meets the state’s financial-responsibility rules. Need, duration and accepted forms differ by state.",
                    "El filing se adjunta a una póliza que cumple las reglas de responsabilidad financiera del estado. La necesidad, duración y formas aceptadas difieren por estado."),
                new("Do not assume every driver needs it", "No asuma que todo conductor lo necesita",
                    "Only drivers who are required to file should treat SR-22 as part of the quote conversation. An agent can review whether a filing applies after you share the requirement.",
                    "Solo los conductores a quienes se les exige presentar el filing deben tratar el SR-22 como parte de la cotización. Un agente puede revisar si aplica después de que usted comparta el requisito.")
            ],
            NoteEn = "Do not imply guaranteed acceptance after tickets, accidents or a lapse.",
            NoteEs = "No se implica aceptación garantizada después de multas, accidentes o un lapse."
        },
        new()
        {
            Id = "new-drivers",
            SlugEn = "/auto-insurance/new-drivers",
            SlugEs = "/es/seguro-auto/conductores-nuevos",
            Icon = "🪪",
            ParentId = "auto",
            EyebrowEn = "New and teen drivers",
            EyebrowEs = "Conductores nuevos y adolescentes",
            H1En = "Insurance for New and Teen Drivers",
            H1Es = "Seguro para Conductores Nuevos y Adolescentes",
            TitleEn = "Insurance for New and Teen Drivers | Carolinas Coverage",
            TitleEs = "Seguro para conductores nuevos | Carolinas Coverage",
            DescEn = "Insurance for new and teen drivers in North Carolina and South Carolina. NC DL-123 and SC road-test proof are different. Call 704-595-3429.",
            DescEs = "Seguro para conductores nuevos y adolescentes en Carolina del Norte y Carolina del Sur. DL-123 de NC y el comprobante de SC son distintos. 704-595-3429.",
            IntroEn = "New and teen drivers often need liability insurance for a license or road test. North Carolina and South Carolina use different proof rules. Pricing and eligibility vary.",
            IntroEs = "Los conductores nuevos y adolescentes suelen necesitar seguro de responsabilidad civil para una licencia o examen. Carolina del Norte y Carolina del Sur usan reglas de comprobante distintas. Precio y elegibilidad varían.",
            QuoteProduct = "auto",
            CtaEn = "Get a driver quote",
            CtaEs = "Cotizar conductor nuevo",
            FaqKey = "new-drivers",
            RelatedIds = ["dl-123", "auto", "non-owner", "north-carolina", "south-carolina"],
            Sections =
            [
                new("North Carolina license proof", "Comprobante de licencia en Carolina del Norte",
                    "Some NC transactions use a DL-123 or other accepted proof. Confirm with NCDMV which document applies.",
                    "Algunos trámites de NC usan DL-123 u otro comprobante aceptado. Confirme con NCDMV qué documento aplica."),
                new("South Carolina road-test proof", "Comprobante de examen en Carolina del Sur",
                    "SCDMV says the test vehicle needs a valid plate, registration and insurance card. Do not apply NC DL-123 rules to a South Carolina road test.",
                    "SCDMV indica que el vehículo del examen necesita placa válida, registro y tarjeta de seguro. No aplique las reglas de DL-123 de NC a un examen en Carolina del Sur.")
            ],
            NoteEn = "Adding a teen to a household policy and buying a standalone policy are different conversations.",
            NoteEs = "Agregar a un adolescente a una póliza del hogar y comprar una póliza aparte son conversaciones distintas."
        },
        HomeInsurance(),
        Renters(),
        Flood(),
        Pet(),
        Business(),
        GeneralLiability(),
        Contractors(),
        CommercialAuto(),
        WorkersComp(),
        Life(),
        TermLife(),
        WholeLife(),
        FinalExpense(),
        About(),
        Contact(),
        Charlotte()
    ];

    private static HubDef HomeInsurance() => new()
    {
        Id = "home-insurance",
        SlugEn = "/home-insurance",
        SlugEs = "/es/seguro-casa",
        Icon = "🏠",
        EyebrowEn = "Home insurance",
        EyebrowEs = "Seguro de casa",
        H1En = "Home Insurance in North Carolina & South Carolina",
        H1Es = "Seguro de Casa en Carolina del Norte y Carolina del Sur",
        TitleEn = "Home Insurance in NC & SC | Carolinas Coverage",
        TitleEs = "Seguro de casa NC y SC | Carolinas Coverage",
        DescEn = "Homeowners insurance in North Carolina and South Carolina. Dwelling, belongings, liability, replacement cost vs ACV, and flood exclusion. Call 704-595-3429.",
        DescEs = "Seguro de casa en Carolina del Norte y Carolina del Sur. Vivienda, pertenencias, responsabilidad, costo de reemplazo vs valor actual, e inundación aparte. 704-595-3429.",
        IntroEn = "Homeowners insurance commonly combines dwelling, personal property and personal liability protection. Flood is generally separate. A mortgage lender usually requires property protection.",
        IntroEs = "El seguro de casa suele reunir vivienda, pertenencias y responsabilidad personal. La inundación generalmente va aparte. Un prestamista hipotecario suele exigir protección de la propiedad.",
        QuoteProduct = "home",
        CtaEn = "Get a home quote",
        CtaEs = "Cotizar seguro de casa",
        FaqKey = "home-insurance",
        RelatedIds = ["flood", "renters", "north-carolina", "south-carolina"],
        Sections =
        [
            new("Replacement cost versus actual cash value", "Costo de reemplazo versus valor actual",
                "Replacement cost generally does not subtract depreciation for covered replacement, while actual cash value generally does. Policy terms, limits and claim conditions control.",
                "El costo de reemplazo generalmente no resta depreciación para el reemplazo cubierto; el valor actual normalmente sí. Mandan los términos, límites y condiciones de la póliza."),
            new("Flood is usually not included", "La inundación normalmente no está incluida",
                "Most standard homeowners policies do not cover flood. A separate flood policy may be needed for the building, contents or both.",
                "La mayoría de las pólizas estándar de casa no cubren inundación. Puede necesitar una póliza separada para el edificio, pertenencias o ambos."),
            new("When a buyer should start", "Cuándo debe empezar un comprador",
                "Start before closing so the lender and insurer have time to review the property, coverage, deductible and effective date.",
                "Empiece antes del cierre para que el prestamista y la aseguradora revisen propiedad, cobertura, deducible y fecha de vigencia.")
        ],
        NoteEn = "An old roof is not automatically replaced just because it is old. Cause of loss, form, exclusions and valuation control.",
        NoteEs = "Un techo viejo no se reemplaza automáticamente por ser viejo. Controlan la causa, el formulario, las exclusiones y la valoración."
    };

    private static HubDef Renters() => new()
    {
        Id = "renters",
        SlugEn = "/renters-insurance",
        SlugEs = "/es/seguro-inquilinos",
        Icon = "🔑",
        EyebrowEn = "Renters insurance",
        EyebrowEs = "Seguro para inquilinos",
        H1En = "Renters Insurance",
        H1Es = "Seguro para Inquilinos",
        TitleEn = "Renters Insurance | Carolinas Coverage",
        TitleEs = "Seguro para inquilinos | Carolinas Coverage",
        DescEn = "Renters insurance for belongings, personal liability and additional living expense in North Carolina and South Carolina. Call 704-595-3429.",
        DescEs = "Seguro para inquilinos: pertenencias, responsabilidad personal y gastos adicionales de vivienda en Carolina del Norte y Carolina del Sur. 704-595-3429.",
        IntroEn = "Renters insurance can include personal property, personal liability and additional living expense for covered losses. A landlord’s policy is usually not designed to insure a tenant’s belongings.",
        IntroEs = "El seguro para inquilinos puede incluir pertenencias, responsabilidad personal y gastos adicionales de vivienda por pérdidas cubiertas. La póliza del propietario normalmente no asegura las pertenencias del inquilino.",
        QuoteProduct = "renters",
        CtaEn = "Get a renters quote",
        CtaEs = "Cotizar seguro de inquilinos",
        FaqKey = "renters",
        RelatedIds = ["home-insurance", "flood"],
        Sections =
        [
            new("What renters insurance may cover", "Qué puede cubrir el seguro para inquilinos",
                "Limits, deductibles and exclusions vary. The building structure is generally the landlord’s responsibility.",
                "Límites, deducibles y exclusiones varían. La estructura del edificio suele ser responsabilidad del propietario."),
            new("Landlord policies and tenants", "Pólizas del propietario e inquilinos",
                "Usually the landlord's property policy is not designed to insure a tenant's personal belongings. Tenants should review renters insurance, including when a lease requires it.",
                "Normalmente la póliza del propietario no está diseñada para asegurar las pertenencias personales del inquilino. Los inquilinos deben revisar este seguro, incluso cuando el contrato lo exige.")
        ],
        NoteEn = "Flood is generally separate from a standard renters policy.",
        NoteEs = "La inundación generalmente va aparte de una póliza estándar de inquilinos."
    };

    private static HubDef Flood() => new()
    {
        Id = "flood",
        SlugEn = "/flood-insurance",
        SlugEs = "/es/seguro-inundacion",
        Icon = "💧",
        EyebrowEn = "Flood insurance",
        EyebrowEs = "Seguro contra inundación",
        H1En = "Flood Insurance",
        H1Es = "Seguro contra Inundación",
        TitleEn = "Flood Insurance | Carolinas Coverage",
        TitleEs = "Seguro contra inundación | Carolinas Coverage",
        DescEn = "Flood insurance is generally separate from homeowners or renters coverage in North Carolina and South Carolina. Ask about available flood options. Call 704-595-3429.",
        DescEs = "El seguro contra inundación generalmente va aparte del seguro de casa o inquilinos en Carolina del Norte y Carolina del Sur. 704-595-3429.",
        IntroEn = "Most standard homeowners and renters policies do not cover flood. A separate flood policy may be needed for the building, contents or both. Availability depends on the property and the program or carrier.",
        IntroEs = "La mayoría de las pólizas estándar de casa e inquilinos no cubren inundación. Puede necesitar una póliza separada para el edificio, pertenencias o ambos. La disponibilidad depende de la propiedad y del programa o compañía.",
        QuoteProduct = "home",
        CtaEn = "Check flood options",
        CtaEs = "Revisar opciones de inundación",
        FaqKey = "flood",
        RelatedIds = ["home-insurance", "renters"],
        Sections =
        [
            new("Why flood is called out separately", "Por qué la inundación se trata aparte",
                "This is generally separate coverage. Do not assume a homeowners, renters or business policy pays a flood loss.",
                "Generalmente es cobertura separada. No asuma que una póliza de casa, inquilinos o negocio paga una pérdida por inundación.")
        ],
        NoteEn = "State clearly this is generally separate coverage. Eligibility and waiting periods depend on the policy.",
        NoteEs = "Debe quedar claro que generalmente es cobertura separada. Elegibilidad y períodos de espera dependen de la póliza."
    };

    private static HubDef Pet() => new()
    {
        Id = "pet",
        SlugEn = "/pet-insurance",
        SlugEs = "/es/seguro-mascotas",
        Icon = "🐾",
        EyebrowEn = "Pet insurance",
        EyebrowEs = "Seguro para mascotas",
        H1En = "Pet Insurance for Dogs and Cats",
        H1Es = "Seguro para Perros y Gatos",
        TitleEn = "Pet Insurance for Dogs and Cats | Carolinas Coverage",
        TitleEs = "Seguro para perros y gatos | Carolinas Coverage",
        DescEn = "Pet insurance for dogs and cats: accident and illness, waiting periods, pre-existing conditions, and how wellness differs from insurance. Call 704-595-3429.",
        DescEs = "Seguro para perros y gatos: accidentes y enfermedad, períodos de espera, preexistentes y cómo el bienestar no es lo mismo que el seguro. 704-595-3429.",
        IntroEn = "Pet insurance can reimburse a percentage of eligible veterinary costs after a deductible. Wellness plans help with routine care and are not the same as insurance, even when sold together.",
        IntroEs = "El seguro de mascotas puede reembolsar un porcentaje de costos veterinarios elegibles después del deducible. Los planes de bienestar ayudan con el cuidado rutinario y no son lo mismo que el seguro, aunque se vendan juntos.",
        QuoteProduct = "pet",
        CtaEn = "Compare pet options",
        CtaEs = "Comparar opciones de mascotas",
        FaqKey = "pet",
        RelatedIds = ["auto", "home-insurance"],
        Sections =
        [
            new("Accident and illness versus wellness", "Accidentes y enfermedad versus bienestar",
                "Insurance addresses covered accident or illness losses. A wellness program generally helps with routine care. They should be explained separately.",
                "El seguro atiende pérdidas cubiertas por accidente o enfermedad. Un programa de bienestar generalmente ayuda con cuidado rutinario. Deben explicarse por separado."),
            new("Pre-existing conditions and waiting periods", "Preexistentes y períodos de espera",
                "Many policies exclude pre-existing conditions. A waiting period is the time after the effective date before certain coverage begins. Accident, illness and orthopedic waiting periods may differ.",
                "Muchas pólizas excluyen condiciones preexistentes. Un período de espera es el tiempo después de la vigencia antes de que empiece cierta cobertura. Accidentes, enfermedad y ortopedia pueden tener plazos distintos.")
        ],
        NoteEn = "The exact reimbursement calculation is policy-specific.",
        NoteEs = "El cálculo exacto de reembolso depende de la póliza."
    };

    private static HubDef Business() => new()
    {
        Id = "business",
        SlugEn = "/business-insurance",
        SlugEs = "/es/seguro-comercial",
        Icon = "🏢",
        EyebrowEn = "Business insurance",
        EyebrowEs = "Seguro comercial",
        H1En = "Business Insurance",
        H1Es = "Seguro Comercial",
        TitleEn = "Business Insurance | Carolinas Coverage",
        TitleEs = "Seguro comercial | Carolinas Coverage",
        DescEn = "Business insurance in NC and SC: general liability, contractors, commercial auto and workers’ compensation. A BOP does not include every exposure. Call 704-595-3429.",
        DescEs = "Seguro comercial en NC y SC: responsabilidad civil, contratistas, auto comercial y compensación laboral. Una BOP no incluye toda exposición. 704-595-3429.",
        IntroEn = "A small business review depends on operations, contracts, employees, vehicles, property, professional services and cyber exposure. A business owners policy packages some coverages for eligible businesses; it does not automatically include auto, professional, cyber or workers’ compensation.",
        IntroEs = "La revisión de un negocio pequeño depende de operaciones, contratos, empleados, vehículos, propiedad, servicios profesionales y exposición cibernética. Una BOP combina algunas coberturas para negocios elegibles; no incluye automáticamente auto, profesional, cyber ni compensación laboral.",
        QuoteProduct = "business",
        CtaEn = "Request a business quote",
        CtaEs = "Solicitar cotización comercial",
        FaqKey = "business",
        RelatedIds = ["general-liability", "contractors", "commercial-auto", "workers-comp"],
        Sections =
        [
            new("What a certificate of insurance is", "Qué es un certificado de seguro",
                "A COI summarizes specified policy information at a point in time. It does not amend coverage or grant rights by itself.",
                "Un COI resume información específica de la póliza en un momento dado. Por sí solo no modifica cobertura ni concede derechos.")
        ],
        NoteEn = "Health, Medicare and disability insurance are not offered.",
        NoteEs = "No se ofrece seguro de salud, Medicare ni disability."
    };

    private static HubDef GeneralLiability() => new()
    {
        Id = "general-liability",
        SlugEn = "/business-insurance/general-liability",
        SlugEs = "/es/seguro-comercial/responsabilidad-civil",
        Icon = "🛡",
        ParentId = "business",
        EyebrowEn = "General liability",
        EyebrowEs = "Responsabilidad civil general",
        H1En = "General Liability Insurance",
        H1Es = "Seguro de Responsabilidad Civil General",
        TitleEn = "General Liability Insurance | Carolinas Coverage",
        TitleEs = "Responsabilidad civil general | Carolinas Coverage",
        DescEn = "General liability insurance for businesses and contractors in North Carolina and South Carolina. Additional insured status is not created by a certificate alone. Call 704-595-3429.",
        DescEs = "Responsabilidad civil general para negocios y contratistas en Carolina del Norte y Carolina del Sur. Un certificado no crea por sí solo el asegurado adicional. 704-595-3429.",
        IntroEn = "General liability can address certain third-party bodily injury, property damage and personal or advertising injury claims, subject to definitions, exclusions and limits.",
        IntroEs = "La responsabilidad civil general puede atender ciertos reclamos de terceros por lesiones corporales, daño material y lesión personal o publicitaria, sujeto a definiciones, exclusiones y límites.",
        QuoteProduct = "business",
        CtaEn = "Get a liability quote",
        CtaEs = "Cotizar responsabilidad civil",
        FaqKey = "general-liability",
        RelatedIds = ["business", "contractors", "workers-comp"],
        Sections =
        [
            new("Additional insured", "Asegurado adicional",
                "An additional insured is a person or organization added under policy terms or endorsement. A certificate alone does not necessarily create that status.",
                "Un asegurado adicional es una persona u organización añadida bajo términos o endoso. Un certificado por sí solo no necesariamente crea ese estado.")
        ],
        NoteEn = "Contract requirements and exclusions vary.",
        NoteEs = "Los requisitos de contrato y las exclusiones varían."
    };

    private static HubDef Contractors() => new()
    {
        Id = "contractors",
        SlugEn = "/business-insurance/contractors",
        SlugEs = "/es/seguro-comercial/contratistas",
        Icon = "🔧",
        ParentId = "business",
        EyebrowEn = "Contractor insurance",
        EyebrowEs = "Seguro para contratistas",
        H1En = "Contractor Insurance",
        H1Es = "Seguro para Contratistas",
        TitleEn = "Contractor Insurance | Carolinas Coverage",
        TitleEs = "Seguro para contratistas | Carolinas Coverage",
        DescEn = "Contractor insurance in NC and SC: general liability, commercial auto, workers’ compensation and tools. Trade, payroll and contracts affect coverage. Call 704-595-3429.",
        DescEs = "Seguro para contratistas en NC y SC: responsabilidad civil, auto comercial, compensación laboral y herramientas. Oficio, nómina y contratos afectan la cobertura. 704-595-3429.",
        IntroEn = "Construction and service contractors often review general liability, commercial auto, workers’ compensation and equipment. Trade, payroll, subcontractors and contracts affect what may be available.",
        IntroEs = "Los contratistas de construcción y servicio suelen revisar responsabilidad civil, auto comercial, compensación laboral y equipo. Oficio, nómina, subcontratistas y contratos afectan lo que puede estar disponible.",
        QuoteProduct = "business",
        CtaEn = "Request contractor coverage",
        CtaEs = "Solicitar cobertura de contratista",
        FaqKey = "contractors",
        RelatedIds = ["general-liability", "workers-comp", "commercial-auto", "business"],
        Sections =
        [
            new("Tools away from the shop", "Herramientas fuera del taller",
                "Inland marine or contractor-equipment coverage may protect scheduled or covered tools in transit or at jobsites, subject to limits, valuation, security rules and exclusions.",
                "Inland marine o cobertura de equipo puede proteger herramientas programadas o cubiertas en tránsito o en trabajos, sujeto a límites, valoración, seguridad y exclusiones.")
        ],
        NoteEn = "Availability depends on appointments, underwriting and state.",
        NoteEs = "La disponibilidad depende de nombramientos, suscripción y estado."
    };

    private static HubDef CommercialAuto() => new()
    {
        Id = "commercial-auto",
        SlugEn = "/business-insurance/commercial-auto",
        SlugEs = "/es/seguro-comercial/auto-comercial",
        Icon = "🚚",
        ParentId = "business",
        EyebrowEn = "Commercial auto",
        EyebrowEs = "Auto comercial",
        H1En = "Commercial Auto Insurance",
        H1Es = "Seguro de Auto Comercial",
        TitleEn = "Commercial Auto Insurance | Carolinas Coverage",
        TitleEs = "Seguro de auto comercial | Carolinas Coverage",
        DescEn = "Commercial auto insurance for businesses using vehicles in North Carolina and South Carolina. Personal auto may not cover work use. Call 704-595-3429.",
        DescEs = "Seguro de auto comercial para negocios que usan vehículos en Carolina del Norte y Carolina del Sur. El auto personal puede no cubrir uso de trabajo. 704-595-3429.",
        IntroEn = "Business use may be limited or excluded under a personal auto policy. Disclose ownership, use, drivers, radius, equipment and deliveries so an agent can review whether commercial auto is appropriate.",
        IntroEs = "El uso comercial puede estar limitado o excluido en una póliza personal. Declare propietario, uso, conductores, radio, equipo y entregas para revisar si corresponde auto comercial.",
        QuoteProduct = "commercial-auto",
        CtaEn = "Get a commercial auto quote",
        CtaEs = "Cotizar auto comercial",
        FaqKey = "commercial-auto",
        RelatedIds = ["business", "auto", "contractors"],
        Sections =
        [
            new("Personal policy versus commercial auto", "Póliza personal versus auto comercial",
                "Work vehicles, deliveries, and employee drivers often need a commercial conversation. Coverage is never guaranteed until underwriting is complete.",
                "Vehículos de trabajo, entregas y conductores empleados suelen requerir una conversación comercial. La cobertura no se garantiza hasta completar la suscripción.")
        ],
        NoteEn = "DOT and cargo needs, when they apply, must be disclosed.",
        NoteEs = "DOT y carga, cuando apliquen, deben declararse."
    };

    private static HubDef WorkersComp() => new()
    {
        Id = "workers-comp",
        SlugEn = "/business-insurance/workers-compensation",
        SlugEs = "/es/seguro-comercial/compensacion-laboral",
        Icon = "🦺",
        ParentId = "business",
        EyebrowEn = "Workers’ compensation",
        EyebrowEs = "Compensación laboral",
        H1En = "Workers' Compensation Insurance",
        H1Es = "Seguro de Compensación Laboral",
        TitleEn = "Workers' Compensation NC & SC | Carolinas Coverage",
        TitleEs = "Compensación laboral NC y SC | Carolinas Coverage",
        DescEn = "Workers’ compensation: North Carolina generally at three or more employees; South Carolina generally at four or more. Exceptions apply. Call 704-595-3429.",
        DescEs = "Compensación laboral: Carolina del Norte generalmente con tres o más empleados; Carolina del Sur generalmente con cuatro o más. Hay excepciones. 704-595-3429.",
        IntroEn = "Employee thresholds and exceptions differ by state. North Carolina and South Carolina rules are stated separately. Classification questions can change whether a person counts.",
        IntroEs = "Los umbrales de empleados y las excepciones difieren por estado. Las reglas de Carolina del Norte y Carolina del Sur se indican por separado. Las preguntas de clasificación pueden cambiar si una persona cuenta.",
        QuoteProduct = "business",
        CtaEn = "Review employee counts",
        CtaEs = "Revisar cantidad de empleados",
        FaqKey = "workers-comp",
        RelatedIds = ["business", "contractors", "north-carolina", "south-carolina"],
        Sections =
        [
            new("North Carolina — generally three or more", "Carolina del Norte — generalmente tres o más",
                "North Carolina generally requires workers’ compensation for businesses with three or more employees, subject to statutory exceptions and classification questions.",
                "Carolina del Norte generalmente la exige para negocios con tres o más empleados, sujeto a excepciones legales y preguntas de clasificación."),
            new("South Carolina — generally four or more", "Carolina del Sur — generalmente cuatro o más",
                "South Carolina generally requires it at four or more employees; part-time and family employees may count. Confirm exceptions with the Commission.",
                "Carolina del Sur generalmente la exige con cuatro o más empleados; empleados de medio tiempo y familiares pueden contar. Confirme excepciones con la Comisión.")
        ],
        NoteEn = "These are general statements. Do not treat them as a determination for a specific business.",
        NoteEs = "Son enunciados generales. No los trate como una determinación para un negocio específico."
    };

    private static HubDef Life() => new()
    {
        Id = "life",
        SlugEn = "/life-insurance",
        SlugEs = "/es/seguro-vida",
        Icon = "🤍",
        EyebrowEn = "Life insurance",
        EyebrowEs = "Seguro de vida",
        H1En = "Life Insurance in North Carolina & South Carolina",
        H1Es = "Seguro de Vida en Carolina del Norte y Carolina del Sur",
        TitleEn = "Life Insurance in NC & SC | Carolinas Coverage",
        TitleEs = "Seguro de vida NC y SC | Carolinas Coverage",
        DescEn = "Life insurance in North Carolina and South Carolina: term, whole life and final expense. Health, Medicare and disability are not offered. Call 704-595-3429.",
        DescEs = "Seguro de vida en Carolina del Norte y Carolina del Sur: término, vida entera y gastos finales. No se ofrece salud, Medicare ni disability. 704-595-3429.",
        IntroEn = "Life insurance can help with income replacement, debts, mortgage, education, caregiving and final expenses. A quote is not a substitute for financial, tax or legal advice. Health insurance, Medicare and disability insurance are not offered.",
        IntroEs = "El seguro de vida puede ayudar con reemplazo de ingresos, deudas, hipoteca, educación, cuidado y gastos finales. Una cotización no sustituye asesoría financiera, fiscal o legal. No se ofrece seguro de salud, Medicare ni disability.",
        QuoteProduct = "life",
        CtaEn = "Request a life quote",
        CtaEs = "Solicitar cotización de vida",
        FaqKey = "life",
        RelatedIds = ["term", "whole", "final-expense"],
        Sections =
        [
            new("No-exam does not mean guaranteed acceptance", "Sin examen no significa aprobación garantizada",
                "Some products use simplified or accelerated underwriting, but availability, questions, records, limits, price and approval vary.",
                "Algunos productos usan suscripción simplificada o acelerada, pero disponibilidad, preguntas, registros, límites, precio y aprobación varían."),
            new("Changing a beneficiary", "Cambiar un beneficiario",
                "Often possible if the beneficiary is revocable and ownership or legal restrictions do not prevent it. Use the carrier’s form and keep confirmation.",
                "Frecuentemente es posible si el beneficiario es revocable y no hay restricciones de propiedad o legales. Use el formulario de la compañía y guarde confirmación.")
        ],
        NoteEn = "Do not mix life insurance with health, Medicare or disability content.",
        NoteEs = "No mezcle seguro de vida con salud, Medicare o disability."
    };

    private static HubDef TermLife() => new()
    {
        Id = "term",
        SlugEn = "/life-insurance/term",
        SlugEs = "/es/seguro-vida/termino",
        Icon = "🤍",
        ParentId = "life",
        EyebrowEn = "Term life",
        EyebrowEs = "Vida a término",
        H1En = "Term Life Insurance",
        H1Es = "Seguro de Vida a Término",
        TitleEn = "Term Life Insurance | Carolinas Coverage",
        TitleEs = "Seguro de vida a término | Carolinas Coverage",
        DescEn = "Term life insurance provides a death benefit for a stated term if the policy is in force. It generally does not build cash value. Call 704-595-3429.",
        DescEs = "El seguro de vida a término ofrece beneficio por fallecimiento durante un plazo si la póliza está vigente. Generalmente no acumula valor en efectivo. 704-595-3429.",
        IntroEn = "Term life provides a death benefit for a stated term if the policy is in force and the claim is covered. It generally does not build cash value.",
        IntroEs = "El seguro a término ofrece beneficio por fallecimiento durante un plazo si la póliza está vigente y el reclamo está cubierto. Generalmente no acumula valor en efectivo.",
        QuoteProduct = "life",
        CtaEn = "Compare term options",
        CtaEs = "Comparar opciones a término",
        FaqKey = "term",
        RelatedIds = ["life", "whole", "final-expense"],
        Sections =
        [
            new("How much coverage to consider", "Cuánta cobertura considerar",
                "Consider income replacement, debts, mortgage, education, caregiving, final expenses, existing assets and how long the need lasts.",
                "Considere reemplazo de ingresos, deudas, hipoteca, educación, cuidado, gastos finales, activos existentes y cuánto dura la necesidad.")
        ],
        NoteEn = "Availability, term length, benefit and underwriting vary.",
        NoteEs = "Disponibilidad, plazo, beneficio y suscripción varían."
    };

    private static HubDef WholeLife() => new()
    {
        Id = "whole",
        SlugEn = "/life-insurance/whole",
        SlugEs = "/es/seguro-vida/vida-entera",
        Icon = "🤍",
        ParentId = "life",
        EyebrowEn = "Whole life",
        EyebrowEs = "Vida entera",
        H1En = "Whole Life Insurance",
        H1Es = "Seguro de Vida Entera",
        TitleEn = "Whole Life Insurance | Carolinas Coverage",
        TitleEs = "Seguro de vida entera | Carolinas Coverage",
        DescEn = "Whole life is permanent coverage that can build cash value. Premiums, guarantees, loans and surrender values need policy-specific review. Call 704-595-3429.",
        DescEs = "Vida entera es cobertura permanente que puede acumular valor en efectivo. Primas, garantías, préstamos y rescates requieren revisión específica. 704-595-3429.",
        IntroEn = "Whole life is permanent coverage designed to remain in force under its terms and can build cash value. This page does not promise investment performance.",
        IntroEs = "Vida entera es cobertura permanente diseñada para continuar según sus términos y puede acumular valor en efectivo. Esta página no promete rendimiento de inversión.",
        QuoteProduct = "life",
        CtaEn = "Compare permanent options",
        CtaEs = "Comparar opciones permanentes",
        FaqKey = "whole",
        RelatedIds = ["life", "term", "final-expense"],
        Sections =
        [
            new("Policy-specific review", "Revisión específica de la póliza",
                "Premiums, guarantees, loans and surrender values require a review of the illustration and contract. Avoid treating any figure on this website as a guaranteed return.",
                "Primas, garantías, préstamos y valores de rescate requieren revisar la ilustración y el contrato. No trate ninguna cifra de este sitio como un rendimiento garantizado.")
        ],
        NoteEn = "Avoid investment-performance promises.",
        NoteEs = "Evite promesas de rendimiento de inversión."
    };

    private static HubDef FinalExpense() => new()
    {
        Id = "final-expense",
        SlugEn = "/life-insurance/final-expense",
        SlugEs = "/es/seguro-vida/gastos-finales",
        Icon = "🤍",
        ParentId = "life",
        EyebrowEn = "Final expense",
        EyebrowEs = "Gastos finales",
        H1En = "Final Expense Life Insurance",
        H1Es = "Seguro de Gastos Finales",
        TitleEn = "Final Expense Life Insurance | Carolinas Coverage",
        TitleEs = "Seguro de gastos finales | Carolinas Coverage",
        DescEn = "Final expense life insurance is generally a benefit intended to help with funeral and other final costs. Underwriting and waiting periods vary. Call 704-595-3429.",
        DescEs = "El seguro de gastos finales es generalmente un beneficio para funeral y otros costos finales. Suscripción y períodos de espera varían. 704-595-3429.",
        IntroEn = "Final expense coverage is generally life insurance with a benefit intended to help beneficiaries with funeral and other final costs. Benefit amount, underwriting and any waiting period vary. Acceptance is not guaranteed.",
        IntroEs = "Generalmente es seguro de vida con un beneficio destinado a ayudar con funeral y otros costos finales. Monto, suscripción y período de espera varían. La aceptación no está garantizada.",
        QuoteProduct = "life",
        CtaEn = "Request a final-expense quote",
        CtaEs = "Solicitar cotización de gastos finales",
        FaqKey = "final-expense",
        RelatedIds = ["life", "term", "whole"],
        Sections =
        [
            new("Not guaranteed approval", "No es aprobación garantizada",
                "Simplified questions do not mean every applicant is accepted. Waiting periods and graded benefits, when they exist, are policy-specific.",
                "Preguntas simplificadas no significan que todo solicitante sea aceptado. Períodos de espera y beneficios graduales, cuando existen, dependen de la póliza.")
        ],
        NoteEn = "Avoid guaranteed approval language.",
        NoteEs = "Evite lenguaje de aprobación garantizada."
    };

    private static HubDef About() => new()
    {
        Id = "about",
        SlugEn = "/about",
        SlugEs = "/es/nosotros",
        Icon = "C",
        EyebrowEn = "The agency",
        EyebrowEs = "La agencia",
        H1En = "About Carolinas Coverage Insurance Agency",
        H1Es = "Acerca de Carolinas Coverage Insurance Agency",
        TitleEn = "About Carolinas Coverage Insurance Agency | Charlotte NC",
        TitleEs = "Acerca de Carolinas Coverage Insurance Agency | Charlotte NC",
        DescEn = "Carolinas Coverage Insurance Agency is an independent insurance agency in Charlotte, NC. Auto, home, renters, pet, business and life in NC and SC. Not Medicare. Call 704-595-3429.",
        DescEs = "Carolinas Coverage Insurance Agency es una agencia independiente en Charlotte, NC. Auto, casa, inquilinos, mascotas, negocio y vida en NC y SC. No Medicare. 704-595-3429.",
        IntroEn = "Carolinas Coverage Insurance Agency is the legal name of this Charlotte office. People searching the name sometimes land on other businesses with similar names. This page states who we are, what we help with, and how to reach the office at 410 Westinghouse Blvd, Suite B.",
        IntroEs = "Carolinas Coverage Insurance Agency es el nombre legal de esta oficina en Charlotte. Quien busca el nombre a veces llega a otros negocios con nombres parecidos. Esta página dice quiénes somos, con qué ayudamos y cómo llegar a 410 Westinghouse Blvd, Suite B.",
        CtaEn = "Request a quote",
        CtaEs = "Solicitar cotización",
        FaqKey = "about",
        Schema = "InsuranceAgency",
        RelatedIds = ["contact", "charlotte", "spanish-agent", "north-carolina", "south-carolina"],
        Sections =
        [
            new("Who we are", "Quiénes somos",
                "We are an independent insurance agency based in Charlotte, North Carolina. Confirmed conversations include auto, home and property, renters, pet, business liability, commercial auto and life insurance for North Carolina and South Carolina applicants. An agent reviews available options after appointments and eligibility are confirmed. This site does not issue a binding quote and does not publish prices.",
                "Somos una agencia de seguros independiente con sede en Charlotte, Carolina del Norte. Las conversaciones confirmadas incluyen auto, casa y propiedad, inquilinos, mascotas, responsabilidad de negocio, auto comercial y vida para solicitantes de Carolina del Norte y Carolina del Sur. Deben confirmarse nombramientos y elegibilidad. Este sitio no emite una cotización vinculante ni publica precios."),
            new("Who we are not", "Quiénes no somos",
                "We are not Carolina Coverage Group in Mooresville and we are not Carolina Coverage Solutions. This agency does not offer health insurance, Medicare or disability insurance. If you need Medicare help, contact a Medicare-licensed source; that is not a service of this office.",
                "No somos Carolina Coverage Group de Mooresville ni Carolina Coverage Solutions. Esta agencia no ofrece seguro de salud, Medicare ni disability. Si necesita ayuda con Medicare, contacte una fuente con licencia de Medicare; no es un servicio de esta oficina."),
            new("Office and languages", "Oficina e idiomas",
                "Address: 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. Phone: 704-595-3429. Confirm hours when you call. English and Spanish contact and quote paths are on this website; confirm Spanish-language staffing when you call. Rock Hill and other nearby communities may be served from this Charlotte office. There is no separate published Rock Hill office.",
                "Dirección: 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. Teléfono: 704-595-3429. Confirme el horario al llamar. Este sitio tiene contacto y cotización en inglés y español; confirme personal en español al llamar. Rock Hill y comunidades cercanas pueden atenderse desde esta oficina de Charlotte. No hay una oficina publicada aparte en Rock Hill.")
        ],
        NoteEn = "License numbers will be published after the agency supplies official evidence. Do not invent EIN, hours, or reviews.",
        NoteEs = "Los números de licencia se publicarán cuando la agencia entregue evidencia oficial. No invente EIN, horario ni reseñas.",
        Priority = "0.9"
    };

    private static HubDef Contact() => new()
    {
        Id = "contact",
        SlugEn = "/contact",
        SlugEs = "/es/contacto",
        Icon = "☎",
        EyebrowEn = "Contact",
        EyebrowEs = "Contacto",
        H1En = "Contact Carolinas Coverage Insurance Agency",
        H1Es = "Contacte a Carolinas Coverage Insurance Agency",
        TitleEn = "Contact Carolinas Coverage Insurance Agency",
        TitleEs = "Contacte a Carolinas Coverage Insurance Agency",
        DescEn = "Call 704-595-3429. 410 Westinghouse Blvd Suite B, Charlotte, NC 28273. Bilingual quote requests for North Carolina and South Carolina.",
        DescEs = "Llame al 704-595-3429. 410 Westinghouse Blvd Suite B, Charlotte, NC 28273. Cotización bilingüe para Carolina del Norte y Carolina del Sur.",
        IntroEn = "Call 704-595-3429 or start a quote worksheet. The office is at 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. Confirm hours when you call.",
        IntroEs = "Llame al 704-595-3429 o inicie una hoja de cotización. La oficina está en 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. Confirme el horario al llamar.",
        CtaEn = "Call 704-595-3429",
        CtaEs = "Llamar al 704-595-3429",
        FaqKey = "contact",
        Schema = "ContactPage",
        RelatedIds = ["spanish-agent", "charlotte", "about", "north-carolina", "south-carolina"],
        Sections =
        [
            new("What to have ready for a quote", "Qué tener listo para una cotización",
                "Applicant details, address, prior coverage, drivers or employees, property or vehicle information, desired limits and relevant loss history.",
                "Datos del solicitante, dirección, cobertura previa, conductores o empleados, información de propiedad o vehículo, límites deseados e historial de pérdidas relevante."),
            new("Claims", "Reclamos",
                "Protect people first. Document the event, notify law enforcement when appropriate, and contact the carrier or agency promptly. Coverage and payment depend on the policy and investigation. No claim outcome is promised.",
                "Proteja primero a las personas. Documente el evento, avise a la policía cuando corresponda y contacte pronto a la compañía o agencia. Cobertura y pago dependen de la póliza e investigación. No se promete el resultado de un reclamo.")
        ],
        NoteEn = "Hours and staff names will be published after the agency confirms them.",
        NoteEs = "Horario y nombres del personal se publicarán cuando la agencia los confirme.",
        Priority = "0.9"
    };

    private static HubDef Charlotte() => new()
    {
        Id = "charlotte",
        SlugEn = "/nc/charlotte-nc",
        SlugEs = "/es/nc/charlotte-nc",
        Icon = "📍",
        EyebrowEn = "Charlotte, North Carolina",
        EyebrowEs = "Charlotte, Carolina del Norte",
        H1En = "Insurance Agency in Charlotte, North Carolina",
        H1Es = "Agencia de Seguros en Charlotte, Carolina del Norte",
        TitleEn = "Insurance Agency in Charlotte, NC | Carolinas Coverage",
        TitleEs = "Agencia de seguros en Charlotte, NC | Carolinas Coverage",
        DescEn = "Carolinas Coverage Insurance Agency, 410 Westinghouse Blvd Suite B, Charlotte NC 28273. Auto, home, business, pet and life. Call 704-595-3429.",
        DescEs = "Carolinas Coverage Insurance Agency, 410 Westinghouse Blvd Suite B, Charlotte NC 28273. Auto, casa, negocio, mascotas y vida. 704-595-3429.",
        IntroEn = "The agency office is at 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273, near the Westinghouse corridor. From here the team helps Charlotte households and businesses, and also serves South Carolina clients. Confirm the product and state for each request.",
        IntroEs = "La oficina está en 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273, cerca del corredor Westinghouse. Desde aquí se atiende a hogares y negocios de Charlotte, y también a clientes de Carolina del Sur. Confirme el producto y el estado en cada solicitud.",
        CtaEn = "Request a Charlotte quote",
        CtaEs = "Solicitar cotización en Charlotte",
        FaqKey = "charlotte",
        Schema = "InsuranceAgency",
        RelatedIds = ["north-carolina", "south-carolina", "contact", "about", "auto", "home-insurance"],
        Sections =
        [
            new("Visit or call the Charlotte office", "Visite o llame a la oficina de Charlotte",
                "Address: 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. Phone: 704-595-3429. Confirm hours when you call. Spanish-language help is available through bilingual quote and contact paths; confirm staffing when you call.",
                "Dirección: 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. Teléfono: 704-595-3429. Confirme horario al llamar. La ayuda en español está disponible por cotización y contacto bilingüe; confirme el personal al llamar."),
            new("Coverage conversations in Charlotte", "Conversaciones de cobertura en Charlotte",
                "Confirmed lines include auto, home and property, business liability, pet and life. Health, Medicare and disability are not offered.",
                "Las líneas confirmadas incluyen auto, casa y propiedad, responsabilidad de negocio, mascotas y vida. No se ofrece salud, Medicare ni disability."),
            new("Nearby communities, including Rock Hill", "Comunidades cercanas, incluido Rock Hill",
                "Households and businesses along the I-77 corridor, including Rock Hill, South Carolina, may request help from this Charlotte office. South Carolina auto, road-test and workers’ compensation rules still apply to South Carolina risks. This is not a claim of a second storefront.",
                "Hogares y negocios del corredor I-77, incluido Rock Hill, Carolina del Sur, pueden pedir ayuda a esta oficina de Charlotte. Las reglas de auto, examen de manejo y compensación laboral de Carolina del Sur siguen aplicando a riesgos de Carolina del Sur. No se afirma un segundo local.")
        ],
        NoteEn = "Nearby city pages are not published without unique local proof.",
        NoteEs = "No se publican páginas de ciudades cercanas sin prueba local única.",
        Priority = "0.9"
    };
}
