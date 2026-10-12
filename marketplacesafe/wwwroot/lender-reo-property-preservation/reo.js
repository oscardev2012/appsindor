const workOrders = [
  [
    "Closing",
    "Contractor scope from lender/appraisal condition / Alcance requerido",
    "$195",
    "$350",
    "$750+",
  ],
  [
    "Closing",
    "Repair estimate package / Paquete de estimado",
    "$195",
    "$450",
    "$1,000+",
  ],
  [
    "Closing",
    "Closing repair mobilization / Movilización urgente",
    "$250",
    "$500",
    "$1,200+",
  ],
  [
    "Closing",
    "WDIR-related wood repair / Reparación relacionada con WDIR",
    "$500",
    "$2,500",
    "$15,000+",
  ],
  [
    "Closing",
    "Roof certification-related repair / Reparación de roof",
    "$350",
    "$1,500",
    "$8,000+",
  ],
  [
    "Closing",
    "Safety or code repair package / Correcciones de seguridad",
    "$500",
    "$2,000",
    "$10,000+",
  ],
  [
    "Secure",
    "Rekey or lock change / Rekey o cerradura",
    "$150",
    "$250",
    "$600+",
  ],
  ["Secure", "Lockbox installation / Instalar lockbox", "$75", "$125", "$225+"],
  [
    "Secure",
    "Initial secure package / Aseguramiento inicial",
    "$250",
    "$650",
    "$1,500+",
  ],
  [
    "Secure",
    "Window or door boarding / Board-up",
    "$150/opening",
    "$350/opening",
    "$900+/opening",
  ],
  ["Secure", "Temporary roof tarp / Tarp temporal", "$350", "$850", "$2,500+"],
  [
    "Secure",
    "Emergency water shutoff / Cierre de agua",
    "$150",
    "$300",
    "$750+",
  ],
  ["Preservation", "Trash-out / Retiro de basura", "$350", "$1,200", "$6,000+"],
  [
    "Preservation",
    "Exterior debris removal / Escombro exterior",
    "$250",
    "$750",
    "$3,500+",
  ],
  [
    "Preservation",
    "Initial lawn service / Lawn inicial",
    "$75",
    "$175",
    "$500+",
  ],
  [
    "Preservation",
    "Recurring lawn service / Lawn recurrente",
    "$50",
    "$95",
    "$250+",
  ],
  ["Preservation", "Winterization / Winterization", "$250", "$450", "$900+"],
  [
    "Preservation",
    "De-winterization / De-winterization",
    "$250",
    "$500",
    "$1,200+",
  ],
  [
    "Preservation",
    "Deep cleaning / Limpieza profunda",
    "$300",
    "$650",
    "$1,500+",
  ],
  [
    "Preservation",
    "Pool safety or basic service / Seguridad de piscina",
    "$250",
    "$600",
    "$2,000+",
  ],
  [
    "Inspection",
    "Occupancy/vacancy field check / Verificación",
    "$75",
    "$125",
    "$250+",
  ],
  [
    "Inspection",
    "Property condition report / Reporte de condición",
    "$125",
    "$250",
    "$600+",
  ],
  [
    "Inspection",
    "Recurring visual inspection / Inspección recurrente",
    "$75",
    "$150",
    "$300+",
  ],
  [
    "Inspection",
    "Damage documentation / Documentación de daños",
    "$150",
    "$350",
    "$900+",
  ],
  [
    "Inspection",
    "Repair completion verification / Verificación final",
    "$125",
    "$250",
    "$500+",
  ],
  ["Repairs", "Plumbing leak repair / Reparar fuga", "$200", "$650", "$1,500+"],
  [
    "Repairs",
    "Electrical safety repair / Reparación eléctrica",
    "$150",
    "$500",
    "$2,500+",
  ],
  [
    "Repairs",
    "HVAC diagnostic and startup / Diagnóstico HVAC",
    "$100",
    "$250",
    "$700+",
  ],
  [
    "Repairs",
    "Water heater replacement / Water heater",
    "$1,199",
    "$1,499",
    "$1,849+",
  ],
  [
    "Repairs",
    "Drywall damage repair / Reparar drywall",
    "$150",
    "$600",
    "$2,000+",
  ],
  [
    "Repairs",
    "Exterior opening repair / Puerta o ventana",
    "$350",
    "$1,200",
    "$5,000+",
  ],
  [
    "Repairs",
    "Crawl-space moisture or wood scope / Crawl space",
    "$500",
    "$3,500",
    "$15,000+",
  ],
  [
    "Market Ready",
    "Make-ready punch list / Lista make-ready",
    "$250",
    "$1,500",
    "$7,500+",
  ],
  [
    "Market Ready",
    "Interior paint / Pintura interior",
    "$1,800",
    "$3,500",
    "$8,000+",
  ],
  [
    "Market Ready",
    "LVP flooring installed / Piso LVP instalado",
    "$5.50/sq.ft.",
    "$6.50/sq.ft.",
    "$9+/sq.ft.",
  ],
  [
    "Market Ready",
    "Kitchen refresh / Renovación de cocina",
    "$7,500",
    "$18,000",
    "$60,000+",
  ],
  [
    "Market Ready",
    "Bathroom refresh / Renovación de baño",
    "$4,500",
    "$10,000",
    "$30,000+",
  ],
  [
    "Market Ready",
    "Full rehab / Rehabilitación completa",
    "$15,000",
    "$45,000",
    "$150,000+",
  ],
  [
    "Market Ready",
    "Final clean and photo closeout / Limpieza final",
    "$250",
    "$550",
    "$1,200+",
  ],
];

