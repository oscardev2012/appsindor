using CalentonesAgua.Models;

namespace CalentonesAgua.Data;

internal static class SeoFaqBank
{
    public static IReadOnlyList<SeoFaq> Build(SeoPageDraft page, IReadOnlyList<SeoQuery> queries)
    {
        var spanish = page.Language.Equals("es", StringComparison.OrdinalIgnoreCase);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var faqs = new List<SeoFaq>();
        var cap = page.Priority == "P0" ? 12 : 8;

        foreach (var q in queries)
        {
            var question = spanish ? FirstNonEmpty(q.Es, q.En) : FirstNonEmpty(q.En, q.Es);
            if (string.IsNullOrWhiteSpace(question)) continue;
            if (!seen.Add(NormalizeQuestion(question))) continue;

            faqs.Add(new SeoFaq
            {
                Question = EnsureQuestionMark(question),
                Answer = Answer(page, q, spanish)
            });

            if (faqs.Count >= cap) break;
        }

        if (faqs.Count == 0)
        {
            faqs.Add(new SeoFaq
            {
                Question = spanish ? "¿Cómo cotizo con SAFE?" : "How do I get a SAFE quote?",
                Answer = spanish
                    ? "Envíe fotos del tanque, la placa de datos y las conexiones al 650-669-6890 o use el formulario de cotización. El paquete eléctrico de 40 galones es $1,199 + impuesto en un reemplazo estándar."
                    : "Text photos of the tank, data plate and connections to 650-669-6890 or use the photo-quote form. The 40-gallon electric package is $1,199 + tax for a standard replacement."
            });
        }

        return faqs;
    }

    private static string Answer(SeoPageDraft page, SeoQuery q, bool es)
    {
        var blob = $"{q.En} {q.Es} {q.Scenario} {q.Cluster} {q.Note}".ToLowerInvariant();
        var scenario = (q.Scenario + " " + q.Cluster).ToLowerInvariant();

        foreach (var rule in Rules)
        {
            if (rule.Needles.Any(n => blob.Contains(n, StringComparison.OrdinalIgnoreCase)))
                return es ? rule.Es : rule.En;
        }

        if (scenario.Contains("cercanía") || blob.Contains("service near me") || blob.Contains("cerca de mí") || blob.Contains("cerca de mi"))
            return es
                ? "SAFE opera desde Charlotte y atiende Shelby, China Grove, Mint Hill y el metro listado en la página de servicio local. Texto 650-669-6890 o llame 704-839-6126. No publicamos páginas de ciudad sin cobertura real."
                : "SAFE is Charlotte-based and serves Shelby, China Grove, Mint Hill and the metro listed on the service-area page. Text 650-669-6890 or call 704-839-6126. We do not publish empty city URLs.";

        return es ? CloserEs(page.Slug) : CloserEn(page.Slug);
    }

    private static string CloserEn(string slug) => slug switch
    {
        "/water-heater-emergency/" => "Treat this as safety first, then text photos only when the area is dry and the gas/CO risk is gone. Call 704-839-6126.",
        "/no-hot-water/" => "Identify electric vs gas before resetting anything twice. Text a data-plate photo if hot water does not return in one cycle.",
        "/water-heater-leaking/" => "Photograph the origin of the water, then the full tank. SAFE will say repair vs the $1,199–$1,349 replacement packages.",
        "/water-heater-buying-guide/" => "Buy the fuel your home already supports unless you are paying for a real conversion. Start with the published Charlotte packages.",
        "/what-size-water-heater-do-i-need/" => "Send household size, bathrooms and a photo of the current label. We will not upsell tankless the panel cannot feed.",
        _ => "Text 650-669-6890 or call 704-839-6126 with photos so SAFE can match a published package or list extras in writing."
    };

