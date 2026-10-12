const services = [
  {
    title: "Crawl Space Problems",
    icon: "⌂",
    category: "structure",
    tags: "crawl space crawlspace under house moisture vapor barrier insulation standing water",
    desc: "Moisture, odors, damaged insulation, standing water, or concerns beneath the home.",
  },
  {
    title: "Moisture & Water Intrusion",
    icon: "≈",
    category: "structure",
    tags: "humidity moisture water leak wet mold drainage vapor",
    desc: "Persistent dampness, condensation, water entry, stains, or musty smells.",
  },
  {
    title: "Rotten or Damaged Wood",
    icon: "▤",
    category: "structure",
    tags: "wood rot rotten joist subfloor beam sill soft floor termite damage",
    desc: "Soft floors, deteriorated joists, damaged beams, subfloor, or sill concerns.",
  },
  {
    title: "Piers, Posts & Supports",
    icon: "▥",
    category: "structure",
    tags: "pier pillar post support leaning sinking floor crawl space",
    desc: "Leaning, shifting, cracked, undersized, or questionable supports under the home.",
  },
  {
    title: "Foundation Concerns",
    icon: "△",
    category: "structure",
    tags: "foundation crack settlement sinking structural wall slab",
    desc: "Cracks, settlement, uneven floors, movement, or visible foundation damage.",
  },
  {
    title: "Structural Repairs",
    icon: "╱",
    category: "structure",
    tags: "structural framing beam load bearing engineer repair wall",
    desc: "Framing, beams, load-bearing concerns, sagging areas, and structural repair scopes.",
  },
  {
    title: "Roofing & Roof Leaks",
    icon: "⌃",
    category: "exterior",
    tags: "roof roofing leak shingle flashing storm gutter",
    desc: "Leaks, missing shingles, flashing issues, storm damage, or aging roof components.",
  },
  {
    title: "Siding, Trim & Exterior Wood",
    icon: "▧",
    category: "exterior",
    tags: "siding fascia soffit trim exterior wood rot",
    desc: "Damaged siding, rotted fascia or trim, soffits, and exterior wood repairs.",
  },
  {
    title: "Drainage & Grading",
    icon: "↘",
    category: "exterior",
    tags: "drainage grading yard water gutter downspout erosion",
    desc: "Water collecting near the house, poor grading, erosion, gutters, or downspouts.",
  },
  {
    title: "Painting",
    icon: "◩",
    category: "interior",
    tags: "paint painting interior exterior wall ceiling trim",
    desc: "Interior or exterior painting, touch-ups, wall preparation, and damaged finishes.",
  },
  {
    title: "Drywall & Ceiling Repairs",
    icon: "□",
    category: "interior",
    tags: "drywall ceiling hole crack texture water stain",
    desc: "Holes, cracks, water-damaged areas, texture matching, and ceiling repairs.",
  },
  {
    title: "Flooring & Subfloor",
    icon: "▦",
    category: "interior",
    tags: "floor flooring subfloor tile vinyl hardwood soft uneven",
    desc: "Damaged flooring, soft spots, uneven areas, installation, and subfloor repairs.",
  },
  {
    title: "Handyman Repairs",
    icon: "◇",
    category: "interior",
    tags: "handyman small repair door cabinet trim punch list",
    desc: "Punch lists, doors, trim, hardware, cabinets, mounting, and smaller repairs.",
  },
  {
    title: "Plumbing",
    icon: "◒",
    category: "systems",
    tags: "plumbing pipe faucet toilet drain water leak",
    desc: "Leaks, fixtures, toilets, drains, supply lines, and plumbing repair needs.",
  },
  {
    title: "Electrical & Lighting",
    icon: "ϟ",
    category: "systems",
    tags: "electrical outlet light fixture switch panel wiring",
    desc: "Outlets, switches, fixtures, lighting, wiring concerns, and electrical repairs.",
  },
  {
    title: "HVAC & Indoor Comfort",
    icon: "❄",
    category: "systems",
    tags: "hvac air conditioning heat heating cooling duct thermostat",
    desc: "Heating, cooling, airflow, thermostat, ductwork, and comfort problems.",
  },
  {
    title: "Water Heaters",
    icon: "♨",
    category: "systems",
    tags: "water heater hot water tank gas electric leak",
    desc: "No hot water, leaking tanks, replacement, installation, and related components.",
  },
  {
    title: "Inspection Report Analysis",
    icon: "✓",
    category: "buying",
    tags: "inspection report analysis buyer realtor closing repairs",
    desc: "Contractor-focused review of findings, priorities, trades, and repair pathways.",
  },
  {
    title: "Pre-Closing Walkthrough",
    icon: "→",
    category: "buying",
    tags: "before closing walk through walkthrough contractor buyer final",
    desc: "Walk the property with a contractor before closing to discuss visible concerns.",
  },
  {
    title: "Pre-Listing Value & Repair Walkthrough",
    icon: "＋",
    category: "buying",
    tags: "selling listing realtor repairs value improvement punch list market",
    desc: "Identify repairs and improvements that may strengthen condition and market readiness.",
  },
  {
    title: "Post-Closing Home Walkthrough",
    icon: "⌂",
    category: "buying",
    tags: "after closing new homeowner walkthrough questions handyman electrician plumber",
    desc: "Get contractor guidance and specialist recommendations after purchasing a home.",
  },
  {
    title: "Home Insurance Comparison",
    icon: "▣",
    category: "buying",
    tags: "insurance homeowners policy coverage quote premium deductible realtor buyer lender aseguranza seguro cobertura",
    desc: "Connect with a licensed insurance agent to compare available home coverage options, limits, deductibles, and premiums.",
  },
];
services.push(
  {
    title: "Wall & Ceiling Cracks",
    icon: "╱",
    category: "interior",
    tags: "crack cracks drywall ceiling wall settlement nail pop",
    desc: "Hairline cracks, recurring cracks, nail pops, separation, and signs that may need structural review.",
  },
  {
    title: "New Electrical Outlet",
    icon: "ϟ",
    category: "systems",
    tags: "new outlet receptacle plug gfci usb exterior power",
    desc: "Add power indoors or outdoors after circuit, access, code, and location are verified.",
  },
  {
    title: "New Light or Ceiling Fan",
    icon: "☼",
    category: "systems",
    tags: "light lighting ceiling fan recessed can fixture install",
    desc: "Install or relocate lighting, fans, switches, and compatible support boxes.",
  },
  {
    title: "Exterior Hose Connection",
    icon: "◒",
    category: "systems",
    tags: "hose hose bib spigot outdoor faucet exterior water",
    desc: "Add or replace an exterior hose bib after routing, shutoff, wall, and freeze protection are reviewed.",
  },
  {
    title: "Crawl Space Dehumidifier",
    icon: "≈",
    category: "structure",
    tags: "dehumidifier crawl space humidity percent moisture drain outlet",
    desc: "Measure humidity, identify moisture sources, size equipment, and plan electrical and drainage needs.",
  },
  {
    title: "Tree Roots Near the House",
    icon: "♧",
    category: "exterior",
    tags: "tree root roots foundation sewer drain driveway close house",
    desc: "Assess visible proximity and route concerns involving foundation, drainage, sewer, paving, or arborist review.",
  },
  {
    title: "Toilet Replacement",
    icon: "◉",
    category: "systems",
    tags: "toilet commode replace install flange wax ring valve",
    desc: "Replace a toilet and evaluate the flange, shutoff, supply line, floor condition, access, and disposal.",
  },
  {
    title: "Sink & Faucet Replacement",
    icon: "◒",
    category: "systems",
    tags: "sink faucet vanity kitchen bathroom drain disposal replace",
    desc: "Replace a sink or faucet after checking the countertop opening, drains, valves, cabinet, and plumbing layout.",
  },
  {
    title: "Door Replacement",
    icon: "▯",
    category: "interior",
    tags: "door interior exterior prehung slab frame trim hardware replace",
    desc: "Repair or replace interior, exterior, prehung, slab, storm, or specialty doors.",
  },
  {
    title: "Drywall Hole Repair",
    icon: "□",
    category: "interior",
    tags: "hole wall drywall patch texture paint plumbing access",
    desc: "Patch holes and damaged areas, restore texture, and prepare for matching paint.",
  },
  {
    title: "Deck Repair & Staining",
    icon: "▤",
    category: "exterior",
    tags: "deck board stain paint sealer rail post stairs color",
    desc: "Clean, repair, stain, seal, or evaluate rails, boards, stairs, posts, and structural connections.",
  },
  {
    title: "Gutters & Downspouts",
    icon: "⌄",
    category: "exterior",
    tags: "gutter gutters downspout clean install drainage overflow",
    desc: "Clean, repair, replace, or redirect gutters and downspouts to manage roof water.",
  },
  {
    title: "House Cleaning & Turnover",
    icon: "✦",
    category: "interior",
    tags: "clean cleaning deep clean move out move in turnover monthly",
    desc: "Routine, deep, move-in, move-out, and property-turnover cleaning coordination.",
  },
  {
    title: "Caulking & Weather Sealing",
    icon: "≈",
    category: "interior",
    tags: "caulk seal weather draft window door bathtub shower",
    desc: "Address deteriorated sealant, drafts, gaps, and water-prone joints around common home surfaces.",
  },
  {
    title: "Cabinets, Shelves & Mounting",
    icon: "▥",
    category: "interior",
    tags: "cabinet shelf shelves tv mount hardware closet",
    desc: "Install, adjust, repair, or securely mount common household items after wall support is confirmed.",
  },
  {
    title: "Fence & Gate Repairs",
    icon: "╫",
    category: "exterior",
    tags: "fence gate post board latch repair wood",
    desc: "Repair boards, posts, gates, latches, and localized damaged sections.",
  },
  {
    title: "Wood Privacy Fence",
    icon: "╫",
    category: "exterior",
    tags: "fence fencing wood wooden privacy pine cedar stockade shadowbox board on board",
    desc: "Pressure-treated pine, cedar, stockade, shadowbox, and board-on-board privacy fencing.",
  },
  {
    title: "Vinyl Privacy Fence",
    icon: "╫",
    category: "exterior",
    tags: "fence fencing vinyl pvc privacy white color woodgrain",
    desc: "Low-maintenance white, colored, or premium woodgrain vinyl privacy fencing.",
  },
  {
    title: "Chain-Link Fence",
    icon: "╫",
    category: "exterior",
    tags: "fence fencing chain link galvanized black coated pet yard",
    desc: "Galvanized or black-coated chain-link fencing for pets, yards, and practical enclosures.",
  },
  {
    title: "Aluminum Fence",
    icon: "╫",
    category: "exterior",
    tags: "fence fencing aluminum ornamental metal pool black bronze",
    desc: "Ornamental aluminum fencing for pools, yards, slopes, and low-maintenance applications.",
  },
  {
    title: "Ranch & Split-Rail Fence",
    icon: "╫",
    category: "exterior",
    tags: "fence fencing ranch split rail farm open wire livestock",
    desc: "Open ranch or split-rail fencing, with optional wire for pets or livestock.",
  },
  {
    title: "Fence Gates & Posts",
    icon: "╫",
    category: "exterior",
    tags: "fence gate gates post posts leaning latch hinge repair replace",
    desc: "Walk gates, drive gates, leaning posts, hinges, latches, and localized structural repairs.",
  },
  {
    title: "Windows & Screens",
    icon: "▦",
    category: "exterior",
    tags: "window screen glass lock draft caulk repair",
    desc: "Repair screens, locks, trim, sealant, drafts, and visible window concerns.",
  },
  {
    title: "Appliance Hookups",
    icon: "▣",
    category: "systems",
    tags: "dishwasher disposal washer dryer range appliance hookup",
    desc: "Coordinate common appliance connections when electrical, gas, plumbing, and permit requirements are satisfied.",
  },
  {
    title: "Concrete Driveways & Patios",
    icon: "▰",
    category: "exterior",
    tags: "concrete driveway patio slab walkway sidewalk pad cement concreto",
    desc: "New concrete, replacement sections, patios, walkways, equipment pads, and driveway work.",
  },
  {
    title: "Bathroom Remodeling",
    icon: "◫",
    category: "interior",
    tags: "bathroom remodel renovation shower tub tile vanity toilet baño",
    desc: "Bathroom updates ranging from fixture and finish replacement to complete remodeling.",
  },
  {
    title: "Kitchen Remodeling",
    icon: "▣",
    category: "interior",
    tags: "kitchen remodel renovation cabinets countertop backsplash cocina",
    desc: "Kitchen improvements involving cabinets, countertops, backsplash, fixtures, finishes, and layout changes.",
  },
  {
    title: "Cabinets & Countertops",
    icon: "▥",
    category: "interior",
    tags: "cabinet cabinets granite quartz countertop kitchen vanity gabinete",
    desc: "Cabinet installation, repair, hardware, granite, quartz, and countertop replacement.",
  },
  {
    title: "New Deck Construction",
    icon: "▤",
    category: "exterior",
    tags: "deck new build wood composite stairs rail railing porch terraza",
    desc: "New wood or composite decks, stairs, guards, handrails, footings, and permit coordination.",
  },
  {
    title: "Insulation",
    icon: "▧",
    category: "interior",
    tags: "insulation attic crawl space wall fiberglass blown spray foam aislamiento",
    desc: "Attic, floor, wall, and crawl-space insulation review, replacement, or installation.",
  },
  {
    title: "Tile Installation",
    icon: "▦",
    category: "interior",
    tags: "tile ceramic porcelain shower backsplash floor grout loseta",
    desc: "Ceramic or porcelain floors, backsplashes, shower surrounds, grout, and localized tile repair.",
  },
  {
    title: "Demolition & Haul-Off",
    icon: "◇",
    category: "interior",
    tags: "demolition demo removal haul debris trash cleanout basura",
    desc: "Selective demolition, construction debris removal, property cleanouts, and haul-off.",
  },
  {
    title: "Pressure Washing",
    icon: "≈",
    category: "exterior",
    tags: "pressure wash power washing house driveway deck siding limpieza",
    desc: "Exterior cleaning for siding, concrete, decks, patios, and other suitable surfaces.",
  },
  {
    title: "Garage Doors",
    icon: "▤",
    category: "exterior",
    tags: "garage door opener spring track panel replace repair garaje",
    desc: "Garage-door replacement, opener installation, panels, tracks, springs, and safety concerns.",
  },
);
const priceGuide = {
  "Crawl Space Problems":
    "Makeover promotion: $3,500 • Broader moisture work: $1,500–$8,000+",
  "Moisture & Water Intrusion":
    "Localized correction: $500–$2,500 • Multi-system work: $2,500–$8,000+",
  "Rotten or Damaged Wood":
    "Joist sistering: $150–$325 each • Full replacement: $500–$2,500 each",
  "Piers, Posts & Supports":
    "Standard post/pier: $650–$1,600 each • engineered/complex: $1,600–$4,000+",
  "Foundation Concerns":
    "Minor crack repair: $500–$2,500 • Major repair: $3,000–$15,000+",
  "Structural Repairs":
    "Localized framing: $750–$3,500 • Major beam/framing: $3,500–$15,000+",
  "Roofing & Roof Leaks":
    "Minor leak/flashing repair: $350–$1,500 • Larger scope: $1,500–$5,000+",
  "Siding, Trim & Exterior Wood":
    "Localized repair: $300–$1,500 • Larger sections: $1,500–$5,000+",
  "Drainage & Grading":
    "Downspout/local correction: $300–$1,200 • Drain/grading project: $1,500–$6,000+",
  "Flooring & Subfloor":
    "Featured LVP option: $5.50/sq. ft. installed — scope exclusions apply",
  "Handyman Repairs": "Typical service visit: $150–$300",
  "Drywall Hole Repair": "Small repair: $150–$350 • Larger patch: $350–$700",
  "Door Replacement":
    "Interior prehung labor reference: $150–$350, plus door/finish",
  "Toilet Replacement":
    "Standard installed replacement: $450–$1,200 • flange/floor/code corrections extra",
  "Sink & Faucet Replacement":
    "Faucet replacement: $150–$350 • sink replacement: $350–$1,200+",
  "New Electrical Outlet": "Typical Charlotte range: $185–$385",
  "New Light or Ceiling Fan":
    "Existing-box fan: $185–$345 • New wiring: $375–$675",
  "Electrical & Lighting": "Electrical diagnostic: $95–$185",
  "Crawl Space Dehumidifier": "Professional installed range: $1,100–$2,800",
  "Deck Repair & Staining": "Standard deck clean/stain reference: $300–$700",
  "Gutters & Downspouts": "Cleaning reference: $75–$225",
  Painting: "Standard bedroom reference: $300–$650",
  "Drywall & Ceiling Repairs": "Typical patch range: $150–$700",
  "Fence & Gate Repairs":
    "Localized section: $250–$600 • Larger repair: $600–$2,500+",
  "Wood Privacy Fence": "SAFE planning price: $24–$60 per linear ft. installed",
  "Vinyl Privacy Fence":
    "SAFE planning price: $30–$75 per linear ft. installed • premium woodgrain may be higher",
  "Chain-Link Fence": "SAFE planning price: $18–$35 per linear ft. installed",
  "Aluminum Fence": "SAFE planning price: $28–$40 per linear ft. installed",
  "Ranch & Split-Rail Fence":
    "SAFE planning price: $14–$22 per linear ft. installed",
  "Fence Gates & Posts":
    "Post replacement: $250–$500 • walk gate: $450–$1,200 • drive gate: $1,200–$5,000+",
  Plumbing: "Common repair: $150–$850 • Larger/hidden-line work: $850–$3,500+",
  "HVAC & Indoor Comfort":
    "Diagnostic/repair: $95–$1,500 • Complete systems: $6,399–$6,799 promotional",
  "Water Heaters":
    "Electric: $1,199–$1,299 + tax • Gas: $1,449–$1,499 + tax • Difficult access: +$100 • Expansion tank: +$100 • Corroded connection package: +$350",
  "Wall & Ceiling Cracks":
    "Cosmetic patch: $150–$700 • Structural cause quoted after review",
  "Exterior Hose Connection":
    "Replacement: $150–$350 • New routed connection: $350–$850",
  "Tree Roots Near the House":
    "Arborist/root review: $150–$500 • Removal or repairs quoted separately",
  "House Cleaning & Turnover":
    "Standard cleaning: $150–$350 • Deep/turnover: $300–$800+",
  "Caulking & Weather Sealing":
    "Localized sealing: $125–$350 • Multi-area work: $350–$1,000+",
  "Cabinets, Shelves & Mounting":
    "Small installation: $125–$350 • Multi-item/project: $350–$1,500+",
  "Windows & Screens":
    "Screen/lock/seal: $75–$350 • Window replacement: $500–$1,500+ each",
  "Appliance Hookups":
    "Standard hookup: $150–$350 • New utility connections quoted separately",
  "Concrete Driveways & Patios":
    "Standard concrete: $9–$16 per sq. ft. • demolition/reinforcement may be extra",
  "Bathroom Remodeling":
    "Refresh: $4,500–$10,000 • full remodel: $10,000–$30,000+",
  "Kitchen Remodeling":
    "Refresh: $7,500–$18,000 • full remodel: $20,000–$60,000+",
  "Cabinets & Countertops":
    "Cabinet scope: $3,000–$15,000+ • countertops: $45–$110 per sq. ft.",
  "New Deck Construction":
    "Pressure-treated wood: $30–$55 per sq. ft. • composite: $45–$80+ per sq. ft.",
  Insulation: "Attic or floor planning range: $1.50–$4.50 per sq. ft.",
  "Tile Installation":
    "Standard installation: $10–$25 per sq. ft. • shower work quoted separately",
  "Demolition & Haul-Off":
    "Small load: $250–$650 • room/project removal: $750–$3,500+",
  "Pressure Washing":
    "House or standard area: $250–$650 • larger combinations: $650–$1,200+",
  "Garage Doors": "Repair: $175–$650 • replacement: $1,200–$3,500+",
  "Inspection Report Analysis":
    "Contractor report analysis: $195–$450 based on report size and scope",
  "Pre-Closing Walkthrough":
    "Contractor walkthrough: $195–$450 based on property size and scope",
  "Pre-Listing Value & Repair Walkthrough":
    "Repair-planning walkthrough: $195–$450 based on property size and scope",
  "Post-Closing Home Walkthrough":
    "Welcome Home guidance: free when qualifying",
  "Home Insurance Comparison":
    "No-cost quote comparison • Final price and eligibility are confirmed by the licensed insurance agent",
};
services.forEach(
  (s) =>
    (s.price =
      priceGuide[s.title] ||
      "Free initial inspection • Written quote after review"),
);
services.push(
  {
    title: "Roof Rafter Repair",
    icon: "⌃",
    category: "structure",
    tags: "rafter roof framing broken split sagging sister repair cabio viga techo",
    desc: "Repair or sister a damaged roof rafter after the load, span, access, and moisture source are checked.",
    price:
      "Localized sister repair: $500–$1,500 each • replacement/complex access: $1,500–$4,000+",
  },
  {
    title: "Floor Joist Repair",
    icon: "▤",
    category: "structure",
    tags: "joist floor framing sister rot crawl space viga madera piso",
    desc: "Sister, reinforce, or replace damaged floor joists in an accessible crawl space or basement.",
    price:
      "Sister repair: $250–$750 each • full/complex replacement: $750–$2,500 each",
  },
  {
    title: "Main Beam or Girder Repair",
    icon: "▬",
    category: "structure",
    tags: "main beam girder structural crawl space sagging madera viga principal",
    desc: "Evaluate and repair a principal beam or girder supporting joists and floor loads.",
    price:
      "Localized reinforcement: $1,500–$4,500 • replacement/major scope: $5,000–$20,000+",
  },
  {
    title: "Sill Plate or Rim Joist Repair",
    icon: "▤",
    category: "structure",
    tags: "sill plate rim joist band rot termite exterior wall crawl space solera",
    desc: "Repair deteriorated perimeter framing where the floor system meets the foundation.",
    price: "Localized section: $750–$2,500 • multiple sections: $2,500–$8,000+",
  },
  {
    title: "Subfloor Repair",
    icon: "▦",
    category: "structure",
    tags: "subfloor plywood osb soft floor rot bathroom kitchen crawl space",
    desc: "Replace soft, swollen, or damaged subfloor before reinstalling the finished flooring.",
    price: "Small area: $400–$1,200 • room/large area: $1,200–$4,500+",
  },
  {
    title: "Crawl Space Wood Repair",
    icon: "⌂",
    category: "structure",
    tags: "crawlspace crawl space wood rot joist subfloor beam repair madera podrida",
    desc: "Localized framing repairs below the home after moisture, insects, and load conditions are identified.",
    price:
      "Minor localized repair: $500–$1,500 • multiple members: $1,500–$7,500+",
  },
  {
    title: "Support Post or Pier Installation",
    icon: "▥",
    category: "structure",
    tags: "pier post pillar support jack crawl space footing pilar soporte",
    desc: "Add or replace a properly supported post or pier beneath a beam after load and footing review.",
    price:
      "Standard support: $650–$1,600 each • engineered/complex support: $1,600–$4,000+",
  },
  {
    title: "Foundation Crack Sealing",
    icon: "△",
    category: "structure",
    tags: "foundation crack seal epoxy polyurethane block wall grieta cimiento",
    desc: "Seal a qualifying non-structural foundation crack and address the likely water path.",
    price:
      "Typical crack: $400–$1,200 • multiple/complex cracks: $1,200–$3,500+",
  },
  {
    title: "Crawl Space Vapor Barrier",
    icon: "≈",
    category: "structure",
    tags: "vapor barrier plastic crawlspace crawl space ground cover moisture plástico barrera",
    desc: "Install ground plastic to reduce soil vapor; wall sealing and full encapsulation are separate.",
    price:
      "Basic ground cover: $1.25–$3.50/sq. ft. • SAFE makeover package: $3,500",
  },
  {
    title: "Crawl Space Encapsulation",
    icon: "⌂",
    category: "structure",
    tags: "encapsulation crawl space sealed walls vapor barrier dehumidifier insulation",
    desc: "Full moisture-control system that may include sealed walls, heavy liner, drainage, insulation, and dehumidification.",
    price: "Typical full system: $8,000–$18,000+ • inspection required",
  },
  {
    title: "Crawl Space Drain or Sump Pump",
    icon: "≈",
    category: "structure",
    tags: "crawl space drain sump pump standing water french drain drainage",
    desc: "Manage recurring water with drainage and/or a sump system after the source and discharge location are verified.",
    price: "Sump pump: $900–$2,500 • interior drain system: $2,500–$8,000+",
  },
  {
    title: "Plumbing Diagnostic Visit",
    icon: "◒",
    category: "systems",
    tags: "plumber diagnostic service call leak inspection diagnosis plomería",
    desc: "Evaluate a plumbing symptom before parts or concealed repairs can be priced.",
    price:
      "Standard diagnostic: $100–$250 • after-hours/emergency may be higher",
  },
  {
    title: "Crawl Space Supply Pipe Repair",
    icon: "◒",
    category: "systems",
    tags: "crawl space pipe plumbing supply leak pex copper cpvc under house tubería fuga",
    desc: "Repair an accessible pressurized water line beneath the house.",
    price:
      "Accessible spot repair: $200–$650 • difficult/multiple repairs: $650–$1,500+",
  },
  {
    title: "Crawl Space Drain Pipe Repair",
    icon: "◒",
    category: "systems",
    tags: "crawl space drain waste pipe plumbing pvc leak sewage under house tubo drenaje",
    desc: "Repair or replace an accessible drain or waste line under the home, including proper support and slope.",
    price: "Localized repair: $300–$900 • longer section/reroute: $900–$3,500+",
  },
  {
    title: "Burst or Leaking Pipe Repair",
    icon: "◒",
    category: "systems",
    tags: "burst pipe leak water line frozen wall ceiling crawl space tubería rota",
    desc: "Stop and repair a leaking supply pipe; access and finish restoration may be separate.",
    price:
      "Accessible repair: $200–$750 • concealed/access restoration: $750–$2,500+",
  },
  {
    title: "Drain Cleaning",
    icon: "◒",
    category: "systems",
    tags: "clog drain cleaning snake sink tub shower main line clogged destapar drenaje",
    desc: "Clear a fixture, branch, or main drain blockage and identify recurring symptoms.",
    price: "Fixture/branch drain: $150–$350 • main line: $250–$650",
  },
  {
    title: "Sewer Camera Inspection",
    icon: "◉",
    category: "systems",
    tags: "sewer camera inspection main drain roots broken pipe video alcantarillado",
    desc: "Inspect the accessible sewer lateral with a camera to locate roots, breaks, offsets, or buildup.",
    price: "Typical inspection: $200–$500 • locating/report may cost extra",
  },
  {
    title: "Sewer Line Repair or Replacement",
    icon: "◒",
    category: "systems",
    tags: "sewer line repair replacement trenchless excavation main drain roots",
    desc: "Repair a damaged sewer lateral using spot excavation, lining, bursting, or full replacement as appropriate.",
    price:
      "Spot repair: $1,500–$5,000 • replacement/trenchless: $4,000–$20,000+",
  },
  {
    title: "Whole-House Repiping",
    icon: "◒",
    category: "systems",
    tags: "repipe whole house pex polybutylene galvanized plumbing pipes pb2110",
    desc: "Replace aging or problematic supply piping with a planned distribution system.",
    price:
      "Typical home: $4,000–$12,000+ • wall repairs/fixtures may be separate",
  },
  {
    title: "Toilet Repair",
    icon: "◉",
    category: "systems",
    tags: "toilet running clogged fill valve flapper wax ring leak commode reparar",
    desc: "Repair running, leaking, rocking, or poorly flushing toilets after the cause is confirmed.",
    price: "Internal parts: $150–$350 • wax ring/reset: $225–$450",
  },
  {
    title: "Toilet Flange Repair",
    icon: "◉",
    category: "systems",
    tags: "toilet flange broken floor rot rocking leak wax ring",
    desc: "Repair or replace a damaged toilet flange; subfloor damage is priced separately.",
    price:
      "Accessible flange repair: $300–$650 • subfloor/access work: $650–$1,500+",
  },
  {
    title: "Faucet Replacement",
    icon: "◒",
    category: "systems",
    tags: "faucet kitchen bathroom replace install leaking grifo llave",
    desc: "Replace a compatible kitchen or bathroom faucet using serviceable existing connections.",
    price: "Standard labor: $150–$350 • specialty/access changes: $350–$750+",
  },
  {
    title: "Garbage Disposal Replacement",
    icon: "◒",
    category: "systems",
    tags: "garbage disposal disposer sink kitchen replace plumbing electrical triturador",
    desc: "Replace a compatible disposal and verify drain and electrical connections.",
    price:
      "Standard installed range: $450–$650 • new wiring/plumbing changes extra",
  },
  {
    title: "Shower or Tub Valve Repair",
    icon: "◒",
    category: "systems",
    tags: "shower tub valve cartridge leak temperature plumbing ducha tina válvula",
    desc: "Repair a cartridge or replace a valve; wall access and finish restoration may be separate.",
    price: "Cartridge repair: $200–$450 • valve replacement: $650–$1,500+",
  },
  {
    title: "Water Main Service Line",
    icon: "◒",
    category: "systems",
    tags: "water main service line yard leak meter trench pipe replacement",
    desc: "Repair or replace the private water service between the meter and home.",
    price: "Spot repair: $750–$2,500 • replacement: $2,500–$8,000+",
  },
  {
    title: "Hose Bib Replacement",
    icon: "◒",
    category: "systems",
    tags: "hose bib spigot outdoor faucet freeze proof replace exterior",
    desc: "Replace an accessible exterior faucet or frost-resistant hose bib.",
    price: "Standard replacement: $200–$450 • wall/access changes: $450–$900+",
  },
  {
    title: "GFCI Outlet Replacement",
    icon: "ϟ",
    category: "systems",
    tags: "gfci outlet receptacle bathroom kitchen exterior replace electrical",
    desc: "Replace a failed GFCI receptacle and test the protected circuit.",
    price: "Single existing-location replacement: $150–$300",
  },
  {
    title: "Switch or Outlet Replacement",
    icon: "ϟ",
    category: "systems",
    tags: "switch outlet receptacle plug dimmer replace electrical interruptor",
    desc: "Replace a standard existing switch, dimmer, or receptacle after basic circuit verification.",
    price:
      "First device/service minimum: $150–$275 • additional standard devices: $40–$100 each",
  },
  {
    title: "Dedicated Electrical Circuit",
    icon: "ϟ",
    category: "systems",
    tags: "dedicated circuit breaker wire appliance microwave outlet electrical circuito",
    desc: "Run a new circuit for an appliance or load after panel capacity and route are verified.",
    price:
      "Typical accessible run: $500–$1,500 • long/complex run: $1,500–$3,000+",
  },
  {
    title: "Circuit Breaker Replacement",
    icon: "ϟ",
    category: "systems",
    tags: "breaker tripping replace panel circuit electrical disyuntor",
    desc: "Diagnose repeated tripping and replace a compatible breaker when that is the confirmed repair.",
    price: "Standard breaker: $200–$450 • specialty/AFCI/GFCI: $300–$650",
  },
  {
    title: "Electrical Panel Replacement",
    icon: "ϟ",
    category: "systems",
    tags: "electrical panel service upgrade breaker box 100 200 amp meter",
    desc: "Replace or upgrade the main panel/service with permits and utility coordination when required.",
    price: "Panel replacement: $2,500–$5,500 • service upgrade: $4,000–$8,500+",
  },
  {
    title: "Recessed Light Installation",
    icon: "☼",
    category: "systems",
    tags: "recessed light canless downlight led new wiring ceiling install",
    desc: "Add LED recessed lights with layout, wiring, switching, and ceiling access considered.",
    price:
      "Existing accessible circuit: $175–$350 each • complex/new circuit extra",
  },
  {
    title: "Smoke or CO Detector Installation",
    icon: "ϟ",
    category: "systems",
    tags: "smoke detector carbon monoxide co alarm hardwired battery install",
    desc: "Replace compatible alarms or add required locations; interconnected wiring may change scope.",
    price: "Battery device: $75–$175 each • hardwired: $150–$350 each",
  },
  {
    title: "EV Charger Installation",
    icon: "ϟ",
    category: "systems",
    tags: "ev charger tesla level 2 electric vehicle 240 volt garage",
    desc: "Install a Level 2 charger after load calculation, panel capacity, route, and permit review.",
    price: "Typical installation: $900–$2,500 • service upgrades separate",
  },
  {
    title: "HVAC Diagnostic",
    icon: "❄",
    category: "systems",
    tags: "hvac diagnostic ac no cooling heat service call diagnosis",
    desc: "Diagnose heating, cooling, airflow, thermostat, electrical, or drainage symptoms.",
    price: "SAFE diagnostic visit: $100 • repair quoted after findings",
  },
  {
    title: "HVAC Capacitor or Contactor",
    icon: "❄",
    category: "systems",
    tags: "ac capacitor contactor outdoor unit not starting hvac repair",
    desc: "Replace a failed compatible electrical component after diagnosis and safety testing.",
    price: "Typical repair: $250–$650",
  },
  {
    title: "HVAC Blower Motor Repair",
    icon: "❄",
    category: "systems",
    tags: "blower motor air handler furnace fan no airflow hvac",
    desc: "Repair or replace the indoor blower motor or related module after diagnosis.",
    price: "Standard motor: $650–$1,400 • variable-speed/ECM: $1,200–$2,500+",
  },
  {
    title: "HVAC Refrigerant Leak Repair",
    icon: "❄",
    category: "systems",
    tags: "refrigerant freon leak coil line set ac not cooling hvac",
    desc: "Locate and repair a qualifying refrigerant leak; refrigerant and component replacement affect cost.",
    price:
      "Diagnostic/minor repair: $350–$1,200 • coil/major repair: $1,500–$4,000+",
  },
  {
    title: "Thermostat Replacement",
    icon: "❄",
    category: "systems",
    tags: "thermostat smart nest ecobee replace install hvac control",
    desc: "Install and configure a compatible standard or smart thermostat.",
    price: "Standard: $200–$450 • smart/rewiring: $300–$700+",
  },
  {
    title: "HVAC Duct Repair",
    icon: "❄",
    category: "systems",
    tags: "duct repair flex duct torn disconnected crawl space attic airflow",
    desc: "Reconnect, seal, or repair a damaged accessible duct up to the verified size.",
    price:
      "SAFE repair reference: $200 per duct up to 8 in. • larger/complex quoted",
  },
  {
    title: "HVAC Duct Replacement",
    icon: "❄",
    category: "systems",
    tags: "duct replacement flex duct crawl space attic airflow insulation",
    desc: "Replace a damaged accessible duct run after diameter, length, insulation, and routing are verified.",
    price:
      "SAFE replacement reference: $300 per duct up to 8 in. • larger/long runs quoted",
  },
  {
    title: "Dryer Vent Cleaning or Replacement",
    icon: "≈",
    category: "systems",
    tags: "dryer vent duct clean lint replace exterior hood fire safety",
    desc: "Clean an accessible dryer exhaust or replace an unsafe/damaged run.",
    price: "Cleaning: $150–$300 • replacement/reroute: $350–$1,200+",
  },
  {
    title: "Roof Shingle Repair",
    icon: "⌃",
    category: "exterior",
    tags: "roof shingles missing damaged repair wind storm tejas techo",
    desc: "Replace a localized group of compatible shingles and inspect nearby flashing.",
    price: "Minor repair: $350–$900 • larger section: $900–$2,500+",
  },
  {
    title: "Roof Flashing Repair",
    icon: "⌃",
    category: "exterior",
    tags: "roof flashing chimney wall step flashing leak boot valley",
    desc: "Repair flashing at a wall, chimney, vent, valley, or other penetration.",
    price:
      "Simple vent/flashing: $350–$900 • chimney/wall system: $900–$2,500+",
  },
  {
    title: "Roof Pipe Boot Replacement",
    icon: "⌃",
    category: "exterior",
    tags: "roof pipe boot vent boot plumbing vent leak rubber collar",
    desc: "Replace a deteriorated plumbing-vent boot and surrounding shingles as needed.",
    price: "Typical boot: $350–$750 each",
  },
  {
    title: "Roof Replacement",
    icon: "⌃",
    category: "exterior",
    tags: "roof replacement shingles tear off square new roofing techo completo",
    desc: "Replace the roofing system after measuring area, layers, pitch, decking, ventilation, and accessories.",
    price: "Asphalt shingle roof: $6–$10/sq. ft. • decking/steep access extra",
  },
  {
    title: "Roof Decking or OSB Replacement",
    icon: "⌃",
    category: "structure",
    tags: "roof decking osb plywood rotten sheathing replace sheet",
    desc: "Replace deteriorated roof sheathing discovered during repair or reroofing.",
    price: "Typical add-on: $125–$250 per sheet • structural framing separate",
  },
  {
    title: "Fascia or Soffit Repair",
    icon: "▧",
    category: "exterior",
    tags: "fascia soffit rot eave wood aluminum repair exterior roof edge",
    desc: "Repair localized deteriorated eave components and identify the moisture source.",
    price: "Localized repair: $350–$1,200 • larger runs: $1,200–$4,000+",
  },
  {
    title: "Gutter Installation",
    icon: "⌄",
    category: "exterior",
    tags: "gutter seamless aluminum downspout install replace roof drainage",
    desc: "Install or replace gutters and downspouts sized and routed for the roof area.",
    price: "Standard aluminum: $8–$16/linear ft. • guards/downspouts extra",
  },
  {
    title: "Downspout Extension or Burial",
    icon: "⌄",
    category: "exterior",
    tags: "downspout extension buried drain pipe yard drainage water foundation",
    desc: "Carry roof water away above grade or through a qualifying buried solid pipe.",
    price: "Above-grade extension: $75–$250 • buried run: $500–$2,000+",
  },
  {
    title: "French Drain Installation",
    icon: "↘",
    category: "exterior",
    tags: "french drain yard drainage water standing trench gravel pipe",
    desc: "Install a subsurface drain after slope, outlet, utilities, and water source are evaluated.",
    price:
      "Typical residential run: $25–$60/linear ft. • complex discharge extra",
  },
  {
    title: "Exterior Siding Repair",
    icon: "▧",
    category: "exterior",
    tags: "siding vinyl fiber cement hardie wood repair replace storm",
    desc: "Repair a localized section of vinyl, fiber-cement, or wood siding with compatible material.",
    price: "Localized repair: $400–$1,500 • larger elevation: $1,500–$6,000+",
  },
  {
    title: "Exterior Trim Wood Repair",
    icon: "▧",
    category: "exterior",
    tags: "exterior trim rot window door corner board wood repair",
    desc: "Replace deteriorated exterior trim and prepare it for sealing and paint.",
    price: "Small repair: $300–$900 • multiple areas: $900–$3,500+",
  },
  {
    title: "Window Replacement",
    icon: "▦",
    category: "exterior",
    tags: "window replacement vinyl wood glass insulated draft rotten frame ventana",
    desc: "Replace a standard window after opening, exterior finish, glass, and access are confirmed.",
    price: "Standard window: $650–$1,500 each • custom/wood: $1,500–$3,500+",
  },
  {
    title: "Exterior Door Replacement",
    icon: "▯",
    category: "exterior",
    tags: "exterior entry door prehung replace frame threshold rot storm",
    desc: "Replace a prehung exterior door and address compatible trim, threshold, and weather sealing.",
    price: "Standard installed: $1,200–$3,000 • premium/custom: $3,000–$7,500+",
  },
  {
    title: "Interior Door Repair or Replacement",
    icon: "▯",
    category: "interior",
    tags: "interior door sticking slab prehung hinge knob frame replace",
    desc: "Adjust a sticking door or replace the slab/prehung unit with compatible trim and hardware.",
    price: "Adjustment: $125–$250 • replacement: $350–$900+",
  },
  {
    title: "Baseboard or Interior Trim",
    icon: "▥",
    category: "interior",
    tags: "baseboard crown molding trim casing install repair carpentry moldura",
    desc: "Repair or install baseboard, casing, or crown molding, with finishing priced by scope.",
    price: "Typical installed: $6–$18/linear ft. • painting may be separate",
  },
  {
    title: "Drywall Ceiling Repair",
    icon: "□",
    category: "interior",
    tags: "ceiling drywall hole water damage sag patch texture repair",
    desc: "Repair a ceiling opening or damaged area after the leak or structural cause is corrected.",
    price: "Small patch: $250–$600 • larger/texture repair: $600–$1,500+",
  },
  {
    title: "Full Room Drywall Replacement",
    icon: "□",
    category: "interior",
    tags: "drywall room replace walls ceiling sheetrock demolition finish",
    desc: "Remove and replace room drywall, finish joints, and prepare for paint.",
    price: "Typical room: $1,500–$5,000+ • insulation/paint/mold separate",
  },
  {
    title: "Interior Room Painting",
    icon: "◩",
    category: "interior",
    tags: "paint bedroom room walls ceiling trim interior pintura cuarto",
    desc: "Prepare and paint a standard room; repairs, color changes, and trim affect price.",
    price:
      "Standard bedroom: $500–$1,200 • walls/ceiling/trim package: $900–$2,000+",
  },
  {
    title: "Whole Interior Painting",
    icon: "◩",
    category: "interior",
    tags: "whole house interior painting walls ceilings trim repaint",
    desc: "Repaint multiple rooms or the occupied interior after measuring surfaces and preparation.",
    price: "Typical home: $4,000–$12,000+ • size/prep/trim affect price",
  },
  {
    title: "Exterior House Painting",
    icon: "◩",
    category: "exterior",
    tags: "exterior house painting siding trim pressure wash prep pintura",
    desc: "Prepare and paint exterior siding and trim using a system compatible with the surface.",
    price: "Typical home: $4,000–$12,000+ • height/repairs affect price",
  },
  {
    title: "LVP Flooring Installation",
    icon: "▦",
    category: "interior",
    tags: "lvp luxury vinyl plank flooring install floating click piso vinilo",
    desc: "Install qualifying click-lock LVP over a ready, level, dry substrate.",
    price: "SAFE featured option: $5.50/sq. ft. installed • prep/removal extra",
  },
  {
    title: "Flooring Removal",
    icon: "▦",
    category: "interior",
    tags: "flooring removal carpet tile hardwood vinyl demo dispose",
    desc: "Remove and dispose of existing floor covering before new installation.",
    price: "Carpet/LVP: $1–$2.50/sq. ft. • tile/hardwood: $3–$8/sq. ft.",
  },
  {
    title: "Floor Leveling or Preparation",
    icon: "▦",
    category: "interior",
    tags: "floor leveling self leveler grind prep subfloor uneven flooring",
    desc: "Prepare a substrate by patching, grinding, or using approved leveling material.",
    price: "Localized prep: $300–$1,000 • broad leveling: $2–$6/sq. ft.",
  },
  {
    title: "Carpet Installation",
    icon: "▦",
    category: "interior",
    tags: "carpet padding install replace bedroom stairs alfombra",
    desc: "Install selected carpet and pad after room measurement and substrate review.",
    price: "Typical installed: $3–$8/sq. ft. • stairs/removal extra",
  },
  {
    title: "Hardwood Floor Refinishing",
    icon: "▦",
    category: "interior",
    tags: "hardwood wood floor refinish sand stain polyurethane piso madera",
    desc: "Sand and refinish qualifying solid hardwood after wear and remaining thickness are checked.",
    price: "Typical range: $4–$9/sq. ft. • repairs/stain changes extra",
  },
  {
    title: "Tile or Grout Repair",
    icon: "▦",
    category: "interior",
    tags: "tile cracked grout repair shower floor backsplash loseta boquilla",
    desc: "Replace accessible matching tile or repair deteriorated grout after the moisture cause is reviewed.",
    price:
      "Localized repair: $250–$900 • shower/waterproofing scope: $1,000–$5,000+",
  },
  {
    title: "Cabinet Repair or Hardware",
    icon: "▥",
    category: "interior",
    tags: "cabinet door hinge drawer hardware pull repair kitchen vanity",
    desc: "Adjust doors, replace compatible hinges/slides, or install knobs and pulls.",
    price:
      "Service visit: $150–$450 • multiple cabinets/hardware: $450–$1,500+",
  },
  {
    title: "Countertop Replacement",
    icon: "▰",
    category: "interior",
    tags: "countertop quartz granite laminate replace kitchen vanity",
    desc: "Template and install selected countertop material; plumbing and backsplash may be separate.",
    price: "Laminate: $35–$75/sq. ft. • quartz/granite: $60–$130/sq. ft.",
  },
  {
    title: "Bathroom Exhaust Fan",
    icon: "≈",
    category: "systems",
    tags: "bathroom exhaust fan vent humidity ceiling roof wall electrical duct",
    desc: "Replace a compatible bath fan or add properly routed exterior exhaust.",
    price:
      "Existing fan replacement: $250–$600 • new duct/wiring: $700–$1,800+",
  },
  {
    title: "Deck Board or Rail Repair",
    icon: "▤",
    category: "exterior",
    tags: "deck board railing handrail post stair repair rotten wood",
    desc: "Replace localized deck boards or repair a qualifying rail section after structural review.",
    price: "Boards: $25–$75 each installed • rail section: $250–$900",
  },
  {
    title: "Deck Staining or Painting",
    icon: "▤",
    category: "exterior",
    tags: "deck stain paint seal pressure wash prep color",
    desc: "Clean, prepare, and coat a wood deck using stain or paint selected for its condition.",
    price: "Stain/seal: $2.50–$6/sq. ft. • paint/heavy prep: $4–$9/sq. ft.",
  },
  {
    title: "Concrete Crack or Section Repair",
    icon: "▰",
    category: "exterior",
    tags: "concrete crack walkway driveway patio section repair trip hazard",
    desc: "Seal a qualifying crack or replace a damaged section after settlement and drainage are considered.",
    price:
      "Crack/local repair: $300–$1,200 • section replacement: $1,200–$5,000+",
  },
  {
    title: "Mailbox Installation",
    icon: "◇",
    category: "exterior",
    tags: "mailbox post install replace concrete curb wall buzón",
    desc: "Install a standard post-mounted mailbox after location and utility clearance are confirmed.",
    price: "Standard post mailbox: $250–$650 • masonry/custom: $650–$1,500+",
  },
  {
    title: "TV or Heavy Item Mounting",
    icon: "▥",
    category: "interior",
    tags: "tv mount mirror shelf heavy item wall studs fireplace mounting",
    desc: "Mount a compatible item after wall framing, anchors, weight, wiring, and height are checked.",
    price: "Standard TV/mirror: $150–$350 • masonry/concealed wiring extra",
  },
  {
    title: "Move-Out or Deep Cleaning",
    icon: "✦",
    category: "interior",
    tags: "move out deep cleaning empty house sale closing clean limpieza",
    desc: "Detailed cleaning before listing, occupancy, turnover, or closing.",
    price: "Typical home: $300–$900+ • size/condition/add-ons affect price",
  },
  {
    title: "Lawn Mowing and Basic Yard Care",
    icon: "♧",
    category: "exterior",
    tags: "lawn mowing grass yard cleanup leaves maintenance landscaping",
    desc: "Routine mowing, edging, blowing, and basic seasonal cleanup.",
    price: "Typical visit: $50–$150 • cleanup/project: $250–$1,500+",
  },
);
const guides = [
  {
    title: "Roof leak or roof damage",
    icon: "⌃",
    match: "roof roofing leak shingle ceiling stain storm",
    intro:
      "A roof leak can come from shingles, flashing, vents, penetrations, or drainage—not always from the spot where water appears inside.",
    check: [
      "From the ground, look for missing or lifted shingles.",
      "Check ceilings and attic areas for stains or dampness.",
      "Note whether the leak happens during wind-driven rain.",
      "Take photos without climbing onto the roof.",
    ],
    stop: "Stay off a wet or damaged roof. Active water near electrical fixtures, sagging ceilings, or storm-created openings need prompt professional attention.",
    prepare:
      "Photos of the exterior and interior stain, property address, approximate roof age, when the leak started, and any inspection-report page.",
    route:
      "We can review the information, explain the likely next step, provide a planning range when enough details are available, and connect the request with a participating roofing company.",
  },
  {
    title: "Crawl space moisture or odor",
    icon: "≈",
    match: "crawlspace crawl space moisture water odor mold humidity",
    intro:
      "Moisture under a home may involve drainage, plumbing, ventilation, ground vapor, insulation, wood damage, or more than one condition.",
    check: [
      "Notice whether the odor is stronger after rain.",
      "Look from the access opening for standing water or fallen insulation.",
      "Check nearby plumbing fixtures for leaks.",
      "Record indoor humidity if you have a monitor.",
    ],
    stop: "Do not enter a flooded, tight, contaminated, or unsafe crawl space. Suspected electrical hazards, sewage, heavy growth, or unstable framing require professional evaluation.",
    prepare:
      "Photos from the access opening, recent weather, odor location, visible water, foundation type, and any prior moisture work.",
    route:
      "We can help separate drainage, plumbing, moisture-control, and structural concerns, then recommend the appropriate participating specialty.",
  },
  {
    title: "Soft or uneven floor",
    icon: "▦",
    match: "soft uneven sagging floor subfloor joist wood rot",
    intro:
      "A soft or uneven floor can involve the finish flooring, subfloor, joists, moisture damage, supports, settlement, or several conditions together.",
    check: [
      "Mark where the floor feels soft or slopes.",
      "Check whether doors nearby stick or have changed.",
      "Look for stains, leaks, or moisture below when safely visible.",
      "Note whether the condition is getting worse.",
    ],
    stop: "Avoid loading an area that feels unstable. Sudden movement, severe sagging, broken framing, or damaged supports needs prompt professional review.",
    prepare:
      "Room location, photos or video, approximate affected size, crawl-space access, leak history, and inspection findings.",
    route:
      "We can help determine whether the first visit should be flooring, crawl-space, structural, plumbing, or another participating contractor.",
  },
  {
    title: "Foundation cracks or movement",
    icon: "△",
    match: "foundation crack settlement movement brick wall uneven",
    intro:
      "Not every crack means the same thing. Location, width, pattern, moisture, and whether it is changing help determine the proper next step.",
    check: [
      "Photograph the crack with a ruler for scale.",
      "Date the photo and monitor visible change.",
      "Check doors, windows, floors, and nearby exterior drainage.",
      "Note stair-step, horizontal, or widening patterns.",
    ],
    stop: "Rapid movement, bowing walls, major separation, falling masonry, or loss of support may require an engineer or qualified structural professional.",
    prepare:
      "Clear photos inside and outside, measurements, date first noticed, drainage conditions, and inspection report.",
    route:
      "SAFE can help organize the information and route the request to a foundation contractor, structural specialist, or engineer when appropriate.",
  },
  {
    title: "Plumbing leak or water damage",
    icon: "◒",
    match: "plumbing water leak pipe drain toilet faucet damage",
    intro:
      "The first priority is limiting water damage and identifying whether the source is a supply line, fixture, drain, appliance, roof, or hidden condition.",
    check: [
      "If safe, turn off the nearest fixture valve.",
      "Note whether the meter moves when fixtures are off.",
      "Photograph the source and affected areas.",
      "Record whether the water is clean, dirty, hot, or continuous.",
    ],
    stop: "Standing water near electrical equipment, sewage, ceiling bulging, or an uncontrolled supply leak needs urgent professional help. Use the main shutoff if you know it is safe to do so.",
    prepare:
      "Source location, photos, when it began, shutoff status, affected rooms, and insurance involvement if any.",
    route:
      "We can help identify whether plumbing, water mitigation, drywall, flooring, or multiple participating companies may be required.",
  },
  {
    title: "Electrical concern",
    icon: "ϟ",
    match: "electrical outlet breaker burning smell flicker light wiring",
    intro:
      "Electrical symptoms should be treated carefully. A warm outlet, repeated breaker trip, flickering, or burning smell may require a licensed electrical professional.",
    check: [
      "Note which outlet, switch, light, or appliance is involved.",
      "Record when the breaker trips—do not repeatedly reset it.",
      "Unplug the device if it can be done safely.",
      "Keep photos limited to visible exterior conditions.",
    ],
    stop: "Sparks, smoke, burning odor, heat, exposed energized wiring, or water near electricity are urgent. Leave the area and contact emergency or utility services when appropriate.",
    prepare:
      "Panel label, affected rooms, recent work, symptom timing, and clear exterior photos—never remove a live panel cover.",
    route:
      "We can guide the request and connect it with an appropriately qualified participating electrical company.",
  },
  {
    title: "Heating or air-conditioning problem",
    icon: "❄",
    match: "hvac ac air conditioning heat furnace thermostat cooling",
    intro:
      "Comfort problems can start with settings, filters, drainage, power, airflow, ductwork, or equipment—not every issue requires replacement.",
    check: [
      "Confirm thermostat mode and set point.",
      "Check whether the filter is visibly dirty.",
      "Note unusual sounds, odors, ice, or water.",
      "Check which rooms are affected and whether the outdoor unit runs.",
    ],
    stop: "Turn the system off for smoke, burning odor, severe electrical noise, suspected gas, or significant water near equipment. Gas concerns require immediate professional or utility guidance.",
    prepare:
      "Equipment photos, thermostat display, filter condition, indoor/outdoor symptoms, and approximate system age.",
    route:
      "We can help distinguish maintenance, repair, airflow, or replacement questions and connect you with a participating HVAC company.",
  },
  {
    title: "Water heater problem",
    icon: "♨",
    match: "water heater hot water tank leaking gas electric",
    intro:
      "No hot water, slow recovery, unusual noises, rusty water, or a leaking tank can have different causes and different levels of urgency.",
    check: [
      "Look for visible water without touching wiring or hot piping.",
      "Note whether the unit is gas or electric.",
      "Photograph the model and data label.",
      "Record tank size, approximate age, and the symptom.",
    ],
    stop: "Active tank leakage, gas odor, scorched wiring, or unsafe venting needs prompt professional attention. Do not attempt internal gas or electrical repairs.",
    prepare:
      "Data-label photo, full-unit photo, fuel type, tank size, location, access, and whether water is actively leaking.",
    route:
      "We can review the setup, discuss repair versus replacement, provide a planning range, and connect you with a participating water-heater installer.",
  },
  {
    title: "Inspection report question",
    icon: "✓",
    match: "inspection report buyer seller realtor closing finding",
    intro:
      "Inspection reports often combine safety items, maintenance, cosmetic findings, and recommendations for specialist evaluation.",
    check: [
      "Mark the items affecting safety or closing decisions.",
      "Group findings by trade: roof, electrical, plumbing, HVAC, structure.",
      "Note the response and closing deadlines.",
      "Avoid assuming the inspection wording is a final repair scope.",
    ],
    stop: "Do not rely on photos alone for safety-critical, structural, gas, electrical, or active-water conditions that need direct specialist evaluation.",
    prepare:
      "Full report or relevant pages, property address, representation side, closing date, repair deadline, and questions.",
    route:
      "SAFE can help organize the findings, identify the specialties needed, develop a repair plan, and coordinate requests for participating-contractor quotes.",
  },
];
const homeQuestions = [
  {
    c: "Floors",
    q: ["What type of flooring do I have?", "¿Qué tipo de piso tengo?"],
    a: [
      "Common types include solid hardwood, engineered wood, laminate, luxury vinyl plank, sheet vinyl, tile, and carpet. Send clear photos of the surface, edges, and transitions and we can help narrow it down.",
      "Los tipos comunes incluyen madera sólida, madera de ingeniería, laminado, vinyl plank, vinilo, loseta y alfombra. Envíenos fotos claras de la superficie, bordes y transiciones y le ayudamos a identificarlo.",
    ],
  },
  {
    c: "Floors",
    q: ["How do I maintain my flooring?", "¿Cómo debo mantener mi piso?"],
    a: [
      "Use the cleaning method approved for the exact material, control moisture, protect high-traffic areas, and avoid soaking wood or laminate. The wrong chemical can damage the finish.",
      "Use el método aprobado para el material exacto, controle la humedad, proteja las áreas de tráfico y no empape madera o laminado. El químico incorrecto puede dañar el acabado.",
    ],
  },
  {
    c: "Floors",
    q: [
      "Why does my floor feel soft or uneven?",
      "¿Por qué mi piso se siente suave o desnivelado?",
    ],
    a: [
      "Possible causes include flooring damage, subfloor deterioration, moisture, joists, supports, or settlement. Mark the area and avoid heavy loads if it feels unstable.",
      "Puede deberse al piso, subfloor, humedad, joists, soportes o asentamiento. Marque el área y evite peso si se siente inestable.",
    ],
  },
  {
    c: "Paint & Walls",
    q: [
      "Why is my paint peeling or bubbling?",
      "¿Por qué se despega o hace burbujas la pintura?",
    ],
    a: [
      "Moisture, poor preparation, incompatible coatings, heat, or an active leak may be involved. Find and correct the source before repainting.",
      "Puede existir humedad, mala preparación, pinturas incompatibles, calor o una fuga activa. Corrija la causa antes de pintar.",
    ],
  },
  {
    c: "Paint & Walls",
    q: [
      "What paint finish should I use?",
      "¿Qué acabado de pintura debo usar?",
    ],
    a: [
      "Flat hides imperfections; eggshell and satin are easier to clean; semi-gloss is commonly used for trim and higher-moisture areas.",
      "Flat disimula imperfecciones; eggshell y satin son más fáciles de limpiar; semi-gloss se usa comúnmente en molduras y zonas húmedas.",
    ],
  },
  {
    c: "Paint & Walls",
    q: [
      "I made a hole in the wall. Can it be repaired?",
      "Hice un agujero en la pared. ¿Se puede reparar?",
    ],
    a: [
      "Most drywall holes can be repaired. Size, backing, texture, paint matching, wiring, and moisture determine the scope. Send one close photo and one wider photo.",
      "La mayoría de agujeros en drywall se pueden reparar. Tamaño, soporte, textura, pintura, cables y humedad determinan el trabajo. Envíe una foto cercana y otra amplia.",
    ],
  },
  {
    c: "Decks & Exterior",
    q: [
      "How do I keep my deck looking good?",
      "¿Cómo mantengo bonito el color de mi deck?",
    ],
    a: [
      "Keep debris out of gaps, clean gently, control standing water, inspect fasteners and rot, and renew the appropriate stain or sealer before the wood loses protection.",
      "Quite basura de las uniones, limpie suavemente, evite agua estancada, revise tornillos y madera podrida, y renueve el stain o sealer antes de perder protección.",
    ],
  },
  {
    c: "Decks & Exterior",
    q: [
      "How often should a deck be stained?",
      "¿Cada cuánto se aplica stain a un deck?",
    ],
    a: [
      "Sun, rain, wood type, product, traffic, and preparation matter. If water quickly absorbs instead of beading, the finish may need evaluation.",
      "Influyen sol, lluvia, madera, producto, tráfico y preparación. Si el agua se absorbe en vez de formar gotas, debe evaluarse el acabado.",
    ],
  },
  {
    c: "Crawl Space",
    q: [
      "How do I know if my home has a crawl space?",
      "¿Cómo sé si mi casa tiene crawl space?",
    ],
    a: [
      "A crawl-space home is raised above the ground and usually has an access opening below the first floor. A slab home typically has no accessible space under the floor.",
      "Una casa con crawl space está levantada y normalmente tiene una entrada debajo del primer piso. Una casa sobre slab normalmente no tiene espacio accesible debajo.",
    ],
  },
  {
    c: "Crawl Space",
    q: [
      "What can I check in my crawl space?",
      "¿Qué puedo revisar en mi crawl space?",
    ],
    a: [
      "From a safe access point, look for standing water, fallen insulation, odors, damaged wood, plumbing leaks, and torn ground covering. Do not enter flooded or unstable areas.",
      "Desde una entrada segura, busque agua, aislamiento caído, olores, madera dañada, fugas y plástico roto. No entre si está inundado o inestable.",
    ],
  },
  {
    c: "Electrical & Lighting",
    q: [
      "Can I install a light in a new location?",
      "¿Puedo instalar una luz en un lugar nuevo?",
    ],
    a: [
      "Often yes, but ceiling access, wiring route, box support, switch location, circuit capacity, location rating, and permits can affect the work. An electrician should verify it.",
      "Muchas veces sí, pero acceso, ruta del cable, soporte, switch, capacidad del circuito, ubicación y permisos afectan el trabajo. Un electricista debe verificarlo.",
    ],
  },
  {
    c: "Electrical & Lighting",
    q: [
      "Why does my breaker keep tripping?",
      "¿Por qué se sigue bajando el breaker?",
    ],
    a: [
      "Possible causes include overload, a faulty device, wiring trouble, moisture, or a circuit fault. Do not repeatedly reset it. Note what was running and request evaluation.",
      "Puede ser sobrecarga, aparato defectuoso, cableado, humedad o falla del circuito. No lo restablezca repetidamente. Anote qué estaba funcionando y solicite evaluación.",
    ],
  },
  {
    c: "Plumbing & Fixtures",
    q: [
      "Can I replace my kitchen or bathroom sink?",
      "¿Puedo reemplazar el sink de cocina o baño?",
    ],
    a: [
      "Usually yes. Sink type, countertop opening, faucet, drains, valves, disposal, cabinet condition, and plumbing changes affect the quote. Photos and measurements help.",
      "Normalmente sí. Tipo de sink, abertura, faucet, drenajes, válvulas, disposal, gabinete y cambios de plomería afectan el quote. Fotos y medidas ayudan.",
    ],
  },
  {
    c: "Plumbing & Fixtures",
    q: [
      "How much does it cost to replace a toilet?",
      "¿Cuánto cuesta reemplazar un toilet?",
    ],
    a: [
      "The total depends on the toilet, removal, flange and valve condition, floor damage, access, disposal, and code corrections. Send photos for a useful planning range.",
      "El total depende del toilet, retiro, flange, válvula, daño del piso, acceso, disposición y correcciones. Envíe fotos para un rango útil.",
    ],
  },
  {
    c: "Plumbing & Fixtures",
    q: [
      "How much does it cost to replace a sink?",
      "¿Cuánto cuesta reemplazar un sink?",
    ],
    a: [
      "Pricing changes with sink style, countertop work, faucet and drain parts, valves, and plumbing changes. Photos and dimensions are needed before giving a responsible range.",
      "El precio cambia por estilo, countertop, faucet, drenaje, válvulas y cambios de plomería. Se necesitan fotos y medidas para dar un rango responsable.",
    ],
  },
  {
    c: "Doors & Handyman",
    q: [
      "How much does it cost to replace a door?",
      "¿Cuánto cuesta reemplazar una puerta?",
    ],
    a: [
      "Interior slab, prehung, exterior, storm, and custom doors are different projects. Frame condition, hardware, paint, trim, size, and security affect cost.",
      "Puerta interior, prehung, exterior, storm y especial son proyectos diferentes. Frame, hardware, pintura, trim, tamaño y seguridad afectan el costo.",
    ],
  },
  {
    c: "Doors & Handyman",
    q: [
      "Can a handyman handle my repair list?",
      "¿Puede un handyman encargarse de mi lista?",
    ],
    a: [
      "A handyman may handle many small repairs. Electrical, plumbing, HVAC, roofing, structural, permit, or licensed work should go to the appropriate qualified company.",
      "Un handyman puede hacer reparaciones pequeñas. Electricidad, plomería, HVAC, roofing, estructura, permisos o trabajos con licencia deben ir a la compañía adecuada.",
    ],
  },
  {
    c: "Roof & Water",
    q: [
      "My ceiling is leaking. What should I do first?",
      "Mi techo interior está goteando. ¿Qué hago primero?",
    ],
    a: [
      "Protect the area, stay away from electrical fixtures, contain water if safe, photograph the damage, and note the weather. A sagging ceiling or water near electricity needs urgent attention.",
      "Proteja el área, aléjese de electricidad, contenga el agua si es seguro, tome fotos y anote el clima. Un cielo abultado o agua cerca de electricidad requiere atención urgente.",
    ],
  },
  {
    c: "Cleaning & Care",
    q: [
      "What cleaning should a home receive every month?",
      "¿Qué limpieza necesita una casa cada mes?",
    ],
    a: [
      "Focus on moisture-prone areas, filters, drains, appliances, floors, bathrooms, kitchen grease, visible vents, and early signs of leaks or pests.",
      "Enfóquese en zonas húmedas, filtros, drenajes, aparatos, pisos, baños, grasa, ventilas y señales de fugas o plagas.",
    ],
  },
  {
    c: "Cleaning & Care",
    q: [
      "Which cleaners are safe for my surfaces?",
      "¿Qué limpiadores son seguros para mis superficies?",
    ],
    a: [
      "Use the manufacturer-approved product for the exact flooring, stone, countertop, paint, fixture, or appliance. Never mix chemicals; test unfamiliar products in a hidden area.",
      "Use el producto aprobado para el piso, piedra, countertop, pintura, fixture o aparato. Nunca mezcle químicos; pruebe productos nuevos en un área escondida.",
    ],
  },
  {
    c: "Buying & Selling",
    q: [
      "What should I repair before selling my home?",
      "¿Qué debo reparar antes de vender mi casa?",
    ],
    a: [
      "Prioritize active water, safety, visible damage, major systems, inspection concerns, and presentation. A pre-listing walkthrough separates essential work from optional improvements.",
      "Priorice agua activa, seguridad, daños visibles, sistemas principales, inspección y presentación. Un walkthrough antes de listar separa lo esencial de lo opcional.",
    ],
  },
  {
    c: "Crawl Space",
    q: [
      "Do I need a crawl-space dehumidifier?",
      "¿Necesito un deshumidificador en mi crawl space?",
    ],
    a: [
      "Measure humidity first and identify water entry, drainage, vapor barrier, and air-sealing conditions. A dehumidifier controls humidity but does not correct every moisture source.",
      "Primero mida la humedad e identifique entrada de agua, drenaje, vapor barrier y sellado. Un deshumidificador controla humedad, pero no corrige todas las causas.",
    ],
  },
  {
    c: "Crawl Space",
    q: [
      "What humidity should my crawl space have?",
      "¿Qué porcentaje de humedad debe tener mi crawl space?",
    ],
    a: [
      "Consistently high relative humidity can damage wood and support growth. The correct target and equipment depend on season, construction, monitoring location, and the complete moisture-control plan.",
      "La humedad relativa alta puede dañar madera y favorecer crecimiento. La meta y el equipo dependen de temporada, construcción, ubicación del sensor y el plan completo de humedad.",
    ],
  },
  {
    c: "Electrical & Lighting",
    q: [
      "Can I add an outlet where I need one?",
      "¿Puedo agregar un outlet donde lo necesito?",
    ],
    a: [
      "Often yes. A licensed electrician should confirm circuit capacity, wiring route, grounding, required GFCI/AFCI protection, wall access, location, and permits.",
      "Muchas veces sí. Un electricista debe confirmar capacidad, ruta del cable, ground, protección GFCI/AFCI, acceso, ubicación y permisos.",
    ],
  },
  {
    c: "Plumbing & Fixtures",
    q: [
      "Can I add a new outdoor hose connection?",
      "¿Puedo agregar una nueva salida para hose afuera?",
    ],
    a: [
      "Usually possible after confirming the nearest water line, shutoff, wall construction, access, drainage, freeze protection, and whether interior finishes must be opened.",
      "Normalmente es posible después de confirmar la tubería cercana, shutoff, tipo de pared, acceso, drenaje, protección contra frío y si se deben abrir acabados interiores.",
    ],
  },
  {
    c: "Decks & Exterior",
    q: [
      "A tree root is getting close to my house. What should I do?",
      "Una raíz se está acercando a mi casa. ¿Qué debo hacer?",
    ],
    a: [
      "Do not cut major roots without professional advice. Document distance and visible movement, then determine whether the concern involves an arborist, foundation, sewer, drainage, driveway, or several specialists.",
      "No corte raíces grandes sin orientación profesional. Documente distancia y movimiento visible, y determine si necesita arborist, foundation, sewer, drenaje, driveway o varios especialistas.",
    ],
  },
];
const grid = document.getElementById("servicesGrid"),
  search = document.getElementById("problemSearch"),
  directorySearch = document.getElementById("directorySearch"),
  noResults = document.getElementById("noResults"),
  select = document.getElementById("serviceSelect");