let lang = localStorage.getItem("safeLanguage") || "en";
const rows = document.getElementById("reoRows");
const search = document.getElementById("reoSearch");
const category = document.getElementById("reoCategory");

function render() {
  const q = search.value.toLowerCase().trim();
  const c = category.value;
  const found = workOrders.filter(
    (x) =>
      (c === "all" || x[0] === c) &&
      (!q || x.join(" ").toLowerCase().includes(q)),
  );
  document.getElementById("reoCount").textContent =
    lang === "es"
      ? `${found.length} órdenes encontradas`
      : `${found.length} work orders found`;
  rows.innerHTML = found.length
    ? found
        .map(
          (x) =>
            `<tr><td><small>${x[0]}</small><strong>${x[1]}</strong></td><td data-label="${lang === "es" ? "Inicial" : "Starting"}">${x[2]}</td><td data-label="${lang === "es" ? "Típico" : "Typical"}">${x[3]}</td><td data-label="${lang === "es" ? "Complejo" : "Complex"}">${x[4]}</td><td><button data-service="${x[1]}">${lang === "es" ? "Enviar orden" : "Send work order"}</button></td></tr>`,
        )
        .join("")
    : `<tr><td class="pm-empty" colspan="5">${lang === "es" ? "No aparece esa orden. Envíenos el alcance para revisarlo." : "That work order is not listed. Send the scope for review."}</td></tr>`;
  rows.querySelectorAll("button").forEach((button) => {
    button.onclick = () => {
      location.href = `sms:+16506696890?&body=${encodeURIComponent(`SAFE lender/REO work order\nService: ${button.dataset.service}\nProperty ID/address:\nOccupancy status:\nDue date:\nNTE:`)}`;
    };
  });
}

function applyLanguage() {
  document.documentElement.lang = lang;
  document.querySelectorAll("[data-en]").forEach((element) => {
    element.textContent = element.dataset[lang];
  });
  search.placeholder =
    lang === "es"
      ? "Busque: secure, trash-out, WDIR, roof, rehab…"
      : "Search: secure, trash-out, WDIR, roof, rehab…";
  document.getElementById("reoLanguage").textContent =
    lang === "en" ? "ES" : "EN";
  render();
}

search.oninput = render;
category.onchange = render;
document.getElementById("reoLanguage").onclick = () => {
  lang = lang === "en" ? "es" : "en";
  localStorage.setItem("safeLanguage", lang);
  applyLanguage();
};
applyLanguage();
