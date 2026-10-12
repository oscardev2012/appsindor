using CalentonesAgua.Models;

namespace CalentonesAgua.Data;

internal static class SeoCopy
{
    internal const string TextHref = SiteContact.TextHref;
    internal const string CallHref = SiteContact.CallHref;
    internal const string QuoteHref = "/water-heater-replacement-quote/";
    internal const string Areas =
        "Charlotte, Shelby, China Grove, Mint Hill, Gastonia, Kings Mountain, Cherryville, Belmont, Mount Holly, Cornelius, Huntersville, Concord, Kannapolis, Salisbury, Mooresville, Matthews and Rock Hill.";

    internal static IReadOnlyList<SeoPageDraft> All() =>
    [
        P0Replacement(),
        P0Cost(),
        P040Gallon(),
        P050Gallon(),
        P0SameDay(),
        P0Emergency(),
        P0Leaking(),
        P0NoHotWater(),
        P0RepairOrReplace(),
        P0NearMe(),
        P0SpanishHub(),
        P0SpanishReplacement(),
        P1ElectricNotWorking(),
        P1GasNotWorking(),
        P1NotEnough(),
        P1Noise(),
        P1Smelly(),
        P1Pressure(),
        P1Size(),
        P1BuyingGuide(),
        P1GasInstall(),
        P1TanklessInstall(),
        P1Financing(),
        P1Code(),
        P1Permit(),
        P1Attic(),
        P1DifficultAccess(),
        P1Process(),
        P1Warranty(),
        P1NewNotWorking(),
        P1Quote(),
        P1Realtor(),
        P1PropertyManager(),
        P2TanklessProblems(),
        P2Maintenance(),
        P2HeatPump(),
        P2Special(),
        P2LeakDamage()
    ];

    private static SeoPageDraft P0Replacement() => new(
        "P0", "en", "/water-heater-replacement-charlotte-nc/",
        "Water Heater Replacement & Installation in Charlotte, NC",
        "Water Heater Replacement Charlotte NC | Installation from $1,199 | SAFE",
        "Water heater replacement and installation in Charlotte, NC—also called a hot water heater swap. Published installed pricing from $1,199 + tax. Photo quotes, old-tank removal, 5-year manufacturer + 2-year SAFE labor. Not a 24-hour plumber.",
        "Reemplazo local", "Cotización e instalación", "CHARLOTTE REPLACEMENT",
        "SAFE is the Charlotte water-heater company to call for replacement and installation—not a general plumber for drains. The published 40-gallon electric package is $1,199 + tax. Homeowners searching water heater replacement Charlotte NC, installation, or a hot water heater quote send photos first so we confirm fuel, size and extras before a crew is scheduled.",
        "Get a photo quote", QuoteHref, "Text 650-669-6890", TextHref, false, true, false,
        "/es/reemplazo-calentador-agua-charlotte-nc/",
        [
            S("Replacement, installation and repair—what we actually sell",
                "Most “water heater repair Charlotte NC” and “affordable water heater installation” searches still end in a like-for-like replacement. SAFE quotes repair only when the tank shell is sound. Installation and replacement are the same job on a tank swap: new unit, old-tank haul-away, startup. We do not run 24-hour water heater repair or “open now” dispatch we cannot staff. Licensed contracting is through HOME INDOR TECHNOLOGY INC—we do not invent a license number on this page. A free water heater replacement estimate means a photo quote, not a blank truck roll."),
            S("Installed pricing we publish",
                "Homeowners searching for water heater replacement in Charlotte, NC usually want a real installed number—not a teaser for an uninstalled tank. SAFE publishes current standard packages and explains what is extra.",
                [
                    "40-gallon electric package: $1,199 + tax ($200 promotional unit + $999 standard installation).",
                    "50-gallon electric: $1,349 + tax for a confirmed standard replacement.",
                    "40-gallon gas: from $1,449 + tax, subject to venting, gas line and combustion-air conditions.",
                    "Expansion tank: from $149; additional piping from $199 when the existing run cannot be reused."
                ]),
            S("Electric, gas and other types",
                "Most Charlotte-area replacements are like-for-like electric or gas tanks. Tankless, power-vent and heat-pump units are quoted after we review electrical or gas capacity, venting, condensate and space. We do not force a conversion unless the home needs it and you approve the extra work in writing."),
            S("Replacement process",
                "A typical like-for-like tank replacement takes about 2–4 hours when access is normal and existing connections are suitable. The crew removes the old tank, sets the new unit, connects water and energy, starts it, tests and walks you through the valves.",
                [
                    "Text address, fuel, size and photos of the tank, label, connections and access path.",
                    "SAFE confirms standard scope or lists extras (pan, T&P pipe, expansion tank, disconnect, venting, relocation).",
                    "You choose cash, the SAFE Homeowner Plan or Synchrony.",
                    "We schedule, install, register warranty coverage and keep a follow-up record."
                ]),
            S("Service area, license and warranty",
                $"HOME INDOR TECHNOLOGY INC is a Charlotte-based water-heater specialist. We currently serve {Areas} City pages are only used where we actually work; we do not publish empty city URLs. Qualifying installations include old-tank removal, startup, testing, the unit’s 5-year limited manufacturer warranty (model terms apply) and a 2-year SAFE installation labor warranty. Manufacturer coverage stays valid when the installation is documented and registered."),
            S("What standard installation includes — and what is extra",
                "Standard scope assumes a normal replacement using suitable existing connections. Code corrections, active leaks that damaged surroundings, electrical or gas changes, relocation, attic or crawlspace extra labor, and non-standard fittings are additional and approved before we schedule."),
            S("Charlotte metro cities we serve (not extra SEO pages)",
                $"Searches for replacement in Shelby, China Grove, Concord, Gastonia, Salisbury, Huntersville, Matthews, Monroe or Rock Hill still land here or on the near-me page. Coverage is the same published packages: {Areas} Dedicated city URLs exist only for Shelby and China Grove because we run those routes regularly.")
        ],
        ["/water-heater-cost-charlotte-nc/", "/40-gallon-electric-water-heater-installation/", "/50-gallon-water-heater-installation/", "/water-heater-installation-process/", "/water-heater-replacement-quote/", "/water-heater-service-near-me/"]);

    private static SeoPageDraft P0Cost() => new(
        "P0", "en", "/water-heater-cost-charlotte-nc/",
        "Water Heater Replacement Cost in Charlotte, NC",
        "Water Heater Replacement Cost Charlotte NC | $1,199 Installed | SAFE",
        "Water heater replacement cost and installation cost in Charlotte, NC: 40-gal electric $1,199 + tax, 50-gal electric $1,349, gas from $1,449. Compare Lowe’s/Home Depot box prices to a local plumber’s installed job.",
        "Precio y cotización", "Comparación comercial", "CHARLOTTE PRICING",
        "A new water heater in Charlotte is priced as an installed job, not a box on a shelf. SAFE’s current 40-gallon electric package is $1,199 + tax. Labor-only quotes without photos are incomplete because access, code items and fuel type change the number.",
        "See pricing / get a quote", QuoteHref, "View 40-gal package", "/40-gallon-electric-water-heater-installation/", false, true, false, null,
        [
            S("Base installed prices",
                "These are the numbers we publish for a confirmed standard replacement. Tax is extra. Promotional equipment pricing applies only to the qualifying 40-gallon electric unit.",
                [
                    "40-gal electric installed: $1,199 + tax.",
                    "50-gal electric installed: $1,349 + tax.",
                    "40-gal gas installed: from $1,449 + tax.",
                    "Standard installation labor in the 40-gal electric package is $999 when existing connections are suitable."
                ]),
            S("What the installed price includes",
                "Old tank removal and disposal when access allows, placement of the qualifying replacement, water and energy connections that reuse suitable existing lines, startup, basic operational testing, homeowner handoff, manufacturer warranty registration support and the 2-year SAFE labor warranty."),
            S("Common extras in Charlotte homes",
                "Closed plumbing systems often need a correctly sized and charged expansion tank (from $149). Drain pans, T&P discharge piping, electrical disconnects, gas-flex or vent corrections, seismic/strapping where required, and extra pipe (from $199) are quoted from photos. Difficult access, attic installs and relocation are never bundled into the $1,199 headline."),
            S("Labor-only vs installed package",
                "Homeowners sometimes ask for labor only. SAFE’s public number is an installed package so the unit, removal, startup and warranties stay together. If you already own a unit, we still need photos: a mismatched voltage, vent or footprint can cost more than buying the package."),
            S("How to get an exact quote",
                "Text or upload photos of the full tank, data plate, water connections, energy connection (breaker or gas), drain pan, T&P pipe, and the path in and out of the mechanical space. We reply with the matching package or a written extra-item list—not a bait price."),
            S("Lowe’s or Home Depot vs a local installer",
                "A box from Lowe’s or Home Depot is not an installed job. You still need labor, old-tank haul-away, code items and a warranty that covers the work. SAFE’s published numbers are installed packages. Buying the unit yourself can cost more if voltage, vent or footprint does not match—send photos before you load a tank in a car."),
            S("Is the permit included?",
                "Permit and inspection, when the jurisdiction requires them, are confirmed on the quote for that address—not hidden in a teaser. They are not the same as an expansion tank or pan. See the Charlotte permit guide for process; the cost page is for the installed package and extras."),
            S("Cash price vs financing",
                "The published $1,199 / $1,349 / from-$1,449 figures are cash installed packages plus tax. The SAFE Homeowner Plan (from $200 toward a qualifying promotional unit, subject to eligibility) and Synchrony on the HOME INDOR TECHNOLOGY INC merchant account are options, not a second secret price. A form is not credit approval."),
            S("Why some quotes look high",
                "Labor-only, attic, crawlspace, extra pipe (from $199), expansion tank (from $149), electrical or gas corrections, and relocation all sit outside the closet-swap headline. Ask what is included. A lower number that omits removal, permit or a pan is not a comparable bid.")
        ],
        ["/water-heater-replacement-charlotte-nc/", "/water-heater-code-upgrades/", "/water-heater-financing/", "/water-heater-permit-charlotte-nc/", "/40-gallon-electric-water-heater-installation/", "/water-heater-replacement-quote/"]);

