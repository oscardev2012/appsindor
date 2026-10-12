namespace RealtorNews.Data;

public sealed record CityPlace(string Name, string County, string State, string Slug);
public sealed record CityRegion(string Title, string Summary, IReadOnlyList<CityPlace> Places);
public sealed record CityGuide(string Slug, string Name, string Founded, string Body, IReadOnlyList<string> Tags, IReadOnlyList<string> Questions, string Tip);
public sealed record SeasonalItem(string Season, string Author, string Role, string Title, string Body, string Advice, string Tag);
public sealed record GuideCard(string Category, string Title, string Summary);
public sealed record VendorService(string Title, string Body);
public sealed record VendorOffer(string Title, string Body, IReadOnlyList<string> Includes);
public sealed record VendorLesson(string Title, string Body, string Author);

public static class LocalCatalog
{
    public static readonly string[] AskTopics = ["Buying", "Selling", "Relocation", "Homeownership"];

    public static readonly string[] VendorServices =
    [
        "Crawl Space Remake",
        "Inspection & Risk Review",
        "Water Heater Replacement",
        "HVAC Replacement",
        "Remodeling Project",
        "Plumbing Service Call",
        "Roof Service",
        "Painting Estimate"
    ];

    public static readonly IReadOnlyList<CityRegion> Regions =
    [
        new("Charlotte & Mecklenburg County", "The center city plus the towns that shape the immediate metro.",
        [
            new("Charlotte", "Mecklenburg County", "NC", "charlotte"),
            new("Matthews", "Mecklenburg County", "NC", "matthews"),
            new("Mint Hill", "Mecklenburg County", "NC", "mint-hill"),
            new("Pineville", "Mecklenburg County", "NC", "pineville"),
            new("Huntersville", "Mecklenburg County", "NC", "huntersville"),
            new("Cornelius", "Mecklenburg County", "NC", "cornelius"),
            new("Davidson", "Mecklenburg / Iredell", "NC", "davidson")
        ]),
        new("North & Lake Norman", "Lake-oriented and growing communities along I-77 and NC-16.",
        [
            new("Mooresville", "Iredell County", "NC", "mooresville"),
            new("Troutman", "Iredell County", "NC", "troutman"),
            new("Statesville", "Iredell County", "NC", "statesville"),
            new("Denver", "Lincoln County", "NC", "denver"),
            new("Lincolnton", "Lincoln County", "NC", "lincolnton"),
            new("Maiden", "Catawba / Lincoln", "NC", "maiden")
        ]),
        new("East, Cabarrus & Rowan", "Historic centers and northeast connections toward Concord and Salisbury.",
        [
            new("Concord", "Cabarrus County", "NC", "concord"),
            new("Kannapolis", "Cabarrus / Rowan", "NC", "kannapolis"),
            new("Harrisburg", "Cabarrus County", "NC", "harrisburg"),
            new("Mount Pleasant", "Cabarrus County", "NC", "mount-pleasant"),
            new("Midland", "Cabarrus County", "NC", "midland"),
            new("Locust", "Stanly / Cabarrus", "NC", "locust"),
            new("Salisbury", "Rowan County", "NC", "salisbury"),
            new("China Grove", "Rowan County", "NC", "china-grove")
        ]),
        new("South & Union County", "Southeast Charlotte, Monroe, and the NC–SC state-line corridor.",
        [
            new("Indian Trail", "Union County", "NC", "indian-trail"),
            new("Stallings", "Union / Mecklenburg", "NC", "stallings"),
            new("Weddington", "Union County", "NC", "weddington"),
            new("Waxhaw", "Union County", "NC", "waxhaw"),
            new("Monroe", "Union County", "NC", "monroe"),
            new("Wesley Chapel", "Union County", "NC", "wesley-chapel"),
            new("Marvin", "Union County", "NC", "marvin")
        ]),
        new("West & Gaston County", "Independent downtowns and west-side routes toward Charlotte.",
        [
            new("Gastonia", "Gaston County", "NC", "gastonia"),
            new("Belmont", "Gaston County", "NC", "belmont"),
            new("Mount Holly", "Gaston County", "NC", "mount-holly"),
            new("Cramerton", "Gaston County", "NC", "cramerton"),
            new("McAdenville", "Gaston County", "NC", "mcadenville"),
            new("Lowell", "Gaston County", "NC", "lowell"),
            new("Dallas", "Gaston County", "NC", "dallas"),
            new("Bessemer City", "Gaston County", "NC", "bessemer-city")
        ]),
        new("South Carolina side", "York and Lancaster communities often included in a Charlotte-region search.",
        [
            new("Fort Mill", "York County", "SC", "fort-mill"),
            new("Tega Cay", "York County", "SC", "tega-cay"),
            new("Rock Hill", "York County", "SC", "rock-hill"),
            new("Lake Wylie", "York County", "SC", "lake-wylie"),
            new("Clover", "York County", "SC", "clover"),
            new("York", "York County", "SC", "york"),
            new("Lancaster", "Lancaster County", "SC", "lancaster")
        ])
    ];

