const fs = require("fs");
const data = JSON.parse(fs.readFileSync("D:/Oscar.Sierra/source/repos/SafeProject/bin/Debug/net8.0/_seo_pages.json", "utf8"));
const arch = JSON.parse(fs.readFileSync("D:/Oscar.Sierra/source/repos/SafeProject/Data/seo-architecture.json", "utf8"));
const known = new Set(arch.map((p) => p.slug));

console.log("count field", data.count);
const byPage = data.byPage;
const pages = Object.keys(byPage);
let total = 0;
const foreign = [];
for (const slug of pages) {
  const qs = byPage[slug];
  total += qs.length;
  if (!known.has(slug)) {
    foreign.push({ slug, n: qs.length, queries: qs });
  }
}
console.log("pages in byPage", pages.length, "total queries", total);
console.log("\n=== FOREIGN DESTINATIONS (not in 38) ===");
for (const f of foreign.sort((a, b) => b.n - a.n)) {
  console.log(`\n${f.slug} (${f.n})`);
  for (const q of f.queries) {
    console.log(`  id=${q.id} | ${q.en} | scenario=${q.scenario} | pri=${q.priority}`);
  }
}

// Compare: queries in _seo but not in architecture (by en string)
const archEns = new Set();
for (const p of arch) for (const q of p.queries || []) archEns.add((q.en || "").toLowerCase().trim());
const missingFromArch = [];
for (const slug of pages) {
  for (const q of byPage[slug]) {
    if (!archEns.has((q.en || "").toLowerCase().trim())) {
      missingFromArch.push({ page: q.page, en: q.en, id: q.id, scenario: q.scenario });
    }
  }
}
console.log("\n=== IN _seo BUT NOT IN architecture.json ===", missingFromArch.length);
for (const q of missingFromArch) {
  console.log(`  ${q.page} | ${q.en} | ${q.scenario}`);
}

// Also: architecture pages that exist but query counts differ
console.log("\n=== QC DIFF arch vs _seo for shared 38 ===");
for (const p of arch) {
  const a = (p.queries || []).length;
  const b = (byPage[p.slug] || []).length;
  if (a !== b) console.log(`${p.slug} arch=${a} seo=${b} delta=${b - a}`);
}