    private static SeoPageDraft P040Gallon() => new(
        "P0", "en", "/40-gallon-electric-water-heater-installation/",
        "40-Gallon Electric Water Heater Installed",
        "40-Gallon Electric Water Heater Installed in Charlotte, NC | $1,199 + tax",
        "SAFE’s 40-gallon electric water heater package is $1,199 + tax in Charlotte: $200 promotional unit + $999 standard installation, old-tank removal, startup and 5-year + 2-year warranties.",
        "Producto estrella", "Compra", "HERO OFFER",
        "The 40-gallon electric tank is SAFE’s published hero package for many Charlotte-area homes: $200 promotional equipment + $999 standard professional installation, totaling $1,199 + tax. It fits households that do not routinely run overlapping showers, laundry and dishwashers at once.",
        "Schedule this package", QuoteHref, "Text photos", TextHref, false, true, false, null,
        [
            S("Who a 40-gallon electric unit serves",
                "A 40-gallon electric heater is a common match for smaller households, apartments, or homes with staggered hot-water use. If you have 3+ bathrooms, teenagers stacking showers, or a soaking tub, review a 50-gallon option or a higher First Hour Rating before buying."),
            S("What the $1,199 package includes",
                "Qualifying 40-gallon electric water heater at the promotional equipment price, standard like-for-like installation using suitable existing 240V connections, old-tank removal, startup and testing, 5-year limited manufacturer warranty on the unit (written model terms apply), and 2-year SAFE installation labor warranty."),
            S("What is not included",
                "A new electrical circuit, panel upgrade, relocation, attic extra labor, code-required pan or expansion tank if missing, damaged piping, and leak-damage repairs. Those items are priced from photos and approved in writing."),
            S("Electric requirements",
                "Most 40-gallon electric tanks need a dedicated 240-volt circuit and a working disconnect. If the breaker trips, wires are scorched, or the home still has undersized wiring, we stop and quote the electrical correction instead of forcing the promotional package.")
        ],
        ["/50-gallon-water-heater-installation/", "/water-heater-cost-charlotte-nc/", "/what-size-water-heater-do-i-need/", "/water-heater-replacement-charlotte-nc/", "/electric-water-heater-not-working/"]);

    private static SeoPageDraft P050Gallon() => new(
        "P0", "en", "/50-gallon-water-heater-installation/",
        "50-Gallon Water Heater Installation",
        "50-Gallon Water Heater Installation in Charlotte, NC | $1,349 + tax electric",
        "Install a 50-gallon water heater in Charlotte. Electric package $1,349 + tax for standard replacement. Compare capacity, gas options, access and when 50 gallons is the right size.",
        "Tamaño 50 galones", "Compra", "MORE CAPACITY",
        "A 50-gallon tank is the usual step up when a 40-gallon electric heater runs out during overlapping showers. SAFE’s current 50-gallon electric installed package is $1,349 + tax for a confirmed standard replacement. Gas 50-gallon units are quoted after venting and gas-line review.",
        "Get a 50-gal quote", QuoteHref, "Compare 40-gal", "/40-gallon-electric-water-heater-installation/", false, true, false, null,
        [
            S("Capacity and First Hour Rating",
                "Gallons on the jacket are storage, not the amount of hot water you can use in one hour. First Hour Rating (FHR) and recovery matter more for families. We match FHR to bathrooms and simultaneous use instead of guessing from square footage alone."),
            S("Electric vs gas 50-gallon",
                "Electric 50-gallon replacements are the common Charlotte request and use the published $1,349 + tax standard package when connections already exist. Gas 50-gallon tanks need correct venting, combustion air and gas supply; pricing starts from the gas installed range and is confirmed on site conditions."),
            S("Access and footprint",
                "A 50-gallon tank is taller and heavier. Closets, attics and tight garages may need extra labor or a different model. Send a photo of the space and the path out of the home so we do not promise a 50-gallon unit that will not physically fit.")
        ],
        ["/40-gallon-electric-water-heater-installation/", "/what-size-water-heater-do-i-need/", "/water-heater-cost-charlotte-nc/", "/gas-water-heater-installation/", "/water-heater-replacement-charlotte-nc/"]);

    private static SeoPageDraft P0SameDay() => new(
        "P0", "en", "/same-day-water-heater-replacement/",
        "Same-Day Water Heater Replacement",
        "Same-Day Water Heater Replacement in Charlotte, NC | SAFE Water Heater",
        "Same-day water heater replacement in Charlotte when the schedule and parts allow. Send photos first. We do not advertise 24/7 coverage we cannot staff.",
        "Urgencia", "Transaccional urgente", "SAME-DAY WHEN AVAILABLE",
        "Same-day replacement is possible when a crew, the correct unit and safe access are available that day—not as a blanket 24/7 promise. Text photos and the address as early as you can. If the tank is flooding or you smell gas, follow emergency shutoff steps first, then call.",
        "Ask about today", TextHref, "Emergency steps", "/water-heater-emergency/", true, true, false, null,
        [
            S("What “same day” means at SAFE",
                "We schedule the next open water-heater crew. Morning photo submissions have the best chance of a same-day window. Weekend and after-hours work is quoted when a crew is actually on call—we will not invent overnight coverage."),
            S("Photos we need before rolling a truck",
                "Full tank photo, data plate, leak location if any, breaker or gas control, and the path to remove the old tank. Without those, a same-day promise would be a guess."),
            S("Limits that block same-day installs",
                "Missing permits when required, unsafe electrical or gas conditions, attic-only access without two-person staging, custom tankless venting, or a size/fuel we do not have on the truck. In those cases we stabilize (shutoffs, advice) and schedule the first real slot."),
            S("Does same-day cost more?",
                "The published packages stay $1,199 + tax (40-gal electric) and $1,349 + tax (50-gal electric) for a confirmed standard replacement even when we can do it today. After-hours, weekend or emergency dispatch—if a crew is actually available—is a separate line you see in writing before you accept. We do not inflate the package to look like a “same-day tax.”")
        ],
        ["/water-heater-emergency/", "/water-heater-leaking/", "/no-hot-water/", "/water-heater-replacement-quote/", "/water-heater-cost-charlotte-nc/"]);

    private static SeoPageDraft P0Emergency() => new(
        "P0", "en", "/water-heater-emergency/",
        "Water Heater Emergency: What to Do Now",
        "Water Heater Emergency: What to Do Now | SAFE Water Heater Charlotte",
        "Water heater emergency steps for Charlotte homeowners: flooding, gas odor, CO alarm, sparks or a bulging tank. Shut off safely, evacuate when needed, then call SAFE or 911.",
        "Seguridad", "Urgente", "SAFETY FIRST",
        "If the tank is flooding, you smell gas, a carbon-monoxide alarm is sounding, you see sparks, or the tank is bulging, this is safety work—not a DIY YouTube repair. Get people out of the hazard, shut off water or energy only from a dry, safe location, then call. SAFE is a Charlotte-area installer, not a 24-hour utility. For gas odor or CO, leave and contact 911 or the gas company first.",
        "Call SAFE now", CallHref, "Text if it is safe", TextHref, true, false, false, null,
        [
            S("Flooding or a fast leak",
                "If you can reach it without standing in water, close the cold-water shutoff on top of the tank (usually a handle on the pipe feeding the heater). If water is near electrical parts, do not touch the tank. Kill power only at a dry breaker. Then call SAFE or a plumber for replacement—tank-shell leaks are not patch jobs."),
            S("Gas odor, yellow flame, soot or CO alarm",
                "Do not operate light switches, phones inside the home, or the water-heater controls. Leave the building, move away, and call 911 or the gas utility. Yellow flames, soot and CO alarms are combustion problems. After the utility clears the home, SAFE can plan a safe gas-tank replacement."),
            S("Electric sparks, hot wires, burning smell",
                "Leave the unit off. Do not keep resetting a hot breaker. From a dry location, switch the water-heater breaker off if it is safe. Burned conductors and sparking need a qualified electrical diagnosis before any new tank is installed."),
            S("Bulging tank, steam or a “boiling” heater",
                "A deformed tank or uncontrolled steam is a pressure/temperature emergency. Stay clear of the T&P discharge. Never cap that pipe. Shut off energy if safe, then arrange immediate replacement."),
            S("Where the shutoffs usually are",
                "Water: valve on the cold inlet above the tank, or the home’s main. Electric: dedicated 240V breaker, often labeled “water heater.” Gas: the small gas shutoff on the line to the control—use it only if you can do so without creating a spark hazard; if you smell gas, leave instead."),
            S("After the hazard is controlled",
                "SAFE can often replace a leaking tank the same day when a crew and unit are available. We do not advertise 24/7 staffing we cannot cover. Send photos once it is safe, or call 704-839-6126."),
            S("Attic leak wetting the ceiling",
                "If an attic tank is dumping onto living space, shut water from a safe location (often the home’s main if you cannot reach the attic valve). Stay off wet insulation. Photograph the ceiling stain and the tank when it is safe. Attic replacements are not the $1,199 closet package—see the attic install guide.")
        ],
        ["/water-heater-leaking/", "/same-day-water-heater-replacement/", "/no-hot-water/", "/gas-water-heater-not-working/", "/water-heater-leak-damage/"]);

