# -*- coding: utf-8 -*-
import json
import re
from collections import defaultdict, Counter
from pathlib import Path

ROOT = Path(r"D:\Oscar.Sierra\source\repos\SafeProject")
arch = json.loads((ROOT / "Data" / "seo-architecture.json").read_text(encoding="utf-8"))
copy_src = (ROOT / "Data" / "SeoCopy.cs").read_text(encoding="utf-8")
faq_src = (ROOT / "Data" / "SeoFaqBank.cs").read_text(encoding="utf-8")

# Extract SeoCopy slug -> section H2 titles and body length
slug_blocks = {}
# Match each P* method roughly by finding slug string and following S("...") titles until next private static or end of All
method_pat = re.compile(
    r'private static SeoPageDraft (\w+)\(\) => new\(\s*'
    r'"([^"]+)",\s*"([^"]+)",\s*"([^"]+)",\s*'
    r'"([^"]*)",\s*'
    r'"([^"]*)",\s*'
    r'"([^"]*)",\s*'
    r'"([^"]*)",\s*"([^"]*)",\s*"([^"]*)",\s*'
    r'"((?:[^"\\]|\\.)*)"',
    re.S,
)
# Simpler: find slug lines and S("title" within each method body
methods = re.split(r'\n    private static SeoPageDraft ', copy_src)
copy_by_slug = {}
for chunk in methods[1:]:
    m = re.search(r'"(/[^"]+/)"', chunk)
    if not m:
        continue
    slug = m.group(1)
    h1m = re.search(r'"(/[^"]+/)",\s*\n\s*"([^"]+)"', chunk)
    h1 = h1m.group(2) if h1m else ""
    sections = re.findall(r'S\("([^"]+)"', chunk)
    # body text approx: all string literals in S( calls - count chars of prose after titles
    prose = re.findall(r'S\("[^"]+",\s*\n\s*"((?:[^"\\]|\\.)*)"', chunk)
    bullets = re.findall(r'"([^"]{20,})"', chunk)
    answer_first = ""
    af = re.search(r'"((?:CHARLOTTE|HERO|MORE|SAME|EMERGENCY|LEAK|NO HOT|REPAIR|NEAR|ES |REEMPLAZO|ELECTRIC|GAS |NOT ENOUGH|NOISE|SMELLY|PRESSURE|SIZE|BUYING|FINANC|CODE|PERMIT|ATTIC|ACCESS|PROCESS|WARRANTY|NEW |QUOTE|REALTOR|PM |TANKLESS|MAINT|HEAT|SPECIAL|LEAK DAMAGE)[^"]*)"', chunk)
    # Get AnswerFirst: after badge string
    lines = chunk.split("\n")
    # Count unique section depth
    body_chars = sum(len(p) for p in prose)
    copy_by_slug[slug] = {
        "sections": sections,
        "section_count": len(sections),
        "body_chars": body_chars,
        "chunk_len": len(chunk),
        "h1": h1,
    }

# Also parse _seo_pages.json if exists for rendered FAQ answers
seo_pages_path = ROOT / "bin" / "Debug" / "net8.0" / "_seo_pages.json"
rendered = {}
if seo_pages_path.exists():
    try:
        pages = json.loads(seo_pages_path.read_text(encoding="utf-8"))
        if isinstance(pages, list):
            for p in pages:
                slug = p.get("slug") or p.get("Slug") or ""
                faqs = p.get("faqs") or p.get("Faqs") or []
                rendered[slug] = faqs
        elif isinstance(pages, dict):
            for slug, p in pages.items():
                faqs = p.get("faqs") or p.get("Faqs") or []
                rendered[slug] = faqs
    except Exception as e:
        print("RENDER_PARSE_ERR", e)

