import fs from "fs";
import path from "path";
import { fileURLToPath } from "url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const orig = __dirname;
const root = path.resolve(__dirname, "..");
const pagesDir = path.join(root, "Pages");
const www = path.join(root, "wwwroot");

const pageMap = [
  { src: path.join(root, "_source.html"), route: "/", out: "Pages/Index.cshtml" },
  { src: "pages/roof-insurance-claim-charlotte-nc.html", route: "/roof-insurance-claim-charlotte-nc", out: "Pages/RoofInsuranceClaimCharlotteNc.cshtml" },
  { src: "pages/es-reclamos-de-seguro-de-techo-charlotte-nc.html", route: "/es/reclamos-de-seguro-de-techo-charlotte-nc", out: "Pages/Es/ReclamosDeSeguroDeTechoCharlotteNc.cshtml" },
  { src: "pages/roof-adjuster-visit-guide-charlotte-nc.html", route: "/roof-adjuster-visit-guide-charlotte-nc", out: "Pages/RoofAdjusterVisitGuideCharlotteNc.cshtml" },
  { src: "pages/es-guia-visita-adjuster-techo-charlotte-nc.html", route: "/es/guia-visita-adjuster-techo-charlotte-nc", out: "Pages/Es/GuiaVisitaAdjusterTechoCharlotteNc.cshtml" },
  { src: "pages/roof-insurance-supplements-charlotte-nc.html", route: "/roof-insurance-supplements-charlotte-nc", out: "Pages/RoofInsuranceSupplementsCharlotteNc.cshtml" },
  { src: "pages/es-suplementos-de-reclamos-de-techo-charlotte-nc.html", route: "/es/suplementos-de-reclamos-de-techo-charlotte-nc", out: "Pages/Es/SuplementosDeReclamosDeTechoCharlotteNc.cshtml" },
  { src: "pages/roofing-charlotte-nc.html", route: "/roofing-charlotte-nc", out: "Pages/RoofingCharlotteNc.cshtml" },
  { src: "pages/es-roofing-charlotte-nc.html", route: "/es/roofing-charlotte-nc", out: "Pages/Es/RoofingCharlotteNc.cshtml" },
  { src: "pages/roofing-rock-hill-sc.html", route: "/roofing-rock-hill-sc", out: "Pages/RoofingRockHillSc.cshtml" },
  { src: "pages/es-roofing-rock-hill-sc.html", route: "/es/roofing-rock-hill-sc", out: "Pages/Es/RoofingRockHillSc.cshtml" },
  { src: "pages/tree-roof-damage-charlotte-nc.html", route: "/tree-roof-damage-charlotte-nc", out: "Pages/TreeRoofDamageCharlotteNc.cshtml" },
  { src: "pages/es-danos-de-arbol-en-techo-charlotte-nc.html", route: "/es/danos-de-arbol-en-techo-charlotte-nc", out: "Pages/Es/DanosDeArbolEnTechoCharlotteNc.cshtml" },
  { src: "pages/active-roof-leak-repair-charlotte-nc.html", route: "/active-roof-leak-repair-charlotte-nc", out: "Pages/ActiveRoofLeakRepairCharlotteNc.cshtml" },
  { src: "pages/es-reparacion-de-liqueo-de-techo-charlotte-nc.html", route: "/es/reparacion-de-liqueo-de-techo-charlotte-nc", out: "Pages/Es/ReparacionDeLiqueoDeTechoCharlotteNc.cshtml" },
  { src: "pages/chimney-flashing-leak-repair-charlotte-nc.html", route: "/chimney-flashing-leak-repair-charlotte-nc", out: "Pages/ChimneyFlashingLeakRepairCharlotteNc.cshtml" },
  { src: "pages/es-reparacion-de-flashing-de-chimenea-charlotte-nc.html", route: "/es/reparacion-de-flashing-de-chimenea-charlotte-nc", out: "Pages/Es/ReparacionDeFlashingDeChimeneaCharlotteNc.cshtml" },
  { src: "pages/wind-missing-shingles-repair-charlotte-nc.html", route: "/wind-missing-shingles-repair-charlotte-nc", out: "Pages/WindMissingShinglesRepairCharlotteNc.cshtml" },
  { src: "pages/es-reparacion-de-shingles-por-viento-charlotte-nc.html", route: "/es/reparacion-de-shingles-por-viento-charlotte-nc", out: "Pages/Es/ReparacionDeShinglesPorVientoCharlotteNc.cshtml" },
  { src: "pages/hail-damage-roof-inspection-charlotte-nc.html", route: "/hail-damage-roof-inspection-charlotte-nc", out: "Pages/HailDamageRoofInspectionCharlotteNc.cshtml" },
  { src: "pages/es-inspeccion-de-dano-de-granizo-charlotte-nc.html", route: "/es/inspeccion-de-dano-de-granizo-charlotte-nc", out: "Pages/Es/InspeccionDeDanoDeGranizoCharlotteNc.cshtml" },
  { src: "pages/open-roof-hole-decking-repair-charlotte-nc.html", route: "/open-roof-hole-decking-repair-charlotte-nc", out: "Pages/OpenRoofHoleDeckingRepairCharlotteNc.cshtml" },
  { src: "pages/es-reparacion-de-agujero-y-decking-charlotte-nc.html", route: "/es/reparacion-de-agujero-y-decking-charlotte-nc", out: "Pages/Es/ReparacionDeAgujeroYDeckingCharlotteNc.cshtml" },
  { src: "pages/roofing-systems-charlotte-nc.html", route: "/roofing-systems-charlotte-nc", out: "Pages/RoofingSystemsCharlotteNc.cshtml" },
  { src: "pages/es-sistemas-de-techo-charlotte-nc.html", route: "/es/sistemas-de-techo-charlotte-nc", out: "Pages/Es/SistemasDeTechoCharlotteNc.cshtml" },
  { src: "pages/roof-warranty-transfer.html", route: "/roof-warranty-transfer", out: "Pages/RoofWarrantyTransfer.cshtml" },
  { src: "pages/es-transferencia-de-garantia-de-techo.html", route: "/es/transferencia-de-garantia-de-techo", out: "Pages/Es/TransferenciaDeGarantiaDeTecho.cshtml" },
];