    private static SeoPageDraft P0Leaking() => new(
        "P0", "en", "/water-heater-leaking/",
        "Water Heater Leaking: Bottom, Top, T&P or Drain",
        "Water Heater Leaking | Hot Water Heater Leak | Charlotte NC | SAFE",
        "Water heater leaking from the bottom, top, side, pressure relief valve or drain? Tank-shell leaks need same-day replacement when a crew is free—not a patch. Who to call in Charlotte: 704-839-6126.",
        "Fuga", "Problema urgente", "LEAK DIAGNOSIS",
        "Most “leaking water heater” calls are one of five things: a wet fitting, a dripping T&P (temperature-and-pressure) valve, a drain-valve seep, harmless condensation, or water from the tank shell itself. Shell leaks mean replace the tank. Fittings can sometimes be repaired. Send a photo of where the water starts—not only the puddle on the floor.",
        "Send leak photos", QuoteHref, "Emergency shutoff steps", "/water-heater-emergency/", true, true, false, null,
        [
            S("Locate the water, then judge severity",
                "Look at the top connections, the T&P discharge pipe, the drain valve near the bottom, the pan, and the tank jacket. Water from the steel tank body, especially with rust streaks, is end-of-life. Water only at a threaded fitting may be a wrench job. Continuous T&P discharge is a pressure or temperature problem—never plug the pipe."),
            S("Repair vs replace",
                "Repair can fit a newer tank with a failed valve, anode-related fitting, or loose flex connector. Replacement is the honest answer for a corroded tank, a unit over ~10–15 years with a leak, or when repair cost approaches a new installed package. Charlotte’s hard water shortens tank life; we see shell failures on heaters that “still make hot water.”"),
            S("What to do in the next 10 minutes",
                "If it is safe, close the cold-water inlet. If water is near electricity, stay back and open the breaker from a dry place. Mop only after power is isolated. Then text SAFE the tank photo, the leak close-up and whether the unit is electric or gas."),
            S("Leak location: bottom, fittings, T&P, drain, condensation",
                "Water from the bottom of the steel tank with rust streaks is almost always a failed tank—replace it. Wet threads at the top or a flex connector can be a repair on a newer unit. T&P discharge that will not stop is pressure or temperature, never a capped pipe. A drain valve seep is often a cheap part. Sweating in a humid closet can look like a leak; a pan that stays empty while the jacket is dry is a clue."),
            S("Attic, garage and “can it wait?”",
                "An attic leak can ruin a ceiling overnight—treat it as urgent, not tomorrow’s project. A garage puddle on concrete is still a live electrical or gas hazard. A tiny fitting drip on a 3-year tank can wait for a scheduled repair; a tank-body leak or T&P dumping water cannot. Tanks do not “burst” like a balloon as often as movies show, but a failed shell can dump 40–50 gallons fast."),
            S("Can the tank be repaired?",
                "You cannot patch a pressure vessel with epoxy and call it safe. Repair means valves, elements, thermostats, T&P, drain or flex lines on a sound jacket. If the replacement path is the published $1,199 or $1,349 package, chasing parts on a 12-year tank is usually the expensive choice.")
        ],
        ["/repair-or-replace-water-heater/", "/water-heater-emergency/", "/water-heater-leak-damage/", "/same-day-water-heater-replacement/", "/water-heater-replacement-charlotte-nc/"]);

    private static SeoPageDraft P0NoHotWater() => new(
        "P0", "en", "/no-hot-water/",
        "No Hot Water? Water Heater Not Working",
        "No Hot Water | Water Heater Not Working | Charlotte NC | SAFE",
        "No hot water in the house, water heater not working, or only cold water from the shower: electric breaker and elements vs gas pilot. Charlotte homeowners text photos to SAFE. Who to call: 704-839-6126.",
        "Sin agua caliente", "Problema urgente", "NO HOT WATER",
        "Start with fuel type. Electric tanks that go fully cold often have a tripped breaker, a failed upper element/thermostat, or a reset button on the thermostat. Gas tanks that go cold often have a pilot/ignition, gas-control or venting issue. Resetting breakers or relighting pilots is not a diagnosis if the problem returns in one cycle.",
        "Get a diagnosis today", TextHref, "Electric guide", "/electric-water-heater-not-working/", false, true, false, null,
        [
            S("Electric tank decision tree",
                "Check the dedicated 240V breaker. If it is on and the tank is cold, look only at the manufacturer’s accessible reset if the cover is designed for a homeowner—do not remove live covers. Repeated trips or a hot breaker: leave it off and call. If the tank is lukewarm, a lower element or thermostat is a common next suspect and needs a technician."),
            S("Gas tank decision tree",
                "If you smell gas, stop and follow emergency steps. If not, a standing-pilot or electronic-ignition failure, thermocouple, or gas valve can leave you with cold water. Soot, yellow flame or a CO alarm is not a relight situation."),
            S("Tankless units",
                "Error codes, insufficient flow, scale and ignition lockouts are the usual “no hot water” causes. See our tankless problem guide rather than forcing a tank checklist onto a tankless heater."),
            S("When SAFE recommends replacement",
                "No hot water plus a leaking tank, a 12–15 year unit, scorched wiring, or a gas control that is no longer available usually beats a parts chase. The published 40-gallon electric package exists for that exact Charlotte job."),
            S("Upstairs only, after vacation, or intermittent heat",
                "Hot downstairs but not upstairs is often a mixing valve, a failed fixture, or a circulation issue—not always a dead tank. Cold after a trip can be a tripped breaker, a vacation mode, or a gas lockout. Intermittent lukewarm water is frequently a failing element or thermostat. Photograph the data plate and say which bathrooms are affected.")
        ],
        ["/electric-water-heater-not-working/", "/gas-water-heater-not-working/", "/not-enough-hot-water/", "/repair-or-replace-water-heater/", "/tankless-water-heater-problems/"]);

    private static SeoPageDraft P0RepairOrReplace() => new(
        "P0", "en", "/repair-or-replace-water-heater/",
        "Water Heater Repair Charlotte NC: Repair or Replace?",
        "Water Heater Repair Charlotte NC | Repair or Replace | SAFE",
        "Decide whether to repair or replace a water heater in Charlotte: tank age, leak location, repair cost vs $1,199 installed. Water heater repair Charlotte NC usually becomes replacement if the tank shell leaks.",
        "Reparar o reemplazar", "Decisión", "REPAIR VS REPLACE",
        "Repair the water heater when the tank shell is sound, the unit is relatively new, and a replaceable part failed. Replace it when the tank leaks, corrosion is advanced, parts cost approaches a new installed package, or the heater is near the end of a 10–15 year service life. Age alone does not decide—leak location and safety do.",
        "Request an evaluation", QuoteHref, "See installed pricing", "/water-heater-cost-charlotte-nc/", false, true, false, null,
        [
            S("Decision table we actually use",
                "We look at four facts together: Is water coming from the tank body? How old is the unit? What would the part and labor cost? Has this failed before?",
                [
                    "Tank-shell leak → replace. Do not epoxy a pressure vessel.",
                    "Failed element, thermostat, T&P, drain valve or flex line on a newer sound tank → repair is often rational.",
                    "Repair quote close to the $1,199–$1,349 installed packages → replace.",
                    "Repeat service calls in the same year → replace.",
                    "Unsafe wiring, venting or a bulging tank → replace, do not patch."
                ]),
            S("Charlotte water and tank life",
                "Hard water and sediment make tanks noisy and then leak from the bottom. Flushing an already leaking or paper-thin tank can make it worse. We would rather quote a clean replacement than “one more anode” on a failing jacket."),
            S("How SAFE evaluates from photos",
                "Send the data plate (age/model), a full-tank shot, the leak or failed part, and household size. You get a replace-or-repair recommendation with the matching package price—not a vague “could be anything.”"),
            S("Age ladder: 8, 10, 12 and 15 years",
                "Around 8 years with a sound tank, a single part repair can still make sense. Near 10 years, weigh the part quote against $1,199–$1,349 installed. At 12 years in Charlotte hard water, replacement is the default if anything else is wrong. At 15 years, plan the swap even if it still heats—the next failure is often a leak. Tankless units last differently; they fail on scale and electronics, not a jacket, and are quoted as a different job."),
            S("Selling the house or an inspector flag",
                "A realtor or home inspector calling out age, a missing pan, or T&P discharge is a replacement or code-correction quote—not a “wait until it leaks on the buyer.” Serial-number photos date the tank. We write a scope buyers and sellers can attach to the contract.")
        ],
        ["/water-heater-leaking/", "/water-heater-cost-charlotte-nc/", "/water-heater-replacement-charlotte-nc/", "/new-water-heater-not-working/", "/water-heater-insurance-warranty/"]);

    private static SeoPageDraft P0NearMe() => new(
        "P0", "en", "/water-heater-service-near-me/",
        "Water Heater Replacement Near Me in Charlotte, NC",
        "Water Heater Replacement Near Me | Installation & Repair | Charlotte NC | SAFE",
        "Water heater replacement, installation and repair near me in the Charlotte metro. SAFE is a water-heater plumber—not a drain company. Text 650-669-6890. Call 704-839-6126. We do not advertise 24/7.",
        "Cercanía", "Local transaccional", "CHARLOTTE METRO",
        "If you searched water heater replacement near me, installation near me, repair near me, or plumber for a water heater, SAFE is the Charlotte-based water-heater company—not a general plumber near me for sewers. We sell, replace and follow up in Charlotte, Shelby, China Grove, Mint Hill and the cities below. Spanish-speaking coordination: see the Spanish hub. Text 650-669-6890 or call 704-839-6126.",
        "Text SAFE", TextHref, "Call now", CallHref, false, true, false, "/es/calentadores-de-agua-charlotte-nc/",
        [
            S("Where we actually work",
                $"Current coverage: {Areas} We keep dedicated pages for Shelby and China Grove because we run those routes regularly. Other cities on this list are served from Charlotte with the same published packages when travel and schedule allow."),
            S("Hours and how to reach us",
                "Business-day photo review is the fastest path to a scheduled replacement. Same-day work depends on crew availability. We do not claim 24/7 dispatch. For gas or CO emergencies, call 911 or the utility first."),
            S("Plumber near me vs a water-heater specialist",
                "“Plumber near me” and “emergency plumber near me” are dominated by 24-hour drain companies. SAFE answers plumber for water heater, no hot water, and leaking water heater in the Charlotte metro. We do not claim licensed plumber for every fixture in the house. Same-day plumber / water heater installer open now only if a crew is actually free—never a fake “open 24 hours” badge."),
            S("Trust signals that matter for a local search",
                "Published installed prices, a 2-year labor warranty on our work, manufacturer warranty registration, and a water-heater-only process. Ask for the written scope before anyone arrives. Licensed contracting work in North Carolina is performed through HOME INDOR TECHNOLOGY INC."),
            S("Hours, bilingual help, licensed work, published price",
                "Photo review on business days is the fastest path. We do not claim to be “open 24 hours.” Spanish-language coordination is available on the Charlotte Spanish hub. Work is performed by HOME INDOR TECHNOLOGY INC. Upfront package prices are on the cost page: $1,199, $1,349 and gas from $1,449 + tax. Concord, Gastonia, Salisbury, Huntersville, Matthews, Monroe and Rock Hill searches should use this page plus Charlotte replacement—not a fake city microsite.")
        ],
        ["/water-heater-replacement-charlotte-nc/", "/shelby", "/china-grove", "/es/calentadores-de-agua-charlotte-nc/", "/water-heater-replacement-quote/", "/same-day-water-heater-replacement/"]);