const instantSearch = document.getElementById("instantSearch");
let category = "all";
function render() {
  const q = (directorySearch.value || search.value).trim().toLowerCase();
  const filtered = services.filter(
    (s) =>
      (category === "all" || s.category === category) &&
      (!q ||
        `${s.title} ${s.tags} ${s.desc} ${s.price}`.toLowerCase().includes(q)),
  );
  grid.innerHTML = filtered
    .map(
      (s) =>
        `<article class="service-card"><span class="service-number">${String(services.indexOf(s) + 1).padStart(2, "0")}</span><span class="service-icon" aria-hidden="true">${s.icon}</span><h3>${s.title}</h3><p>${s.desc}</p><small class="price-label">${language === "es" ? "Rango para planificar" : "Planning range"}</small><small class="price-range">${s.price}</small><button class="request-service" data-service="${s.title}">${language === "es" ? "Confirmar mi precio con SAFE" : "Confirm my price with SAFE"} →</button></article>`,
    )
    .join("");
  noResults.hidden = filtered.length > 0;
  document.getElementById("directoryCount").textContent =
    language === "es"
      ? `${filtered.length} servicios encontrados`
      : `${filtered.length} services found`;
  wireRequests();
}
function renderSuggestions() {
  const box = document.getElementById("searchSuggestions"),
    q = directorySearch.value.trim().toLowerCase();
  if (!q) {
    box.hidden = true;
    box.innerHTML = "";
    return;
  }
  const matches = services
    .filter((s) => `${s.title} ${s.tags} ${s.desc}`.toLowerCase().includes(q))
    .slice(0, 6);
  box.innerHTML = matches.length
    ? matches
        .map(
          (s) =>
            `<button type="button" data-suggestion="${s.title}"><span>${s.icon}</span><div><strong>${s.title}</strong><small>${s.price}</small></div><b>→</b></button>`,
        )
        .join("")
    : `<p>${language === "es" ? "No hay coincidencia exacta. Escriba el problema con sus propias palabras." : "No exact match. Describe the problem in your own words."}</p>`;
  box.hidden = false;
  box.querySelectorAll("[data-suggestion]").forEach(
    (b) =>
      (b.onclick = () => {
        directorySearch.value = search.value = b.dataset.suggestion;
        box.hidden = true;
        category = "all";
        document
          .querySelectorAll(".tab")
          .forEach((t) =>
            t.classList.toggle("active", t.dataset.category === "all"),
          );
        render();
      }),
  );
}
function renderInstantSuggestions() {
  const box = document.getElementById("instantSuggestions"),
    q = instantSearch.value.trim().toLowerCase();
  if (!q) {
    box.hidden = true;
    box.innerHTML = "";
    return;
  }
  const matches = services
    .filter((s) => `${s.title} ${s.tags} ${s.desc}`.toLowerCase().includes(q))
    .slice(0, 5);
  box.innerHTML = matches.length
    ? matches
        .map(
          (s) =>
            `<button type="button" data-instant-suggestion="${s.title}"><span>${s.icon}</span><div><strong>${s.title}</strong><small>${s.price}</small></div><b>→</b></button>`,
        )
        .join("")
    : `<p>${language === "es" ? "No hay coincidencia exacta. Presione Buscar y describa lo que está viendo." : "No exact match. Press Search and describe what you are seeing."}</p>`;
  box.hidden = false;
  box.querySelectorAll("[data-instant-suggestion]").forEach(
    (b) =>
      (b.onclick = () => {
        instantSearch.value =
          directorySearch.value =
          search.value =
            b.dataset.instantSuggestion;
        box.hidden = true;
        category = "all";
        document
          .querySelectorAll(".tab")
          .forEach((t) =>
            t.classList.toggle("active", t.dataset.category === "all"),
          );
        render();
        document
          .getElementById("find-help")
          .scrollIntoView({ behavior: "smooth" });
      }),
  );
}
function startQuickWithService(service) {
  selectedProblem = service;
  selectedRole = selectedRole || "Homeowner";
  showFlowStep(3);
  document
    .getElementById("quickFlow")
    .scrollIntoView({ behavior: "smooth", block: "center" });
}
function wireRequests() {
  document
    .querySelectorAll(".request-service")
    .forEach((btn) =>
      btn.addEventListener("click", () =>
        startQuickWithService(btn.dataset.service),
      ),
    );
}
services.forEach((s) => {
  const o = document.createElement("option");
  o.value = o.textContent = s.title;
  select.appendChild(o);
});
[
  "Contractor Inspection Report Analysis",
  "Pre-Closing Contractor Walkthrough",
  "Pre-Listing Value & Repair Walkthrough",
  "Post-Closing Home Walkthrough",
  "Repair Planning & Contractor Matching",
].forEach((x) => {
  if (![...select.options].some((o) => o.value === x)) {
    const o = document.createElement("option");
    o.value = o.textContent = x;
    select.appendChild(o);
  }
});
document.querySelectorAll(".tab").forEach((btn) =>
  btn.addEventListener("click", () => {
    document.querySelectorAll(".tab").forEach((b) => {
      b.classList.remove("active");
      b.setAttribute("aria-selected", "false");
    });
    btn.classList.add("active");
    btn.setAttribute("aria-selected", "true");
    category = btn.dataset.category;
    render();
  }),
);
document.querySelectorAll(".quick-links button").forEach((btn) =>
  btn.addEventListener("click", () => {
    search.value = directorySearch.value = btn.dataset.query;
    category = "all";
    document
      .querySelectorAll(".tab")
      .forEach((b) =>
        b.classList.toggle("active", b.dataset.category === "all"),
      );
    render();
    document.getElementById("find-help").scrollIntoView({ behavior: "smooth" });
  }),
);
search.addEventListener("input", () => {
  directorySearch.value = search.value;
  render();
  renderSuggestions();
});
directorySearch.addEventListener("input", () => {
  search.value = directorySearch.value;
  render();
  renderSuggestions();
});
document.getElementById("searchButton").addEventListener("click", () => {
  directorySearch.value = search.value;
  render();
  renderSuggestions();
  document.getElementById("find-help").scrollIntoView({ behavior: "smooth" });
});
document
  .getElementById("directorySearchButton")
  .addEventListener("click", () => {
    render();
    document.getElementById("searchSuggestions").hidden = true;
  });
