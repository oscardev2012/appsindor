import fs from "fs";
import path from "path";
import { fileURLToPath } from "url";

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
const i18n = JSON.parse(fs.readFileSync(path.join(root, "wwwroot/js/i18n.json"), "utf8"));
let html = fs.readFileSync(path.join(root, "Pages/Index.cshtml"), "utf8");

const extras = [
  ["Skip to content", "Ir al contenido"],
  ["Primary navigation", "Navegación principal"],
  ["Roofing help options", "Opciones de ayuda"],
  ["Free storm inspection", "Inspección de tormenta gratis"],
  ["Claims documentation · same-day response", "Documentación · respuesta el mismo día"],
  ["$350 Roof Tune-Up", "Roof Tune-Up $350"],
  ["Annual general roof maintenance", "Mantenimiento general anual"],
  ["Plan a replacement", "Planear reemplazo"],
  ["Private price range, no form first", "Rango de precio sin formulario primero"],
  ["Start with the service that matches your situation. Majestic will confirm the actual scope after reviewing the property.", "Comience con el servicio que coincide con su situación. Majestic confirma el alcance real después de revisar la propiedad."],
  ["REAL PROJECT PHOTOGRAPHY", "FOTOGRAFÍA DE PROYECTO REAL"],
  ["Architectural shingle roofing", "Techos de shingles arquitectónicos"],
  ["See the roof system—not just a color sample.", "Vea el sistema del techo, no solamente una muestra de color."],
  ["Architectural asphalt shingles create the dimensional pattern visible in these projects. The final roof is more than the exposed shingle: underlayment, flashing, valleys, ventilation, starter and ridge components all affect performance. Majestic confirms the selected manufacturer, product line, color and written scope before installation.", "Los shingles asfálticos arquitectónicos crean el patrón dimensional visible en estos proyectos. El techo terminado es más que el shingle expuesto: underlayment, flashing, valles, ventilación, starter y ridge también afectan su funcionamiento. Majestic confirma fabricante, línea, color y alcance escrito antes de instalar."],
  ["Dimensional shingle appearance", "Apariencia dimensional"],
  ["Complete roof-system planning", "Planificación del sistema completo"],
  ["Final selection confirmed in writing", "Selección final confirmada por escrito"],
  ["Plan a shingle roof", "Planear techo de shingles"],
  ["Compare roof systems", "Comparar sistemas"],
  ["No contact information required to see the range.", "No necesita dejar datos para ver el rango."],
  ["Adjusts for stories, roof shape, slope, material, and tear-off.", "Se ajusta por niveles, forma, inclinación, material y remoción."],
  ["A field inspection makes it exact.", "Una inspección en sitio lo hace exacto."],
  ["Your planning range", "Su rango de planificación"],
  ["Finished home size (sq ft)", "Tamaño habitable de la casa (pies²)"],
  ["Use the living-area number from your listing or tax record. Do not include the garage unless it is finished living space.", "Use el área habitable de su listing o registro de impuestos. No incluya el garaje si no es espacio habitable."],
  ["1 story", "1 nivel"],
  ["2 stories", "2 niveles"],
  ["3+ stories", "3+ niveles"],
  ["Roof material", "Material del techo"],
  ["Architectural shingles", "Shingles arquitectónicos"],
  ["Premium shingles", "Shingles premium"],
  ["Metal roofing", "Metal roofing"],
  ["Roof shape", "Forma del techo"],
  ["Simple / gable", "Simple / dos aguas"],
  ["Standard / hips & valleys", "Estándar / hips y valles"],
  ["Complex / many cutups", "Complejo / muchos cortes"],
  ["Include standard existing-roof tear-off", "Incluir remoción estándar del techo existente"],
  ["How steep is your roof? (roof pitch)", "¿Qué tan inclinado es su techo? (roof pitch)"],
  ["Pitch means how many inches the roof rises for every 12 inches it runs horizontally. If you are unsure, choose the picture that looks closest; Majestic will verify it.", "Pitch significa cuántas pulgadas sube el techo por cada 12 pulgadas horizontales. Si no está seguro, elija el dibujo más parecido; Majestic lo verificará."],
  ["Low / gentle", "Bajo / suave"],
  ["Under 6/12 · looks flatter", "Menos de 6/12 · se ve más plano"],
  ["6/12–8/12 · common home slope", "6/12–8/12 · inclinación común"],
  ["9/12+ · sharp angle, extra safety", "9/12+ · ángulo fuerte, seguridad adicional"],
  ["Protect the roof you have", "Proteja el techo que tiene"],
  ["annual Roof Tune-Up", "Roof Tune-Up anual"],
  ["A general maintenance visit for accessible roof areas, built to identify visible concerns and renew exposed roof sealing details before a small issue becomes a larger project.", "Una visita de mantenimiento general para zonas accesibles, pensada para identificar problemas visibles y renovar sellos expuestos antes de que un detalle pequeño se convierta en un proyecto mayor."],
  ["Accessible roof review", "Revisión de áreas accesibles"],
  ["Sealant maintenance at exposed fasteners and accessible penetrations", "Mantenimiento de sellador en clavos y penetraciones accesibles"],
  ["Chimney water-entry detail review", "Revisión de detalles de agua en chimenea"],
  ["Visible-concern recommendations", "Recomendaciones sobre problemas visibles"],
  ["Request a Tune-Up", "Solicitar Tune-Up"],
  ["Keep water out while planning the next step", "Evite que entre agua mientras planeamos el próximo paso"],
  ["emergency tarp up to 20 × 20 ft", "carpeta de emergencia hasta 20 × 20 pies"],
  ["A safe-site emergency tarp option for active roof damage. A larger standard tarp is $150; larger, multiple, or unusual installations are confirmed before work begins.", "Una opción de carpeta de emergencia para daño activo, siempre que el sitio sea seguro. Una carpeta estándar más grande cuesta $150; instalaciones grandes, múltiples o inusuales se confirman antes de comenzar."],
  ["Safe site assessment before installation", "Evaluación de seguridad antes de instalar"],
  ["Tarp installation for temporary water protection", "Instalación temporal para proteger contra agua"],
  ["Further repair or replacement can be scoped separately", "La reparación o reemplazo posterior se cotiza por separado"],
  ["Call for emergency help", "Llamar para emergencia"],
  ["Is this a roof emergency?", "¿Es una emergencia de techo?"],
  ["Choose the problem to see the first steps, our process and repair options.", "Seleccione el problema para ver primeros pasos, nuestro proceso y opciones de reparación."],
  ["Do not climb onto a wet, damaged or unstable roof. If anyone is in immediate danger, call 911 first. Emergency work depends on safe access and current weather.", "No se suba a un techo mojado, dañado o inestable. Si alguien está en peligro inmediato, llame primero al 911. El trabajo de emergencia depende del acceso seguro y del clima."],
  ["Tree or debris impact", "Árbol o escombros"],
  ["A limb or tree struck the roof, decking or structure.", "Una rama o árbol golpeó el techo, decking o estructura."],
  ["Active leak", "Liqueo activo"],
  ["Water is entering through the roof, ceiling or attic.", "Está entrando agua por el techo, cielo o attic."],
  ["Chimney or flashing leak", "Chimenea o flashing"],
  ["Water appears near a chimney, wall transition or flashing.", "El agua aparece cerca de chimenea, pared o flashing."],
  ["Wind or missing shingles", "Viento o shingles faltantes"],
  ["Shingles lifted, tore, slid or disappeared after wind.", "Los shingles se levantaron, rompieron, deslizaron o desaparecieron."],
  ["Hail or storm impact", "Granizo o tormenta"],
  ["Possible impacts, granule loss or widespread storm concerns.", "Posibles impactos, pérdida de gránulos o daño extendido."],
  ["Open hole or exposed decking", "Agujero o decking expuesto"],
  ["The roof covering is open and temporary protection may be needed.", "La cubierta está abierta y puede necesitar protección temporal."],
  ["A contractor documents the roof. Your carrier decides coverage.", "El contratista documenta el techo. Su aseguradora decide la cobertura."],
  ["If conditions are safe, temporary tarping can help limit additional water entry.", "Si las condiciones son seguras, una carpeta temporal puede ayudar a limitar la entrada adicional de agua."],
  ["Review visible roof and exterior conditions without promising a claim result.", "Revisar condiciones visibles del techo y exterior sin prometer el resultado del reclamo."],
  ["Organize photos, measurements, roof findings and the contractor scope.", "Organizar fotos, medidas, hallazgos y el alcance del contratista."],
  ["Meet at the property with the adjuster when requested and authorized.", "Reunirse en la propiedad con el adjuster cuando sea solicitado y autorizado."],
  ["Confirm the written construction scope, schedule the work and document completion.", "Confirmar el alcance escrito, agendar el trabajo y documentar la terminación."],
  ["Open the complete claim guide", "Abrir la guía completa"],
  ["Read the NC DOI claims guide ↗", "Leer la guía de reclamos de NC DOI ↗"],
  ["Built for Charlotte weather", "Preparado para el clima de Charlotte"],
  ["Local readiness before the next hard rain.", "Preparación local antes de la próxima lluvia fuerte."],
  ["Charlotte–Mecklenburg planning resources identify hurricanes, tornadoes, flooding, power outages and downed trees among the hazards local properties may face. Majestic’s role is the roof: prevention, temporary protection when safe, clear documentation and an honest repair-or-replace plan.", "Los recursos de Charlotte-Mecklenburg identifican huracanes, tornados, inundaciones, apagones y árboles caídos entre los riesgos que pueden afectar propiedades locales. El papel de Majestic es el techo: prevención, protección temporal cuando sea seguro, documentación clara y un plan honesto de reparación o reemplazo."],
  ["OFFICIAL LOCAL RESOURCE", "RECURSO LOCAL OFICIAL"],
  ["Charlotte emergency preparation", "Preparación de emergencias de Charlotte"],
  ["Get local weather alerts", "Reciba alertas meteorológicas locales"],
  ["per month", "por mes"],
  ["Confirm my address", "Confirmar mi dirección"],
  ["These images show completed roof surfaces and geometry. They do not identify a manufacturer, product line, warranty or claim outcome.", "Estas imágenes muestran superficies y geometría terminadas. No identifican fabricante, línea de producto, garantía ni resultado de reclamo."],
  ["More Majestic project updates on Instagram", "Más actualizaciones de Majestic en Instagram"],
  ["Shingle roof project", "Proyecto de shingles"],
  ["CALL MAJESTIC", "LLAME A MAJESTIC"],
  ["Your name", "Su nombre"],
  ["Property address", "Dirección de la propiedad"],
  ["Street, city, ZIP", "Calle, ciudad, ZIP"],
  ["What service do you need?", "¿Qué servicio necesita?"],
  ["Roof inspection", "Inspección de techo"],
  ["Roof replacement", "Reemplazo de techo"],
  ["Roof repair", "Reparación de techo"],
  ["Storm damage documentation", "Documentación de tormenta"],
  ["Emergency tarp", "Carpeta de emergencia"],
  ["Commercial roofing", "Roofing comercial"],
  ["What happened?", "¿Qué pasó?"],
  ["Not sure — please inspect", "No estoy seguro — inspeccionar"],
  ["Active roof leak", "Liqueo activo"],
  ["Tree / debris hit the roof", "Árbol / escombros golpearon techo"],
  ["Chimney / flashing problem", "Problema de chimenea / flashing"],
  ["Wind / missing shingles", "Viento / shingles faltantes"],
  ["Hail / storm concern", "Granizo / tormenta"],
  ["Hole / exposed roof area", "Agujero / área expuesta"],
  ["Old or worn roof", "Techo antiguo o desgastado"],
  ["Project details", "Detalles del proyecto"],
  ["What changed? When did you notice it? Is water entering now?", "¿Qué cambió? ¿Cuándo lo notó? ¿Está entrando agua ahora?"],
  ["roof insurance claim documentation", "documentación de reclamos de techo"],
  ["Charlotte, NC · Rock Hill, SC · Coverage confirmed by address", "Charlotte, NC · Rock Hill, SC · Cobertura confirmada por dirección"],
  ["Stories", "Niveles"],
  ["Standard", "Estándar"],
  ["Steep", "Inclinado"],
  ["Protect", "Proteger"],
  ["Inspect", "Inspeccionar"],
  ["Document", "Documentar"],
  ["Coordinate", "Coordinar"],
  ["Build", "Construir"],
  ["Phone", "Teléfono"],
  ["EMAIL", "CORREO"],
];