    private static SeoPageDraft P0SpanishHub() => new(
        "P0", "es", "/es/calentadores-de-agua-charlotte-nc/",
        "Calentador de Agua cerca de mí en Charlotte, NC",
        "Calentador de Agua cerca de mí | Reemplazo e instalación Charlotte NC | SAFE",
        "Calentador de agua cerca de mí en Charlotte: reparación, reemplazo e instalación. Calentón o boiler que no calienta, water heater goteando, precio instalado desde $1,199 + impuesto. Plomero que habla español. Financiamiento. Texto 650-669-6890.",
        "Español local", "Local transaccional", "CHARLOTTE · ESPAÑOL",
        "SAFE Water Heater instala y reemplaza calentadores de agua en Charlotte y ciudades cercanas. El paquete eléctrico de 40 galones cuesta $1,199 + impuesto. Envíe fotos por mensaje de texto al 650-669-6890 o llame al 704-839-6126. Coordinamos en español: plomero que habla español para calentador, no un desagüe 24 horas.",
        "Llamar en español", CallHref, "Enviar fotos", TextHref, false, true, false, "/water-heater-service-near-me/",
        [
            S("Servicios",
                "Venta del equipo, reemplazo profesional, retiro del tanque viejo, arranque, registro de garantía y seguimiento. No somos una ferretería: el precio que publicamos es instalado, con las condiciones de una instalación estándar."),
            S("Precios actuales",
                "40 galones eléctrico: $1,199 + impuesto. 50 galones eléctrico: $1,349 + impuesto. 40 galones a gas: desde $1,449 + impuesto. Tanque de expansión: desde $149. Extra de tubería: desde $199. Los correcciones de código, áticos y reubicaciones se cotizan aparte."),
            S("Garantía y cobertura",
                "El equipo calificado incluye garantía limitada de fabricante de 5 años según el modelo, más 2 años de mano de obra de instalación SAFE. Hay que registrar el equipo y conservar las fotos del trabajo."),
            S("Zonas",
                $"Atendemos {Areas} Las páginas de ciudad existen solo donde realmente trabajamos (por ejemplo Shelby y China Grove)."),
            S("Si el calentón, boiler o water heater no calienta",
                "Calentón de agua no funciona, mi calentón no calienta, boiler de agua no prende o water heater no calienta: es el mismo equipo. Envíe fotos de la placa y del tanque. No prometemos plomero 24 horas; el mismo día depende de cuadrilla."),
            S("Water heater goteando",
                "Si está goteando, cierre el agua fría si es seguro y no se pare en charcos junto a electricidad. Fuga del tanque casi siempre es reemplazo, no parche. Texto con foto del origen de la fuga."),
            S("Precio instalado, 40 galones y financiamiento",
                "Precio de water heater instalado: 40 galones eléctrico $1,199 + impuesto; 50 galones $1,349; gas desde $1,449. Calentador eléctrico de 40 galones es el paquete más pedido. Financiamiento para calentador de agua: plan SAFE (desde $200 hacia un equipo promocional calificado) o Synchrony. Un formulario no es aprobación.")
        ],
        ["/es/reemplazo-calentador-agua-charlotte-nc/", "/water-heater-service-near-me/", "/water-heater-cost-charlotte-nc/", "/water-heater-replacement-quote/"]);

    private static SeoPageDraft P0SpanishReplacement() => new(
        "P0", "es", "/es/reemplazo-calentador-agua-charlotte-nc/",
        "Reemplazo e Instalación de Water Heater en Charlotte",
        "Reemplazo de Calentador de Agua en Charlotte, NC | Instalación de water heater | SAFE",
        "Reemplazo e instalación de water heater en Charlotte: proceso, precios, tamaños 40 y 50 galones, extras de código y garantías. Cotice con fotos. HOME INDOR TECHNOLOGY INC.",
        "Reemplazo español", "Compra", "REEMPLAZO EN CHARLOTTE",
        "El reemplazo estándar toma unas 2 a 4 horas cuando el acceso es normal. SAFE publica $1,199 + impuesto para el paquete eléctrico de 40 galones. Mande fotos del tanque, la placa de datos y las conexiones antes de agendar para no sorprenderse con extras.",
        "Cotizar con fotos", QuoteHref, "Ver precios", "/water-heater-cost-charlotte-nc/", false, true, false, "/water-heater-replacement-charlotte-nc/",
        [
            S("Proceso",
                "1) Fotos y dirección. 2) Confirmamos si aplica el paquete estándar o hay extras (charola, tubería T&P, tanque de expansión, gas o electricidad). 3) Elige pago de contado, plan SAFE o Synchrony. 4) Instalamos, retiramos el tanque viejo y registramos la garantía."),
            S("Tamaños y combustible",
                "40 galones eléctrico es el paquete más pedido. 50 galones eléctrico cuesta $1,349 + impuesto en reemplazo estándar. Gas y tankless se cotizan según ventilación, línea de gas y demanda de agua caliente."),
            S("Extras y garantías",
                "Ático, crawlspace, reubicación y cambios de código no caben en el precio de anuncio. La garantía de fabricante de 5 años depende del modelo escrito; SAFE agrega 2 años de mano de obra sobre nuestra instalación original."),
            S("¿Mismo día?",
                "El reemplazo el mismo día es posible si hay cuadrilla, equipo y acceso seguro—no prometemos 24/7. Los paquetes publicados ($1,199 y $1,349 + impuesto) no suben solo por “hoy”; horas extra, si existen, se cotizan por escrito.")
        ],
        ["/es/calentadores-de-agua-charlotte-nc/", "/water-heater-replacement-charlotte-nc/", "/40-gallon-electric-water-heater-installation/", "/water-heater-installation-process/"]);

    private static SeoPageDraft P1ElectricNotWorking() => new(
        "P1", "en", "/electric-water-heater-not-working/",
        "Electric Water Heater Not Working",
        "Electric Water Heater Not Working | Charlotte, NC | SAFE Water Heater",
        "Electric water heater not heating: breaker, high-limit reset, elements and thermostats. Safety first—do not keep resetting a tripping breaker. SAFE diagnoses or replaces in Charlotte.",
        "Eléctrico", "Diagnóstico", "ELECTRIC TANKS",
        "When an electric water heater stops heating, the usual causes are a tripped 240V breaker, a high-limit reset, a failed heating element, or a thermostat. If the breaker is hot, the wires are discolored, or it trips again, leave the circuit off. That is not a homeowner reset loop.",
        "Request diagnosis", TextHref, "No hot water overview", "/no-hot-water/", false, false, false, null,
        [
            S("Breaker and disconnect",
                "The water heater should have a dedicated double-pole breaker. A tripped breaker can be a one-time event or a shorted element. Repeated trips need a technician. Do not install a larger breaker to “make it stay on.”"),
            S("Reset button, elements and thermostats",
                "Many tanks have a red high-limit reset behind the upper access panel. Follow only the manufacturer’s homeowner instructions; live 240V is behind those covers. Failed upper elements often mean no hot water; failed lower elements mean you run out fast."),
            S("When replacement is smarter",
                "A 12-year tank with two dead elements, a leaking jacket, or burned leads is usually replaced with the 40- or 50-gallon electric package instead of stacking parts."),
            S("Element, thermostat and “repair cost”",
                "A heating element or thermostat on a sound, newer tank can be a repair. We do not publish a bait “element-only” URL because the real decision is whether the jacket, wiring and age still justify parts. If the repair quote nears $1,199 installed, replace. Do not keep resetting a tripping breaker to save a service fee.")
        ],
        ["/no-hot-water/", "/not-enough-hot-water/", "/40-gallon-electric-water-heater-installation/", "/repair-or-replace-water-heater/"]);

    private static SeoPageDraft P1GasNotWorking() => new(
        "P1", "en", "/gas-water-heater-not-working/",
        "Gas Water Heater Not Working",
        "Gas Water Heater Not Working | Charlotte, NC | SAFE Water Heater",
        "Gas water heater not lighting or staying lit: pilot, igniter, thermocouple, gas control and venting. Gas odor or CO alarm: leave and call 911. SAFE replaces gas tanks in Charlotte.",
        "Gas", "Diagnóstico", "GAS TANKS",
        "A gas water heater that will not heat is often a pilot or electronic-ignition failure, a thermocouple, a gas-control valve, or a venting/combustion-air problem. If you smell gas, see soot, or a CO alarm sounds, leave the home. Do not keep clicking the igniter.",
        "Call about gas service", CallHref, "Emergency steps", "/water-heater-emergency/", true, true, false, null,
        [
            S("Pilot, igniter and flame color",
                "A healthy gas flame is mostly blue. Yellow flame, rolling flame or soot means incomplete combustion. That is a shut-down and service condition, not a “relight it one more time” task."),
            S("Venting and combustion air",
                "Blocked vents, nested animals, or tight closets without enough air will shut a modern control down or produce CO. Attic and closet gas tanks in Charlotte houses need this check before we quote a simple swap."),
            S("Replacement path",
                "Gas installed pricing starts from $1,449 + tax for a qualifying 40-gallon like-for-like job. Flex-line, shutoff, vent and combustion-air corrections are extra and written before work."),
            S("Pilot, thermocouple and control “repair cost”",
                "A thermocouple or igniter on a newer sound tank can be worth repairing. A failing gas-control valve on a 12-year unit usually loses to replacement. Status lights and blink codes need the model booklet—not a generic YouTube reset. Gas odor still means leave first.")
        ],
        ["/no-hot-water/", "/gas-water-heater-installation/", "/water-heater-emergency/", "/water-heater-replacement-charlotte-nc/"]);