document.getElementById("floatingSearch").addEventListener("click", () => {
  document.getElementById("find-help").scrollIntoView({ behavior: "smooth" });
  setTimeout(() => directorySearch.focus(), 500);
});
directorySearch.addEventListener("focus", renderSuggestions);
instantSearch.addEventListener("input", () => {
  directorySearch.value = search.value = instantSearch.value;
  renderInstantSuggestions();
});
document.getElementById("instantSearchButton").addEventListener("click", () => {
  directorySearch.value = search.value = instantSearch.value;
  category = "all";
  render();
  document.getElementById("instantSuggestions").hidden = true;
  document.getElementById("find-help").scrollIntoView({ behavior: "smooth" });
});
instantSearch.addEventListener("focus", renderInstantSuggestions);
const safeTextNumber = "+16506696890";
function openSafeText(message) {
  window.location.href = `sms:${safeTextNumber}?&body=${encodeURIComponent(message)}`;
}
document.getElementById("helpForm").addEventListener("submit", (e) => {
  e.preventDefault();
  const d = new FormData(e.target),
    message = `SAFE Contractor request\nName: ${d.get("name")}\nPhone: ${d.get("phone")}\nEmail: ${d.get("email") || "Not provided"}\nProperty/ZIP: ${d.get("location") || "Not provided"}\nService: ${d.get("service")}\nDetails: ${d.get("details") || "Not provided"}`;
  document.getElementById("successMessage").hidden = false;
  e.target.querySelector(".submit-button").textContent =
    language === "es" ? "Abrir mensajes ✓" : "Open messages ✓";
  openSafeText(message);
});
const roleProblems = {
  Homeowner: [
    "Roof leak or roof damage",
    "Crawl space or moisture",
    "Foundation or uneven floor",
    "Plumbing or water leak",
    "Electrical or lighting",
    "Heating or air conditioning",
    "Painting, drywall or flooring",
    "Handyman or small repairs",
  ],
  "Buyer or seller": [
    "Inspection report analysis",
    "Walkthrough before closing",
    "Walkthrough after closing",
    "Prepare home before listing",
    "Roofing concern",
    "Repair estimate before closing",
    "Verify completed repairs",
    "Multiple inspection repairs",
  ],
  Realtor: [
    "Pre-offer repair assessment",
    "Inspection report analysis",
    "Repair estimate for a client",
    "Walkthrough before closing",
    "Compare home insurance",
    "Prepare a listing",
    "Roofing concern",
    "Coordinate multiple repairs",
    "Verify completed repairs",
    "Urgent repair before closing",
    "Repairs paid at closing",
    "Post-closing home services",
  ],
  "Property manager": [
    "Turnover or make-ready",
    "Occupied-unit repair",
    "Emergency maintenance",
    "Cleaning or trash-out",
    "Plumbing or electrical",
    "HVAC or water heater",
    "Roof or exterior",
    "Multiple units or construction project",
  ],
  "Lender or REO": [
    "Lender-required closing repair",
    "Initial secure or lock change",
    "Occupancy or condition check",
    "Trash-out or debris removal",
    "Winterization or recurring preservation",
    "Emergency mitigation",
    "Repair bid or rehabilitation",
    "Market-ready turn and closeout",
  ],
};
const roleProblemsEs = {
  Homeowner: [
    "Fuga o daño en el roof",
    "Crawl space o humedad",
    "Foundation o piso desnivelado",
    "Plomería o fuga de agua",
    "Electricidad o iluminación",
    "Calefacción o aire acondicionado",
    "Pintura, drywall o piso",
    "Handyman o reparaciones pequeñas",
  ],
  "Buyer or seller": [
    "Análisis del reporte de inspección",
    "Walkthrough antes del cierre",
    "Walkthrough después del cierre",
    "Preparar la casa antes de vender",
    "Problema de roofing",
    "Estimado antes del cierre",
    "Verificar reparaciones terminadas",
    "Varias reparaciones de inspección",
  ],
  Realtor: [
    "Evaluación de reparaciones antes de ofertar",
    "Análisis del reporte de inspección",
    "Estimado para un cliente",
    "Walkthrough antes del cierre",
    "Comparar seguro de casa",
    "Preparar una propiedad para venta",
    "Problema de roofing",
    "Coordinar varias reparaciones",
    "Verificar reparaciones terminadas",
    "Reparación urgente antes del cierre",
    "Reparaciones pagadas al cierre",
    "Servicios después del cierre",
  ],
  "Property manager": [
    "Turnover o make-ready",
    "Reparación en unidad ocupada",
    "Mantenimiento de emergencia",
    "Limpieza o trash-out",
    "Plomería o electricidad",
    "HVAC o water heater",
    "Roof o exterior",
    "Varias unidades o proyecto de construcción",
  ],
  "Lender or REO": [
    "Reparación requerida para cierre",
    "Initial secure o cambio de lock",
    "Verificación de ocupación o condición",
    "Trash-out o retiro de escombro",
    "Winterization o preservación recurrente",
    "Mitigación de emergencia",
    "Bid de reparación o rehabilitación",
    "Preparar para mercado y cerrar orden",
  ],
};
let selectedRole = "",
  selectedProblem = "";
