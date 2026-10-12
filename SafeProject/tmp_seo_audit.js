const fs = require("fs");
const path = require("path");

const ROOT = String.raw`D:\Oscar.Sierra\source\repos\SafeProject`;
const arch = JSON.parse(fs.readFileSync(path.join(ROOT, "Data", "seo-architecture.json"), "utf8"));
const copySrc = fs.readFileSync(path.join(ROOT, "Data", "SeoCopy.cs"), "utf8");

const methods = copySrc.split(/\n    private static SeoPageDraft /);
const copyBySlug = {};
function pageTextForSlug(slug) {
  for (const chunk of methods.slice(1)) {
    if (chunk.includes(`"${slug}"`)) return chunk.toLowerCase();
  }
  return "";
}
for (const chunk of methods.slice(1)) {
  const m = chunk.match(/"(\/[^"]+\/)"/);
  if (!m) continue;
  const slug = m[1];
  const sections = [...chunk.matchAll(/S\("([^"]+)"/g)].map((x) => x[1]);
  const prose = [...chunk.matchAll(/S\("[^"]+",\s*\n\s*"((?:[^"\\]|\\.)*)"/g)].map((x) => x[1]);
  const bodyChars = prose.reduce((a, b) => a + b.length, 0);
  copyBySlug[slug] = { sections, sectionCount: sections.length, bodyChars, chunkLen: chunk.length };
}

const FAQ_RULES = [
  ["safety_24h", ["24-hour", "24 hour", "24 horas", "24/7"]],
  ["safety_gas", ["smell gas", "olor a gas", "carbon monoxide", "monóxido", "co alarm", "yellow flame", "llama amarilla", "soot", "hollín"]],
  ["safety_flood", ["flood", "inund", "bulging", "abultado", "steam", "vapor", "boiling", "hirviendo"]],
  ["leak", ["leaking", "fuga", "leak"]],
  ["price", ["how much", "cost", "cuánto", "cuesta", "price", "precio", "labor", "mano de obra"]],
  ["same_day", ["same day", "mismo día", "today", "hoy", "weekend", "fin de semana"]],
  ["40gal", ["40 gallon", "40-gal", "40 galones"]],
  ["50gal", ["50 gallon", "50-gal", "50 galones"]],
  ["size", ["size", "tamaño", "what size", "first hour"]],
  ["permit", ["permit", "permiso"]],
  ["removal_expansion", ["expansion", "expansión", "include old", "retiro", "removal"]],
  ["financing", ["financ", "synchrony", "$200"]],
  ["warranty", ["warranty", "garantía", "insurance", "seguro"]],
  ["near_me", ["near me", "cerca", "charlotte", "shelby", "china grove"]],
  ["noise", ["noise", "ruido", "popping", "rumbling"]],
  ["smell_water", ["smell", "olor", "rusty", "oxid", "discolor", "color"]],
  ["repair_replace", ["repair or replace", "reparar o", "should i repair"]],
  ["no_hot", ["no hot water", "sin agua caliente", "not working", "no funciona", "not heating"]],
  ["tankless", ["tankless"]],
  ["attic_access", ["attic", "ático", "crawl", "difficult", "difícil"]],
];

function classifyFaq(q) {
  const blob = `${q.en || ""} ${q.es || ""} ${q.scenario || ""} ${q.cluster || ""} ${q.note || ""}`.toLowerCase();
  if (
    ["spark", "chispa", "burned wire", "cable quemado", "hot wire", "cables", "breaker"].some((x) => blob.includes(x)) &&
    ["dangerous", "peligro", "hot", "caliente", "emergency", "emergencia", "spark", "burned"].some((x) => blob.includes(x))
  ) {
    return "safety_electric";
  }
  for (const [name, needles] of FAQ_RULES) {
    if (needles.some((n) => blob.includes(n.toLowerCase()))) return name;
  }
  return "fallback";
}

function depthLabel(secN, bodyChars) {
  if (secN >= 5 && bodyChars >= 800) return "strong";
  if (secN >= 4 && bodyChars >= 500) return "good";
  if (secN >= 3 && bodyChars >= 350) return "moderate";
  if (secN >= 2) return "thin";
  return "missing";
}

const checks = [
  ["attic", ["attic", "ático", "atico"], ["attic", "ático"]],
  ["city_named", ["shelby", "china grove", "mint hill", "gastonia", "concord", "rock hill", "huntersville", "matthews", "mooresville"], ["shelby", "china grove", "mint hill", "gastonia"]],
  ["fuel_gas", [" gas ", "gas water", "natural gas", "propane"], ["gas"]],
  ["fuel_electric", ["electric"], ["electric"]],
  ["price", ["how much", "cost", "price", "cuánto", "cuesta"], ["$1,199", "$1199", "$1,349", "$1349"]],
  ["permit", ["permit", "permiso"], ["permit"]],
  ["lowe_hd", ["lowe", "home depot"], ["lowe", "home depot"]],
  ["cash", ["cash price", "cash", "contado", "efectivo"], ["cash", "contado", "efectivo"]],
  ["weekend", ["weekend", "fin de semana", "saturday", "sunday"], ["weekend", "saturday", "after-hours", "after hours"]],
  ["safety", ["smell gas", "carbon monoxide", "911", "dangerous", "spark", "flood"], ["911", "evacuate", "gas utility", "do not"]],
  ["expansion", ["expansion"], ["expansion"]],
  ["labor_only", ["labor", "mano de obra"], ["labor", "$999", "999"]],
  ["hard_water", ["hard water", "sediment", "popping", "rumbling"], ["hard water", "sediment"]],
  ["financing", ["financ", "synchrony", "payment plan"], ["financ", "synchrony", "homeowner plan"]],
  ["warranty", ["warranty", "garantía"], ["warrant", "garant"]],
  ["crawl", ["crawl"], ["crawl"]],
  ["tankless", ["tankless"], ["tankless"]],
  ["heat_pump", ["heat pump", "hybrid"], ["heat pump", "hybrid"]],
  ["same_day", ["same day", "today", "mismo día", "hoy"], ["same-day", "same day"]],
  ["insurance", ["insurance", "seguro", "claim"], ["insurance"]],
  ["realtor", ["realtor", "real estate", "closing"], ["realtor", "closing", "listing"]],
  ["property_manager", ["property manager", "rental", "tenant"], ["property manager", "tenant", "rental"]],
];

const archSlugs = arch.map((p) => p.slug);
console.log("page_count", arch.length);
console.log("total_queries", arch.reduce((a, p) => a + (p.queries || []).length, 0));
console.log("copy_slugs", Object.keys(copyBySlug).length);
console.log("arch_not_in_copy", archSlugs.filter((s) => !copyBySlug[s]));
console.log("copy_not_in_arch", Object.keys(copyBySlug).filter((s) => !archSlugs.includes(s)));

const faqBucketGlobal = {};
const fallbackExamples = [];
const pageRows = [];

for (const p of arch) {
  const slug = p.slug;
  const qs = p.queries || [];
  const top10 = qs.slice(0, 10);
  const top10Buckets = {};
  for (const q of top10) {
    const b = classifyFaq(q);
    top10Buckets[b] = (top10Buckets[b] || 0) + 1;
    faqBucketGlobal[b] = (faqBucketGlobal[b] || 0) + 1;
  }
  for (const q of qs.slice(10)) {
    const b = classifyFaq(q);
    faqBucketGlobal[b] = (faqBucketGlobal[b] || 0) + 1;
  }
  const fallbackN = top10Buckets.fallback || 0;
  const most = Math.max(0, ...Object.values(top10Buckets), 0);
  const dupRatio = top10.length ? most / top10.length : 0;
  const copy = copyBySlug[slug] || { sectionCount: 0, bodyChars: 0, sections: [] };
  const body = pageTextForSlug(slug);
  const queryBlob = qs.map((q) => `${q.en || ""} ${q.scenario || ""} ${q.note || ""}`).join(" ").toLowerCase();
  const missing = [];
  for (const [name, qneedles, bneedles] of checks) {
    if (qneedles.some((n) => queryBlob.includes(n)) && !bneedles.some((n) => body.includes(n))) {
      missing.push(name);
    }
  }
  const uniqueDepth = depthLabel(copy.sectionCount, copy.bodyChars);
  let faqRisk = "low";
  if (fallbackN >= 3 || dupRatio >= 0.5) faqRisk = "high";
  else if (fallbackN >= 1 || dupRatio >= 0.35) faqRisk = "medium";

  const notes = qs.filter((q) => (q.note || "").trim());
  const scenarios = [...new Set(qs.map((q) => q.scenario || ""))];

  pageRows.push({
    priority: p.priority,
    language: p.language,
    slug,
    qc: qs.length,
    secs: copy.sectionCount,
    body: copy.bodyChars,
    depth: uniqueDepth,
    faqRisk,
    fallback10: fallbackN,
    dup: +dupRatio.toFixed(2),
    missing: missing.join(","),
    buckets: top10Buckets,
    sections: copy.sections,
    minContent: p.minContent,
    h1: p.h1,
    noteCount: notes.length,
    scenarios,
  });

  console.log(
    `SLUG|${p.priority}|${p.language}|${slug}|qc=${qs.length}|secs=${copy.sectionCount}|body=${copy.bodyChars}|depth=${uniqueDepth}|faq_risk=${faqRisk}|fallback10=${fallbackN}|dup=${dupRatio.toFixed(2)}|missing=${missing.join(",")}|buckets=${JSON.stringify(top10Buckets)}`
  );
  console.log(`  SECTIONS|${copy.sections.join(" || ")}`);
  console.log(`  SCENARIOS|${scenarios.length}|${scenarios.slice(0, 30).join(" || ")}`);
  for (const q of notes.slice(0, 10)) {
    console.log(`  NOTE|${(q.en || "").slice(0, 90)}|${(q.note || "").slice(0, 140)}`);
  }
  for (const q of top10) {
    if (classifyFaq(q) === "fallback") fallbackExamples.push([slug, q.en, q.scenario]);
  }
}

console.log("\n=== FALLBACK FAQ EXAMPLES ===");
for (const item of fallbackExamples.slice(0, 60)) {
  console.log("FB|", item[0], "|", item[1], "|", item[2]);
}

console.log("\n=== GLOBAL FAQ BUCKETS ===");
Object.entries(faqBucketGlobal)
  .sort((a, b) => b[1] - a[1])
  .forEach(([k, v]) => console.log(k, v));

// Foreign destinations in seo json files
console.log("\n=== FOREIGN SLUGS IN SEO JSON ===");
const known = new Set(archSlugs);
function walk(dir) {
  for (const ent of fs.readdirSync(dir, { withFileTypes: true })) {
    if (ent.name === "node_modules" || ent.name === ".git" || ent.name === "obj") continue;
    const full = path.join(dir, ent.name);
    if (ent.isDirectory()) walk(full);
    else if (/\.json$/i.test(ent.name) && /seo/i.test(ent.name)) {
      const text = fs.readFileSync(full, "utf8");
      const found = [...text.matchAll(/"(\/[a-z0-9\-]+\/)"/g)].map((m) => m[1]);
      const water = [...new Set(found.filter((s) => /water|calentador|heater|es\//.test(s)))];
      const extra = water.filter((s) => !known.has(s));
      if (extra.length) {
        console.log("FILE", path.relative(ROOT, full), "extra", extra.sort().join(", "), "count", extra.length);
      }
    }
  }
}
walk(ROOT);

console.log("\n=== P0 DETAILED ===");
for (const p of arch.filter((x) => x.priority === "P0")) {
  const slug = p.slug;
  const qs = p.queries || [];
  const copy = copyBySlug[slug] || { sections: [] };
  console.log(`\n## ${slug} qc=${qs.length}`);
  console.log("sections:", copy.sections.join(" | "));
  const themes = {};
  for (const q of qs) {
    const b = classifyFaq(q);
    if (!themes[b]) themes[b] = [];
    themes[b].push(q.en || q.scenario);
  }
  for (const [b, items] of Object.entries(themes).sort((a, b) => b[1].length - a[1].length)) {
    console.log(`  theme[${b}] n=${items.length} e.g. ${(items[0] || "").slice(0, 100)}`);
  }
  const beyond = qs.slice(10);
  if (beyond.length) {
    const bt = {};
    for (const q of beyond) {
      const b = classifyFaq(q);
      bt[b] = (bt[b] || 0) + 1;
    }
    console.log("  beyond_faq_cap", beyond.length, JSON.stringify(bt));
    for (const q of beyond.slice(0, 20)) {
      console.log(
        `    + ${q.scenario}|${(q.en || "").slice(0, 80)}|faq=${classifyFaq(q)}|note=${(q.note || "").slice(0, 80)}`
      );
    }
  }
}

// Save condensed JSON report
fs.writeFileSync(path.join(ROOT, "tmp_seo_audit_out.json"), JSON.stringify({ pageRows, fallbackExamples, faqBucketGlobal }, null, 2));
console.log("\nWROTE tmp_seo_audit_out.json");