    private static SeoPageDraft P1NotEnough() => new(
        "P1", "en", "/not-enough-hot-water/",
        "Why Do I Run Out of Hot Water?",
        "Why Do I Run Out of Hot Water? | Size, Sediment, Elements | SAFE Charlotte",
        "Hot water runs out too fast: undersized tank, failed lower element, sediment, dip tube or extra demand. SAFE sizes 40- vs 50-gallon replacements in Charlotte.",
        "Poca agua", "Diagnóstico/compra", "SHORT HOT WATER",
        "Running out of hot water mid-shower is usually capacity, a failed lower heating element, sediment insulating the tank bottom, a broken dip tube, or the household using more fixtures at once than the tank’s First Hour Rating. It is a different problem from no hot water at all.",
        "Calculate the right size", "/what-size-water-heater-do-i-need/", "See 50-gal package", "/50-gallon-water-heater-installation/", false, true, false, null,
        [
            S("Capacity vs demand",
                "Two bathrooms used at once will empty a 40-gallon electric tank. A 50-gallon electric package at $1,349 + tax is the common Charlotte upgrade. Tankless is only better if the gas or electrical infrastructure can support the GPM you actually need."),
            S("Sediment and elements",
                "Hard-water sediment makes recovery slow and noisy. A dead lower element on an electric tank leaves a thin layer of hot water on top. Both can look like “the tank is too small” when the tank is simply sick."),
            S("What we check from photos and a short interview",
                "People in the home, bathrooms, laundry overlap, tank age, and whether cold water dumps suddenly (dip tube) vs fades (capacity/element). Then we quote repair or the correct package."),
            S("Recovery time, hot-to-cold showers, and 40 vs 50",
                "Electric tanks recover slower than gas. A shower that starts hot then goes cold mid-way is often a depleted tank or a failed lower element, not “the city water.” Scalding then ice-cold can be a mixing or thermostat fault—treat overheating as safety. If 40 gallons cannot cover two bathrooms, the $1,349 50-gallon package is the usual Charlotte step-up.")
        ],
        ["/what-size-water-heater-do-i-need/", "/50-gallon-water-heater-installation/", "/electric-water-heater-not-working/", "/water-heater-making-noise/"]);

    private static SeoPageDraft P1Noise() => new(
        "P1", "en", "/water-heater-making-noise/",
        "Water Heater Noises Explained",
        "Water Heater Noises Explained | Popping, Rumbling, Ticking | SAFE Charlotte",
        "Popping, rumbling, banging or whistling water heater noises in Charlotte are often sediment or expansion. Learn urgency and when replacement beats flushing.",
        "Ruidos", "Diagnóstico", "NOISE GUIDE",
        "Popping and rumbling while the tank heats almost always mean sediment on the bottom—very common in the Charlotte metro. Ticking can be pipe expansion. Hammering may be a water-hammer or check-valve issue. A scream or whistle at the T&P valve is urgent.",
        "Send a short video", TextHref, "Repair or replace?", "/repair-or-replace-water-heater/", false, false, false, null,
        [
            S("Noise and what it usually means",
                "Match the sound to when it happens: while heating, on start, or at the T&P pipe. Charlotte hard water makes popping the most common call.",
                [
                    "Popping / rumbling: sediment and steam bubbles. Service or replace based on age and leaks.",
                    "Ticking in pipes: thermal expansion; sometimes needs an expansion tank.",
                    "Banging on start: water hammer or loose pipes—not always the tank.",
                    "Whistle or hiss at T&P: pressure/temperature—do not cap the pipe; call.",
                    "Crackling with a leak: assume tank failure until proven otherwise."
                ]),
            S("Should you flush it?",
                "Flushing helps some healthy tanks. Flushing a 14-year rusty tank can open a leak. SAFE would rather see a video and the data plate than have you disturb a fragile drain valve.")
        ],
        ["/water-heater-maintenance/", "/repair-or-replace-water-heater/", "/water-heater-code-upgrades/", "/not-enough-hot-water/"]);

    private static SeoPageDraft P1Smelly() => new(
        "P1", "en", "/smelly-discolored-hot-water/",
        "Smelly, Rusty or Discolored Hot Water",
        "Smelly, Rusty or Discolored Hot Water | Charlotte, NC | SAFE Water Heater",
        "Rotten-egg smell, rusty or cloudy hot water: anode, bacteria, tank corrosion or minerals. Compare hot vs cold. SAFE inspects and replaces failing tanks in Charlotte.",
        "Calidad del agua", "Diagnóstico", "WATER QUALITY",
        "If only the hot side is rusty, smelly or cloudy, the water heater (or its anode) is the suspect. If hot and cold are both affected, the well, municipal supply or a softener setup may be involved. Rotten-egg odor on hot water is often sulfate-reducing bacteria reacting with a standard anode.",
        "Request an inspection", TextHref, "Leaking tank?", "/water-heater-leaking/", false, false, false, null,
        [
            S("Color",
                "Brown or red hot water often means internal corrosion or a depleted anode. Black specks can be anode or dip-tube debris. Do not drink discolored hot water; use cold and have the tank inspected."),
            S("Odor",
                "A sulfur smell that appears after the tank sat unused (vacation, vacant house) is a known pattern. Anode swaps and disinfection are technician work. A leaking rusty tank is a replacement, not an odor gadget."),
            S("Safety",
                "Odor of gas is not “smelly water.” Leave and call the utility. Electrical burning smell is an emergency shutdown.")
        ],
        ["/water-heater-leaking/", "/repair-or-replace-water-heater/", "/water-heater-maintenance/", "/water-heater-special-installations/"]);

    private static SeoPageDraft P1Pressure() => new(
        "P1", "en", "/low-hot-water-pressure/",
        "Low Hot Water Pressure",
        "Low Hot Water Pressure | Charlotte, NC | SAFE Water Heater",
        "Low hot-water pressure: clogged aerators, failing shutoff, PRV, expansion tank or tank sediment. SAFE isolates house vs heater in Charlotte.",
        "Presión", "Diagnóstico", "PRESSURE",
        "If only hot water is weak, start at the faucet aerator, then the tank’s isolation valves, then sediment or a failing mixing valve. If all fixtures are weak, the home may have a pressure-reducing valve, municipal issue, or a well-pump problem—not a “bigger water heater.”",
        "Get a diagnosis", TextHref, "Code extras", "/water-heater-code-upgrades/", false, false, false, null,
        [
            S("House vs heater",
                "Test a washing-machine hot hose or a tub spout (fewer aerators). If that flow is strong, clean fixture aerators. If every hot fixture is weak, look at the tank shutoffs fully open and for kinks in flex lines."),
            S("PRV, expansion and T&P",
                "High street pressure plus a closed system causes T&P dribble and noisy tanks. The fix is often a PRV and/or expansion tank—not a new 50-gallon heater. We measure pressure before selling equipment.")
        ],
        ["/water-heater-code-upgrades/", "/water-heater-making-noise/", "/not-enough-hot-water/", "/water-heater-leaking/"]);

    private static SeoPageDraft P1Size() => new(
        "P1", "en", "/what-size-water-heater-do-i-need/",
        "What Size Water Heater Do I Need?",
        "What Size Water Heater Do I Need? | 40 vs 50 Gallon | SAFE Charlotte",
        "Size a water heater by people, bathrooms and First Hour Rating—not square feet. Charlotte guide for 40-gallon vs 50-gallon electric and when tankless makes sense.",
        "Tamaño", "Investigación comercial", "SIZING",
        "Size from peak hour demand: number of people, bathrooms used at once, laundry overlap and fixture flow—not the square footage of the house. In Charlotte, many two-person homes stay on a 40-gallon electric package; families stacking showers usually need 50 gallons or a higher FHR.",
        "Get a size recommendation", QuoteHref, "40-gal package", "/40-gallon-electric-water-heater-installation/", false, true, false, null,
        [
            S("A practical Charlotte shortcut",
                "",
                [
                    "1–2 people, 1–2 baths, staggered use → 40-gallon electric ($1,199 + tax package) is often enough.",
                    "3–4 people or 2+ baths used together → start at 50-gallon electric ($1,349 + tax).",
                    "Soaking tubs, body sprays or back-to-back showers → confirm FHR; consider 50-gal or tankless with real GPM math.",
                    "Gas vs electric changes recovery: gas often reheats faster, so a smaller gas tank can sometimes match a larger electric."
                ]),
            S("First Hour Rating beats the gallon sticker",
                "Manufacturers publish FHR: how much hot water the unit can deliver in an hour starting with a full tank. We match FHR to your peak hour rather than arguing 40 vs 50 as a personality test."),
            S("Send us a few facts",
                "People, baths, electric or gas, and a photo of the current data plate. We will not upsell a tankless system that the panel or gas meter cannot feed.")
        ],
        ["/40-gallon-electric-water-heater-installation/", "/50-gallon-water-heater-installation/", "/water-heater-buying-guide/", "/not-enough-hot-water/", "/tankless-water-heater-installation/"]);

    private static SeoPageDraft P1BuyingGuide() => new(
        "P1", "en", "/water-heater-buying-guide/",
        "Water Heater Buying Guide",
        "Water Heater Buying Guide | Types, Fuel, Cost | SAFE Charlotte, NC",
        "Buy a water heater in Charlotte: tank vs tankless vs heat pump, electric vs gas, efficiency, total installed cost and when SAFE’s 40-gal package is the right buy.",
        "Comparación", "Investigación comercial", "BUYING GUIDE",
        "Buy the fuel your home already supports unless you are paying for a real conversion. For most Charlotte replacements, a like-for-like tank is faster, cheaper and easier to warranty than a tankless dream that needs a new gas line or a 60-amp circuit.",
        "Choose equipment with SAFE", QuoteHref, "See cost page", "/water-heater-cost-charlotte-nc/", false, true, false, null,
        [
            S("Types",
                "",
                [
                    "Electric tank: simplest swap when 240V already exists; hero package $1,199 + tax for qualifying 40-gal.",
                    "Gas tank: faster recovery; needs vent, combustion air and a sound gas line; from $1,449 + tax installed.",
                    "Tankless: endless hot water only if GPM and infrastructure match; higher installed cost and yearly descaling in hard water.",
                    "Heat pump (hybrid): high efficiency, needs space, condensate drain and a location that can tolerate cooler air and noise."
                ]),
            S("Total cost of ownership",
                "Sticker efficiency means little if the install is wrong. Include expansion tank, pan, permits, and energy price. A $1,199 electric package that fits beats a $4,000 tankless that error-codes in winter."),
            S("Brands and warranties",
                "SAFE installs qualifying units with a 5-year limited manufacturer warranty as written for that model, plus 2-year labor on our installation. Register the serial number. Mixing random big-box tanks with unknown prior wiring is how warranties get denied.")
        ],
        ["/what-size-water-heater-do-i-need/", "/tankless-water-heater-installation/", "/heat-pump-water-heater-installation/", "/gas-water-heater-installation/", "/water-heater-cost-charlotte-nc/"]);