function showFlowStep(n) {
  ["flowStep1", "flowStep2", "flowStep3", "flowSuccess"].forEach(
    (id, i) => (document.getElementById(id).hidden = i !== n - 1),
  );
  document
    .querySelectorAll(".flow-progress span")
    .forEach((s, i) => s.classList.toggle("active", i < n));
  if (n === 2) {
    const choices = language === "es" ? roleProblemsEs : roleProblems;
    document.getElementById("problemChoices").innerHTML = choices[selectedRole]
      .map(
        (p) =>
          `<button type="button" data-problem="${p}">${p}<span>→</span></button>`,
      )
      .join("");
    document.querySelectorAll("[data-problem]").forEach(
      (b) =>
        (b.onclick = () => {
          selectedProblem = b.dataset.problem;
          showFlowStep(3);
        }),
    );
  }
  if (n === 3)
    document.getElementById("flowSummary").textContent =
      `${selectedRole} • ${selectedProblem}`;
}
document.querySelectorAll(".role-choice").forEach((b) =>
  b.addEventListener("click", () => {
    selectedRole = b.dataset.role;
    showFlowStep(2);
  }),
);
document
  .querySelectorAll(".flow-back")
  .forEach((b) =>
    b.addEventListener("click", () => showFlowStep(Number(b.dataset.back))),
  );
