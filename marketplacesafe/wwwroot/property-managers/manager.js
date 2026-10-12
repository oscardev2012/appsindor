const workOrders = [
  [
    "Turnover",
    "Move-out inspection / Inspección de salida",
    "$100",
    "$195",
    "$350+",
  ],
  [
    "Turnover",
    "Make-ready punch list / Lista make-ready",
    "$150",
    "$450",
    "$1,500+",
  ],
  ["Turnover", "Lock change / Cambio de cerradura", "$150", "$250", "$600+"],
  [
    "Turnover",
    "Interior door repair / Reparar puerta",
    "$125",
    "$250",
    "$900+",
  ],
  [
    "Turnover",
    "Blind replacement / Reemplazo de blinds",
    "$125",
    "$225",
    "$500+",
  ],
  ["Turnover", "Smoke/CO detector / Detector humo/CO", "$75", "$175", "$350+"],
  ["Turnover", "Appliance hookup / Conectar aparato", "$150", "$300", "$750+"],
  ["Turnover", "Trash-out / Retiro de basura", "$250", "$650", "$3,500+"],
  [
    "Cleaning",
    "Standard unit cleaning / Limpieza estándar",
    "$150",
    "$275",
    "$500+",
  ],
  [
    "Cleaning",
    "Deep/turnover cleaning / Limpieza profunda",
    "$300",
    "$550",
    "$900+",
  ],
  [
    "Cleaning",
    "Carpet cleaning / Limpieza de alfombra",
    "$150",
    "$300",
    "$650+",
  ],
  [
    "Cleaning",
    "Pressure washing / Lavado a presión",
    "$250",
    "$500",
    "$1,200+",
  ],
  [
    "Cleaning",
    "Common-area cleaning / Áreas comunes",
    "$250",
    "$650",
    "$1,500+",
  ],
  ["Cleaning", "Lawn service / Servicio de lawn", "$50", "$95", "$250+"],
  ["Interior", "Drywall patch / Parche drywall", "$150", "$350", "$700+"],
  [
    "Interior",
    "Drywall ceiling repair / Reparar cielo",
    "$250",
    "$600",
    "$1,500+",
  ],
  ["Interior", "Room painting / Pintar cuarto", "$500", "$900", "$2,000+"],
  [
    "Interior",
    "Full unit painting / Pintar unidad",
    "$1,800",
    "$3,500",
    "$7,500+",
  ],
  [
    "Interior",
    "LVP installed / LVP instalado",
    "$5.50/sq.ft.",
    "$6.50/sq.ft.",
    "$9+/sq.ft.",
  ],
  [
    "Interior",
    "Floor removal / Retirar piso",
    "$1/sq.ft.",
    "$3/sq.ft.",
    "$8+/sq.ft.",
  ],
  [
    "Interior",
    "Subfloor repair / Reparar subfloor",
    "$400",
    "$1,200",
    "$4,500+",
  ],
  ["Interior", "Cabinet/hardware repair / Gabinete", "$150", "$450", "$1,500+"],
  [
    "Interior",
    "Countertop replacement / Countertop",
    "$35/sq.ft.",
    "$75/sq.ft.",
    "$130+/sq.ft.",
  ],
  ["Interior", "Tile/grout repair / Tile y grout", "$250", "$650", "$2,500+"],
  ["Plumbing", "Plumbing diagnostic / Diagnóstico", "$100", "$175", "$250+"],
  ["Plumbing", "Toilet repair / Reparar toilet", "$150", "$275", "$450+"],
  [
    "Plumbing",
    "Toilet replacement / Reemplazar toilet",
    "$450",
    "$750",
    "$1,200+",
  ],
  [
    "Plumbing",
    "Faucet replacement / Reemplazar faucet",
    "$150",
    "$300",
    "$750+",
  ],
  ["Plumbing", "Garbage disposal / Disposal", "$450", "$550", "$650+"],
  ["Plumbing", "Drain cleaning / Destapar drenaje", "$150", "$325", "$650+"],
  [
    "Plumbing",
    "Accessible pipe repair / Tubería accesible",
    "$200",
    "$650",
    "$1,500+",
  ],
  ["Plumbing", "Sewer camera / Cámara sewer", "$200", "$350", "$500+"],
  [
    "Plumbing",
    "Electric water heater / Eléctrico",
    "$1,199",
    "$1,299",
    "$1,649+",
  ],
  ["Plumbing", "Gas water heater / Gas", "$1,449", "$1,499", "$1,849+"],
  [
    "Electrical",
    "Electrical diagnostic / Diagnóstico",
    "$100",
    "$185",
    "$300+",
  ],
  ["Electrical", "Switch/outlet replacement / Outlet", "$150", "$225", "$450+"],
  ["Electrical", "GFCI replacement / GFCI", "$150", "$250", "$450+"],
  ["Electrical", "Light fixture / Lámpara", "$150", "$300", "$675+"],
  [
    "Electrical",
    "Dedicated circuit / Circuito dedicado",
    "$500",
    "$1,200",
    "$3,000+",
  ],
  ["Electrical", "Breaker replacement / Breaker", "$200", "$400", "$650+"],
  [
    "Electrical",
    "Panel replacement / Panel eléctrico",
    "$2,500",
    "$4,500",
    "$8,500+",
  ],
  ["HVAC", "HVAC diagnostic / Diagnóstico HVAC", "$100", "$100", "$200+"],
  ["HVAC", "Thermostat replacement / Termostato", "$200", "$400", "$700+"],
  ["HVAC", "Duct repair up to 8 in. / Reparar ducto", "$200", "$400", "$800+"],
  [
    "HVAC",
    "Duct replacement up to 8 in. / Ducto nuevo",
    "$300",
    "$600",
    "$1,200+",
  ],
  ["HVAC", "Capacitor/contactor repair", "$250", "$450", "$650+"],
  ["HVAC", "Blower motor / Motor blower", "$650", "$1,200", "$2,500+"],
  [
    "HVAC",
    "Complete system change-out / Sistema completo",
    "$6,399",
    "$6,799",
    "$10,000+",
  ],
  ["Exterior", "Roof leak repair / Fuga de roof", "$350", "$900", "$2,500+"],
  ["Exterior", "Roof pipe boot / Pipe boot", "$350", "$550", "$750+"],
  ["Exterior", "Gutter cleaning / Limpiar gutters", "$100", "$225", "$450+"],
  [
    "Exterior",
    "Gutter replacement / Gutters nuevos",
    "$8/ft.",
    "$12/ft.",
    "$16+/ft.",
  ],
  [
    "Exterior",
    "Siding/trim repair / Siding o trim",
    "$300",
    "$1,200",
    "$5,000+",
  ],
  ["Exterior", "Fence repair / Reparar fence", "$250", "$600", "$2,500+"],
  ["Exterior", "Deck repair / Reparar deck", "$250", "$900", "$4,000+"],
  ["Exterior", "Window replacement / Ventana", "$650", "$1,200", "$3,500+"],
  [
    "Exterior",
    "Exterior door / Puerta exterior",
    "$1,200",
    "$2,500",
    "$7,500+",
  ],
  [
    "Projects",
    "Bathroom refresh / Renovar baño",
    "$4,500",
    "$10,000",
    "$30,000+",
  ],
  [
    "Projects",
    "Kitchen refresh / Renovar cocina",
    "$7,500",
    "$18,000",
    "$60,000+",
  ],
  [
    "Projects",
    "Concrete work / Concrete",
    "$9/sq.ft.",
    "$13/sq.ft.",
    "$16+/sq.ft.",
  ],
  ["Projects", "Crawl-space makeover", "$3,500", "$3,500", "$8,000+"],
  [
    "Projects",
    "Structural wood repair / Madera estructural",
    "$500",
    "$2,500",
    "$15,000+",
  ],
  [
    "Projects",
    "Inspection-report scope / Reporte inspección",
    "$195",
    "$350",
    "$750+",
  ],
];
let lang = localStorage.getItem("safeLanguage") || "en";
const rows = document.getElementById("pmRows"),
  search = document.getElementById("pmSearch"),
  category = document.getElementById("pmCategory");