    private static SeoPageDraft P1GasInstall() => new(
        "P1", "en", "/gas-water-heater-installation/",
        "Gas Water Heater Installation",
        "Gas Water Heater Installation in Charlotte, NC | From $1,449 + tax | SAFE",
        "Gas water heater installation in Charlotte: venting, combustion air, gas line, 40-gal pricing from $1,449 + tax, and safety checks before lighting.",
        "Gas compra", "Transaccional", "GAS INSTALLATION",
        "A gas water heater install is a combustion appliance job: correct venting, gas supply, shutoff, combustion air and a leak-free start-up. SAFE’s published 40-gallon gas installed price starts from $1,449 + tax for a like-for-like replacement when those conditions already work.",
        "Quote a gas install", QuoteHref, "Gas not lighting?", "/gas-water-heater-not-working/", true, true, false, null,
        [
            S("What we inspect before quoting the from-price",
                "Vent type (atmospheric, power-vent, direct-vent), pitch and termination, gas-pipe size, shutoff location, and whether the closet can breathe. Photos of the flue and gas control save a wasted trip."),
            S("Safety",
                "We do not light a unit with soot, a backdrafting vent, or a CO history until those defects are corrected. That work is extra and written."),
            S("What the from-$1,449 price assumes",
                "Like-for-like 40-gallon gas tank, usable vent, adequate pipe, and a working shutoff. Power-vent conversions, new penetrations, and gas-line upsizing are a different quote. Compare that honestly against the $1,199 electric package if the home already has 240V."),
            S("After lighting",
                "We check flame, draft, and leak-free joints, then show you the gas shutoff. If a CO alarm has ever sounded in that mechanical room, say so before we schedule.")
        ],
        ["/gas-water-heater-not-working/", "/water-heater-replacement-charlotte-nc/", "/water-heater-permit-charlotte-nc/", "/water-heater-cost-charlotte-nc/"]);

    private static SeoPageDraft P1TanklessInstall() => new(
        "P1", "en", "/tankless-water-heater-installation/",
        "Tankless Water Heater Installation",
        "Tankless Water Heater Installation in Charlotte, NC | SAFE Water Heater",
        "Tankless water heater installation in Charlotte: GPM sizing, gas or electrical upgrades, condensate, hard-water maintenance and installed cost quoted from site conditions.",
        "Tankless compra", "Transaccional", "TANKLESS",
        "Tankless is not a drop-in for a 40-gallon electric closet. Gas units often need a larger gas line and new venting; electric whole-home units can demand more amperage than the panel has. SAFE quotes tankless only after GPM, fuel and space are real.",
        "Request a tankless quote", QuoteHref, "Tankless problems", "/tankless-water-heater-problems/", false, true, false, null,
        [
            S("Sizing by GPM, not by “endless” marketing",
                "Add the fixtures you want to run together (shower + sink). Charlotte winter groundwater is colder, so the same heater produces fewer GPM in January than in July. We size for winter."),
            S("Infrastructure and maintenance",
                "Expect possible gas upsizing, 120V outlet for some models, condensate handling, and annual descaling in hard water. Skip tankless if nobody will maintain it—error codes will return you to a tank later."),
            S("Cost honesty",
                "A tankless install in Charlotte is often multiple times the $1,199 electric tank package once venting, gas, and electrical are real. Ask for that total in writing before you fall in love with a brochure GPM number."),
            S("When we recommend keeping a tank",
                "Small gas meters, 100-amp panels, hard water with no treatment plan, or a closet that cannot vent. A 50-gallon electric at $1,349 + tax is frequently the better value.")
        ],
        ["/tankless-water-heater-problems/", "/water-heater-buying-guide/", "/what-size-water-heater-do-i-need/", "/water-heater-cost-charlotte-nc/"]);

    private static SeoPageDraft P1Financing() => new(
        "P1", "en", "/water-heater-financing/",
        "Water Heater Financing Charlotte NC",
        "Water Heater Financing Charlotte NC | Monthly Payments | SAFE Plan + Synchrony",
        "Water heater financing Charlotte NC: SAFE Homeowner Plan from $200 toward a qualifying unit, or apply with Synchrony through HOME INDOR TECHNOLOGY INC. Approval not guaranteed. Utility rebates are not a SAFE quote line.",
        "Financiamiento", "Transaccional", "PAY YOUR WAY",
        "You can pay cash for the published packages, request the SAFE Homeowner Plan (start with $200 toward a qualifying promotional water heater), or apply with Synchrony on the merchant account for HOME INDOR TECHNOLOGY INC. Submitting a form is not credit approval.",
        "Check SAFE eligibility", "/financing/safe-plan", "Synchrony option", "/financing", false, true, false, null,
        [
            S("SAFE Homeowner Plan",
                "Preliminary eligibility may let an approved homeowner start with $200 toward a qualifying promotional tank and arrange the eligible installation balance under written SAFE terms. SAFE describes the screening as no traditional credit-score check, but identity, homeownership and property fit still matter. It is not guaranteed approval."),
            S("Synchrony",
                "Synchrony is a separate lender. Rates, promotions and repayment are Synchrony’s. Use the official application linked from our financing page; do not send card numbers to a text thread."),
            S("What financing does not change",
                "Scope, tax, extras and warranties stay the same. We will not hide a code-required expansion tank inside a financed “promo.”"),
            S("Monthly payments, zero-down ads and rebates",
                "A published package is still $1,199 / $1,349 / from-$1,449 + tax in cash. Monthly payment, buy-now-pay-later or zero-down language only applies if SAFE Plan eligibility or Synchrony actually approves you—we do not invent a payment. Duke Energy or North Carolina rebates, if any exist this month, are the utility’s program; we do not print a rebate we cannot verify. Check Duke Energy, then we write the installed job.")
        ],
        ["/financing", "/financing/safe-plan", "/water-heater-cost-charlotte-nc/", "/water-heater-replacement-quote/"]);

    private static SeoPageDraft P1Code() => new(
        "P1", "en", "/water-heater-code-upgrades/",
        "Water Heater Code Upgrades and Extra Costs",
        "Water Heater Code Upgrades and Extra Costs | Charlotte, NC | SAFE",
        "Water heater extras in Charlotte: expansion tank from $149, pans, T&P piping, disconnects, venting and seismic straps. See why the $1,199 package cannot include every code item.",
        "Código/extras", "Comercial", "CODE + EXTRAS",
        "The $1,199 electric package is a standard swap—not a full mechanical-room remodel. Closed plumbing systems, missing pans, illegal T&P terminations and missing disconnects show up constantly in Charlotte photos. We list those extras before scheduling.",
        "Evaluate extras from photos", QuoteHref, "Permit info", "/water-heater-permit-charlotte-nc/", false, true, false, null,
        [
            S("Typical extra items",
                "",
                [
                    "Expansion tank: from $149, charged to match house pressure.",
                    "Additional piping: from $199 when the existing run is unusable.",
                    "Drain pan and drain line where required.",
                    "T&P discharge piped to a safe location—never capped.",
                    "Electrical disconnect or gas-flex/shutoff corrections.",
                    "Venting or combustion-air fixes on gas tanks."
                ]),
            S("Why photos matter",
                "A headline price that “includes everything” is how change-orders appear on the driveway. Send the pan, T&P pipe, and mechanical-room photos with the quote request.")
        ],
        ["/water-heater-cost-charlotte-nc/", "/water-heater-permit-charlotte-nc/", "/attic-water-heater-installation/", "/water-heater-replacement-quote/"]);

    private static SeoPageDraft P1Permit() => new(
        "P1", "en", "/water-heater-permit-charlotte-nc/",
        "Water Heater Permits in Charlotte, NC",
        "Water Heater Permits in Charlotte, NC | SAFE Water Heater",
        "Water heater permit and inspection expectations in Charlotte, NC. SAFE treats permit needs as part of a professional replacement and quotes them honestly from current local rules.",
        "Permiso", "Informacional local", "PERMITS",
        "Water heater replacements in Charlotte and nearby jurisdictions may require a permit and inspection depending on the fuel, whether it is a like-for-like swap, and current Mecklenburg County or city rules. SAFE will not pretend a permit is never needed. We confirm the current requirement for your address before work, because municipal rules change.",
        "Quote an installed job", QuoteHref, "Code extras", "/water-heater-code-upgrades/", false, true, false, null,
        [
            S("How we handle it",
                "If a permit is required for your replacement, it is part of a professional install—not a homeowner surprise after the tank is already in. Fees and inspection scheduling are disclosed. We follow the inspector’s corrections if the existing vent or drain was never legal."),
            S("Official sources",
                "Use the City of Charlotte / Mecklenburg County permitting portals and the NC plumbing/mechanical code as the authority. This page is guidance for homeowners, not a substitute for the permit office. We describe our process and point you to the government source rather than scraping a permit cheat-sheet that may be stale."),
            S("What homeowners mix up",
                "A permit is not the same as an expansion tank, a pan, or a T&P pipe. Those are code items that may be required whether or not a permit is pulled. See the extras page for those dollars."),
            S("Shelby, China Grove and nearby cities",
                "Permit rules follow the address, not our Charlotte shop. We confirm the jurisdiction on the quote so a China Grove job is not quoted as a Charlotte closet swap.")
        ],
        ["/water-heater-replacement-charlotte-nc/", "/water-heater-code-upgrades/", "/water-heater-installation-process/", "/water-heater-cost-charlotte-nc/"]);

    private static SeoPageDraft P1Attic() => new(
        "P1", "en", "/attic-water-heater-installation/",
        "Attic Water Heater Replacement",
        "Attic Water Heater Replacement in Charlotte, NC | SAFE Water Heater",
        "Replacing a water heater in an attic: drain pan, overflow, flooring, two-person access and leak risk. Charlotte photo-quote from SAFE—not the $1,199 standard closet price.",
        "Acceso difícil", "Transaccional", "ATTIC INSTALLS",
        "An attic water heater is a different job: limited flooring, hotter working conditions, drain-pan and overflow piping, and a real leak-damage risk to ceilings. The $1,199 closet package does not apply automatically. Send photos of the hatch, walkway and pan.",
        "Attic evaluation", QuoteHref, "Tight spaces", "/difficult-access-water-heater-installation/", false, true, false, null,
        [
            S("What we require",
                "A proper pan, a drain that actually leaves the attic, safe walking boards, and two-person handling for a 40- or 50-gallon tank. We will refuse an unsafe hatch rather than drop a tank through drywall."),
            S("Cost",
                "Expect extra labor above standard replacement. Same-day attic jobs are less likely because staging takes time. Pan, overflow piping and hatch constraints are quoted from photos—never assume the $1,199 closet number."),
            S("Leak damage above living space",
                "An attic tank that fails can wet insulation and ceilings. Pair this page with the leak-damage guide: stop water, photograph, then replace. A cheap closet price does not apply."),
            S("Photos that change the quote",
                "Hatch size, walkway boards, pan drain route, and whether two people can stand beside the tank. A 50-gallon unit that will not fit the hatch has to be discussed before we load the truck.")
        ],
        ["/difficult-access-water-heater-installation/", "/water-heater-leak-damage/", "/water-heater-code-upgrades/", "/water-heater-replacement-quote/"]);