document.getElementById("quickForm").addEventListener("submit", (e) => {
  e.preventDefault();
  const d = new FormData(e.target),
    message = `SAFE Contractor quick request\nRole: ${selectedRole}\nNeed: ${selectedProblem}\nName: ${d.get("name")}\nPhone: ${d.get("phone")}\nProperty ZIP: ${d.get("zip")}\nNote: ${d.get("detail") || "Not provided"}`;
  showFlowStep(4);
  openSafeText(message);
});
document.getElementById("startAnother").addEventListener("click", () => {
  selectedRole = "";
  selectedProblem = "";
  document.getElementById("quickForm").reset();
  showFlowStep(1);
});
const guideList = document.getElementById("guideList"),
  guideDetail = document.getElementById("guideDetail"),
  guideSearch = document.getElementById("guideSearch");
function showGuide(g) {
  document
    .querySelectorAll(".guide-button")
    .forEach((b) => b.classList.toggle("active", b.dataset.title === g.title));
  guideDetail.innerHTML = `<div class="guide-title"><span>${g.icon}</span><div><p class="eyebrow">Problem guide</p><h3>${g.title}</h3></div></div><p class="guide-intro">${g.intro}</p><div class="guide-columns"><section><h4>What you can check safely</h4><ul>${g.check.map((x) => `<li>${x}</li>`).join("")}</ul></section><section class="warning-box"><h4>Stop and get professional help</h4><p>${g.stop}</p></section></div><section class="prepare-box"><h4>What to have ready</h4><p>${g.prepare}</p></section><section class="route-box"><h4>How SAFE can guide you</h4><p>${g.route}</p><div><button class="guide-action" data-action="Ask SAFE about this problem">Ask SAFE →</button><button class="guide-action secondary" data-action="Cost planning and contractor quotes">Get cost guidance & quotes</button></div></section><p class="guide-note">Educational guidance only. It is not a diagnosis, inspection, engineering opinion, or final quote. Cost guidance depends on property conditions, location, access, materials, permits, and the verified scope.</p>`;
  document.querySelectorAll(".guide-action").forEach(
    (b) =>
      (b.onclick = () => {
        selectedRole = "Homeowner";
        selectedProblem = `${g.title} — ${b.dataset.action}`;
        showFlowStep(3);
        document
          .getElementById("quickFlow")
          .scrollIntoView({ behavior: "smooth", block: "center" });
      }),
  );
}
function renderGuides() {
  const q = guideSearch.value.trim().toLowerCase();
  const found = guides.filter(
    (g) => !q || `${g.title} ${g.match} ${g.intro}`.toLowerCase().includes(q),
  );
  guideList.innerHTML =
    found
      .map(
        (g) =>
          `<button class="guide-button" type="button" data-title="${g.title}"><span>${g.icon}</span>${g.title}<b>→</b></button>`,
      )
      .join("") ||
    '<p class="guide-empty">No exact match. Ask SAFE and describe what you are seeing.</p>';
  document
    .querySelectorAll(".guide-button")
    .forEach(
      (b) =>
        (b.onclick = () =>
          showGuide(guides.find((g) => g.title === b.dataset.title))),
    );
  if (found.length && (!guideDetail.dataset.loaded || q)) {
    showGuide(found[0]);
    guideDetail.dataset.loaded = "true";
  }
}
guideSearch.addEventListener("input", renderGuides);
renderGuides();
let language = localStorage.getItem("safeLanguage") || "en",
  questionCategory = "All";