const hrefs = [
  ["/roof-insurance-claim-charlotte-nc", "/es/reclamos-de-seguro-de-techo-charlotte-nc"],
  ["/roofing-systems-charlotte-nc", "/es/sistemas-de-techo-charlotte-nc"],
  ["/tree-roof-damage-charlotte-nc", "/es/danos-de-arbol-en-techo-charlotte-nc"],
  ["/active-roof-leak-repair-charlotte-nc", "/es/reparacion-de-liqueo-de-techo-charlotte-nc"],
  ["/chimney-flashing-leak-repair-charlotte-nc", "/es/reparacion-de-flashing-de-chimenea-charlotte-nc"],
  ["/wind-missing-shingles-repair-charlotte-nc", "/es/reparacion-de-shingles-por-viento-charlotte-nc"],
  ["/hail-damage-roof-inspection-charlotte-nc", "/es/inspeccion-de-dano-de-granizo-charlotte-nc"],
  ["/open-roof-hole-decking-repair-charlotte-nc", "/es/reparacion-de-agujero-y-decking-charlotte-nc"],
  ["/roofing-charlotte-nc", "/es/roofing-charlotte-nc"],
  ["/roofing-rock-hill-sc", "/es/roofing-rock-hill-sc"],
];

function walk(obj, lang) {
  const out = [];
  if (typeof obj === "string") return out;
  if (Array.isArray(obj)) {
    obj.forEach((item) => out.push(...walk(item, lang)));
    return out;
  }
  for (const [k, v] of Object.entries(obj)) {
    if (k === "en" || k === "es") continue;
    if (v && typeof v === "object" && "en" in Object(i18n.u) === false) {
      out.push(...walk(v, lang));
    }
  }
  return out;
}