    private static SeoPageDraft P1DifficultAccess() => new(
        "P1", "en", "/difficult-access-water-heater-installation/",
        "Water Heater Installation in Tight or Difficult Spaces",
        "Difficult Access Water Heater Installation | Crawlspace, Closet, Stairs | SAFE",
        "Water heater replacement in crawlspaces, tight closets and stair carries in Charlotte. Photo-based extra labor—SAFE will not pretend it is a standard $1,199 swap.",
        "Acceso difícil", "Transaccional", "TIGHT ACCESS",
        "Closets with no pull space, crawlspace tanks, basement stairs with a turn, and fenced yards all add labor. SAFE prices that from photos of the removal path—not from an optimistic phone call.",
        "Upload path photos", QuoteHref, "Attic installs", "/attic-water-heater-installation/", false, true, false, null,
        [
            S("What to photograph",
                "The tank, the doorway, every turn, stairs, and the truck-to-door path. A 50-gallon tank will not make a corner a 40-gallon barely cleared."),
            S("Relocation",
                "Moving a heater to a garage or closet to fix a bad attic is a plumbing redesign (drain, pan, electric or gas, possible permit). It is quoted as a project, not a package upsell.")
        ],
        ["/attic-water-heater-installation/", "/water-heater-replacement-quote/", "/water-heater-code-upgrades/", "/water-heater-installation-process/"]);

    private static SeoPageDraft P1Process() => new(
        "P1", "en", "/water-heater-installation-process/",
        "What to Expect During Water Heater Installation",
        "Water Heater Installation Process in Charlotte, NC | SAFE Water Heater",
        "What happens during a SAFE water heater installation: photos, scope, arrival window, 2–4 hour swap, old-tank removal, testing, warranty registration and inspection if required.",
        "Proceso", "Informacional comercial", "THE PROCESS",
        "A standard like-for-like tank replacement with SAFE is a planned 2–4 hour visit: protect the path, drain and disconnect the old unit, set the new one, connect water and energy, fill, start, test fixtures, and register warranty records. You should know the scope and price before the truck arrives.",
        "Schedule installation", QuoteHref, "Charlotte replacement", "/water-heater-replacement-charlotte-nc/", false, true, false, null,
        [
            S("Before we arrive",
                "Clear the path, secure pets, and keep the electrical panel or gas meter accessible. If the tank is leaking, complete emergency shutoffs. Have a decision-maker on site for any hidden damage."),
            S("During the visit",
                "The crew confirms the quoted extras, installs, and shows you the shutoffs. Hot water return time depends on tank fill and first recovery—not a 30-second miracle."),
            S("After",
                "Keep the data plate photo and your SAFE warranty reference. Watch for drips over the first day and text us if something is wet. Permit inspections, when required, are scheduled per the jurisdiction.")
        ],
        ["/water-heater-replacement-charlotte-nc/", "/water-heater-replacement-quote/", "/water-heater-permit-charlotte-nc/", "/water-heater-insurance-warranty/"]);

    private static SeoPageDraft P1Warranty() => new(
        "P1", "en", "/water-heater-insurance-warranty/",
        "Water Heater Warranty and Insurance Guide",
        "Water Heater Warranty and Insurance Guide | Charlotte, NC | SAFE",
        "Manufacturer vs SAFE labor warranty, registration, transfer, and how homeowners document leak damage for insurance. Register your unit after installation.",
        "Garantía/seguro", "Protección", "WARRANTY + INSURANCE",
        "There are two warranties on a qualifying SAFE install: the manufacturer’s limited unit warranty (commonly 5 years on the tanks we promote, model booklet controls) and SAFE’s 2-year limited installation labor warranty for defects caused by our original workmanship. Homeowners insurance is a third conversation—leak damage to floors is not the tank warranty.",
        "Register a unit", "/warranty/register", "Coverage details", "/warranty", false, false, false, null,
        [
            S("Manufacturer vs labor",
                "The factory covers specified tank or parts failures per the written warranty, usually requiring professional installation and registration. SAFE labor covers our workmanship, not someone else’s later repairs, owner abuse, or drywall from a leak."),
            S("Insurance and leak damage",
                "Photograph the tank, serial number, and rooms before demolition. Shut off water. Call your carrier. SAFE can replace the heater; we are not the public adjuster. See the leak-damage page for mitigation order."),
            S("Keep coverage valid",
                "Register at /warranty/register with address, photos and install date. Unregistered units and homemade electrical splices are how claims die.")
        ],
        ["/warranty", "/warranty/register", "/water-heater-leak-damage/", "/new-water-heater-not-working/"]);

    private static SeoPageDraft P1NewNotWorking() => new(
        "P1", "en", "/new-water-heater-not-working/",
        "New Water Heater Not Working",
        "New Water Heater Not Working | Warranty Checklist | SAFE Charlotte",
        "New water heater not heating after install: breaker, gas, air in lines, mixing valve. SAFE warranty support for Charlotte installations we performed.",
        "Post-instalación", "Soporte", "NEW UNIT SUPPORT",
        "A brand-new tank that is cold is usually power/gas not actually on, a tripped high-limit from a dry-fire, air in hot lines, or a closed isolation valve—not “the new heater is junk.” If SAFE installed it, use warranty support instead of a second contractor.",
        "Request support", TextHref, "Register warranty", "/warranty/register", false, false, false, null,
        [
            S("Non-invasive checklist",
                "",
                [
                    "Confirm the 240V breaker or gas shutoff is on.",
                    "Confirm cold-inlet and hot-outlet valves are open.",
                    "Give an electric tank a full recovery cycle after filling—never energize an empty tank.",
                    "If you smell gas or see sparks, stop and follow emergency steps."
                ]),
            S("Still down?",
                "Text the install address, serial number and a photo of the control or breaker. Warranty labor applies to qualifying SAFE workmanship; manufacturer parts follow the factory process.")
        ],
        ["/water-heater-insurance-warranty/", "/electric-water-heater-not-working/", "/gas-water-heater-not-working/", "/water-heater-installation-process/"]);

    private static SeoPageDraft P1Quote() => new(
        "P1", "en", "/water-heater-replacement-quote/",
        "Water Heater Quote From Photos",
        "Water Heater Quote From Photos | Charlotte, NC | SAFE Water Heater",
        "Get a Charlotte water heater replacement quote from eight photos: tank, label, connections, energy, pan, T&P, access and leak. Text 650-669-6890 or submit the form.",
        "Cotización remota", "Transaccional", "PHOTO QUOTE",
        "SAFE quotes replacements from photos so the $1,199 or $1,349 package is only promised when it fits. Fill the form first, then text the same pictures to 650-669-6890. A quote is not a diagnosis of hidden wiring inside walls.",
        "Text photos now", TextHref, "Call SAFE", CallHref, false, true, true, null,
        [
            S("The eight photos that prevent bad quotes",
                "",
                [
                    "1. Full tank from 6–8 feet away.",
                    "2. Data plate / serial label.",
                    "3. Water connections on top.",
                    "4. Electrical junction or gas control and shutoff.",
                    "5. Drain pan (or the floor if there is no pan).",
                    "6. T&P discharge pipe.",
                    "7. The path out of the home (doors, stairs, hatch).",
                    "8. Close-up of any leak, rust or error code."
                ]),
            S("What you get back",
                "Matching published package, or a short list of extras (expansion tank, pan, access labor). We call or text to schedule. Financing remains optional via the SAFE plan or Synchrony.")
        ],
        ["/water-heater-cost-charlotte-nc/", "/water-heater-replacement-charlotte-nc/", "/water-heater-code-upgrades/", "/financing"]);

    private static SeoPageDraft P1Realtor() => new(
        "P1", "en", "/realtor-water-heater-services/",
        "Water Heater Services for Realtors",
        "Water Heater Services for Realtors | Charlotte Closings | SAFE",
        "Realtor water heater help in Charlotte: inspection failures, pre-listing replacement, photo quotes and documentation for closing. HOME INDOR TECHNOLOGY INC.",
        "Realtor B2B", "B2B", "REALTORS",
        "Inspection-day water heater failures should not kill a Charlotte closing. SAFE provides photo quotes, standard packages, and install documentation (model, serial, warranties) agents can share with both sides. We are not a home inspector.",
        "Open a realtor request", TextHref, "Property managers", "/property-manager-water-heater-program/", false, true, false, null,
        [
            S("Typical requests",
                "Failed inspection (age, T&P, pan, rust), vacant-house cold water, and “replace before listing.” Tell us the closing date; same-day is only when a crew exists."),
            S("How to start",
                "Text the property address, access instructions, and the inspection note. We return a written scope that a buyer, seller or attorney can attach."),
            S("What we will put in writing",
                "Model/size recommendation, published package or extra-item list, whether a permit is likely, and warranty registration after install. We do not write “as-is heater is fine” opinions that belong to the inspector."),
            S("Timing vs closing",
                "A standard closet swap is often 2–4 hours once parts and access are confirmed. Attic, gas venting, and occupied-showings schedules are the usual delays—not the $1,199 headline.")
        ],
        ["/property-manager-water-heater-program/", "/water-heater-replacement-quote/", "/water-heater-permit-charlotte-nc/", "/water-heater-insurance-warranty/"]);