const promotions = {
  en: [
    {
      tag: "Crawl space",
      title: "Crawl Space Makeover",
      price: "$3,500",
      sub: "Cleanup + ground vapor barrier • not encapsulation",
      image: "assets/crawl-space-before-after.png",
      items: [
        "Remove accessible loose debris and old plastic",
        "Install new protective plastic over exposed soil",
        "Orderly overlaps and placement around piers",
        "Condition review and completion photos",
        "One localized minor joist repair; a second adjacent joist may qualify in the same accessible repair area",
      ],
      terms:
        "This package is not full encapsulation. Main beams or girders, extensive rot, subfloor, piers or foundation work, mold or pest remediation, standing water or drainage correction, insulation, wall sealing, dehumidifier, plumbing, HVAC, electrical, ducts, permits, and access changes are separate.",
      cta: "Check my crawl space",
    },
    {
      tag: "Flooring",
      title: "Featured 20-mil LVP",
      price: "$5.50 / sq. ft.",
      sub: "Installed promotional price",
      image: "assets/flooring-installed.jpeg",
      items: [
        "Real installed-project example",
        "Mohawk / UNICLIC featured product",
        "20-mil wear layer",
        "5 mm total thickness",
        "Color 822 • 7.46 in. × 47.8 in. planks",
      ],
      terms:
        "For qualifying open, ready-to-install areas. Removal, subfloor repair or leveling, moisture correction, stairs, trim, transitions, furniture moving, special layouts, disposal, tax, and permits may be additional.",
      cta: "Measure my flooring",
    },
    {
      tag: "Water heaters",
      title: "Installed water-heater specials",
      price: "From $1,199 + tax",
      sub: "Standard replacement • normal garage access",
      image: "assets/water-heater-electric.jpeg",
      items: [
        "40-gal electric: $1,199 + tax",
        "50-gal electric: $1,299 + tax",
        "40-gal natural gas: $1,449 + tax",
        "50-gal natural gas: $1,499 + tax",
        "Second floor, attic, or crawl space: +$100",
        "Expansion tank when needed: +$100",
        "Corroded fittings + expansion-tank package: +$350",
        "Removal, startup and testing",
      ],
      terms:
        "Most standard replacements can reuse serviceable existing connections. The promotional price applies to a qualifying standard replacement with normal garage access. Second-floor, attic, or crawl-space access adds $100. An expansion tank adds $100 when required. If corroded fittings and the expansion tank need replacement together, the complete package adds $350. Code corrections, electrical or gas changes, vent corrections, relocation, permits, and any other non-standard work are explained and require approval before they are added.",
      cta: "Check availability",
    },
    {
      tag: "Heating & air",
      title: "Ducane complete change-out",
      price: "From $6,399",
      sub: "2–2.5 ton • standard like-for-like",
      image: "assets/hvac-electric.jpeg",
      items: [
        "Electric system: $6,399",
        "Gas system: $6,799",
        "Equipment and standard installation",
        "10-year parts warranty*",
        "1-year labor warranty*",
      ],
      terms:
        "Offer ends November 14, 2026. Existing ducts, lines, electrical, gas, venting, and drainage must be reusable and code-compliant. Repairs and modifications are separate. Manufacturer registration required.",
      cta: "Schedule free inspection",
    },
  ],
  es: [
    {
      tag: "Crawl space",
      title: "Crawl Space Makeover",
      price: "$3,500",
      sub: "Limpieza + vapor barrier sobre el suelo • no encapsulation",
      image: "assets/crawl-space-before-after.png",
      items: [
        "Retiro de basura suelta accesible y plástico viejo",
        "Plástico protector nuevo sobre la tierra expuesta",
        "Traslapes ordenados y colocación alrededor de pilares",
        "Revisión de condición y fotos finales",
        "Una reparación menor de joist; un segundo joist adyacente puede calificar en la misma área accesible",
      ],
      terms:
        "Este paquete no es encapsulation completa. Vigas principales, daño extenso, subfloor, pilares o foundation, moho o plagas, agua o drenaje, insulation, sellado de paredes, deshumidificador, plomería, HVAC, electricidad, ductos, permisos y cambios de acceso se cotizan aparte.",
      cta: "Revisar mi crawl space",
    },
    {
      tag: "Pisos",
      title: "LVP destacado de 20 mil",
      price: "$5.50 / pie²",
      sub: "Precio promocional instalado",
      image: "assets/flooring-installed.jpeg",
      items: [
        "Ejemplo real de piso instalado",
        "Producto destacado Mohawk / UNICLIC",
        "Capa de uso de 20 mil",
        "Grosor total de 5 mm",
        "Color 822 • tablas de 7.46 × 47.8 pulgadas",
      ],
      terms:
        "Para áreas abiertas, listas para instalar y que califiquen. Retiro, reparación o nivelación del subfloor, humedad, escaleras, trim, transiciones, muebles, diseños especiales, basura, impuestos y permisos pueden costar extra.",
      cta: "Medir mi piso",
    },
    {
      tag: "Water heaters",
      title: "Especiales instalados",
      price: "Desde $1,199 + tax",
      sub: "Reemplazo estándar • acceso normal en garaje",
      image: "assets/water-heater-electric.jpeg",
      items: [
        "Eléctrico 40 gal: $1,199 + tax",
        "Eléctrico 50 gal: $1,299 + tax",
        "Gas natural 40 gal: $1,449 + tax",
        "Gas natural 50 gal: $1,499 + tax",
        "Segundo piso, ático o crawl space: +$100",
        "Expansion tank cuando se necesita: +$100",
        "Fittings corroídos + expansion tank: +$350",
        "Retiro, encendido y prueba",
      ],
      terms:
        "En la mayoría de reemplazos estándar se pueden reutilizar las conexiones existentes que todavía están en buenas condiciones. El precio promocional aplica con acceso normal en garaje. Segundo piso, ático o crawl space agrega $100. El expansion tank agrega $100 cuando se requiere. Si deben cambiarse juntos los fittings corroídos y el expansion tank, el paquete completo agrega $350. Correcciones de código, cambios eléctricos o de gas, ventilación, reubicación, permisos y cualquier trabajo no estándar se explican y requieren aprobación antes de agregarse.",
      cta: "Ver disponibilidad",
    },
    {
      tag: "Calefacción y aire",
      title: "Cambio completo Ducane",
      price: "Desde $6,399",
      sub: "2–2.5 toneladas • reemplazo similar",
      image: "assets/hvac-electric.jpeg",
      items: [
        "Sistema eléctrico: $6,399",
        "Sistema de gas: $6,799",
        "Equipo e instalación estándar",
        "10 años de garantía en partes*",
        "1 año de garantía laboral*",
      ],
      terms:
        "Oferta termina el 14 de noviembre de 2026. Ductos, líneas, electricidad, gas, ventilación y drenaje deben poder reutilizarse y cumplir código. Cambios se cotizan aparte. Requiere registro del fabricante.",
      cta: "Programar inspección gratis",
    },
  ],
};
function renderPromotions() {
  document.getElementById("promotionGrid").innerHTML = promotions[language]
    .map(
      (p, i) =>
        `<article class="promotion-card ${i === 0 ? "featured" : ""}"><div class="promotion-image"><img src="${p.image}" alt="${p.title}" loading="lazy"><span>${p.tag}</span></div><div class="promotion-body"><h3>${p.title}</h3><strong class="promotion-price">${p.price}</strong><p class="promotion-sub">${p.sub}</p><ul>${p.items.map((x) => `<li>${x}</li>`).join("")}</ul><details><summary>${language === "es" ? "Condiciones importantes" : "Important conditions"}</summary><p>${p.terms}</p></details><button class="promotion-action" data-service="${p.title} — ${p.price}">${p.cta} →</button></div></article>`,
    )
    .join("");
  document
    .querySelectorAll(".promotion-action")
    .forEach(
      (b) => (b.onclick = () => startQuickWithService(b.dataset.service)),
    );
}
const ui = {
  en: {
    findHelp: "Find help",
    homeGuides: "Home guides",
    questions: "Questions",
    buyingHome: "Buying a home",
    requestHelp: "Request help",
    heroEyebrow: "Home repair help, made simple",
    heroTitle: "Find the right home repair help in Charlotte.",
    heroLead:
      "You don’t need to know the technical name. Choose who you are, select the problem, and send a short request.",
    searchLabel: "Search by problem, room, or service",
    libraryEyebrow: "Your home, explained",
    libraryTitle: "Questions homeowners ask every day.",
    libraryLead:
      "Explore repairs, upgrades, cleaning, finishes, maintenance, and what information to collect before requesting a quote.",
    costDisclaimer:
      "Planning ranges are educational—not final quotes. Actual pricing depends on materials, property conditions, access, location, permits, and the verified scope.",
    step1: "Step 1 of 3",
    step2: "Step 2 of 3",
    step3: "Step 3 of 3",
    whoNeeds: "Who needs help?",
    whatHelp: "What do you need help with?",
    whereContact: "Where should we contact you?",
    homeowner: "Homeowner",
    homeownerSub: "I need help with my home",
    buyerSeller: "Buyer or seller",
    buyerSellerSub: "Before or after a sale",
    realtorSub: "I’m helping a client",
    notSure: "I’m not sure or have several problems",
    ask: "Ask SAFE",
    quote: "Cost guidance & quote",
  },
  es: {
    findHelp: "Buscar ayuda",
    homeGuides: "Guías de la casa",
    questions: "Preguntas",
    buyingHome: "Comprar una casa",
    requestHelp: "Pedir ayuda",
    heroEyebrow: "Ayuda para su casa, sin complicaciones",
    heroTitle: "Encuentre la ayuda correcta para su casa en Charlotte.",
    heroLead:
      "No necesita conocer el nombre técnico. Díganos quién es, seleccione el problema y envíe una solicitud corta.",
    searchLabel: "Busque por problema, cuarto o servicio",
    libraryEyebrow: "Entienda su casa",
    libraryTitle: "Preguntas que hacen los propietarios todos los días.",
    libraryLead:
      "Conozca reparaciones, mejoras, limpieza, acabados, mantenimiento y qué información preparar antes de pedir un quote.",
    costDisclaimer:
      "Los rangos son para planificación, no son cotizaciones finales. El precio depende de materiales, condición, acceso, ubicación, permisos y alcance verificado.",
    step1: "Paso 1 de 3",
    step2: "Paso 2 de 3",
    step3: "Paso 3 de 3",
    whoNeeds: "¿Quién necesita ayuda?",
    whatHelp: "¿Con qué necesita ayuda?",
    whereContact: "¿Dónde podemos contactarlo?",
    homeowner: "Dueño de casa",
    homeownerSub: "Necesito ayuda con mi casa",
    buyerSeller: "Comprador o vendedor",
    buyerSellerSub: "Antes o después de una venta",
    realtorSub: "Estoy ayudando a un cliente",
    notSure: "No estoy seguro o tengo varios problemas",
    ask: "Preguntar a SAFE",
    quote: "Orientación de costo y quote",
  },
};
const categoryEs = {
  All: "Todos",
  Floors: "Pisos",
  "Paint & Walls": "Pintura y paredes",
  "Decks & Exterior": "Decks y exterior",
  "Crawl Space": "Crawl space",
  "Electrical & Lighting": "Electricidad y luces",
  "Plumbing & Fixtures": "Plomería y fixtures",
  "Doors & Handyman": "Puertas y handyman",
  "Roof & Water": "Roofing y agua",
  "Cleaning & Care": "Limpieza y cuidado",
  "Buying & Selling": "Compra y venta",
};
Object.assign(ui.en, {
  managerRole: "Property manager",
  managerRoleSub: "Units, turnovers or projects",
});
Object.assign(ui.es, {
  managerRole: "Property manager",
  managerRoleSub: "Unidades, turnovers o proyectos",
});
Object.assign(ui.en, {
  freeInspection: "FREE initial contractor property review",
  freeInspectionSub:
    "A practical first look to identify the right next step—not a licensed home inspection. Specialized testing, engineering, permits, and diagnostics may be separate.",
});
Object.assign(ui.es, {
  freeInspection: "REVISIÓN INICIAL GRATIS con contratista",
  freeInspectionSub:
    "Una primera revisión práctica para identificar el siguiente paso; no reemplaza una inspección residencial autorizada. Pruebas, ingeniería, permisos y diagnósticos pueden ser separados.",
});
Object.assign(ui.en, {
  promotions: "Promotions",
  promoEyebrow: "Limited-time savings",
  promoTitle: "Current home-service promotions",
  promoLead:
    "Straightforward starting prices, a free initial property inspection, and written scope confirmation before work begins.",
  promoDisclaimer:
    "Promotional prices apply only to qualifying standard conditions and are subject to availability. Tax, permits, code corrections, difficult access, hidden damage, specialty testing, engineering, and work outside the written scope may cost extra. Final eligibility is confirmed after the free initial inspection.",
});
Object.assign(ui.es, {
  promotions: "Promociones",
  promoEyebrow: "Ahorros por tiempo limitado",
  promoTitle: "Promociones actuales para su casa",
  promoLead:
    "Precios iniciales claros, inspección inicial gratis y alcance confirmado por escrito antes de comenzar.",
  promoDisclaimer:
    "Los precios promocionales aplican solamente a condiciones estándar que califiquen y están sujetos a disponibilidad. Impuestos, permisos, correcciones de código, acceso difícil, daño oculto, pruebas especializadas, ingeniería y trabajo fuera del alcance escrito pueden costar extra. La elegibilidad final se confirma después de la inspección inicial gratis.",
});
Object.assign(ui.en, {
  priceDirectoryTitle: "130+ Home Repair Price Directory",
  priceDirectoryText:
    "Search specific Charlotte-area ranges—from a toilet or outlet to a rafter, crawl-space pipe, roof, fence, or complete remodel. For a confirmed price, send photos or schedule a free initial property review.",
  pricePromise:
    "After the quantity, condition, access, and written scope are verified, SAFE will confirm a price range and maximum. If the approved scope does not change, we will respect that written maximum. No additional work is performed without your approval.",
});
Object.assign(ui.es, {
  priceDirectoryTitle: "Directorio de 130+ precios de reparación",
  priceDirectoryText:
    "Busque rangos específicos del área de Charlotte: desde un toilet u outlet hasta un rafter, tubería bajo la casa, roof, fence o remodelación completa. Para confirmar el precio, envíe fotos o programe una revisión inicial gratis.",
  pricePromise:
    "Después de verificar la cantidad, condición, acceso y alcance escrito, SAFE confirmará un rango y precio máximo. Si el alcance aprobado no cambia, respetaremos ese máximo por escrito. No se realizará trabajo adicional sin su aprobación.",
});
Object.assign(ui.en, {
  crawlExplainEyebrow: "What the $3,500 makeover means",
  crawlExplainTitle: "A cleaner crawl space with ground-vapor protection.",
  crawlExplainLead:
    "This is a cleanup and ground vapor-barrier service—not full encapsulation. The plastic is installed over the soil to reduce ground moisture vapor entering the crawl space.",
  crawlNotEncapTitle: "Important: this is not encapsulation.",
  crawlNotEncapText:
    "Wall sealing, insulation systems, dehumidification, drainage, and mechanical conditioning are not included.",
  crawlStep1Title: "Clean the ground",
  crawlStep1Text: "Remove loose debris, damaged plastic, and accessible waste.",
  crawlStep2Title: "Review visible conditions",
  crawlStep2Text:
    "Look for accessible signs of leaks, wood damage, standing water, ducts, wiring, plumbing, and support concerns.",
  crawlStep3Title: "Install the vapor barrier",
  crawlStep3Text:
    "Lay protective plastic over exposed soil with orderly overlaps and placement around piers.",
  crawlStep4Title: "Complete the minor wood allowance",
  crawlStep4Text:
    "Complete the qualifying localized joist repair included in the written package.",
  crawlStep5Title: "Final review and photos",
  crawlStep5Text:
    "Document the finished condition and explain separate recommendations.",
  crawlWhyTitle: "Why the ground plastic matters",
  crawlBenefit1: "Reduces moisture vapor rising from exposed soil",
  crawlBenefit2: "Helps keep the area cleaner for future access",
  crawlBenefit3: "Supports a better moisture-control plan",
  crawlBenefit4: "Makes leaks and new problems easier to notice",
  crawlWhyNote:
    "A vapor barrier does not repair drainage, plumbing leaks, mold, termites, structural damage, or every humidity source. Those conditions are identified and priced separately.",
});
Object.assign(ui.es, {
  crawlExplainEyebrow: "Qué significa el makeover de $3,500",
  crawlExplainTitle:
    "Un crawl space más limpio con protección contra vapor del suelo.",
  crawlExplainLead:
    "Este servicio incluye limpieza y vapor barrier sobre la tierra; no es encapsulation completa. El plástico ayuda a reducir el vapor de humedad que sube del suelo.",
  crawlNotEncapTitle: "Importante: esto no es encapsulation.",
  crawlNotEncapText:
    "El sellado de paredes, insulation, deshumidificador, drenaje y acondicionamiento mecánico no están incluidos.",
  crawlStep1Title: "Limpiar el suelo",
  crawlStep1Text:
    "Retiramos basura suelta, plástico dañado y desperdicios accesibles.",
  crawlStep2Title: "Revisar condiciones visibles",
  crawlStep2Text:
    "Buscamos señales accesibles de fugas, madera dañada, agua, ductos, electricidad, plomería y soportes.",
  crawlStep3Title: "Instalar el vapor barrier",
  crawlStep3Text:
    "Colocamos plástico protector sobre la tierra con traslapes ordenados y alrededor de pilares.",
  crawlStep4Title: "Completar la reparación menor",
  crawlStep4Text:
    "Realizamos la reparación localizada de joist que califique dentro del paquete escrito.",
  crawlStep5Title: "Revisión final y fotografías",
  crawlStep5Text:
    "Documentamos el resultado y explicamos recomendaciones separadas.",
  crawlWhyTitle: "Por qué importa el plástico sobre la tierra",
  crawlBenefit1: "Reduce el vapor de humedad que sube de la tierra",
  crawlBenefit2: "Ayuda a mantener el área más limpia para acceso futuro",
  crawlBenefit3: "Apoya un mejor plan de control de humedad",
  crawlBenefit4: "Permite notar más fácilmente fugas o problemas nuevos",
  crawlWhyNote:
    "El vapor barrier no corrige drenaje, fugas de plomería, moho, termitas, daño estructural ni todas las fuentes de humedad. Esas condiciones se identifican y cotizan por separado.",
});
Object.assign(ui.en, {
  directorySearchLabel: "Search the service and price directory",
  directorySearchPlaceholder:
    "Search: wood fence, outlet, roof leak, water heater…",
  directorySearchButton: "Search prices",
});
Object.assign(ui.es, {
  directorySearchLabel: "Buscar en el directorio de servicios y precios",
  directorySearchPlaceholder:
    "Busque: fence de madera, outlet, fuga del roof, water heater…",
  directorySearchButton: "Buscar precios",
});
Object.assign(ui.en, { floatingSearch: "Search" });
Object.assign(ui.es, { floatingSearch: "Buscar" });
Object.assign(ui.en, {
  instantSearchTitle: "What does your home need?",
  instantSearchText: "Type the problem, service, or part of the house.",
  instantSearchPlaceholder: "Try: fence, no hot water, roof leak…",
});
Object.assign(ui.es, {
  instantSearchTitle: "¿Qué necesita su casa?",
  instantSearchText: "Escriba el problema, servicio o parte de la casa.",
  instantSearchPlaceholder:
    "Pruebe: fence, no hay agua caliente, fuga del roof…",
});
Object.assign(ui.en, {
  menuButton: "Menu",
  menuTitle: "Find it quickly",
  menuSubtitle: "Search or choose an option",
  menuSearchLabel: "What do you need?",
  menuSearchPlaceholder: "Roof leak, toilet, rafter…",
  menuSearchButton: "Search prices",
  menuPrices: "Price directory",
  menuPricesSub: "Search 130+ home services",
  menuPromosSub: "Current SAFE offers",
  menuGuidesSub: "Understand the problem first",
  menuQuestionsSub: "Answers for homeowners",
  menuBuyerSeller: "Buyer & seller guide",
  menuBuyerSellerSub: "Inspection through closing",
  menuManagers: "Property managers",
  menuManagersSub: "Work orders and price levels",
  menuRealtors: "Realtor support",
  menuRealtorsSub: "Reports, quotes and coordination",
  menuRepairLibrary: "Repair Scope Library",
  menuRepairLibrarySub: "Prices, paint planner and smart combos",
  menuInspection: "Review Inspection Report",
  menuInspectionSub: "Organize findings before due diligence ends",
  menuRepairPlan: "Build My Repair Plan",
  menuRepairPlanSub: "NEW · Findings, pricing, approval and schedule",
  menuInsuranceSub: "Compare home coverage",
  menuHelpSub: "Send your property question",
  menuCall: "Call SAFE",
  menuText: "Text photos",
});
Object.assign(ui.es, {
  menuButton: "Menú",
  menuTitle: "Encuéntrelo rápido",
  menuSubtitle: "Busque o elija una opción",
  menuSearchLabel: "¿Qué necesita?",
  menuSearchPlaceholder: "Fuga del roof, toilet, rafter…",
  menuSearchButton: "Buscar precios",
  menuPrices: "Directorio de precios",
  menuPricesSub: "Busque 130+ servicios",
  menuPromosSub: "Ofertas actuales de SAFE",
  menuGuidesSub: "Entienda primero el problema",
  menuQuestionsSub: "Respuestas para propietarios",
  menuBuyerSeller: "Guía de compra y venta",
  menuBuyerSellerSub: "De la inspección al cierre",
  menuManagers: "Property managers",
  menuManagersSub: "Órdenes y niveles de precio",
  menuRealtors: "Ayuda para Realtors",
  menuRealtorsSub: "Reportes, quotes y coordinación",
  menuRepairLibrary: "Librería de reparaciones",
  menuRepairLibrarySub: "Precios, pintura y combos",
  menuInspection: "Revisar inspección",
  menuInspectionSub: "Organiza hallazgos antes de due diligence",
  menuRepairPlan: "Crear mi plan de reparaciones",
  menuRepairPlanSub: "NUEVO · Hallazgos, precios, aprobación y agenda",
  menuInsuranceSub: "Compare cobertura de casa",
  menuHelpSub: "Envíe su pregunta de propiedad",
  menuCall: "Llamar a SAFE",
  menuText: "Mandar fotos",
});
Object.assign(ui.en, {
  menuLenderReo: "Lenders & REO",
  menuLenderReoSub: "Closing repairs and preservation",
  lenderReoRole: "Lender / REO manager",
  lenderReoRoleSub: "Closing conditions or asset work",
});
Object.assign(ui.es, {
  menuLenderReo: "Lenders y REO",
  menuLenderReoSub: "Reparaciones de cierre y preservación",
  lenderReoRole: "Lender / REO manager",
  lenderReoRoleSub: "Condiciones de cierre o manejo del activo",
});
Object.assign(ui.en, {
  insuranceNav: "Insurance",
  insuranceEyebrow: "Insurance support for the transaction",
  insuranceTitle: "Compare home coverage with confidence.",
  insuranceLead:
    "For buyers, homeowners, and Realtors who need a clear path from property questions to an insurance quote. Carolinas Coverage Insurance Agency can review the home details, compare available options, and explain the differences before the client chooses a policy.",
  insurancePartner: "Insurance resource",
  insuranceCall: "Call a licensed insurance agent",
  insuranceStep1Title: "Share the property basics",
  insuranceStep1Text:
    "Address, closing date, occupancy, year built, roof age, major updates, lender requirements, and current policy when applicable.",
  insuranceStep2Title: "Compare coverage—not only price",
  insuranceStep2Text:
    "Review dwelling limits, deductibles, personal property, liability, loss of use, water-related limitations, endorsements, and important exclusions.",
  insuranceStep3Title: "Choose and complete the policy",
  insuranceStep3Text:
    "A licensed agent confirms eligibility, carrier terms, premium, effective date, required documents, and the steps to purchase coverage.",
  insuranceChecklistEyebrow: "A smarter comparison",
  insuranceChecklistTitle: "Questions worth asking before closing",
  insuranceQ1:
    "Is the dwelling limit based on an appropriate rebuilding estimate?",
  insuranceQ2: "Are the wind/hail and all-other-peril deductibles different?",
  insuranceQ3: "How are the roof’s age and payment method handled?",
  insuranceQ4:
    "Are water backup, service lines, equipment breakdown, or special items optional?",
  insuranceQ5:
    "What is excluded, and is separate flood coverage worth discussing?",
  insuranceForRealtors: "For Realtors",
  insuranceActionTitle: "Give your client one simple next step.",
  insuranceActionText:
    "Request a coverage comparison early enough to address lender deadlines and property questions without waiting until the last minute.",
  insuranceQuoteButton: "Request an insurance comparison",
  insurancePhoneButton: "Call 704-595-3429",
  insuranceDisclaimer:
    "SAFE Contractor is not an insurance company and does not bind or guarantee coverage, price, savings, or eligibility. Insurance advice, quotes, applications, and policy placement are handled by a licensed insurance agent. Do not send Social Security numbers, payment information, or other sensitive personal data through this website.",
  insuranceEducationText:
    "Want to understand the basics first? Review the North Carolina Department of Insurance consumer guide.",
  insuranceEducationLink: "Open the official guide →",
});
Object.assign(ui.es, {
  insuranceNav: "Seguros",
  insuranceEyebrow: "Apoyo de seguros para la transacción",
  insuranceTitle: "Compare cobertura para la casa con confianza.",
  insuranceLead:
    "Para compradores, dueños de casa y Realtors que necesitan un camino claro desde las preguntas sobre la propiedad hasta una cotización de seguro. Carolinas Coverage Insurance Agency puede revisar los datos de la casa, comparar opciones disponibles y explicar las diferencias antes de que el cliente elija una póliza.",
  insurancePartner: "Recurso de seguros",
  insuranceCall: "Llame a un agente de seguros autorizado",
  insuranceStep1Title: "Comparta los datos básicos de la propiedad",
  insuranceStep1Text:
    "Dirección, fecha de cierre, ocupación, año de construcción, edad del techo, mejoras principales, requisitos del prestamista y póliza actual cuando corresponda.",
  insuranceStep2Title: "Compare cobertura, no solamente precio",
  insuranceStep2Text:
    "Revise límites de vivienda, deducibles, propiedad personal, responsabilidad civil, pérdida de uso, limitaciones de agua, coberturas adicionales y exclusiones importantes.",
  insuranceStep3Title: "Elija y complete la póliza",
  insuranceStep3Text:
    "Un agente autorizado confirma elegibilidad, condiciones de la compañía, prima, fecha de vigencia, documentos requeridos y los pasos para comprar la cobertura.",
  insuranceChecklistEyebrow: "Una comparación más inteligente",
  insuranceChecklistTitle: "Preguntas que conviene hacer antes del cierre",
  insuranceQ1:
    "¿El límite de la vivienda está basado en un estimado adecuado de reconstrucción?",
  insuranceQ2:
    "¿Son diferentes el deducible de viento y granizo y el de otros riesgos?",
  insuranceQ3:
    "¿Cómo se consideran la edad del techo y el método de pago de reclamaciones?",
  insuranceQ4:
    "¿Se pueden agregar respaldo de agua, líneas de servicio, avería de equipos o artículos especiales?",
  insuranceQ5:
    "¿Qué está excluido y conviene hablar de cobertura separada para inundación?",
  insuranceForRealtors: "Para Realtors",
  insuranceActionTitle: "Dé a su cliente un solo paso sencillo.",
  insuranceActionText:
    "Pida la comparación con suficiente tiempo para atender fechas del prestamista y preguntas sobre la propiedad sin esperar hasta el último momento.",
  insuranceQuoteButton: "Pedir comparación de seguros",
  insurancePhoneButton: "Llamar al 704-595-3429",
  insuranceDisclaimer:
    "SAFE Contractor no es una compañía de seguros y no garantiza ni emite cobertura, precio, ahorro o elegibilidad. La asesoría, cotización, solicitud y emisión de pólizas son manejadas por un agente de seguros autorizado. No envíe números de Seguro Social, información de pago ni otros datos personales sensibles por este sitio.",
  insuranceEducationText:
    "¿Quiere entender primero lo básico? Consulte la guía del Departamento de Seguros de Carolina del Norte.",
  insuranceEducationLink: "Abrir la guía oficial →",
});
Object.assign(ui.en, {
  insuranceSpotlightLabel: "NEW • For buyers, homeowners & Realtors",
  insuranceSpotlightTitle:
    "Need home insurance? Compare coverage with Carolinas Coverage.",
  insuranceSpotlightText:
    "Review coverage, deductibles and available pricing with a licensed insurance agent before you choose.",
  insuranceSpotlightCompare: "Compare insurance",
  insuranceSpotlightCall: "Call 704-595-3429",
});
Object.assign(ui.es, {
  insuranceSpotlightLabel: "NUEVO • Para compradores, propietarios y Realtors",
  insuranceSpotlightTitle:
    "¿Necesita seguro para la casa? Compare cobertura con Carolinas Coverage.",
  insuranceSpotlightText:
    "Revise cobertura, deducibles y precios disponibles con un agente autorizado antes de elegir.",
  insuranceSpotlightCompare: "Comparar seguros",
  insuranceSpotlightCall: "Llamar al 704-595-3429",
});
function renderQuestions() {
  const q = document
      .getElementById("questionSearch")
      .value.trim()
      .toLowerCase(),
    cats = ["All", ...new Set(homeQuestions.map((x) => x.c))],
    ix = language === "es" ? 1 : 0;
  document.getElementById("libraryCategories").innerHTML = cats
    .map(
      (c) =>
        `<button class="${c === questionCategory ? "active" : ""}" data-qcat="${c}">${language === "es" ? categoryEs[c] || c : c}</button>`,
    )
    .join("");
  document.querySelectorAll("[data-qcat]").forEach(
    (b) =>
      (b.onclick = () => {
        questionCategory = b.dataset.qcat;
        renderQuestions();
      }),
  );
  const found = homeQuestions.filter(
    (x) =>
      (questionCategory === "All" || x.c === questionCategory) &&
      (!q ||
        `${x.q[0]} ${x.q[1]} ${x.a[0]} ${x.a[1]}`.toLowerCase().includes(q)),
  );
  document.getElementById("questionGrid").innerHTML = found
    .map(
      (x) =>
        `<article class="question-card"><span>${language === "es" ? categoryEs[x.c] || x.c : x.c}</span><h3>${x.q[ix]}</h3><p>${x.a[ix]}</p><div><button class="question-action" data-problem="${x.q[ix]}">${ui[language].ask} →</button><button class="question-action cost" data-problem="${x.q[ix]} — ${ui[language].quote}">${ui[language].quote}</button></div></article>`,
    )
    .join("");
  document
    .querySelectorAll(".question-action")
    .forEach(
      (b) => (b.onclick = () => startQuickWithService(b.dataset.problem)),
    );
}
function applyLanguage() {
  document.documentElement.lang = language;
  document
    .querySelectorAll("[data-i18n]")
    .forEach((el) => (el.textContent = ui[language][el.dataset.i18n]));
  document
    .querySelectorAll("[data-i18n-placeholder]")
    .forEach(
      (el) => (el.placeholder = ui[language][el.dataset.i18nPlaceholder]),
    );
  document
    .querySelectorAll("#languageToggle span")
    .forEach((s, i) =>
      s.classList.toggle("active", language === "en" ? i === 0 : i === 1),
    );
  document.getElementById("questionSearch").placeholder =
    language === "es"
      ? "Pregunte sobre pisos, pintura, decks, toilets, luces…"
      : "Ask about floors, paint, decks, toilets, lights…";
  renderPromotions();
  renderQuestions();
  render();
}
document.querySelectorAll(".insurance-request").forEach((b) =>
  b.addEventListener("click", () => {
    selectedRole = "Realtor";
    selectedProblem =
      language === "es"
        ? "Comparar seguro de casa con Carolinas Coverage"
        : "Compare home insurance with Carolinas Coverage";
    showFlowStep(3);
    document
      .getElementById("quickFlow")
      .scrollIntoView({ behavior: "smooth", block: "center" });
  }),
);
document.getElementById("languageToggle").addEventListener("click", () => {
  language = language === "en" ? "es" : "en";
  localStorage.setItem("safeLanguage", language);
  applyLanguage();
});
document
  .getElementById("questionSearch")
  .addEventListener("input", renderQuestions);