# FAQ keyword buckets from SeoFaqBank order (first match wins)
FAQ_RULES = [
    ("safety_24h", ["24-hour", "24 hour", "24 horas", "24/7"]),
    ("safety_gas", ["smell gas", "olor a gas", "carbon monoxide", "monóxido", "co alarm", "yellow flame", "llama amarilla", "soot", "hollín"]),
    ("safety_electric", ["spark", "chispa", "burned wire", "cable quemado"]),  # partial - has compound
    ("safety_flood", ["flood", "inund", "bulging", "abultado", "steam", "vapor", "boiling", "hirviendo"]),
    ("leak", ["leaking", "fuga", "leak"]),
    ("price", ["how much", "cost", "cuánto", "cuesta", "price", "precio", "labor", "mano de obra"]),
    ("same_day", ["same day", "mismo día", "today", "hoy", "weekend", "fin de semana"]),
    ("40gal", ["40 gallon", "40-gal", "40 galones"]),
    ("50gal", ["50 gallon", "50-gal", "50 galones"]),
    ("size", ["size", "tamaño", "what size", "first hour"]),
    ("permit", ["permit", "permiso"]),
    ("removal_expansion", ["expansion", "expansión", "include old", "retiro", "removal"]),
    ("financing", ["financ", "synchrony", "$200", "200"]),
    ("warranty", ["warranty", "garantía", "insurance", "seguro"]),
    ("near_me", ["near me", "cerca", "charlotte", "shelby", "china grove"]),
    ("noise", ["noise", "ruido", "popping", "rumbling"]),
    ("smell_water", ["smell", "olor", "rusty", "oxid", "discolor", "color"]),
    ("repair_replace", ["repair or replace", "reparar o", "should i repair"]),
    ("no_hot", ["no hot water", "sin agua caliente", "not working", "no funciona", "not heating"]),
    ("tankless", ["tankless"]),
    ("attic_access", ["attic", "ático", "crawl", "difficult", "difícil"]),
    ("fallback", []),
]

def classify_faq(q):
    blob = f"{q.get('en','')} {q.get('es','')} {q.get('scenario','')} {q.get('cluster','')} {q.get('note','')}".lower()
    # special compound for electric danger
    if any(x in blob for x in ["spark", "chispa", "burned wire", "cable quemado", "hot wire", "cables", "breaker"]) and \
       any(x in blob for x in ["dangerous", "peligro", "hot", "caliente", "emergency", "emergencia", "spark", "burned"]):
        return "safety_electric"
    for name, needles in FAQ_RULES:
        if name == "fallback" or not needles:
            continue
        if any(n.lower() in blob for n in needles):
            return name
    return "fallback"

# Content gap heuristics: keywords that should appear in page body sections
GAP_SIGNALS = {
    "price_numbers": [r"\$1,?199", r"\$1,?349", r"\$1,?449", r"\$999", r"\$149"],
    "attic": [r"attic", r"ático"],
    "crawl": [r"crawl"],
    "permit": [r"permit", r"permiso"],
    "expansion": [r"expansion"],
    "fuel_gas": [r"\bgas\b"],
    "fuel_electric": [r"electric"],
    "charlotte": [r"charlotte"],
    "same_day": [r"same.?day", r"mismo d[ií]a"],
    "safety_911": [r"911", r"gas utility", r"evacuate"],
    "lowe.?home.?depot": [r"lowe", r"home depot"],
    "labor.?only": [r"labor.?only", r"mano de obra"],
    "cash": [r"\bcash\b", r"contado", r"efectivo"],
    "financing": [r"financ", r"synchrony"],
    "warranty": [r"warrant", r"garant"],
    "hard.?water|sediment": [r"hard water", r"sediment", r"sedimento"],
    "first.?hour|fhr": [r"first hour", r"\bfhr\b"],
    "tankless": [r"tankless"],
    "heat.?pump": [r"heat.?pump", r"bomba de calor"],
}

def page_text_for_slug(slug):
    # extract method chunk
    for chunk in methods[1:]:
        if f'"{slug}"' in chunk:
            return chunk.lower()
    return ""

print("=== ARCH PAGES ===")
print("page_count", len(arch))
total_q = sum(len(p.get("queries") or []) for p in arch)
print("total_queries", total_q)
print("copy_slugs", len(copy_by_slug))

arch_slugs = [p["slug"] for p in arch]
print("\n=== SLUG MATCH ===")
missing_in_copy = [s for s in arch_slugs if s not in copy_by_slug]
extra_in_copy = [s for s in copy_by_slug if s not in arch_slugs]
print("arch_not_in_copy", missing_in_copy)
print("copy_not_in_arch", extra_in_copy)

# Per-page analysis
print("\n=== PER PAGE ===")
faq_bucket_global = Counter()
fallback_examples = []
duplicate_risk_pages = []