    public static readonly IReadOnlyList<CityGuide> FeaturedCities =
    [
        new("charlotte", "Charlotte", "Incorporated in 1768",
            "Charlotte is the hub of the region: Uptown work and culture, older neighborhoods, newer communities, parks, and routes in every direction. The useful question is not “best neighborhood,” but which daily routine, budget, home type, and commute you can live with.",
            ["Uptown & urban living", "Established neighborhoods", "Suburban communities", "Relocation"],
            ["Which areas fit the commute you will actually make?", "Do you want an older home, new construction, or lower-maintenance living?", "Which costs and upkeep change with the home type?"],
            "Rank payment, commute, home age, space, and setting before you start touring."),
        new("gastonia", "Gastonia", "Incorporated in 1877",
            "West of Charlotte, Gastonia has its own downtown, long-standing streets, newer development, and easier access to outdoor Gaston County. Compare home age, lot size, commute at your real travel hour, and which city or county services cover the address.",
            ["Historic homes", "Downtown", "Outdoor access", "West-side commute"],
            ["How does the commute work at your actual travel time?", "What should you inspect carefully in an older property?", "Which city or county services apply to the address?"],
            "Ask about electrical, plumbing, roof, drainage, HVAC, crawl space, and permits before planning cosmetic work."),
        new("concord", "Concord", "Founded in 1796",
            "Concord mixes a historic center with established neighborhoods, expanding residential areas, and access toward northeast Charlotte. Confirm city limits, utilities, school assignment, and the routes you will use every week.",
            ["Historic center", "Newer communities", "Northeast access"],
            ["Is the property inside city limits and which utilities serve it?", "How do older and newer sections compare for maintenance?", "Which routes matter for work, school, or regular activities?"],
            "Verify jurisdiction and utilities for the exact address, not just the mailing city."),
        new("matthews", "Matthews", "Incorporated in 1879",
            "Matthews is known for a walkable downtown, parks, greenways, and access to southeast Charlotte. Homes range from established streets to newer communities, so age, lot, traffic, and town boundaries are the useful comparison points.",
            ["Historic downtown", "Parks & greenways", "Southeast Charlotte access"],
            ["Is the address in the Town of Matthews or only using a Matthews mailing address?", "What home age and maintenance level fit your plans?", "How does traffic affect the routine you expect?"],
            "A Matthews ZIP code is not the same as living inside the town."),
        new("kannapolis", "Kannapolis", "Incorporated in 1984",
            "Kannapolis grew from mill history and now has a renewed downtown, research activity, rail access, and neighborhoods across two counties. Compare county, utilities, commute, property age, and how close you want to be to downtown.",
            ["Mill history", "Downtown renewal", "Two-county living"],
            ["Which county contains the property?", "How close do you want to be to the downtown core?", "How do older housing areas compare with newer construction?"],
            "County lines change taxes, schools, and some services—confirm them early.")
    ];

    public static readonly IReadOnlyList<SeasonalItem> Seasonal =
    [
        new("Fall 2026", "Local Realtors", "Charlotte network",
            "Schedule the heating check before the first cold morning.",
            "Do not wait until the thermostat fails on a cold day. Confirm it responds, change the filter when it is due, and book a qualified tune-up while calendars still have openings.",
            "Keep the service record. If you plan to sell, share known system concerns early.",
            "Heating readiness"),
        new("Fall 2026", "Local Realtors", "Charlotte network",
            "Look at the crawl space before fall rain settles in.",
            "Summer humidity plus fall rain can show moisture, odors, damaged insulation, standing water, or wood wear. View from a safe access point and stay away from electrical lines, wildlife, or suspected mold.",
            "Photograph what you see and ask the right professional before moisture spreads.",
            "Crawl space & rain"),
        new("Fall 2026", "Preferred Vendors", "Home services",
            "Five exterior checks before fall rain.",
            "Clear roof drainage, confirm downspouts send water away from the foundation, look for low spots that hold water, check crawl-space vents or doors, and note localized wood damage.",
            "Fix the water source before repairing cosmetic damage.",
            "Drainage & projects"),
        new("Fall 2026", "Preferred Vendors", "Roofing",
            "Review the roof from the ground before leaves and winter wind.",
            "Look for missing or lifted shingles, loose flashing, clogged gutters, heavy branches, and new ceiling stains. Do not climb a wet, steep, or damaged roof.",
            "Schedule a review when you see a change instead of waiting for an interior leak.",
            "Roof & gutters"),
        new("Winter 2026–2027", "Preferred Vendors", "HVAC",
            "Cold-weather checklist for heat and hot water.",
            "Keep supply and return vents open, use a clean filter that fits the system, know how to shut off water, and watch the water-heater area for moisture or corrosion.",
            "If you smell gas or suspect carbon monoxide, leave and call emergency or utility services.",
            "Winter systems"),
        new("Winter 2026–2027", "Local Realtors", "Selling",
            "A winter showing should still feel comfortable and cared for.",
            "Confirm the heat works, keep walkways safe, stop active leaks, use enough light, and have service records ready for major systems when you can.",
            "Solve safety and active damage before spending on seasonal decoration.",
            "Winter selling")
    ];