applyLanguage();
render();
const mobileMenu = document.getElementById("mobileMenu"),
  mobileMenuButton = document.getElementById("mobileMenuButton"),
  mobileMenuBackdrop = document.getElementById("mobileMenuBackdrop"),
  mobileMenuClose = document.getElementById("mobileMenuClose");
function setMobileMenu(open) {
  mobileMenu.classList.toggle("open", open);
  mobileMenuBackdrop.classList.toggle("open", open);
  mobileMenuBackdrop.hidden = !open;
  mobileMenu.setAttribute("aria-hidden", String(!open));
  mobileMenuButton.setAttribute("aria-expanded", String(open));
  document.body.classList.toggle("menu-open", open);
  if (open)
    setTimeout(
      () => document.getElementById("mobileMenuSearchInput").focus(),
      120,
    );
}
mobileMenuButton.addEventListener("click", () => setMobileMenu(true));
mobileMenuClose.addEventListener("click", () => setMobileMenu(false));
mobileMenuBackdrop.addEventListener("click", () => setMobileMenu(false));
document.addEventListener("keydown", (e) => {
  if (e.key === "Escape") setMobileMenu(false);
});
mobileMenu
  .querySelectorAll("a")
  .forEach((a) => a.addEventListener("click", () => setMobileMenu(false)));
document.getElementById("mobileMenuSearch").addEventListener("submit", (e) => {
  e.preventDefault();
  const q = document.getElementById("mobileMenuSearchInput").value.trim();
  if (!q) return;
  instantSearch.value = directorySearch.value = search.value = q;
  category = "all";
  document
    .querySelectorAll(".tab")
    .forEach((t) => t.classList.toggle("active", t.dataset.category === "all"));
  render();
  setMobileMenu(false);
  document.getElementById("find-help").scrollIntoView({ behavior: "smooth" });
  setTimeout(() => directorySearch.focus(), 500);
});
