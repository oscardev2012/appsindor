namespace CarolinasCoverage.Site;

public static class FaqCatalog
{
    public static IReadOnlyList<FaqItem> Home(bool en) => For("homepage", en);

    public static IReadOnlyList<FaqItem> For(string key, bool en)
    {
        if (!_pages.TryGetValue(key, out var items))
        {
            return [];
        }

        return items.Select(item => en
            ? new FaqItem(item.QEn, item.AEn)
            : new FaqItem(item.QEs, item.AEs)).ToArray();
    }

    private sealed record Row(string QEn, string QEs, string AEn, string AEs);

    private static readonly Dictionary<string, Row[]> _pages = new(StringComparer.OrdinalIgnoreCase)
    {
        ["homepage"] =
        [
            new("What types of insurance does Carolinas Coverage Insurance Agency help with?",
                "¿Con qué tipos de seguro ayuda Carolinas Coverage Insurance Agency?",
                "The agency has confirmed auto, home/property, business liability, pet and life insurance services in North Carolina and South Carolina. Exact products and carriers should be confirmed for each applicant.",
                "La agencia confirmó servicios de seguro de auto, casa/propiedad, negocio y responsabilidad, mascotas y vida en Carolina del Norte y Carolina del Sur. Deben confirmarse los productos y compañías disponibles para cada solicitante."),
            new("Does the agency offer health, Medicare or disability insurance?",
                "¿La agencia ofrece seguro de salud, Medicare o disability?",
                "No. Health insurance, Medicare and disability insurance are not offered.",
                "No. La agencia no ofrece seguro de salud, Medicare ni disability."),
            new("Can I get help in Spanish?",
                "¿Puedo recibir ayuda en español?",
                "The website offers bilingual contact and quote paths. Confirm staff availability and hours before expecting immediate Spanish-language service.",
                "El sitio ofrece contacto y cotización bilingüe. Confirme la disponibilidad y horario del personal antes de esperar atención inmediata en español."),
            new("Can the agency compare multiple insurance carriers?",
                "¿La agencia puede comparar varias compañías de seguro?",
                "An agent can review available options after appointments are confirmed for the requested product and state. Availability is never guaranteed.",
                "Un agente puede revisar opciones disponibles después de confirmar los nombramientos para el producto y estado solicitados. La disponibilidad nunca está garantizada."),
            new("Can the agency guarantee the cheapest insurance?",
                "¿La agencia puede garantizar el seguro más barato?",
                "No responsible quote should guarantee the lowest price. Rates depend on underwriting, coverage, deductibles, location, history and carrier.",
                "Una cotización responsable no debe garantizar el precio más bajo. Las tarifas dependen de suscripción, cobertura, deducibles, ubicación, historial y compañía."),
            new("What information is needed for an insurance quote?",
                "¿Qué información se necesita para una cotización de seguro?",
                "It depends on the product, but commonly includes applicant details, address, prior coverage, drivers or employees, property or vehicle information, desired limits and relevant loss history.",
                "Depende del producto, pero normalmente incluye datos del solicitante, dirección, cobertura previa, conductores o empleados, información de propiedad o vehículo, límites deseados e historial de pérdidas relevante."),
            new("Is Carolinas Coverage the same as Carolina Coverage Group or Carolina Coverage Solutions?",
                "¿Carolinas Coverage es lo mismo que Carolina Coverage Group o Carolina Coverage Solutions?",
                "No. This office is Carolinas Coverage Insurance Agency at 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. It does not offer Medicare. Other businesses with similar names are not this agency.",
                "No. Esta oficina es Carolinas Coverage Insurance Agency en 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. No ofrece Medicare. Otros negocios con nombres parecidos no son esta agencia.")
        ],
        ["spanish-agent"] =
        [
            new("Can I get help in Spanish?",
                "¿Puedo recibir ayuda en español?",
                "The website offers bilingual contact and quote paths. Confirm staff availability and hours before expecting immediate Spanish-language service.",
                "El sitio ofrece contacto y cotización bilingüe. Confirme la disponibilidad y horario del personal antes de esperar atención inmediata en español."),
            new("What types of insurance can we discuss in Spanish?",
                "¿Qué tipos de seguro podemos conversar en español?",
                "The agency has confirmed auto, home/property, business liability, pet and life insurance in North Carolina and South Carolina. Health, Medicare and disability are not offered.",
                "La agencia confirmó auto, casa/propiedad, negocio y responsabilidad, mascotas y vida en Carolina del Norte y Carolina del Sur. No se ofrece salud, Medicare ni disability."),
            new("Can the agency guarantee the cheapest insurance?",
                "¿La agencia puede garantizar el seguro más barato?",
                "No. Rates depend on underwriting, coverage, deductibles, location, history and carrier.",
                "No. Las tarifas dependen de suscripción, cobertura, deducibles, ubicación, historial y compañía.")
        ],
        ["north-carolina"] =
        [
            new("What are North Carolina's current minimum auto liability limits?",
                "¿Cuáles son los límites mínimos actuales de auto en Carolina del Norte?",
                "For new or renewed policies effective on or after July 1, 2025, the NC Department of Insurance states 50/100/50. Verify the effective date and current rule before relying on this figure.",
                "Para pólizas nuevas o renovadas vigentes a partir del 1 de julio de 2025, el Departamento de Seguros de NC indica 50/100/50. Verifique la fecha de vigencia y la regla actual antes de depender de esta cifra."),
            new("What is a DL-123?",
                "¿Qué es un DL-123?",
                "A DL-123 is one form North Carolina accepts as proof of liability insurance for certain driver-license transactions. Ask the agency and NCDMV which proof applies to your situation.",
                "Un DL-123 es una forma que Carolina del Norte acepta como comprobante de responsabilidad civil para ciertos trámites de licencia. Consulte a la agencia y al NCDMV cuál comprobante corresponde."),
            new("When is workers' compensation generally required in North Carolina?",
                "¿Cuándo se requiere generalmente compensación laboral en Carolina del Norte?",
                "North Carolina generally requires it for businesses with three or more employees, subject to statutory exceptions and classification questions.",
                "Carolina del Norte generalmente la exige para negocios con tres o más empleados, sujeto a excepciones legales y preguntas de clasificación.")
        ],
        ["south-carolina"] =
        [
            new("What are South Carolina's minimum auto insurance limits?",
                "¿Cuáles son los límites mínimos de seguro de auto en Carolina del Sur?",
                "South Carolina lists minimum liability and uninsured motorist limits of 25/50/25. Underinsured motorist coverage must be offered but is not required.",
                "Carolina del Sur indica límites mínimos de responsabilidad civil y conductor sin seguro de 25/50/25. La cobertura para conductor con seguro insuficiente debe ofrecerse, pero no es obligatoria."),
            new("What proof is needed for a South Carolina road test?",
                "¿Qué comprobante se necesita para el examen de manejo de Carolina del Sur?",
                "SCDMV says the test vehicle needs a valid plate, registration and insurance card. Rental vehicles have additional contract requirements.",
                "SCDMV indica que el vehículo del examen necesita placa válida, registro y tarjeta de seguro. Los vehículos alquilados tienen requisitos adicionales del contrato."),
            new("When is workers' compensation generally required in South Carolina?",
                "¿Cuándo se requiere generalmente compensación laboral en Carolina del Sur?",
                "South Carolina generally requires it at four or more employees; part-time and family employees may count. Confirm exceptions with the Commission.",
                "Carolina del Sur generalmente la exige con cuatro o más empleados; empleados de medio tiempo y familiares pueden contar. Confirme excepciones con la Comisión.")
        ],
        ["auto"] =
        [
            new("What is liability-only auto insurance?",
                "¿Qué es seguro de auto solo responsabilidad civil?",
                "It generally pays covered bodily injury or property damage you cause to others, up to policy limits. It does not usually pay to repair your own vehicle after a collision.",
                "Generalmente paga lesiones corporales o daños materiales cubiertos que usted causa a otros, hasta los límites de la póliza. Normalmente no paga la reparación de su propio vehículo después de una colisión."),
            new("What does “full coverage” mean?",
                "¿Qué significa “cobertura completa”?",
                "“Full coverage” is not a single standardized policy term. People commonly mean liability plus collision and comprehensive coverage, but limits, deductibles and extras still vary.",
                "“Cobertura completa” no es un término único estandarizado. Normalmente significa responsabilidad civil más colisión y comprensiva, pero límites, deducibles y extras varían."),
            new("Do financed or leased cars need full coverage?",
                "¿Los carros financiados o arrendados necesitan cobertura completa?",
                "A lender or leasing company commonly requires collision and comprehensive coverage. Check the finance or lease contract for exact limits and deductibles.",
                "Un prestamista o arrendador comúnmente exige colisión y comprensiva. Revise el contrato de financiamiento o lease para límites y deducibles exactos."),
            new("Why did my car insurance price increase?",
                "¿Por qué aumentó el precio de mi seguro de auto?",
                "Possible factors include statewide rate changes, claims trends, driving history, vehicle, location, coverage, discounts or carrier underwriting. A policy review can identify the factors that apply.",
                "Los factores pueden incluir cambios estatales de tarifas, reclamos, historial de manejo, vehículo, ubicación, cobertura, descuentos o suscripción. Una revisión puede identificar lo que aplica."),
            new("Does auto insurance cover theft or hail?",
                "¿El seguro de auto cubre robo o granizo?",
                "Comprehensive coverage may cover theft or hail subject to the deductible, exclusions and claim facts. Liability-only coverage normally does not protect your own vehicle for those losses.",
                "La cobertura comprensiva puede cubrir robo o granizo sujeto al deducible, exclusiones y hechos del reclamo. Responsabilidad civil sola normalmente no protege su vehículo por esas pérdidas."),
            new("Does car insurance cover a borrowed car?",
                "¿El seguro de auto cubre un carro prestado?",
                "Coverage depends on the vehicle's policy, permissive use, the driver's policy, exclusions and the circumstances. Review both policies before relying on coverage.",
                "La cobertura depende de la póliza del vehículo, permiso de uso, póliza del conductor, exclusiones y circunstancias. Revise ambas pólizas antes de depender de cobertura.")
        ],
        ["liability-full"] =
        [
            new("What is liability-only auto insurance?",
                "¿Qué es seguro de auto solo responsabilidad civil?",
                "It generally pays covered bodily injury or property damage you cause to others, up to policy limits. It does not usually pay to repair your own vehicle after a collision.",
                "Generalmente paga lesiones corporales o daños materiales cubiertos que usted causa a otros, hasta los límites de la póliza. Normalmente no paga la reparación de su propio vehículo después de una colisión."),
            new("What does “full coverage” mean?",
                "¿Qué significa “cobertura completa”?",
                "“Full coverage” is not a single standardized policy term. People commonly mean liability plus collision and comprehensive coverage, but limits, deductibles and extras still vary.",
                "“Cobertura completa” no es un término único estandarizado. Normalmente significa responsabilidad civil más colisión y comprensiva, pero límites, deducibles y extras varían."),
            new("Do financed or leased cars need full coverage?",
                "¿Los carros financiados o arrendados necesitan cobertura completa?",
                "A lender or leasing company commonly requires collision and comprehensive coverage. Check the finance or lease contract for exact limits and deductibles.",
                "Un prestamista o arrendador comúnmente exige colisión y comprensiva. Revise el contrato de financiamiento o lease para límites y deducibles exactos.")
        ],
        ["dl-123"] =
        [
            new("What is a DL-123?",
                "¿Qué es un DL-123?",
                "A DL-123 is one form North Carolina accepts as proof of liability insurance for certain driver-license transactions. Ask the agency and NCDMV which proof applies to your situation.",
                "Un DL-123 es una forma que Carolina del Norte acepta como comprobante de responsabilidad civil para ciertos trámites de licencia. Consulte a la agencia y al NCDMV cuál comprobante corresponde."),
            new("Can I take a North Carolina road test without owning a car?",
                "¿Puedo hacer el examen de manejo de Carolina del Norte sin tener carro?",
                "You may need acceptable proof of liability insurance and an eligible vehicle for the test. Requirements depend on the transaction; confirm directly with NCDMV and the insurer.",
                "Puede necesitar un comprobante aceptable de responsabilidad civil y un vehículo elegible para el examen. Los requisitos dependen del trámite; confirme con NCDMV y la aseguradora.")
        ],
        ["non-owner"] =
        [
            new("What is non-owner car insurance?",
                "¿Qué es seguro de auto para no propietario?",
                "It is liability-focused coverage for some licensed drivers who do not own a vehicle. It does not insure a specific car and may not satisfy every road-test, registration or filing need.",
                "Es cobertura principalmente de responsabilidad civil para ciertos conductores con licencia que no poseen vehículo. No asegura un carro específico y puede no cumplir todo requisito de examen, registro o filing.")
        ],
        ["sr-22"] =
        [
            new("What is an SR-22?",
                "¿Qué es un SR-22?",
                "An SR-22 is a financial-responsibility filing that a state may require after certain violations or suspensions. It is not required for every driver.",
                "Un SR-22 es un comprobante de responsabilidad financiera que un estado puede exigir después de ciertas infracciones o suspensiones. No se requiere para todo conductor.")
        ],
        ["new-drivers"] =
        [
            new("What is a DL-123?",
                "¿Qué es un DL-123?",
                "A DL-123 is one form North Carolina accepts as proof of liability insurance for certain driver-license transactions. Ask the agency and NCDMV which proof applies to your situation.",
                "Un DL-123 es una forma que Carolina del Norte acepta como comprobante de responsabilidad civil para ciertos trámites de licencia. Consulte a la agencia y al NCDMV cuál comprobante corresponde."),
            new("What proof is needed for a South Carolina road test?",
                "¿Qué comprobante se necesita para el examen de manejo de Carolina del Sur?",
                "SCDMV says the test vehicle needs a valid plate, registration and insurance card. Rental vehicles have additional contract requirements. North Carolina uses different proof rules, including DL-123 in some cases.",
                "SCDMV indica que el vehículo del examen necesita placa válida, registro y tarjeta de seguro. Los vehículos alquilados tienen requisitos adicionales del contrato. Carolina del Norte usa reglas distintas, incluyendo DL-123 en algunos casos.")
        ],
        ["home-insurance"] =
        [
            new("Does homeowners insurance cover flood?",
                "¿El seguro de casa cubre inundación?",
                "Most standard homeowners policies do not cover flood. A separate flood policy may be needed for the building, contents or both.",
                "La mayoría de las pólizas estándar de casa no cubren inundación. Puede necesitar una póliza separada para el edificio, pertenencias o ambos."),
            new("What is replacement cost versus actual cash value?",
                "¿Qué es costo de reemplazo versus valor actual?",
                "Replacement cost generally does not subtract depreciation for covered replacement, while actual cash value generally does. Policy terms, limits and claim conditions control.",
                "El costo de reemplazo generalmente no resta depreciación para el reemplazo cubierto, mientras el valor actual normalmente sí. Mandan los términos, límites y condiciones de la póliza."),
            new("When should a home buyer arrange insurance?",
                "¿Cuándo debe un comprador de casa organizar el seguro?",
                "Start before closing so the lender and insurer have time to review the property, coverage, deductible and effective date. The lender or closing team may set a deadline.",
                "Empiece antes del cierre para que el prestamista y la aseguradora revisen propiedad, cobertura, deducible y fecha de vigencia. El prestamista o equipo de cierre puede fijar fecha límite."),
            new("Will homeowners insurance replace an old roof?",
                "¿El seguro de casa reemplazará un techo viejo?",
                "Not simply because the roof is old. Coverage depends on the cause of damage, policy form, exclusions, deductible, roof valuation and claim investigation.",
                "No solamente porque el techo sea viejo. La cobertura depende de la causa, formulario, exclusiones, deducible, valoración del techo e investigación del reclamo.")
        ],
        ["renters"] =
        [
            new("What does renters insurance cover?",
                "¿Qué cubre el seguro para inquilinos?",
                "It can include personal property, personal liability and additional living expense for covered losses. Limits, deductibles and exclusions vary.",
                "Puede incluir pertenencias, responsabilidad personal y gastos adicionales de vivienda por pérdidas cubiertas. Límites, deducibles y exclusiones varían."),
            new("Does a landlord's policy cover a tenant's belongings?",
                "¿La póliza del propietario cubre las pertenencias del inquilino?",
                "Usually the landlord's property policy is not designed to insure a tenant's personal belongings. Tenants should review renters insurance.",
                "Normalmente la póliza del propietario no está diseñada para asegurar las pertenencias personales del inquilino. Los inquilinos deben revisar seguro de renta.")
        ],
        ["flood"] =
        [
            new("Does homeowners insurance cover flood?",
                "¿El seguro de casa cubre inundación?",
                "Most standard homeowners policies do not cover flood. A separate flood policy may be needed for the building, contents or both.",
                "La mayoría de las pólizas estándar de casa no cubren inundación. Puede necesitar una póliza separada para el edificio, pertenencias o ambos.")
        ],
        ["pet"] =
        [
            new("Does pet insurance cover pre-existing conditions?",
                "¿El seguro de mascotas cubre condiciones preexistentes?",
                "Many policies exclude pre-existing conditions, though definitions and treatment of curable conditions can vary. Review the policy and medical-history rules.",
                "Muchas pólizas excluyen condiciones preexistentes, aunque las definiciones y el tratamiento de condiciones curables pueden variar. Revise póliza e historial médico."),
            new("What is a pet insurance waiting period?",
                "¿Qué es un período de espera del seguro de mascotas?",
                "It is the time after the effective date before certain coverage begins. Accident, illness and orthopedic waiting periods may differ.",
                "Es el tiempo después de la fecha de vigencia antes de que empiece cierta cobertura. Los períodos para accidentes, enfermedad y ortopedia pueden ser diferentes."),
            new("Is a wellness plan the same as pet insurance?",
                "¿Un plan de bienestar es igual al seguro de mascotas?",
                "No. A wellness program generally helps with routine care, while insurance addresses covered accident or illness losses. They may be sold together but should be explained separately.",
                "No. Un programa de bienestar generalmente ayuda con cuidado rutinario, mientras el seguro atiende pérdidas cubiertas por accidente o enfermedad. Pueden venderse juntos, pero deben explicarse por separado."),
            new("How are pet claims reimbursed?",
                "¿Cómo se reembolsan los reclamos de mascotas?",
                "Many plans reimburse a percentage of eligible veterinary costs after a deductible, subject to exclusions and limits. The exact calculation is policy-specific.",
                "Muchos planes reembolsan un porcentaje de costos veterinarios elegibles después del deducible, sujeto a exclusiones y límites. El cálculo exacto depende de la póliza.")
        ],
        ["business"] =
        [
            new("What insurance does a small business need?",
                "¿Qué seguro necesita un negocio pequeño?",
                "It depends on operations, contracts, employees, vehicles, property, professional services and cyber exposure. A risk review can identify general liability, BOP, workers' compensation, commercial auto and other needs.",
                "Depende de operaciones, contratos, empleados, vehículos, propiedad, servicios profesionales y exposición cibernética. Una revisión puede identificar responsabilidad civil, BOP, compensación laboral, auto comercial y otras necesidades."),
            new("What is a business owners policy?",
                "¿Qué es una póliza BOP para dueño de negocio?",
                "A BOP packages common coverages, often property and liability, for eligible businesses. It does not automatically include every exposure or professional, auto, cyber or workers' compensation need.",
                "Una BOP combina coberturas comunes, frecuentemente propiedad y responsabilidad, para negocios elegibles. No incluye automáticamente toda exposición ni necesidad profesional, auto, cyber o compensación laboral."),
            new("What is a certificate of insurance?",
                "¿Qué es un certificado de seguro?",
                "A COI summarizes specified policy information at a point in time. It does not amend coverage or grant rights by itself.",
                "Un COI resume información específica de la póliza en un momento dado. Por sí solo no modifica cobertura ni concede derechos.")
        ],
        ["general-liability"] =
        [
            new("What does general liability insurance cover?",
                "¿Qué cubre responsabilidad civil general?",
                "It can address certain third-party bodily injury, property damage and personal/advertising injury claims, subject to definitions, exclusions and limits.",
                "Puede atender ciertos reclamos de terceros por lesiones corporales, daño material y lesión personal/publicitaria, sujeto a definiciones, exclusiones y límites."),
            new("What is an additional insured?",
                "¿Qué es un asegurado adicional?",
                "It is a person or organization added under policy terms or endorsement for certain protection. A certificate alone does not necessarily create that status.",
                "Es una persona u organización añadida bajo términos o endoso para cierta protección. Un certificado por sí solo no necesariamente crea ese estado.")
        ],
        ["contractors"] =
        [
            new("What insurance does a small business need?",
                "¿Qué seguro necesita un negocio pequeño?",
                "It depends on operations, contracts, employees, vehicles, property, professional services and cyber exposure. Contractors often review general liability, commercial auto, workers' compensation and tools coverage.",
                "Depende de operaciones, contratos, empleados, vehículos, propiedad, servicios profesionales y exposición cibernética. Los contratistas suelen revisar responsabilidad civil, auto comercial, compensación laboral y herramientas."),
            new("What insurance protects contractor tools away from the shop?",
                "¿Qué seguro protege herramientas del contratista fuera del taller?",
                "Inland marine or contractor-equipment coverage may protect scheduled or covered tools in transit or at jobsites, subject to limits, valuation, security rules and exclusions.",
                "Inland marine o cobertura de equipo de contratista puede proteger herramientas programadas o cubiertas en tránsito o en trabajos, sujeto a límites, valoración, seguridad y exclusiones.")
        ],
        ["commercial-auto"] =
        [
            new("Does personal auto insurance cover a work vehicle?",
                "¿El seguro personal de auto cubre un vehículo de trabajo?",
                "Business use may be limited or excluded under a personal policy. Disclose ownership, use, drivers, radius, equipment and deliveries to determine whether commercial auto is appropriate.",
                "El uso comercial puede estar limitado o excluido en una póliza personal. Declare propietario, uso, conductores, radio, equipo y entregas para determinar si auto comercial corresponde.")
        ],
        ["workers-comp"] =
        [
            new("When is workers' compensation generally required in North Carolina?",
                "¿Cuándo se requiere generalmente compensación laboral en Carolina del Norte?",
                "North Carolina generally requires it for businesses with three or more employees, subject to statutory exceptions and classification questions.",
                "Carolina del Norte generalmente la exige para negocios con tres o más empleados, sujeto a excepciones legales y preguntas de clasificación."),
            new("When is workers' compensation generally required in South Carolina?",
                "¿Cuándo se requiere generalmente compensación laboral en Carolina del Sur?",
                "South Carolina generally requires it at four or more employees; part-time and family employees may count. Confirm exceptions with the Commission.",
                "Carolina del Sur generalmente la exige con cuatro o más empleados; empleados de medio tiempo y familiares pueden contar. Confirme excepciones con la Comisión.")
        ],
        ["life"] =
        [
            new("How much life insurance do I need?",
                "¿Cuánto seguro de vida necesito?",
                "Consider income replacement, debts, mortgage, education, caregiving, final expenses, existing assets and the time the need lasts. A quote is not a substitute for financial, tax or legal advice.",
                "Considere reemplazo de ingresos, deudas, hipoteca, educación, cuidado, gastos finales, activos existentes y duración de la necesidad. Una cotización no sustituye asesoría financiera, fiscal o legal."),
            new("Can I get life insurance without a medical exam?",
                "¿Puedo obtener seguro de vida sin examen médico?",
                "Some products use simplified or accelerated underwriting, but availability, questions, records, limits, price and approval vary. No-exam does not mean guaranteed acceptance.",
                "Algunos productos usan suscripción simplificada o acelerada, pero disponibilidad, preguntas, registros, límites, precio y aprobación varían. Sin examen no significa aprobación garantizada."),
            new("Can I change my life insurance beneficiary?",
                "¿Puedo cambiar el beneficiario de mi seguro de vida?",
                "Often yes if the beneficiary is revocable and ownership or legal restrictions do not prevent it. Use the carrier's form and keep confirmation.",
                "Frecuentemente sí si el beneficiario es revocable y no existen restricciones de propiedad o legales. Use el formulario de la compañía y guarde confirmación.")
        ],
        ["term"] =
        [
            new("What is term life insurance?",
                "¿Qué es seguro de vida a término?",
                "Term life provides a death benefit for a stated term if the policy is in force and the claim is covered. It generally does not build cash value.",
                "El seguro a término ofrece beneficio por fallecimiento durante un plazo si la póliza está vigente y el reclamo está cubierto. Generalmente no acumula valor en efectivo.")
        ],
        ["whole"] =
        [
            new("What is whole life insurance?",
                "¿Qué es seguro de vida entera?",
                "Whole life is permanent coverage designed to remain in force under its terms and can build cash value. Premiums, guarantees, loans and surrender values require policy-specific review.",
                "Vida entera es cobertura permanente diseñada para continuar según sus términos y puede acumular valor en efectivo. Primas, garantías, préstamos y valores de rescate requieren revisión específica.")
        ],
        ["final-expense"] =
        [
            new("What is final expense insurance?",
                "¿Qué es seguro de gastos finales?",
                "It is generally life insurance with a benefit intended to help beneficiaries with funeral and other final costs. Benefit amount, underwriting and any waiting period vary.",
                "Generalmente es seguro de vida con beneficio destinado a ayudar a beneficiarios con funeral y otros costos finales. Monto, suscripción y período de espera varían.")
        ],
        ["contact"] =
        [
            new("What information is needed for an insurance quote?",
                "¿Qué información se necesita para una cotización de seguro?",
                "It depends on the product, but commonly includes applicant details, address, prior coverage, drivers or employees, property or vehicle information, desired limits and relevant loss history.",
                "Depende del producto, pero normalmente incluye datos del solicitante, dirección, cobertura previa, conductores o empleados, información de propiedad o vehículo, límites deseados e historial de pérdidas relevante."),
            new("How do I report an auto claim?",
                "¿Cómo reporto un reclamo de auto?",
                "Protect people first, document the event, exchange required information, notify law enforcement when appropriate and contact the carrier or agency promptly. Coverage and payment depend on the policy and investigation.",
                "Proteja primero a las personas, documente el evento, intercambie la información requerida, avise a la policía cuando corresponda y contacte pronto a la compañía o agencia. Cobertura y pago dependen de la póliza e investigación.")
        ],
        ["charlotte"] =
        [
            new("Does Carolinas Coverage serve Charlotte, North Carolina?",
                "¿Carolinas Coverage atiende Charlotte, Carolina del Norte?",
                "Yes. The office is at 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. The agency also helps clients in South Carolina. Confirm the product and state for each request.",
                "Sí. La oficina está en 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. La agencia también ayuda a clientes en Carolina del Sur. Confirme el producto y el estado para cada solicitud."),
            new("Can I get help in Spanish in Charlotte?",
                "¿Puedo recibir ayuda en español en Charlotte?",
                "The website offers bilingual contact and quote paths. Call 704-595-3429 to confirm Spanish-language availability and hours.",
                "El sitio ofrece contacto y cotización bilingüe. Llame al 704-595-3429 para confirmar disponibilidad y horario en español."),
            new("Do you have a Rock Hill office?",
                "¿Tienen oficina en Rock Hill?",
                "No separate Rock Hill office is published. South Carolina applicants, including Rock Hill, may request help from the Charlotte office. Confirm the product and South Carolina rules for that risk.",
                "No se publica una oficina aparte en Rock Hill. Solicitantes de Carolina del Sur, incluido Rock Hill, pueden pedir ayuda a la oficina de Charlotte. Confirme el producto y las reglas de Carolina del Sur para ese riesgo.")
        ],
        ["about"] =
        [
            new("What is the legal name of this agency?",
                "¿Cuál es el nombre legal de esta agencia?",
                "Carolinas Coverage Insurance Agency. The office is at 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. Call 704-595-3429.",
                "Carolinas Coverage Insurance Agency. La oficina está en 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. Llame al 704-595-3429."),
            new("Is this the same company as Carolina Coverage Group or Carolina Coverage Solutions?",
                "¿Es la misma compañía que Carolina Coverage Group o Carolina Coverage Solutions?",
                "No. Those are other businesses. This agency does not offer Medicare, health insurance or disability insurance.",
                "No. Esos son otros negocios. Esta agencia no ofrece Medicare, seguro de salud ni disability."),
            new("What insurance does Carolinas Coverage help with?",
                "¿Con qué seguros ayuda Carolinas Coverage?",
                "Confirmed lines include auto, home and property, renters, pet, business, commercial auto and life in North Carolina and South Carolina. Availability depends on appointments, state and underwriting.",
                "Las líneas confirmadas incluyen auto, casa y propiedad, inquilinos, mascotas, negocio, auto comercial y vida en Carolina del Norte y Carolina del Sur. La disponibilidad depende de nombramientos, estado y suscripción.")
        ]
    };
}