    public static readonly IReadOnlyList<GuideCard> StageGuides =
    [
        new("Buying", "How much cash do you really need to buy?", "Separate down payment, due diligence, earnest money, inspection, appraisal, and reserves."),
        new("Selling", "What to fix before listing—and what to leave alone", "Prioritize safety, function, and buyer confidence before cosmetic projects."),
        new("Investing", "A simple framework for evaluating a rental", "Compare purchase price, repairs, realistic rent, operating costs, and exit options."),
        new("Homeownership", "Your first-year home maintenance calendar", "A Charlotte-season checklist for HVAC, roof, drainage, crawl space, plumbing, and safety."),
        new("Buying", "Inspection report: urgent issue or normal wear?", "Separate safety concerns, major systems, maintenance items, and preferences."),
        new("Selling", "How Realtors build a pricing range", "Location, condition, timing, concessions, and comparable sales matter more than one estimate.")
    ];

    public static readonly IReadOnlyList<VendorService> VendorWork =
    [
        new("Inspection & risk review", "Mechanical and structural risk awareness, approximate scope, technical explanations, and notes that support an informed negotiation."),
        new("HVAC & water heaters", "Condition review, repair or replacement planning, warranty questions, installation, maintenance, and current equipment promotions."),
        new("Crawl Space Remake", "Debris cleanout, presentation refresh, ground covering, moisture observations, and up to two small localized wood repairs when they fit the advertised scope."),
        new("Kitchens, bathrooms & interiors", "Renovation planning including layout, finishes, cabinetry, plumbing or electrical coordination, drywall, flooring, and painting."),
        new("Exterior repairs & improvements", "Painting, siding, windows, doors, patios, concrete or driveway work, and weather protection for the project."),
        new("Roof evaluation & repair coordination", "Roof-condition questions, maintenance, repairs, replacement options, and storm or tree-damage support through the roofing partner."),
        new("Electrical & smoke/CO safety", "Electrical correction coordination and interconnected smoke-detector work, subject to the property and confirmed code requirements."),
        new("Major repairs & rehabilitation", "Repair planning for structural, roof, water, fire, and other significant findings after the required professional evaluation.")
    ];

    public static readonly IReadOnlyList<VendorOffer> VendorOffers =
    [
        new("Crawl Space Remake", "A focused clean-up so the crawl space is cleaner, more orderly, and easier to evaluate.",
            ["Debris cleanout", "Visual condition review", "Ground-covering reset within confirmed scope", "Up to two small localized wood repairs when suitable", "Before-and-after documentation"]),
        new("40 & 50-gallon water heaters", "Installed promotional prices, plus tax, for a qualifying standard replacement.",
            ["Electric: 40 gal $1,199 · 50 gal $1,299", "Natural gas: 40 gal $1,449 · 50 gal $1,499", "Standard qualifying replacement scope", "Warranty details confirmed before installation"]),
        new("A/C & HVAC replacement options", "Cooling and heating change-outs selected around the home and existing installation.",
            ["Replacement-only option: $2,400 for a qualifying standard change-out", "Ducane gas complete qualifying change-out shown at $6,799", "Sizing and compatibility confirmed after inspection", "Ductwork and modifications quoted separately"]),
        new("Plumbing inspection or emergency call", "A licensed visit to inspect a concern or respond to an urgent request.",
            ["Initial assessment", "Problem explanation", "Recommended next step", "Repair pricing before additional work"]),
        new("Roof service & maintenance", "Review and maintenance support through a Charlotte-area preferred roofing vendor.",
            ["Roof condition review", "Maintenance recommendations", "Repair and replacement options", "Emergency tree-damage support"]),
        new("Interior & exterior painting", "Estimates based on preparation, size, materials, and finish expectations.",
            ["Project walk-through", "Scope and preparation review", "Interior or exterior options", "Written custom estimate"])
    ];

    public static readonly IReadOnlyList<VendorLesson> VendorLessons =
    [
        new("When is crawl-space wood damage a small repair?", "A localized, accessible section may fit a limited repair. Widespread moisture, sagging, inaccessible framing, or engineered work needs a separate evaluation.", "SAFE Project Solution"),
        new("Three signs your water heater needs attention.", "Rust-colored water, moisture around the tank, and uneven hot water deserve a look before a small issue becomes an emergency.", "SAFE HVAC Solution"),
        new("What should you check after a Charlotte storm?", "From the ground, look for missing shingles, displaced flashing, gutter debris, ceiling stains, and changes around vents. Do not climb a wet roof.", "Majestic Roofing")
    ];
}
