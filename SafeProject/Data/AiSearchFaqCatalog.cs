using CalentonesAgua.Models;

namespace CalentonesAgua.Data;

/// <summary>
/// Questions people type into Google, People Also Ask and chat tools
/// (how much, how do I, near me, vs). Answers lead with the fact.
/// </summary>
public static class AiSearchFaqCatalog
{
    public static IReadOnlyList<AiFaqGroup> Groups { get; } =
    [
        new("cost", "How much / cost", "Cuánto cuesta",
        [
            Q("How much does water heater replacement cost in Charlotte NC?",
                "A standard like-for-like 40-gallon electric replacement with SAFE is $1,199 + tax when access is normal. A 50-gallon electric package is $1,349 + tax. Gas 40-gallon starts at $1,449 + tax; venting, gas line, attic, code and extra pipe are written extras.",
                "/water-heater-cost-charlotte-nc/"),
            Q("How much is a 40 gallon electric water heater installed?",
                "SAFE’s published 40-gallon electric package is $1,199 + tax: a qualifying promotional unit plus standard professional installation. Expansion tank from $149. Extra piping from $199.",
                "/40-gallon-electric-water-heater-installation/"),
            Q("Is $1,199 the tank only or installed?",
                "Installed. It is not a tank-only teaser. Non-standard work (attic, relocation, leak damage, code) is approved in writing before SAFE schedules.",
                "/water-heater-cost-charlotte-nc/"),
            Q("Does a water heater replacement need a permit in Charlotte?",
                "Many replacements need a permit or inspection depending on fuel, location and whether the job is like-for-like. SAFE reviews code items in the photo quote instead of guessing on the phone.",
                "/water-heater-permit-charlotte-nc/")
        ]),
        new("how", "How to / what should I do", "Qué hago",
        [
            Q("What should I do if my water heater is leaking?",
                "Shut off the water supply and the breaker or gas control if it is safe. Water from a fitting may be repairable; water from the tank body usually means replace. Text dry photos to 650-669-6890. Do not stand in water near electric parts.",
                "/water-heater-leaking/"),
            Q("Why is my water heater not producing hot water?",
                "Electric tanks often fail on a breaker, element or thermostat. Gas tanks fail on ignition, control or venting. If you smell gas or a CO alarm sounds, leave and call 911 or the utility—do not troubleshoot.",
                "/no-hot-water/"),
            Q("Should I repair or replace my water heater?",
                "Repair a newer tank with a failed part. Replace if the tank leaks, corrosion is advanced, repair cost is high, or the unit is near end of life. SAFE says which after photos, not a sales script.",
                "/repair-or-replace-water-heater/"),
            Q("How long does water heater replacement take?",
                "A typical like-for-like tank swap is about 2–4 hours on site. Gas, attic, code corrections and relocation take longer. Same-day only if a crew and parts are actually free.",
                "/water-heater-installation-process/"),
            Q("What temperature should a water heater be set to?",
                "About 120°F is the usual starting point for scald risk and energy use. Mixing valves or sanitation needs can change that. Do not keep raising the dial to hide a failing tank.",
                "/water-heater-maintenance/")
        ]),
        new("which", "Which / vs", "Cuál elegir",
        [
            Q("Do I need a 40 or 50 gallon water heater?",
                "40-gallon electric fits many smaller Charlotte homes. More bathrooms or overlapping showers often need 50 gallons or a different recovery rate. Send household size and the data-plate photo; SAFE will not upsell tankless the panel cannot feed.",
                "/what-size-water-heater-do-i-need/"),
            Q("Tankless vs tank water heater in Charlotte?",
                "Tankless needs gas or electrical capacity, venting or condensate, and a custom quote. It is not the $1,199 electric package. Most like-for-like jobs stay tank unless the home is ready for the upgrade.",
                "/tankless-water-heater-installation/"),
            Q("Gas vs electric water heater replacement?",
                "Keep the fuel the home already has unless you are paying for a real conversion. Electric packages start at $1,199 + tax. Gas 40-gallon starts at $1,449 + tax plus venting or line work if needed.",
                "/gas-water-heater-installation/")
        ]),
        new("near", "Near me / Charlotte", "Cerca de mí",
        [
            Q("Who installs water heaters near me in Charlotte?",
                "SAFE Water Heater is a Charlotte-based specialist (HOME INDOR TECHNOLOGY INC), not a 24-hour drain plumber. We replace tanks across Charlotte, Matthews, Mint Hill, Huntersville, Gastonia, Concord, Shelby and China Grove. Call 704-839-6126 or text 650-669-6890.",
                "/water-heater-service-near-me/"),
            Q("Are you open 24 hours for water heater emergencies?",
                "No. SAFE does not advertise 24/7 dispatch. Gas odor or CO alarm: leave and call 911 or the gas utility. For a leaking or dead tank during published hours, call 704-839-6126.",
                "/water-heater-emergency/"),
            Q("How do I get a water heater quote from SAFE?",
                "Text photos of the tank, data plate and connections to 650-669-6890 or use the quote form. SMS is optional. Call 704-839-6126 if you do not want texts.",
                "/water-heater-replacement-quote/")
        ]),
        new("pay", "Pay / warranty", "Pago y garantía",
        [
            Q("Does SAFE offer water heater financing?",
                "Two paths: SAFE Homeowner Plan (start with $200 toward a qualifying promotional unit, subject to eligibility) and Synchrony through HOME INDOR TECHNOLOGY INC. Neither is guaranteed approval.",
                "/water-heater-financing/"),
            Q("What warranty comes with a SAFE water heater install?",
                "The qualifying unit may include a five-year limited manufacturer warranty (model booklet). SAFE adds a two-year limited installation labor warranty on qualifying workmanship after inspection.",
                "/water-heater-insurance-warranty/")
        ]),
        new("es", "Preguntas en español", "Preguntas en español",
        [
            Q("¿Cuánto cuesta cambiar el calentador de agua en Charlotte?",
                "El paquete eléctrico de 40 galones con instalación estándar es $1,199 + impuesto. El de 50 galones es $1,349 + impuesto. Gas desde $1,449 + impuesto. Ático, código y tubería extra se cotizan por escrito.",
                "/es/reemplazo-calentador-agua-charlotte-nc/"),
            Q("¿Cómo cotizo con SAFE?",
                "Envíe fotos del tanque y la placa al 650-669-6890 o llame al 704-839-6126. El SMS no es obligatorio. SAFE Water Heater es la marca de HOME INDOR TECHNOLOGY INC.",
                "/es/calentadores-de-agua-charlotte-nc/")
        ])
    ];

    public static IReadOnlyList<SeoFaq> AllFaqs { get; } = Groups
        .SelectMany(g => g.Items)
        .Select(i => new SeoFaq { Question = i.Question, Answer = i.Answer })
        .ToArray();

    private static AiFaqItem Q(string question, string answer, string more) =>
        new(question, answer, more);
}

public sealed record AiFaqGroup(string Id, string Label, string EsLabel, IReadOnlyList<AiFaqItem> Items);

public sealed record AiFaqItem(string Question, string Answer, string MoreHref);