for p in arch:
    slug = p["slug"]
    qs = p.get("queries") or []
    buckets = Counter(classify_faq(q) for q in qs)
    faq_bucket_global.update(buckets)
    # Only first 10 become FAQs
    top10 = qs[:10]
    top10_buckets = Counter(classify_faq(q) for q in top10)
    fallback_n = top10_buckets.get("fallback", 0)
    # uniqueness of answers among top10 (same bucket = duplicate answer text)
    dup_ratio = 0
    if top10:
        # most common bucket share
        most = top10_buckets.most_common(1)[0][1]
        dup_ratio = most / len(top10)

    copy = copy_by_slug.get(slug, {})
    body = page_text_for_slug(slug)
    # scenario themes in queries
    scenarios = [q.get("scenario") or "" for q in qs]
    notes = [(q.get("en"), q.get("note")) for q in qs if (q.get("note") or "").strip()]

    # Check gaps vs query keywords
    query_blob = " ".join(
        f"{q.get('en','')} {q.get('scenario','')} {q.get('note','')}" for q in qs
    ).lower()

    missing_modifiers = []
    checks = [
        ("attic", ["attic", "ático", "atico"], ["attic", "ático"]),
        ("city_named", ["shelby", "china grove", "mint hill", "gastonia", "concord", "rock hill", "huntersville", "matthews", "mooresville"], ["shelby", "china grove", "mint hill", "gastonia"]),
        ("fuel_gas", [" gas ", "gas water", "natural gas", "propane"], ["gas"]),
        ("fuel_electric", ["electric"], ["electric"]),
        ("price", ["how much", "cost", "price", "cuánto", "cuesta"], ["$1,199", "$1199", "$1,349", "$1349"]),
        ("permit", ["permit", "permiso"], ["permit"]),
        ("lowe_hd", ["lowe", "home depot"], ["lowe", "home depot"]),
        ("cash", ["cash price", "cash", "contado", "efectivo"], ["cash", "contado", "efectivo"]),
        ("weekend", ["weekend", "fin de semana", "saturday", "sunday"], ["weekend", "saturday", "after-hours", "after hours"]),
        ("safety", ["smell gas", "carbon monoxide", "911", "dangerous", "spark", "flood"], ["911", "evacuate", "gas utility", "do not"]),
        ("expansion", ["expansion"], ["expansion"]),
        ("labor_only", ["labor", "mano de obra"], ["labor", "$999", "999"]),
        ("hard_water", ["hard water", "sediment", "popping", "rumbling"], ["hard water", "sediment"]),
        ("financing", ["financ", "synchrony", "payment plan"], ["financ", "synchrony", "homeowner plan"]),
        ("warranty", ["warranty", "garantía"], ["warrant", "garant"]),
        ("crawl", ["crawl"], ["crawl"]),
        ("tankless", ["tankless"], ["tankless"]),
        ("heat_pump", ["heat pump", "hybrid"], ["heat pump", "hybrid"]),
        ("same_day", ["same day", "today", "mismo día", "hoy"], ["same-day", "same day"]),
        ("insurance", ["insurance", "seguro", "claim"], ["insurance"]),
        ("realtor", ["realtor", "real estate", "closing"], ["realtor", "closing", "listing"]),
        ("property_manager", ["property manager", "rental", "tenant"], ["property manager", "tenant", "rental"]),
    ]
    for name, qneedles, bneedles in checks:
        if any(n in query_blob for n in qneedles):
            if not any(n in body for n in bneedles):
                missing_modifiers.append(name)

    # Unique depth score
    sec_n = copy.get("section_count", 0)
    body_chars = copy.get("body_chars", 0)
    unique_depth = "none"
    if sec_n >= 5 and body_chars >= 800:
        unique_depth = "strong"
    elif sec_n >= 4 and body_chars >= 500:
        unique_depth = "good"
    elif sec_n >= 3 and body_chars >= 350:
        unique_depth = "moderate"
    elif sec_n >= 2:
        unique_depth = "thin"
    else:
        unique_depth = "missing"

    # FAQ generic risk
    faq_risk = "low"
    if fallback_n >= 3 or dup_ratio >= 0.5:
        faq_risk = "high"
    elif fallback_n >= 1 or dup_ratio >= 0.35:
        faq_risk = "medium"

    if faq_risk != "low":
        duplicate_risk_pages.append(slug)

    print(f"SLUG|{p['priority']}|{p['language']}|{slug}|qc={len(qs)}|secs={sec_n}|body={body_chars}|depth={unique_depth}|faq_risk={faq_risk}|fallback10={fallback_n}|dup={dup_ratio:.2f}|missing={','.join(missing_modifiers)}|buckets={dict(top10_buckets)}")
    if notes:
        for en, note in notes[:8]:
            print(f"  NOTE|{en[:80]}|{note[:120]}")
    # list scenarios unique
    uniq_sc = list(dict.fromkeys(scenarios))
    print(f"  SCENARIOS|{len(uniq_sc)}|{ ' || '.join(uniq_sc[:25]) }")
    if fallback_n and top10:
        for q in top10:
            if classify_faq(q) == "fallback":
                fallback_examples.append((slug, q.get("en"), q.get("scenario")))