    private static string CloserEs(string slug) => slug switch
    {
        "/es/calentadores-de-agua-charlotte-nc/" => "Llame al 704-839-6126 o envíe fotos al 650-669-6890. El paquete eléctrico de 40 galones es $1,199 + impuesto cuando la instalación es estándar.",
        "/es/reemplazo-calentador-agua-charlotte-nc/" => "Mande fotos del tanque y la placa. Confirmamos si aplica el paquete de $1,199 o hay extras de código, ático o gas.",
        _ => "Texto 650-669-6890 o llame 704-839-6126 con fotos para cotizar el paquete publicado o listar extras."
    };

    private static readonly FaqRule[] Rules =
    [
        new(["plumber near me", "plomero cerca", "licensed plumber", "emergency plumber"],
            "SAFE is a water-heater plumber in the Charlotte metro—not a 24-hour drain or sewer company. For replacement, no hot water or a leaking tank, text 650-669-6890 or call 704-839-6126. Licensed contracting is through HOME INDOR TECHNOLOGY INC; we do not post a fake license number here.",
            "SAFE es plomero de calentadores en el metro de Charlotte, no una empresa de drenajes 24/7. Texto 650-669-6890 o 704-839-6126. El trabajo lo hace HOME INDOR TECHNOLOGY INC."),
        new(["open now", "abierto ahora", "24 hour water heater", "24-hour water heater", "24/7", "24-7"],
            "We do not advertise 24 hour water heater repair or “installer open now” coverage we cannot staff. Same-day replacement is possible when a crew is free. Gas or CO: 911 first. For a leak, shut water off if safe and call 704-839-6126.",
            "No anunciamos reparación 24 horas. El mismo día es posible si hay cuadrilla. Gas o CO: 911 primero. Fuga: cierre el agua si es seguro y llame al 704-839-6126."),
        new(["free water heater", "free estimate", "cotización gratis", "quote from pictures", "quote from photos"],
            "A SAFE estimate is a photo quote: tank, data plate, connections and access. It is free to send photos. It is not a blank promise of a free truck roll. Use the quote form or text 650-669-6890.",
            "La cotización SAFE es por fotos, sin costo enviarlas. No es una visita gratis en blanco. Formulario o texto 650-669-6890."),
        new(["hot water heater", "calentón", "boiler de agua"],
            "Hot water heater is the same appliance as a water heater. SAFE’s Charlotte installed packages start at $1,199 + tax for 40-gal electric.",
            "Calentón, boiler de agua y water heater son el mismo equipo. El paquete de 40 gal eléctrico en Charlotte es $1,199 + impuesto."),
        new(["affordable", "cheap", "barato", "coupon", "deals", "specials"],
            "SAFE publishes installed packages ($1,199 / $1,349 + tax) instead of bait “cheap water heater” ads. Extras are written from photos. We do not run fake coupons.",
            "SAFE publica paquetes instalados, no anuncios de “barato.” Los extras van por escrito."),
        new(["licensed water heater", "licensed installer", "licencia"],
            "Work is performed by HOME INDOR TECHNOLOGY INC, a Charlotte-based contractor. We do not invent a license number on marketing pages. Ask for the written scope and company name on the invoice.",
            "El trabajo lo realiza HOME INDOR TECHNOLOGY INC. No inventamos un número de licencia en la web. Pida el alcance por escrito."),
        new(["rebate", "duke energy", "reembolso", "duke"],
            "SAFE does not publish a Duke Energy rebate amount we cannot verify this week. Check Duke Energy’s current water-heater program, then we put the installed package in writing. SAFE Plan and Synchrony are separate from any utility rebate.",
            "SAFE no publica un reembolso de Duke Energy que no pueda verificar esta semana. Consulte el programa actual de Duke. El plan SAFE y Synchrony son aparte de cualquier reembolso."),
        new(["smell gas", "olor a gas", "carbon monoxide", "monóxido", "co alarm", "yellow flame", "llama amarilla", "soot", "hollín"],
            "Do not operate switches or restart the heater. Leave the home and call 911 or the gas utility. Soot, a yellow flame and CO alarms are combustion problems. SAFE replaces the unit after the home is cleared.",
            "No encienda interruptores ni reinicie el calentador. Salga y llame al 911 o al proveedor de gas. Hollín, llama amarilla y alarmas de CO son combustión."),
        new(["turn off power", "corto la electricidad", "how do i turn off power"],
            "From a dry location, switch off the dedicated 240V breaker labeled water heater. Do not stand in water. A hot or tripping breaker stays off until a technician checks it.",
            "Desde un lugar seco, apague el breaker de 240V del calentador. No se pare en el agua. Un breaker caliente se deja apagado."),
        new(["shut off gas", "cierro el gas", "how do i shut off gas"],
            "If you smell gas, leave instead of hunting for the valve. If the area is clear, the gas shutoff is usually on the line to the control. Do not create sparks.",
            "Si huele a gas, salga; no busque la válvula. Si el área está despejada, el cierre suele estar en la línea al control."),
        new(["shutoff valve", "válvula para cerrar", "where is the water heater shutoff"],
            "The tank shutoff is usually on the cold inlet on top of the heater. If you cannot reach it safely, use the home’s main water shutoff, then isolate power or gas from a dry place.",
            "La válvula del tanque suele estar en la entrada de frío arriba. Si no llega con seguridad, use la llave general de la casa."),
        new(["spark", "chispa", "burned wire", "cable quemado", "wires on my water heater getting hot"],
            "Do not touch the heater if water or damaged wires are present. Switch the breaker off from a dry location. Do not keep resetting it. SAFE will not ask a homeowner to open live 240V covers.",
            "No toque el calentador si hay agua o cables dañados. Corte el breaker desde un lugar seco. No lo resetee."),
        new(["flood", "inund", "bulging", "abultado", "steam coming", "vapor", "boiling", "hirviendo", "pressure relief valve is releasing"],
            "Stay clear of T&P discharge and never cap that pipe. Shut water and energy off if it is safe. A deformed tank or uncontrolled steam is replacement, not sealant.",
            "Aléjese de la descarga T&P y nunca la tape. Cierre agua y energía si es seguro. Un tanque deformado se reemplaza."),
        new(["overnight", "dejar encendido"],
            "Do not leave a leaking heater powered overnight. Shut cold water and energy if safe, keep people away from standing water, then call SAFE.",
            "No deje encendido un calentador con fuga. Cierre agua y energía si es seguro y llame a SAFE."),
        new(["bottom of", "leaking from the bottom", "desde abajo", "tank body", "shell leak"],
            "Water from the steel tank body, especially with rust at the bottom, means replacement. Do not epoxy a pressure vessel. Photograph the origin, then the full tank.",
            "Agua del cuerpo de acero, sobre todo abajo con óxido, es reemplazo. No parche el tanque. Foto del origen y del tanque completo."),
        new(["t&p", "t and p", "relief valve", "válvula de alivio", "discharge pipe"],
            "Continuous T&P discharge is pressure or temperature—never cap the pipe. A brief drip after heating can be expansion. Send a photo of the pipe and whether water is also at the tank jacket.",
            "Descarga continua de T&P es presión o temperatura: nunca tape el tubo. Un goteo breve puede ser expansión. Foto del tubo y del tanque."),
        new(["can it wait", "until tomorrow", "can it burst", "explot", "repair the tank", "se puede reparar el tanque"],
            "A tank-body leak or T&P dumping water should not wait overnight. Tanks rarely explode like in movies, but a failed shell can dump 40–50 gallons fast. Repair means valves and fittings on a sound jacket—not a patched shell.",
            "Una fuga del cuerpo o T&P tirando agua no espera al día siguiente. Reparar es válvulas y conexiones en un tanque sano, no parchar el casco."),
        new(["leaking", "fuga", "leak", "goteando", "tirando agua"],
            "Shut the cold-water inlet if you can without standing in water next to electricity. Water from the tank body means replacement. A loose fitting can sometimes be repaired. Photograph where the leak starts.",
            "Cierre la entrada de frío si puede sin pisar agua junto a electricidad. Agua del cuerpo del tanque = reemplazo. Foto del origen de la fuga."),
        new(["how long do", "how long does", "lifespan", "cuánto duran", "cuanto duran", "years old", "años tiene", "serial number age"],
            "Typical tank life in Charlotte hard water is about 8–15 years. At 8, a single part repair can still make sense. Near 10–12, compare the part quote to $1,199–$1,349 installed. At 15, plan replacement. Tankless life depends on descaling, not a jacket.",
            "En el agua dura de Charlotte un tanque suele durar 8–15 años. A los 8, una pieza puede valer. A los 10–12 compare con $1,199–$1,349 instalado. Tankless depende del descalcificado."),
        new(["lowe's", "lowes", "home depot", "big box", "ferretería", "buy my own", "already bought"],
            "A store tank is not an installed job. SAFE publishes installed packages ($1,199 / $1,349 + tax). If you already bought a unit, we still need photos: wrong voltage, vent or size can cost more than the package.",
            "Un tanque de tienda no es un trabajo instalado. SAFE publica paquetes instalados. Si ya compró el equipo, igual necesitamos fotos: voltaje o ventilación incorrectos salen más caros."),
        new(["cash price", "upfront", "de contado", "quote too high", "too expensive", "why is it so much", "labor only"],
            "Published figures are cash installed packages plus tax. A higher number usually means attic, extra pipe (from $199), expansion tank (from $149), electrical/gas work or relocation. Labor-only without photos is incomplete. Financing is optional, not a second hidden price.",
            "Los precios publicados son paquetes de contado + impuesto. Un número más alto suele ser ático, tubería extra, expansión o gas/eléctrico. Financiamiento es opcional."),
        new(["permit included", "permiso incluido", "does the price include permit"],
            "When your jurisdiction requires a permit, SAFE confirms it on the written quote for that address. It is separate from pans and expansion tanks. See the Charlotte permit page for process.",
            "Si su jurisdicción exige permiso, SAFE lo confirma en la cotización de esa dirección. No es lo mismo que charola o tanque de expansión."),
        new(["how much", "cost", "cuánto", "cuesta", "price", "precio", "labor", "mano de obra"],
            "40-gal electric installed: $1,199 + tax. 50-gal electric: $1,349 + tax. 40-gal gas: from $1,449 + tax. Expansion tank from $149. Attic, code and hard access are extra from photos.",
            "40 gal eléctrico instalado: $1,199 + impuesto. 50 gal: $1,349. Gas 40 gal desde $1,449. Expansión desde $149. Ático y código se cotizan con fotos."),
        new(["same day", "mismo día", "weekend", "fin de semana"],
            "Same-day is possible when a crew, the unit and safe access exist that day—not a 24/7 promise. Morning photos help. Weekend or after-hours, if offered, is disclosed in writing first.",
            "El mismo día es posible si hay cuadrilla, equipo y acceso—no es 24/7. Fotos por la mañana ayudan."),
        new(["40 gallon", "40-gal", "40 galones"],
            "The 40-gallon electric package is $1,199 + tax ($200 promotional unit + $999 standard install). If showers run out, compare the 50-gallon package at $1,349 + tax.",
            "El paquete eléctrico de 40 galones es $1,199 + impuesto. Si se acaban las duchas, compare 50 galones a $1,349 + impuesto."),
        new(["50 gallon", "50-gal", "50 galones"],
            "The 50-gallon electric installed package is $1,349 + tax for a confirmed standard replacement. It is the usual step up when a 40-gallon tank cannot cover overlapping showers.",
            "El paquete eléctrico de 50 galones es $1,349 + impuesto. Es el salto cuando un tanque de 40 no cubre duchas simultáneas."),
        new(["what size", "tamaño", "first hour", "how many gallon"],
            "Size by people, overlapping bathrooms and First Hour Rating—not square footage. 1–2 people often fit 40-gal electric; families stacking showers usually need 50.",
            "Se dimensiona por personas, baños simultáneos y First Hour Rating, no por pies cuadrados."),
        new(["permit", "permiso"],
            "Charlotte and nearby jurisdictions may require a permit and inspection depending on fuel and whether the job is like-for-like. SAFE confirms the current rule for your address.",
            "Charlotte y vecinos pueden exigir permiso e inspección. SAFE lo confirma para su dirección porque las reglas cambian."),
        new(["expansion", "expansión", "include old", "retiro", "removal"],
            "Old-tank removal is in the standard scope when access allows. An expansion tank (from $149) is added if the plumbing is closed or code requires it.",
            "El retiro del tanque viejo entra en el alcance estándar si el acceso lo permite. El tanque de expansión (desde $149) se agrega si el sistema es cerrado o el código lo pide."),
        new(["financ", "synchrony", "$200"],
            "Pay cash, request the SAFE Homeowner Plan (from $200 toward a qualifying promotional unit, subject to eligibility), or apply with Synchrony on the HOME INDOR TECHNOLOGY INC merchant account. A form is not approval.",
            "Pague de contado, pida el plan SAFE (desde $200 hacia un equipo promocional calificado) o Synchrony. El formulario no es aprobación."),
        new(["warranty", "garantía", "insurance", "seguro"],
            "Qualifying units: limited manufacturer warranty (typically 5 years on our packages; the model booklet controls) plus 2 years of SAFE labor on our installation. Insurance covers structure, not the tank. Register the serial number.",
            "Equipo calificado: garantía limitada de fabricante (típicamente 5 años; manda el folleto) más 2 años de mano de obra SAFE. El seguro cubre la vivienda, no el tanque."),
        new(["popping", "rumbling", "noise", "ruido", "banging", "whistle"],
            "Popping or rumbling while heating is usually sediment—common with Charlotte hard water. A short video and the data plate tell us whether maintenance or replacement is smarter.",
            "Palomitas o retumbo al calentar suelen ser sedimento, común con el agua dura de Charlotte. Un video y la placa de datos ayudan a decidir."),
        new(["rotten egg", "smelly", "rusty", "oxid", "discolor", "olor a huevo", "sulfur"],
            "Compare hot and cold. If only hot is affected, the tank or anode is the suspect. A gas smell is not smelly water—evacuate. Do not drink rusty hot water.",
            "Compare caliente y fría. Si solo la caliente está mal, el tanque o el ánodo. Olor a gas: evacúe. No beba agua caliente oxidada."),
        new(["repair or replace", "reparar o", "should i repair"],
            "Repair when the shell is sound, the unit is relatively new, and the part is replaceable. Replace when the tank leaks, corrosion is advanced, or repair nears the $1,199–$1,349 installed packages.",
            "Repare si el casco está sano y la pieza es reemplazable. Reemplace si el tanque gotea o la reparación se acerca a $1,199–$1,349 instalados."),
        new(["not enough", "run out", "poca agua", "se acaba", "runs out"],
            "Running out mid-shower is capacity, a failed lower element, sediment, a broken dip tube, or overlapping fixtures—not the same as no hot water at all. Many Charlotte families step up to 50-gal electric.",
            "Que se acabe a mitad de ducha es capacidad, elemento inferior, sedimento o uso simultáneo. En Charlotte muchas familias pasan a 50 galones."),
        new(["low pressure", "poca presión", "low hot water pressure"],
            "If only hot is weak, start with aerators and the tank isolation valves. If the whole house is weak, it may be a PRV or supply issue—not a bigger tank.",
            "Si solo la caliente es débil, empiece por aireadores y las válvulas del tanque. Si toda la casa está débil, puede ser PRV o la red."),
        new(["electric water heater not", "breaker", "element", "thermostat", "reset"],
            "Electric no-heat: dedicated 240V breaker, high-limit reset, elements and thermostats. Repeated trips or a hot breaker: leave it off. A 12-year leaking tank is usually replaced, not restocked with parts.",
            "Eléctrico sin calor: breaker 240V, reset, elementos y termostatos. Si el breaker salta otra vez, déjelo apagado."),
        new(["gas water heater not", "pilot", "ignition", "thermocouple"],
            "A gas tank that will not heat is often pilot/ignition, thermocouple, gas control or venting. If you smell gas or see soot, stop and evacuate. Gas installs start from $1,449 + tax like-for-like.",
            "Un tanque a gas que no calienta suele ser piloto, ignición, termopar, válvula o ventilación. Olor a gas: evacúe. Instalación a gas desde $1,449 + impuesto."),
        new(["no hot water", "sin agua caliente", "not working", "no funciona", "not heating"],
            "Start with fuel type. Electric: breaker, high-limit, elements. Gas: pilot, ignition, valve, vent. If the breaker trips again or you smell gas, stop. SAFE diagnoses or quotes replacement from photos.",
            "Empiece por el combustible. Eléctrico: breaker y elementos. Gas: piloto y ventilación. Si el breaker salta o huele a gas, deténgase."),
        new(["tankless"],
            "Tankless needs real winter GPM, enough gas or amperage, and descaling in Charlotte hard water. It is not a drop-in for a 40-gallon closet. Photograph error codes; gas odor is an emergency.",
            "Tankless necesita GPM de invierno, gas o amperaje suficientes y desincrustado. No es un recambio directo de 40 galones. Olor a gas es emergencia."),
        new(["attic", "ático", "crawl", "difficult", "difícil", "tight"],
            "Attic, crawlspace and tight closets do not automatically get the $1,199 closet price. Send photos of the removal path, pan and hatch. Extra labor is written before we schedule.",
            "Ático, crawlspace y closets estrechos no usan el $1,199 de clóset automáticamente. Envíe fotos de la ruta, la charola y la escotilla."),
        new(["realtor", "closing", "inspection"],
            "Send the property address, access notes and the inspection line. SAFE returns a written scope buyers and sellers can attach. We are not a home inspector and do not promise same-day unless a crew is free.",
            "Envíe dirección, acceso y la nota de inspección. SAFE entrega un alcance escrito. No somos inspectores de vivienda."),
        new(["property manager", "tenant", "multi"],
            "SAFE can replace tanks across a small portfolio with photo intake, published packages and a warranty record per address. We will not write a 1-hour 24/7 SLA we cannot staff.",
            "Podemos reemplazar tanques en un portafolio pequeño con fotos, paquetes publicados y garantía por dirección. No prometemos un SLA 24/7 que no podamos cumplir."),
        new(["heat pump", "hybrid"],
            "A heat-pump water heater needs air volume, a condensate drain and a place that can tolerate fan noise. Tiny closets usually fail. Quoted as custom work, not the $1,199 electric tank package.",
            "Un calentador híbrido necesita volumen de aire, desagüe de condensado y tolerancia al ruido. Un clóset mínimo no sirve. Se cotiza aparte del paquete de $1,199."),
        new(["mobile", "manufactured", "well water", "condo", "hoa"],
            "Manufactured homes, wells, hard water and HOA closets are custom quotes. Send the HUD label, well notes or HOA spec with the tank photos. A standard residential tank may not be legal in a mobile-home closet.",
            "Casas manufacturadas, pozos y HOA se cotizan con fotos y documentos. Un tanque residencial estándar puede no ser legal en un clóset de casa móvil."),
        new(["new water heater not", "just installed", "brand new"],
            "A new tank that is cold is often power/gas not on, a dry-fire high-limit, air in lines, or a closed isolation valve. If SAFE installed it, use warranty support instead of a second contractor.",
            "Un tanque nuevo frío suele ser energía apagada, reset por arranque en seco o una válvula cerrada. Si lo instaló SAFE, use la garantía.")
    ];

    private static string FirstNonEmpty(string a, string b) =>
        string.IsNullOrWhiteSpace(a) ? b : a;

    private static string NormalizeQuestion(string q) =>
        new string(q.ToLowerInvariant().Where(char.IsLetterOrDigit).ToArray());

    private static string EnsureQuestionMark(string q)
    {
        q = q.Trim();
        if (q.EndsWith('?') || q.EndsWith('¿')) return q;
        if (q.StartsWith('¿')) return q + "?";
        return q + "?";
    }
}

internal readonly record struct FaqRule(string[] Needles, string En, string Es);
