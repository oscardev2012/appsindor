const lang = document.documentElement.lang === "es" ? "es" : "en";

const answers = lang === "es" ? {
  leak: "Cierre el agua si es seguro. Una fuga en una conexión a veces se repara; agua del tanque casi siempre es reemplazo. Envíe una foto al 650-669-6890.",
  hot: "Sin agua caliente en eléctrico suele ser breaker, elemento o termostato. En gas, ignición, piloto o control. No siga reseteando. Envíe foto de la etiqueta.",
  noise: "Palomitas o retumbo suelen ser sedimento, común en Charlotte por el agua dura. Programe una revisión.",
  rust: "Agua caliente oxidada o con olor puede ser ánodo gastado o corrosión. Compare caliente y fría. Si solo afecta el caliente, llame a SAFE.",
  price: "El paquete eléctrico de 40 galones calificado es $200 del equipo promocional + $999 de instalación = $1,199 + impuesto. Lo no estándar se aprueba por escrito.",
  finance: "Plan SAFE: empiece con $200 hacia un equipo calificado. Synchrony es una solicitud aparte a nombre de HOME INDOR TECHNOLOGY INC.",
  warranty: "Instalaciones calificadas: 5 años de fabricante (manda el modelo) más 2 años de mano de obra SAFE. Registre el equipo después de instalar."
} : {
  leak: "Shut off the water if it is safe. A fitting leak may be repairable; water from the tank shell usually means replacement. Text a photo of the tank and the wet area to 650-669-6890.",
  hot: "No hot water on electric units is often a breaker, element or thermostat. On gas, it can be ignition, pilot or gas control. Do not keep resetting. Text SAFE a photo of the label.",
  noise: "Popping or rumbling is usually sediment on the tank bottom, common in the Charlotte metro because of hard water. Schedule a professional inspection.",
  rust: "Rusty or smelly hot water can mean a spent anode or internal corrosion. Compare hot vs cold. If only hot is affected, call SAFE.",
  price: "The qualifying 40-gal electric package is $200 promotional equipment + $999 standard installation = $1,199 + tax. Non-standard work is approved in writing first.",
  finance: "SAFE Homeowner Plan: start with $200 toward a qualifying unit and request eligibility for the installation balance. Synchrony is a separate application for HOME INDOR TECHNOLOGY INC.",
  warranty: "Qualifying installs include a 5-year limited manufacturer warranty (model terms apply) plus a 2-year SAFE installation labor warranty. Register the unit online after install."
};

const categories = lang === "es" ? [
  { id: "problems", label: "Problemas", hint: "Sin agua caliente, fugas, ruido", qs: [
    ["Sin agua caliente", "hot"], ["Tanque con fuga", "leak"], ["Palomitas / retumbo", "noise"], ["Agua oxidada o con olor", "rust"]
  ]},
  { id: "pricing", label: "Precio y pago", hint: "Contado, plan SAFE, Synchrony", qs: [
    ["¿Qué incluye $1,199?", "price"], ["Plan SAFE", "finance"], ["Opción Synchrony", "finance"]
  ]},
  { id: "warranty", label: "Garantía", hint: "Cobertura y registro", qs: [
    ["¿Qué garantía incluye?", "warranty"], ["¿Cómo registro?", "warranty"]
  ]}
] : [
  { id: "problems", label: "Problems", hint: "No hot water, leaks, noise", qs: [
    ["No hot water", "hot"], ["Leaking tank", "leak"], ["Popping / rumbling", "noise"], ["Rusty or smelly water", "rust"]
  ]},
  { id: "pricing", label: "Pricing + pay", hint: "Cash, SAFE plan, Synchrony", qs: [
    ["What is the $1,199 offer?", "price"], ["SAFE Homeowner Plan", "finance"], ["Synchrony option", "finance"]
  ]},
  { id: "warranty", label: "Warranty", hint: "Coverage and registration", qs: [
    ["What warranty is included?", "warranty"], ["How do I register?", "warranty"]
  ]}
];