function render() {
  const q = search.value.toLowerCase().trim(),
    c = category.value;
  const found = workOrders.filter(
    (x) =>
      (c === "all" || x[0] === c) &&
      (!q || x.join(" ").toLowerCase().includes(q)),
  );
  document.getElementById("pmCount").textContent =
    lang === "es"
      ? `${found.length} servicios encontrados`
      : `${found.length} services found`;
  rows.innerHTML = found.length
    ? found
        .map(
          (x) =>
            `<tr><td><small>${x[0]}</small><strong>${x[1]}</strong></td><td data-label="${lang === "es" ? "Inicial" : "Starting"}">${x[2]}</td><td data-label="${lang === "es" ? "Típico" : "Typical"}">${x[3]}</td><td data-label="${lang === "es" ? "Complejo" : "Complex"}">${x[4]}</td><td><button data-service="${x[1]}">${lang === "es" ? "Pedir precio" : "Request price"}</button></td></tr>`,
        )
        .join("")
    : `<tr><td class="pm-empty" colspan="5">${lang === "es" ? "No encontramos ese servicio. Envíe su orden y le ayudamos." : "No exact service found. Send the work order and we will help."}</td></tr>`;
  rows
    .querySelectorAll("button")
    .forEach(
      (b) =>
        (b.onclick = () =>
          (location.href = `sms:+16506696890?&body=${encodeURIComponent("Property management work order: " + b.dataset.service + " | Property/unit: | Priority: ")}`)),
    );
}
function language() {
  document.documentElement.lang = lang;
  document
    .querySelectorAll("[data-en]")
    .forEach((e) => (e.textContent = e.dataset[lang]));
  search.placeholder =
    lang === "es"
      ? "Busque: turnover, toilet, pintura, HVAC, roof…"
      : "Search: turnover, toilet, paint, HVAC, roof…";
  document.getElementById("pmLanguage").textContent =
    lang === "en" ? "ES" : "EN";
  render();
}
search.oninput = render;
category.onchange = render;
document.getElementById("pmLanguage").onclick = () => {
  lang = lang === "en" ? "es" : "en";
  localStorage.setItem("safeLanguage", lang);
  language();
};
language();