    private static SeoPageDraft P1PropertyManager() => new(
        "P1", "en", "/property-manager-water-heater-program/",
        "Water Heater Replacement for Property Managers",
        "Water Heater Replacement for Property Managers | Charlotte | SAFE",
        "Property-manager water heater program in Charlotte: multi-unit replacements, photo intake, per-address warranty records and clear installed pricing.",
        "Property manager", "B2B", "PROPERTY MANAGERS",
        "SAFE can replace tanks across a small portfolio with one intake process: photos, published packages, extra-item lists, and a warranty record per address. We specialize in water heaters, which keeps turnover faster than a general plumber who is also on a slab leak.",
        "Start a PM account", TextHref, "Realtor services", "/realtor-water-heater-services/", false, true, false, null,
        [
            S("What managers get",
                "Consistent 40- and 50-gallon electric pricing, gas quotes from photos, tenant-safe emergency shutoff guidance, and serial-number records for owners."),
            S("SLA honesty",
                "We will not write a 1-hour 24/7 SLA we cannot staff. Urgent leaks are triaged the same day when a crew is free; otherwise we help you shut the unit down safely and book the first real slot.")
        ],
        ["/realtor-water-heater-services/", "/water-heater-emergency/", "/water-heater-replacement-quote/", "/water-heater-service-near-me/"]);

    private static SeoPageDraft P2TanklessProblems() => new(
        "P2", "en", "/tankless-water-heater-problems/",
        "Tankless Water Heater Problems",
        "Tankless Water Heater Problems | Error Codes, Flow, Scale | SAFE Charlotte",
        "Tankless water heater problems: error codes, cold-water sandwich, scale, ignition and recirculation. Charlotte hard water makes descaling part of ownership.",
        "Tankless diagnóstico", "Informacional", "TANKLESS ISSUES",
        "Tankless complaints cluster around error codes, not enough flow to fire the burner, scale on the heat exchanger, ignition lockout, and recirculation timing (cold-water sandwich). Hard water around Charlotte makes annual maintenance a requirement, not an upsell.",
        "Request tankless diagnosis", TextHref, "Tankless install", "/tankless-water-heater-installation/", false, false, false, null,
        [
            S("Before you reset it 20 times",
                "Photograph the error code, the inlet filter, and the gas or electrical supply. Low flow at one shower may be a fixture, not the heater. No fire plus a gas smell is an emergency, not a code reset."),
            S("When a tank is the better repair",
                "If the panel or gas meter cannot support a proper tankless, replacing with a 40- or 50-gallon tank is the honest path. We will say so.")
        ],
        ["/tankless-water-heater-installation/", "/no-hot-water/", "/water-heater-maintenance/", "/water-heater-buying-guide/"]);

    private static SeoPageDraft P2Maintenance() => new(
        "P2", "en", "/water-heater-maintenance/",
        "Water Heater Maintenance Guide",
        "Water Heater Maintenance Guide | Flush, Anode, T&P | SAFE Charlotte",
        "Water heater maintenance in Charlotte: leak checks, T&P, anode, flush decisions and hard-water sediment. Know when flushing a weak tank causes a leak.",
        "Mantenimiento", "Informacional", "MAINTENANCE",
        "Monthly: look for leaks, pan water, and rust. Periodically: confirm T&P is not dripping and the area is dry. Annual professional service can include anode inspection, pressure check and a flush—only if the tank is healthy enough to survive it.",
        "Schedule a visit", TextHref, "Noisy tank?", "/water-heater-making-noise/", false, false, false, null,
        [
            S("Flushing is not automatic",
                "Manufacturer instructions, water quality and tank age decide. An old Charlotte tank with a rusty drain valve can fail the moment you open it. We inspect before we flush."),
            S("Temperature",
                "120°F is a common scald-reduction setpoint. Homes with mixing valves or special sanitizing needs should be set by a technician, not a random YouTube number."),
            S("What maintenance cannot do",
                "It cannot save a tank-shell leak or a 15-year unit that is already rumbling and rusty. That is a replacement conversation.")
        ],
        ["/water-heater-making-noise/", "/repair-or-replace-water-heater/", "/smelly-discolored-hot-water/", "/water-heater-leaking/"]);

    private static SeoPageDraft P2HeatPump() => new(
        "P2", "en", "/heat-pump-water-heater-installation/",
        "Heat Pump Water Heater Installation",
        "Heat Pump Water Heater Installation in Charlotte, NC | SAFE",
        "Heat pump (hybrid) water heater install in Charlotte: space, noise, condensate, 240V circuit and efficiency. Quoted only when the room can actually support one.",
        "Heat pump", "Investigación comercial", "HEAT PUMP / HYBRID",
        "A heat-pump water heater pulls heat from the air, so it needs cubic footage, a condensate drain, and a place where cooler air and fan noise are acceptable. Garages sometimes work; tiny closets usually do not. SAFE quotes these as custom jobs, not as the $1,199 electric tank package.",
        "Evaluate a hybrid", QuoteHref, "Buying guide", "/water-heater-buying-guide/", false, true, false, null,
        [
            S("Site requirements",
                "Clearances per the manufacturer, a dedicated circuit, condensate disposal, and enough air volume. Attic hybrids add leak risk like any attic tank."),
            S("Efficiency vs fit",
                "The operating-cost story is real in mild climates when the unit can breathe. If the only location is a 24-inch closet, buy a standard electric tank.")
        ],
        ["/water-heater-buying-guide/", "/40-gallon-electric-water-heater-installation/", "/water-heater-code-upgrades/", "/what-size-water-heater-do-i-need/"]);

    private static SeoPageDraft P2Special() => new(
        "P2", "en", "/water-heater-special-installations/",
        "Water Heaters for Special Home Conditions",
        "Water Heaters for Mobile Homes, Wells, Hard Water and Condos | SAFE Charlotte",
        "Special water heater installs: manufactured homes, well water, hard water, condos and HOA rules. Charlotte-area custom quotes from SAFE.",
        "Casos especiales", "Investigación", "SPECIAL CONDITIONS",
        "Mobile/manufactured homes, private wells, very hard water, and condos with HOA closet rules do not use a generic internet kit. SAFE quotes these from photos and, when needed, the home’s HUD label, well setup or HOA spec.",
        "Request a custom evaluation", QuoteHref, "Hard water smell", "/smelly-discolored-hot-water/", false, true, false, null,
        [
            S("Manufactured homes",
                "Often need listed mobile-home heaters, specific strapping and exterior closets. A standard residential tank may not be legal or safe there."),
            S("Wells and hard water",
                "Sediment, odor and anode wear accelerate. Pretreatment and maintenance matter more than brand slogans. We will not ignore a sand-clogged tank."),
            S("Condos and HOAs",
                "Pan, drain, and replacement window rules can dominate the job. Send the HOA document with the photos.")
        ],
        ["/water-heater-code-upgrades/", "/smelly-discolored-hot-water/", "/attic-water-heater-installation/", "/water-heater-buying-guide/"]);

    private static SeoPageDraft P2LeakDamage() => new(
        "P2", "en", "/water-heater-leak-damage/",
        "Water Heater Leak Damage: Next Steps",
        "Water Heater Leak Damage: Next Steps | Charlotte, NC | SAFE",
        "Water heater leak damage: stop the water, document for insurance, dry the area, then replace the tank. SAFE handles the heater; your carrier handles structure.",
        "Daño por fuga", "Informacional", "LEAK DAMAGE",
        "Stop the water first, then photograph everything, then dry, then replace the heater. Mixing those steps is how mold grows and insurance claims get messy. SAFE replaces the water heater; flooring and drywall are restoration or insurance territory unless you hire that separately.",
        "Get heater help", CallHref, "Leaking tank guide", "/water-heater-leaking/", true, false, false, null,
        [
            S("Order of operations",
                "",
                [
                    "Shut cold water and energy if it is safe.",
                    "Photograph tank, serial, rooms and standing water.",
                    "Notify your insurer if structure is wet.",
                    "Extract water; do not run HVAC blindly over wet cavities without advice.",
                    "Replace the failed tank—do not reinstall a rusted unit behind new drywall."
                ]),
            S("What we need",
                "Access to remove the tank and a decision on the replacement package. We can often work the same week as restoration if the area is safe."),
            S("Insurance vs tank warranty",
                "The manufacturer warranty and SAFE labor warranty do not pay for soaked flooring. Photograph serial numbers and rooms before demolition. Then replace the heater so a new tank is not sitting in a still-wet closet."),
            S("Do not delay the tank",
                "Restoration can take weeks. The leaking shell should come out as soon as the area is electrically safe. A $1,199–$1,349 electric package is usually cheaper than a second flood from the same unit.")
        ],
        ["/water-heater-leaking/", "/water-heater-emergency/", "/water-heater-insurance-warranty/", "/same-day-water-heater-replacement/"]);

    private static SeoSection S(string heading, string body, IReadOnlyList<string>? bullets = null) =>
        new() { Heading = heading, Body = body, Bullets = bullets };
}

internal sealed class SeoPageDraft
{
    public string Priority { get; }
    public string Language { get; }
    public string Slug { get; }
    public string H1 { get; }
    public string Title { get; }
    public string Description { get; }
    public string Cluster { get; }
    public string Intent { get; }
    public string Eyebrow { get; }
    public string AnswerFirst { get; }
    public string CtaLabel { get; }
    public string CtaHref { get; }
    public string SecondaryCtaLabel { get; }
    public string SecondaryCtaHref { get; }
    public bool ShowSafetyBar { get; }
    public bool ShowPricing { get; }
    public bool IsQuotePage { get; }
    public string? AlternateLangSlug { get; }
    public IReadOnlyList<SeoSection> Sections { get; }
    public IReadOnlyList<string> RelatedSlugs { get; }

    public SeoPageDraft(
        string priority, string language, string slug, string h1, string title, string description,
        string cluster, string intent, string eyebrow, string answerFirst,
        string ctaLabel, string ctaHref, string secondaryCtaLabel, string secondaryCtaHref,
        bool showSafetyBar, bool showPricing, bool isQuotePage, string? alternateLangSlug,
        IReadOnlyList<SeoSection> sections, IReadOnlyList<string> relatedSlugs)
    {
        Priority = priority;
        Language = language;
        Slug = slug;
        H1 = h1;
        Title = title;
        Description = description;
        Cluster = cluster;
        Intent = intent;
        Eyebrow = eyebrow;
        AnswerFirst = answerFirst;
        CtaLabel = ctaLabel;
        CtaHref = ctaHref;
        SecondaryCtaLabel = secondaryCtaLabel;
        SecondaryCtaHref = secondaryCtaHref;
        ShowSafetyBar = showSafetyBar;
        ShowPricing = showPricing;
        IsQuotePage = isQuotePage;
        AlternateLangSlug = alternateLangSlug;
        Sections = sections;
        RelatedSlugs = relatedSlugs;
    }
}