document.addEventListener("DOMContentLoaded", () => {
  const panel = document.getElementById("ask-safe-panel");
  const body = document.getElementById("askSafeBody");
  const step = document.getElementById("askSafeStep");
  const open = document.getElementById("askSafeOpen");
  const close = document.getElementById("askSafeClose");
  const back = document.getElementById("askSafeBack");
  const reset = document.getElementById("askSafeReset");
  const send = document.getElementById("askSafeSend");
  const input = document.getElementById("askSafeInput");
  if (!panel || !body) return;

  let view = "menu";

  const setOpen = (isOpen) => {
    panel.hidden = !isOpen;
    open?.setAttribute("aria-expanded", String(isOpen));
  };

  const renderMenu = () => {
    view = "menu";
    step.textContent = lang === "es" ? "Menú" : "Menu";
    body.innerHTML = `
      <div class="ask-safe-welcome"><div class="chat-message safe">${lang === "es" ? "Hola — Ask SAFE le indica el siguiente paso. No es un diagnóstico. Emergencias: salga y llame al 911." : "Hi — Ask SAFE can point you to the right next step. This is not a diagnosis. Emergencies: leave and call 911."}</div></div>
      <div class="ask-safe-categories">
        ${categories.map(c => `<button type="button" data-cat="${c.id}"><span>${c.label[0]}</span><b>${c.label}</b><small>${c.hint}</small><i>›</i></button>`).join("")}
      </div>`;
    body.querySelectorAll("[data-cat]").forEach(btn => btn.addEventListener("click", () => renderCat(btn.getAttribute("data-cat"))));
  };

  const renderCat = (id) => {
    const cat = categories.find(c => c.id === id);
    if (!cat) return;
    view = "cat";
    step.textContent = cat.label;
    body.innerHTML = `<div class="ask-safe-question-list"><p>${cat.hint}</p>${cat.qs.map(([q, key]) => `<button type="button" data-key="${key}">${q}<span>›</span></button>`).join("")}</div>`;
    body.querySelectorAll("[data-key]").forEach(btn => btn.addEventListener("click", () => renderAnswer(btn.dataset.key, btn.textContent)));
  };

  const renderAnswer = (key, title) => {
    view = "answer";
    step.textContent = "Answer";
    const text = answers[key] || answers.price;
    body.innerHTML = `
      <div class="chat-messages">
        <div class="chat-message user">${title || "Question"}</div>
        <div class="chat-message safe">${text}</div>
      </div>
      <div class="answer-actions">
        <a href="sms:+16506696890">Text a photo</a>
        <a href="tel:+17048396126">Call SAFE</a>
      </div>`;
  };

  const match = (q) => {
    const t = q.toLowerCase();
    if (t.includes("leak")) return "leak";
    if (t.includes("hot") || t.includes("cold") || t.includes("no water")) return "hot";
    if (t.includes("pop") || t.includes("rumbl") || t.includes("noise")) return "noise";
    if (t.includes("rust") || t.includes("smell")) return "rust";
    if (t.includes("warrant")) return "warranty";
    if (t.includes("financ") || t.includes("200") || t.includes("synchrony")) return "finance";
    if (t.includes("price") || t.includes("1199") || t.includes("cost")) return "price";
    return "price";
  };

  open?.addEventListener("click", () => { setOpen(true); if (view === "menu") renderMenu(); });
  close?.addEventListener("click", () => setOpen(false));
  reset?.addEventListener("click", renderMenu);
  back?.addEventListener("click", () => view === "answer" ? renderMenu() : renderMenu());
  send?.addEventListener("click", () => {
    const q = input.value.trim();
    if (!q) return;
    renderAnswer(match(q), q);
    input.value = "";
  });
  input?.addEventListener("keydown", (e) => { if (e.key === "Enter") send?.click(); });
  renderMenu();
});
