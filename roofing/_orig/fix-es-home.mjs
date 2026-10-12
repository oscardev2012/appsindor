import fs from "fs";
const p = "D:/Oscar.Sierra/source/repos/roofing/Pages/Es/Index.cshtml";
let t = fs.readFileSync(p, "utf8");
t = t.replaceAll("MAJENTIC", "MAJESTIC");
t = t.replaceAll("Documentaración", "Documentación");
t = t.replace(
  `ViewData["Description"] = "Bilingual documentación de reclamos de techo, free storm inspections, roof replacement, repairs, emergency tarping, gutters and fascia across Charlotte, NC and Rock Hill, SC.";`,
  `ViewData["Description"] = "Documentación bilingüe de reclamos de techo, inspecciones gratuitas de tormenta, reemplazo, reparaciones, carpetas de emergencia, gutters y fascia en Charlotte, NC y Rock Hill, SC.";`
);
t = t.replace(
  `<p class="eyebrow eyebrow-light">Roof insurance claim documentation · Charlotte &amp; Rock Hill</p>`,
  `<p class="eyebrow eyebrow-light">Documentación de reclamos de techo · Charlotte y Rock Hill</p>`
);
t = t.replace(`<h3>Repairs &amp; leak diagnosis</h3>`, `<h3>Reparaciones y liqueos</h3>`);
t = t.replace(`<h3>Gutters, fascia &amp; soffit</h3>`, `<h3>Gutters, fascia y soffit</h3>`);
t = t.replace(
  `data-lang="es" aria-pressed="true"><span class="lang-code">EN</span>`,
  `data-lang="es" aria-pressed="true"><span class="lang-code">ES</span>`
);
fs.writeFileSync(p, t);
console.log("MAJENTIC leftover", t.includes("MAJENTIC"));
console.log("MAJESTIC count", (t.match(/MAJESTIC/g) || []).length);
console.log("ES code", t.includes(`data-lang="es" aria-pressed="true"><span class="lang-code">ES</span>`));