const pairs = [];
function collect(enNode, esNode) {
  if (typeof enNode === "string" && typeof esNode === "string" && enNode !== esNode) {
    pairs.push([enNode, esNode]);
    return;
  }
  if (Array.isArray(enNode) && Array.isArray(esNode)) {
    enNode.forEach((item, i) => collect(item, esNode[i]));
    return;
  }
  if (enNode && esNode && typeof enNode === "object") {
    for (const key of Object.keys(enNode)) collect(enNode[key], esNode[key]);
  }
}
collect(i18n.u.en, i18n.u.es);
collect(i18n.d.en, i18n.d.es);
collect(i18n.f.en, i18n.f.es);
collect(i18n.p.en, i18n.p.es);
pairs.push(...extras);

pairs.sort((a, b) => b[0].length - a[0].length);
for (const [en, es] of pairs) {
  if (!en) continue;
  html = html.split(en).join(es);
}

for (const [en, es] of hrefs) {
  html = html.split(`href="${en}"`).join(`href="${es}"`);
}

html = html.replace('@page "/"', '@page "/es"');
html = html.replace(
  'ViewData["Title"] = "Majestic Roofing | Roof Insurance Claims Charlotte &amp; Rock Hill";',
  'ViewData["Title"] = "Majestic Roofing | Reclamos de seguro de techo Charlotte y Rock Hill";'
);
html = html.replace(
  'ViewData["Title"] = "Majestic Roofing | Roof Insurance Claims Charlotte & Rock Hill";',
  'ViewData["Title"] = "Majestic Roofing | Reclamos de seguro de techo Charlotte y Rock Hill";'
);
html = html.replace('ViewData["Lang"] = "en";', 'ViewData["Lang"] = "es";');
html = html.replace('<main lang="en">', '<main lang="es">');
html = html.replace('aria-label="Language"', 'aria-label="Idioma"');
html = html.replace(
  'class="lang-option active" type="button" data-lang="en" aria-pressed="true"',
  'class="lang-option" type="button" data-lang="en" aria-pressed="false"'
);
html = html.replace(
  'class="lang-option" type="button" data-lang="es" aria-pressed="false"',
  'class="lang-option active" type="button" data-lang="es" aria-pressed="true"'
);
html = html.replace('href="#top"', 'href="/es"');
html = html.replace(
  'ViewData["Description"] = "Bilingual roof insurance claim documentation, free storm inspections, roof replacement, repairs, emergency tarping, gutters and fascia across Charlotte, NC and Rock Hill, SC.";',
  'ViewData["Description"] = "Documentación bilingüe de reclamos de techo, inspecciones gratuitas de tormenta, reemplazo, reparaciones, carpetas de emergencia, gutters y fascia en Charlotte, NC y Rock Hill, SC.";'
);

const out = path.join(root, "Pages/Es/Index.cshtml");
fs.writeFileSync(out, html);
console.log("wrote", out, html.length);
