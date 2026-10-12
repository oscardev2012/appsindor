const fs = require("fs");
const text = fs.readFileSync("D:/Oscar.Sierra/source/repos/SafeProject/bin/Debug/net8.0/_seo_pages.json", "utf8");
const data = JSON.parse(text);
const arch = JSON.parse(fs.readFileSync("D:/Oscar.Sierra/source/repos/SafeProject/Data/seo-architecture.json", "utf8"));
const known = new Set(arch.map((p) => p.slug));

console.log("type", Array.isArray(data) ? "array" : typeof data, "keys", Object.keys(data).slice(0, 5));
if (data.pages) console.log("pages", Object.keys(data.pages || {}).length);
if (data.byPage) console.log("byPage", Object.keys(data.byPage).length);

// Find structure
function findMap(obj, depth = 0) {
  if (!obj || typeof obj !== "object" || depth > 2) return;
  const keys = Object.keys(obj);
  const slugKeys = keys.filter((k) => k.startsWith("/") && k.endsWith("/"));
  if (slugKeys.length > 5) {
    console.log("FOUND SLUG MAP at depth", depth, "count", slugKeys.length);
    const foreign = slugKeys.filter((k) => !known.has(k));
    console.log("foreign count", foreign.length);
    for (const f of foreign) {
      const v = obj[f];
      const n = Array.isArray(v) ? v.length : typeof v;
      console.log("FOREIGN", f, "n=", n);
      if (Array.isArray(v)) {
        for (const item of v.slice(0, 3)) {
          console.log("  ", JSON.stringify(item).slice(0, 200));
        }
      }
    }
    // also total queries in map
    let total = 0;
    for (const k of slugKeys) {
      if (Array.isArray(obj[k])) total += obj[k].length;
    }
    console.log("total query rows in map", total);
    console.log("known in map", slugKeys.filter((k) => known.has(k)).length);
    return;
  }
  for (const k of keys.slice(0, 20)) findMap(obj[k], depth + 1);
}
findMap(data);

// Top-level keys
console.log("top keys", Object.keys(data));