print("\n=== FALLBACK FAQ EXAMPLES (first 40) ===")
for item in fallback_examples[:40]:
    print("FB|", item[0], "|", item[1], "|", item[2])

print("\n=== GLOBAL FAQ BUCKETS ===")
for k, v in faq_bucket_global.most_common():
    print(k, v)

# Find leaked destinations: queries whose destination isn't in 38
# Architecture maps queries to page; check _seo if it has destination field
print("\n=== CHECK _seo_pages / other json for foreign destinations ===")
# Search for slug-like destinations in architecture that aren't in the 38
known = set(arch_slugs)
# Also check if any query has a destination field
foreign = []
for p in arch:
    for q in p.get("queries") or []:
        for k in ("destination", "url", "slug", "page"):
            if k in q and q[k] and q[k] not in known:
                foreign.append((p["slug"], q.get("en"), q[k]))

# Scan other seo json files
for path in ROOT.rglob("*.json"):
    if "seo" not in path.name.lower() and "_seo" not in path.name.lower():
        continue
    try:
        text = path.read_text(encoding="utf-8", errors="ignore")
    except Exception:
        continue
    # find /something-water-heater.../ or similar paths
    found = set(re.findall(r'"(/[a-z0-9\-]+/)"', text))
    # filter likely page slugs
    water = [s for s in found if "water" in s or "calentador" in s or "heater" in s or s.startswith("/es/")]
    extra = [s for s in water if s not in known]
    if extra:
        print("FILE", path.relative_to(ROOT), "extra_slugs", sorted(extra)[:50], "count", len(extra))

# Also list all query destinations if architecture embeds differently
print("\n=== ARCH SLUG LIST WITH QC ===")
for p in sorted(arch, key=lambda x: (x["priority"], -len(x.get("queries") or []), x["slug"])):
    print(f"{p['priority']}\t{len(p.get('queries') or [])}\t{p['slug']}\t{p.get('minContent','')[:60]}")

# Detailed gap: for each P0 page, list queries where FAQ is fallback OR modifiers missing from body
print("\n=== P0 DETAILED GAPS ===")
for p in arch:
    if p["priority"] != "P0":
        continue
    slug = p["slug"]
    body = page_text_for_slug(slug)
    qs = p.get("queries") or []
    print(f"\n## {slug} qc={len(qs)} sections={copy_by_slug.get(slug,{}).get('sections')}")
    # theme clusters among queries
    themes = defaultdict(list)
    for q in qs:
        b = classify_faq(q)
        themes[b].append(q.get("en") or q.get("scenario"))
    for b, items in sorted(themes.items(), key=lambda x: -len(x[1])):
        print(f"  theme[{b}] n={len(items)} e.g. {items[0][:90]}")
    # Queries beyond FAQ cap 10 that need H2 coverage
    beyond = qs[10:]
    if beyond:
        beyond_themes = Counter(classify_faq(q) for q in beyond)
        print(f"  beyond_faq_cap={len(beyond)} themes={dict(beyond_themes)}")
        # sample unique scenarios beyond 10
        for q in beyond[:15]:
            print(f"    + {q.get('scenario')}|{q.get('en')[:70]}|faq={classify_faq(q)}|note={(q.get('note') or '')[:60]}")

print("\nDONE")