function decodeHtml(s) {
  return s
    .replace(/&amp;/g, "&")
    .replace(/&lt;/g, "<")
    .replace(/&gt;/g, ">")
    .replace(/&quot;/g, '"')
    .replace(/&#39;/g, "'");
}

function razorEscape(html) {
  return html
    .replace(/<!-- -->/g, "")
    .replace(/@/g, "@@");
}

function extract(html) {
  const title = decodeHtml((html.match(/<title>(.*?)<\/title>/s) || [, "Majestic Roofing"])[1]);
  const desc = decodeHtml((html.match(/name="description" content="([^"]*)"/) || [, ""])[1]);
  let ogImage = (html.match(/property="og:image" content="([^"]*)"/) || [, "/projects/roof-project-11.jpeg"])[1];
  ogImage = ogImage.replace("https://majestic-roofing.jrricardo29.chatgpt.site", "");
  if (!ogImage.startsWith("/")) ogImage = "/projects/roof-project-11.jpeg";
  const lang = (html.match(/<html lang="([^"]+)"/) || html.match(/<main[^>]*lang="([^"]+)"/) || [, "en"])[1];
  const mainMatch = html.match(/<main[\s\S]*?<\/main>/);
  if (!mainMatch) throw new Error("No <main> found");
  return { title, desc, ogImage, lang, main: mainMatch[0] };
}

function ensureDir(p) {
  fs.mkdirSync(path.dirname(p), { recursive: true });
}

function copyDir(src, dest) {
  fs.mkdirSync(dest, { recursive: true });
  for (const entry of fs.readdirSync(src, { withFileTypes: true })) {
    const s = path.join(src, entry.name);
    const d = path.join(dest, entry.name);
    if (entry.isDirectory()) copyDir(s, d);
    else fs.copyFileSync(s, d);
  }
}

fs.mkdirSync(path.join(www, "css"), { recursive: true });
fs.mkdirSync(path.join(www, "js"), { recursive: true });
fs.mkdirSync(pagesDir, { recursive: true });
fs.mkdirSync(path.join(pagesDir, "Shared"), { recursive: true });
fs.mkdirSync(path.join(pagesDir, "Es"), { recursive: true });
fs.mkdirSync(path.join(root, "Properties"), { recursive: true });

fs.copyFileSync(path.join(orig, "assets", "index-D1vmm-Sm.css"), path.join(www, "css", "site.css"));
fs.copyFileSync(path.join(orig, "favicon.svg"), path.join(www, "favicon.svg"));
copyDir(path.join(orig, "projects"), path.join(www, "projects"));
copyDir(path.join(orig, "damage-guides"), path.join(www, "damage-guides"));

fs.writeFileSync(
  path.join(root, "MajesticRoofing.csproj"),
  `<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <RootNamespace>MajesticRoofing</RootNamespace>
  </PropertyGroup>
</Project>
`
);

fs.writeFileSync(
  path.join(root, "Program.cs"),
  `var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();
`
);

fs.writeFileSync(
  path.join(root, "appsettings.json"),
  `{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
`
);

fs.writeFileSync(
  path.join(root, "Properties", "launchSettings.json"),
  `{
  "profiles": {
    "MajesticRoofing": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "http://localhost:5080",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
`
);

fs.writeFileSync(
  path.join(pagesDir, "_ViewImports.cshtml"),
  `@namespace MajesticRoofing.Pages
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
`
);

fs.writeFileSync(
  path.join(pagesDir, "_ViewStart.cshtml"),
  `@{
    Layout = "_Layout";
}
`
);

fs.writeFileSync(
  path.join(pagesDir, "Shared", "_Layout.cshtml"),
  `<!DOCTYPE html>
<html lang="@(ViewData["Lang"] ?? "en")">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>@ViewData["Title"]</title>
    <meta name="description" content="@ViewData["Description"]" />
    <meta property="og:title" content="@ViewData["Title"]" />
    <meta property="og:description" content="@ViewData["Description"]" />
    <meta property="og:type" content="website" />
    <meta property="og:locale" content="@((ViewData["Lang"] as string) == "es" ? "es_US" : "en_US")" />
    <meta property="og:image" content="@(ViewData["OgImage"] ?? "/projects/roof-project-11.jpeg")" />
    <meta name="twitter:card" content="summary_large_image" />
    <meta name="twitter:title" content="@ViewData["Title"]" />
    <meta name="twitter:description" content="@ViewData["Description"]" />
    <meta name="twitter:image" content="@(ViewData["OgImage"] ?? "/projects/roof-project-11.jpeg")" />
    <link rel="shortcut icon" href="/favicon.svg" />
    <link rel="icon" href="/favicon.svg" />
    <link rel="stylesheet" href="/css/site.css" />
</head>
<body>
    @RenderBody()
    <script src="/js/site.js" defer></script>
</body>
</html>
`
);

for (const page of pageMap) {
  const srcPath = path.isAbsolute(page.src) ? page.src : path.join(orig, page.src);
  const html = fs.readFileSync(srcPath, "utf8");
  const meta = extract(html);
  const body = razorEscape(meta.main);
  const titleLit = meta.title.replace(/"/g, '""');
  const descLit = meta.desc.replace(/"/g, '""');
  const razor = `@page "${page.route}"
@{
    ViewData["Title"] = "${titleLit}";
    ViewData["Description"] = "${descLit}";
    ViewData["OgImage"] = "${meta.ogImage}";
    ViewData["Lang"] = "${meta.lang}";
}
${body}
`;
  const outPath = path.join(root, page.out);
  ensureDir(outPath);
  fs.writeFileSync(outPath, razor, "utf8");
  console.log("wrote", page.out, body.length);
}

console.log("pages generated:", pageMap.length);
