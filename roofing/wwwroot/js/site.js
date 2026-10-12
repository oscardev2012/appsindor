(() => {
  const EMAIL = "ericb.majestic@gmail.com";
  const money = (n) =>
    new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: "USD",
      maximumFractionDigits: 0,
    }).format(Math.round(n));

  const extras = {
    en: {
      skip: "Skip to content",
      navAria: "Primary navigation",
      helpAria: "Roofing help options",
      stormHref: "/roof-insurance-claim-charlotte-nc",
      inspectionHref: "/free-roof-inspection-charlotte-nc",
      replacementHref: "/roof-replacement-charlotte-nc",
      repairHref: "/roof-repair-charlotte-nc",
      faqHref: "/faqs",
      faqNav: "FAQs",
      emergencyPageHref: "/emergency-roof-repair-charlotte-nc",
      tuneUpHref: "/roof-tune-up-charlotte-nc",
      systemsHref: "/roofing-systems-charlotte-nc",
      treeHref: "/tree-roof-damage-charlotte-nc",
      leakHref: "/active-roof-leak-repair-charlotte-nc",
      chimneyHref: "/chimney-flashing-leak-repair-charlotte-nc",
      windHref: "/wind-missing-shingles-repair-charlotte-nc",
      hailHref: "/hail-damage-roof-inspection-charlotte-nc",
      holeHref: "/open-roof-hole-decking-repair-charlotte-nc",
      charlotteHref: "/roofing-charlotte-nc",
      rockHillHref: "/roofing-rock-hill-sc",
      quick1t: "Free storm inspection",
      quick1s: "Claims documentation · same-day response",
      quick2t: "$350 Roof Tune-Up",
      quick2s: "Annual general roof maintenance",
      quick3t: "Plan a replacement",
      quick3s: "Private price range, no form first",
      servicesLead:
        "Start with the service that matches your situation. Majestic will confirm the actual scope after reviewing the property.",
      shinglePhoto: "REAL PROJECT PHOTOGRAPHY",
      shingleEyebrow: "Architectural shingle roofing",
      shingleTitle: "See the roof system—not just a color sample.",
      shingleText:
        "Architectural asphalt shingles create the dimensional pattern visible in these projects. The final roof is more than the exposed shingle: underlayment, flashing, valleys, ventilation, starter and ridge components all affect performance. Majestic confirms the selected manufacturer, product line, color and written scope before installation.",
      shingleF1: "Dimensional shingle appearance",
      shingleF2: "Complete roof-system planning",
      shingleF3: "Final selection confirmed in writing",
      shingleCta: "Plan a shingle roof",
      systemsCta: "Compare roof systems",
      plan1: "No contact information required to see the range.",
      plan2: "Adjusts for stories, roof shape, slope, material, and tear-off.",
      plan3: "A field inspection makes it exact.",
      rangeLabel: "Your planning range",
      homeSize: "Finished home size (sq ft)",
      homeHint:
        "Use the living-area number from your listing or tax record. Do not include the garage unless it is finished living space.",
      stories: "Stories",
      story1: "1 story",
      story2: "2 stories",
      story3: "3+ stories",
      material: "Roof material",
      matArch: "Architectural shingles",
      matPrem: "Premium shingles",
      matMetal: "Metal roofing",
      shape: "Roof shape",
      shapeSimple: "Simple / gable",
      shapeStd: "Standard / hips & valleys",
      shapeComplex: "Complex / many cutups",
      tearOff: "Include standard existing-roof tear-off",
      pitchLegend: "How steep is your roof? (roof pitch)",
      pitchHelp:
        "Pitch means how many inches the roof rises for every 12 inches it runs horizontally. If you are unsure, choose the picture that looks closest; Majestic will verify it.",
      pitchLow: "Low / gentle",
      pitchLowS: "Under 6/12 · looks flatter",
      pitchStd: "Standard",
      pitchStdS: "6/12–8/12 · common home slope",
      pitchSteep: "Steep",
      pitchSteepS: "9/12+ · sharp angle, extra safety",
      tuneOver: "Protect the roof you have",
      tuneSpan: "annual Roof Tune-Up",
      tuneText:
        "A general maintenance visit for accessible roof areas, built to identify visible concerns and renew exposed roof sealing details before a small issue becomes a larger project.",
      tuneL1: "Accessible roof review",
      tuneL2: "Sealant maintenance at exposed fasteners and accessible penetrations",
      tuneL3: "Chimney water-entry detail review",
      tuneL4: "Visible-concern recommendations",
      tuneCta: "Request a Tune-Up",
      emOver: "Keep water out while planning the next step",
      emSpan: "emergency tarp up to 20 × 20 ft",
      emText:
        "A safe-site emergency tarp option for active roof damage. A larger standard tarp is $150; larger, multiple, or unusual installations are confirmed before work begins.",
      emL1: "Safe site assessment before installation",
      emL2: "Tarp installation for temporary water protection",
      emL3: "Further repair or replacement can be scoped separately",
      emCta: "Call for emergency help",
      emEye: "Is this a roof emergency?",
      emTitle: "Choose the problem to see the first steps, our process and repair options.",
      emLead:
        "Do not climb onto a wet, damaged or unstable roof. If anyone is in immediate danger, call 911 first. Emergency work depends on safe access and current weather.",
      tTree: "Tree or debris impact",
      dTree: "A limb or tree struck the roof, decking or structure.",
      tLeak: "Active leak",
      dLeak: "Water is entering through the roof, ceiling or attic.",
      tChim: "Chimney or flashing leak",
      dChim: "Water appears near a chimney, wall transition or flashing.",
      tWind: "Wind or missing shingles",
      dWind: "Shingles lifted, tore, slid or disappeared after wind.",
      tHail: "Hail or storm impact",
      dHail: "Possible impacts, granule loss or widespread storm concerns.",
      tHole: "Open hole or exposed decking",
      dHole: "The roof covering is open and temporary protection may be needed.",
      insLabel: "A contractor documents the roof. Your carrier decides coverage.",
      st1t: "Protect",
      st1d: "If conditions are safe, temporary tarping can help limit additional water entry.",
      st2t: "Inspect",
      st2d: "Review visible roof and exterior conditions without promising a claim result.",
      st3t: "Document",
      st3d: "Organize photos, measurements, roof findings and the contractor scope.",
      st4t: "Coordinate",
      st4d: "Meet at the property with the adjuster when requested and authorized.",
      st5t: "Build",
      st5d: "Confirm the written construction scope, schedule the work and document completion.",
      claimCta: "Open the complete claim guide",
      doi: "Read the NC DOI claims guide ↗",
      cltEye: "Built for Charlotte weather",
      cltTitle: "Local readiness before the next hard rain.",
      cltText:
        "Charlotte–Mecklenburg planning resources identify hurricanes, tornadoes, flooding, power outages and downed trees among the hazards local properties may face. Majestic’s role is the roof: prevention, temporary protection when safe, clear documentation and an honest repair-or-replace plan.",
      res1s: "OFFICIAL LOCAL RESOURCE",
      res1t: "Charlotte emergency preparation",
      res2s: "CHARMECK ALERTS",
      res2t: "Get local weather alerts",
      perMonth: "per month",
      confirmAddr: "Confirm my address",
      proofNote:
        "These images show completed roof surfaces and geometry. They do not identify a manufacturer, product line, warranty or claim outcome.",
      insta: "More Majestic project updates on Instagram · @majesticroofing1 ↗",
      projectCap: "Shingle roof project",
      call: "CALL MAJESTIC",
      emailLbl: "EMAIL",
      yourName: "Your name",
      namePh: "Name",
      phone: "Phone",
      address: "Property address",
      addrPh: "Street, city, ZIP",
      need: "What service do you need?",
      optInsp: "Roof inspection",
      optRepl: "Roof replacement",
      optRep: "Roof repair",
      optStorm: "Storm damage documentation",
      optEm: "Emergency tarp",
      optTune: "$350 Roof Tune-Up",
      optGutter: "Gutters / fascia / soffit",
      optComm: "Commercial roofing",
      happened: "What happened?",
      issNs: "Not sure — please inspect",
      issLeak: "Active roof leak",
      issTree: "Tree / debris hit the roof",
      issChim: "Chimney / flashing problem",
      issWind: "Wind / missing shingles",
      issHail: "Hail / storm concern",
      issHole: "Hole / exposed roof area",
      issOld: "Old or worn roof",
      details: "Project details",
      detailsPh: "What changed? When did you notice it? Is water entering now?",
      footerClaim: "roof insurance claim documentation",
      footerBottom: "5122 Belstone Lane, Charlotte, NC 28215 · (980) 800-5991",
      mailSubject: "Website project request - Majestic Roofing",
      mailNote: "Your email app is opening with the project details.",
    },
    es: {
      skip: "Ir al contenido",
      navAria: "Navegación principal",
      helpAria: "Opciones de ayuda",
      stormHref: "/es/reclamos-de-seguro-de-techo-charlotte-nc",
      inspectionHref: "/es/inspeccion-gratis-de-techo-charlotte-nc",
      replacementHref: "/es/reemplazo-techo-charlotte-nc",
      repairHref: "/es/reparacion-de-techo-charlotte-nc",
      faqHref: "/es/preguntas-frecuentes",
      faqNav: "FAQ",
      emergencyPageHref: "/es/reparacion-de-emergencia-techo-charlotte-nc",
      tuneUpHref: "/es/mantenimiento-de-techo-charlotte-nc",
      systemsHref: "/es/sistemas-de-techo-charlotte-nc",
      treeHref: "/es/danos-de-arbol-en-techo-charlotte-nc",
      leakHref: "/es/reparacion-de-liqueo-de-techo-charlotte-nc",
      chimneyHref: "/es/reparacion-de-flashing-de-chimenea-charlotte-nc",
      windHref: "/es/reparacion-de-shingles-por-viento-charlotte-nc",
      hailHref: "/es/inspeccion-de-dano-de-granizo-charlotte-nc",
      holeHref: "/es/reparacion-de-agujero-y-decking-charlotte-nc",
      charlotteHref: "/es/roofing-charlotte-nc",
      rockHillHref: "/es/roofing-rock-hill-sc",
      quick1t: "Inspección de tormenta gratis",
      quick1s: "Documentación · respuesta el mismo día",
      quick2t: "Roof Tune-Up $350",
      quick2s: "Mantenimiento general anual",
      quick3t: "Planear reemplazo",
      quick3s: "Rango de precio sin formulario primero",
      servicesLead:
        "Comience con el servicio que coincide con su situación. Majestic confirma el alcance real después de revisar la propiedad.",
      shinglePhoto: "FOTOGRAFÍA DE PROYECTO REAL",
      shingleEyebrow: "Techos de shingles arquitectónicos",
      shingleTitle: "Vea el sistema del techo, no solamente una muestra de color.",
      shingleText:
        "Los shingles asfálticos arquitectónicos crean el patrón dimensional visible en estos proyectos. El techo terminado es más que el shingle expuesto: underlayment, flashing, valles, ventilación, starter y ridge también afectan su funcionamiento. Majestic confirma fabricante, línea, color y alcance escrito antes de instalar.",
      shingleF1: "Apariencia dimensional",
      shingleF2: "Planificación del sistema completo",
      shingleF3: "Selección final confirmada por escrito",
      shingleCta: "Planear techo de shingles",
      systemsCta: "Comparar sistemas",
      plan1: "No necesita dejar datos para ver el rango.",
      plan2: "Se ajusta por niveles, forma, inclinación, material y remoción.",
      plan3: "Una inspección en sitio lo hace exacto.",
      rangeLabel: "Su rango de planificación",
      homeSize: "Tamaño habitable de la casa (pies²)",
      homeHint:
        "Use el área habitable de su listing o registro de impuestos. No incluya el garaje si no es espacio habitable.",
      stories: "Niveles",
      story1: "1 nivel",
      story2: "2 niveles",
      story3: "3+ niveles",
      material: "Material del techo",
      matArch: "Shingles arquitectónicos",
      matPrem: "Shingles premium",
      matMetal: "Metal roofing",
      shape: "Forma del techo",
      shapeSimple: "Simple / dos aguas",
      shapeStd: "Estándar / hips y valles",
      shapeComplex: "Complejo / muchos cortes",
      tearOff: "Incluir remoción estándar del techo existente",
      pitchLegend: "¿Qué tan inclinado es su techo? (roof pitch)",
      pitchHelp:
        "Pitch significa cuántas pulgadas sube el techo por cada 12 pulgadas horizontales. Si no está seguro, elija el dibujo más parecido; Majestic lo verificará.",
      pitchLow: "Bajo / suave",
      pitchLowS: "Menos de 6/12 · se ve más plano",
      pitchStd: "Estándar",
      pitchStdS: "6/12–8/12 · inclinación común",
      pitchSteep: "Inclinado",
      pitchSteepS: "9/12+ · ángulo fuerte, seguridad adicional",
      tuneOver: "Proteja el techo que tiene",
      tuneSpan: "Roof Tune-Up anual",
      tuneText:
        "Una visita de mantenimiento general para zonas accesibles, pensada para identificar problemas visibles y renovar sellos expuestos antes de que un detalle pequeño se convierta en un proyecto mayor.",
      tuneL1: "Revisión de áreas accesibles",
      tuneL2: "Mantenimiento de sellador en clavos y penetraciones accesibles",
      tuneL3: "Revisión de detalles de agua en chimenea",
      tuneL4: "Recomendaciones sobre problemas visibles",
      tuneCta: "Solicitar Tune-Up",
      emOver: "Evite que entre agua mientras planeamos el próximo paso",
      emSpan: "lona de emergencia hasta 20 × 20 pies",
      emText:
        "Una opción de lona de emergencia para daño activo, siempre que el sitio sea seguro. Una lona estándar más grande cuesta $150; instalaciones grandes, múltiples o inusuales se confirman antes de comenzar.",
      emL1: "Evaluación de seguridad antes de instalar",
      emL2: "Instalación temporal para proteger contra agua",
      emL3: "La reparación o reemplazo posterior se cotiza por separado",
      emCta: "Llamar para emergencia",
      emEye: "¿Es una emergencia de techo?",
      emTitle: "Seleccione el problema para ver primeros pasos, nuestro proceso y opciones de reparación.",
      emLead:
        "No se suba a un techo mojado, dañado o inestable. Si alguien está en peligro inmediato, llame primero al 911. El trabajo de emergencia depende del acceso seguro y del clima.",
      tTree: "Árbol o escombros",
      dTree: "Una rama o árbol golpeó el techo, decking o estructura.",
      tLeak: "Liqueo activo",
      dLeak: "Está entrando agua por el techo, cielo o attic.",
      tChim: "Chimenea o flashing",
      dChim: "El agua aparece cerca de chimenea, pared o flashing.",
      tWind: "Viento o shingles faltantes",
      dWind: "Los shingles se levantaron, rompieron, deslizaron o desaparecieron.",
      tHail: "Granizo o tormenta",
      dHail: "Posibles impactos, pérdida de gránulos o daño extendido.",
      tHole: "Agujero o decking expuesto",
      dHole: "La cubierta está abierta y puede necesitar protección temporal.",
      insLabel: "El contratista documenta el techo. Su aseguradora decide la cobertura.",
      st1t: "Proteger",
      st1d: "Si las condiciones son seguras, una lona temporal puede ayudar a limitar la entrada adicional de agua.",
      st2t: "Inspeccionar",
      st2d: "Revisar condiciones visibles del techo y exterior sin prometer el resultado del reclamo.",
      st3t: "Documentar",
      st3d: "Organizar fotos, medidas, hallazgos y el alcance del contratista.",
      st4t: "Coordinar",
      st4d: "Reunirse en la propiedad con el adjuster cuando sea solicitado y autorizado.",
      st5t: "Construir",
      st5d: "Confirmar el alcance escrito, agendar el trabajo y documentar la terminación.",
      claimCta: "Abrir la guía completa",
      doi: "Leer la guía de reclamos de NC DOI ↗",
      cltEye: "Preparado para el clima de Charlotte",
      cltTitle: "Preparación local antes de la próxima lluvia fuerte.",
      cltText:
        "Los recursos de Charlotte-Mecklenburg identifican huracanes, tornados, inundaciones, apagones y árboles caídos entre los riesgos que pueden afectar propiedades locales. El papel de Majestic es el techo: prevención, protección temporal cuando sea seguro, documentación clara y un plan honesto de reparación o reemplazo.",
      res1s: "RECURSO LOCAL OFICIAL",
      res1t: "Preparación de emergencias de Charlotte",
      res2s: "CHARMECK ALERTS",
      res2t: "Reciba alertas meteorológicas locales",
      perMonth: "por mes",
      confirmAddr: "Confirmar mi dirección",
      proofNote:
        "Estas imágenes muestran superficies y geometría terminadas. No identifican fabricante, línea de producto, garantía ni resultado de reclamo.",
      insta: "Más actualizaciones de Majestic en Instagram · @majesticroofing1 ↗",
      projectCap: "Proyecto de shingles",
      call: "LLAME A MAJESTIC",
      emailLbl: "CORREO",
      yourName: "Su nombre",
      namePh: "Nombre",
      phone: "Teléfono",
      address: "Dirección de la propiedad",
      addrPh: "Calle, ciudad, ZIP",
      need: "¿Qué servicio necesita?",
      optInsp: "Inspección de techo",
      optRepl: "Reemplazo de techo",
      optRep: "Reparación de techo",
      optStorm: "Documentación de tormenta",
      optEm: "Lona de emergencia",
      optTune: "Roof Tune-Up de $350",
      optGutter: "Gutters / fascia / soffit",
      optComm: "Roofing comercial",
      happened: "¿Qué pasó?",
      issNs: "No estoy seguro — inspeccionar",
      issLeak: "Liqueo activo",
      issTree: "Árbol / escombros golpearon techo",
      issChim: "Problema de chimenea / flashing",
      issWind: "Viento / shingles faltantes",
      issHail: "Granizo / tormenta",
      issHole: "Agujero / área expuesta",
      issOld: "Techo antiguo o desgastado",
      details: "Detalles del proyecto",
      detailsPh: "¿Qué cambió? ¿Cuándo lo notó? ¿Está entrando agua ahora?",
      footerClaim: "documentación de reclamos de techo",
      footerBottom: "5122 Belstone Lane, Charlotte, NC 28215 · (980) 800-5991",
      mailSubject: "Solicitud de proyecto desde el website - Majestic Roofing",
      mailNote: "Se está abriendo su correo con los detalles del proyecto.",
    },
  };

  let i18nPack = null;
  let lang = "en";
  let lastRange = { low: 16961, high: 21704 };

  function text(el, value) {
    if (el) el.textContent = value;
  }

  function setButtonLabel(el, label) {
    if (!el) return;
    const arrow = el.querySelector('[aria-hidden="true"]');
    const arrowHtml = arrow ? ` ${arrow.outerHTML}` : "";
    el.innerHTML = `${label}${arrowHtml}`;
  }

  function firstText(el, value) {
    if (!el) return;
    const node = [...el.childNodes].find((n) => n.nodeType === Node.TEXT_NODE && n.textContent.trim());
    if (node) node.textContent = value;
    else {
      const b = el.querySelector("b, i");
      if (b && b.nextSibling && b.nextSibling.nodeType === Node.TEXT_NODE) b.nextSibling.textContent = value;
    }
  }

  async function loadI18n() {
    if (i18nPack) return i18nPack;
    const res = await fetch("/js/i18n.json");
    i18nPack = await res.json();
    return i18nPack;
  }

  function applyHomeLanguage(next) {
    const pack = i18nPack;
    if (!pack) return;
    const A = pack.u[next];
    const X = extras[next];
    const services = pack.d[next];
    const process = pack.f[next];
    const faqs = pack.p[next];
    lang = next;
    window.MajesticLang?.set(next);

    const main = document.querySelector("main");
    if (main) main.lang = next;
    document.documentElement.lang = next;

    text(document.querySelector(".skip-link"), X.skip);
    text(document.querySelector(".emergency-inner span"), A.emergency);
    setButtonLabel(document.querySelector(".emergency-inner a"), A.emergencyCta);
    document.querySelector(".emergency-inner a")?.setAttribute("href", X.emergencyPageHref);
    const nav = document.querySelector(".site-header nav");
    if (nav) {
      nav.setAttribute("aria-label", X.navAria);
      const links = nav.querySelectorAll("a");
      text(links[0], A.nav.services);
      text(links[1], A.nav.calculator);
      text(links[2], A.nav.storms);
      if (links[2]) links[2].setAttribute("href", X.stormHref);
      if (links[3]) {
        text(links[3], X.faqNav);
        links[3].setAttribute("href", X.faqHref);
      }
    }
    text(document.querySelector(".header-actions .button-small"), A.nav.contact);
    syncSwitcher(next);

    text(document.querySelector(".hero-copy .eyebrow"), A.heroEyebrow);
    text(document.querySelector(".hero-copy h1"), A.heroTitle);
    text(document.querySelector(".hero-lede"), A.heroText);
    const heroBtns = document.querySelectorAll(".hero-copy .hero-actions a");
    setButtonLabel(heroBtns[0], A.primaryCta);
    text(heroBtns[1], A.secondaryCta);
    const heroNote = document.querySelector(".hero-note");
    if (heroNote) {
      const t = [...heroNote.childNodes].find((n) => n.nodeType === Node.TEXT_NODE && n.textContent.trim());
      if (t) t.textContent = ` ${A.heroNote}`;
    }

    const panel = document.querySelector(".hero-panel");
    if (panel) {
      panel.setAttribute("aria-label", X.helpAria);
      text(panel.querySelector(".panel-kicker"), A.fast);
      const qs = panel.querySelectorAll(".quick-link");
      qs[0]?.setAttribute("href", X.inspectionHref);
      qs[1]?.setAttribute("href", X.tuneUpHref);
      qs[2]?.setAttribute("href", X.replacementHref);
      text(qs[0]?.querySelector("strong"), X.quick1t);
      text(qs[0]?.querySelector("small"), X.quick1s);
      text(qs[1]?.querySelector("strong"), X.quick2t);
      text(qs[1]?.querySelector("small"), X.quick2s);
      text(qs[2]?.querySelector("strong"), X.quick3t);
      text(qs[2]?.querySelector("small"), X.quick3s);
    }

    const svcHead = document.querySelector(".service-section .split-heading");
    text(svcHead?.querySelector(".eyebrow"), A.servicesEyebrow);
    text(svcHead?.querySelector("h2"), A.servicesTitle);
    text(svcHead?.querySelector(":scope > p"), X.servicesLead);
    document.querySelectorAll(".service-card").forEach((card, i) => {
      const row = services[i];
      if (!row) return;
      text(card.querySelector("h3"), row[1]);
      text(card.querySelector("p"), row[2]);
      const mappedHref = row[3] === "#tune-up" ? X.tuneUpHref : row[3];
      card.setAttribute("href", mappedHref);
      const link = card.querySelector(".inline-link");
      if (link) {
        const b = link.querySelector("b");
        link.innerHTML = `${A.serviceCta} ${b ? b.outerHTML : ""}`;
      }
    });

    const shingle = document.querySelector(".shingle-section");
    text(shingle?.querySelector(".shingle-visual > span"), X.shinglePhoto);
    text(shingle?.querySelector(".eyebrow"), X.shingleEyebrow);
    text(shingle?.querySelector("h2"), X.shingleTitle);
    text(shingle?.querySelector(".shingle-layout > div:last-child > p:not(.eyebrow)"), X.shingleText);
    const facts = shingle?.querySelectorAll(".shingle-facts span") || [];
    firstText(facts[0], X.shingleF1);
    firstText(facts[1], X.shingleF2);
    firstText(facts[2], X.shingleF3);
    const shBtns = shingle?.querySelectorAll(".hero-actions a") || [];
    setButtonLabel(shBtns[0], X.shingleCta);
    setButtonLabel(shBtns[1], X.systemsCta);
    if (shBtns[1]) shBtns[1].setAttribute("href", X.systemsHref);

    text(document.querySelector(".process-section .eyebrow"), A.processEyebrow);
    text(document.querySelector(".process-section h2"), A.processTitle);
    document.querySelectorAll(".process-card").forEach((card, i) => {
      const row = process[i];
      if (!row) return;
      text(card.querySelector("h3"), row[1]);
      text(card.querySelector("p"), row[2]);
    });

    const planner = document.querySelector(".planner-section");
    text(planner?.querySelector(".planner-intro .eyebrow"), A.calculatorEyebrow);
    text(planner?.querySelector(".planner-intro h2"), A.calculatorTitle);
    text(planner?.querySelector(".planner-intro > p:not(.eyebrow)"), A.calculatorText);
    const pts = planner?.querySelectorAll(".planner-points span") || [];
    firstText(pts[0], X.plan1);
    firstText(pts[1], X.plan2);
    firstText(pts[2], X.plan3);
    text(planner?.querySelector(".calculator-header > span"), X.rangeLabel);
    const labels = planner?.querySelectorAll(".calculator-fields label") || [];
    text(labels[0]?.querySelector("span"), X.homeSize);
    text(labels[0]?.querySelector("small"), X.homeHint);
    text(labels[1]?.querySelector("span"), X.stories);
    const storyOpts = labels[1]?.querySelectorAll("option") || [];
    text(storyOpts[0], X.story1);
    text(storyOpts[1], X.story2);
    text(storyOpts[2], X.story3);
    text(labels[2]?.querySelector("span"), X.material);
    const matOpts = labels[2]?.querySelectorAll("option") || [];
    text(matOpts[0], X.matArch);
    text(matOpts[1], X.matPrem);
    text(matOpts[2], X.matMetal);
    text(labels[3]?.querySelector("span"), X.shape);
    const shOpts = labels[3]?.querySelectorAll("option") || [];
    text(shOpts[0], X.shapeSimple);
    text(shOpts[1], X.shapeStd);
    text(shOpts[2], X.shapeComplex);
    text(labels[4]?.querySelector("span"), X.tearOff);
    text(planner?.querySelector(".pitch-picker legend"), X.pitchLegend);
    text(planner?.querySelector(".pitch-picker > p"), X.pitchHelp);
    const pitches = planner?.querySelectorAll(".pitch-option") || [];
    text(pitches[0]?.querySelector("strong"), X.pitchLow);
    text(pitches[0]?.querySelector("small"), X.pitchLowS);
    text(pitches[1]?.querySelector("strong"), X.pitchStd);
    text(pitches[1]?.querySelector("small"), X.pitchStdS);
    text(pitches[2]?.querySelector("strong"), X.pitchSteep);
    text(pitches[2]?.querySelector("small"), X.pitchSteepS);
    const discs = planner?.querySelectorAll(".calculator-disclaimer") || [];
    text(discs[0], A.includes);
    text(discs[1], A.excludes);
    setButtonLabel(planner?.querySelector(".calculator-card .full-button"), A.calculatorCta);
    updateCalculator();

    const tune = document.querySelector(".feature-tune");
    text(tune?.querySelector(".feature-overline"), X.tuneOver);
    text(tune?.querySelector(".price-lockup span"), X.tuneSpan);
    text(tune?.querySelector(":scope > p"), X.tuneText);
    const tuneLis = tune?.querySelectorAll("li") || [];
    text(tuneLis[0], X.tuneL1);
    text(tuneLis[1], X.tuneL2);
    text(tuneLis[2], X.tuneL3);
    text(tuneLis[3], X.tuneL4);
    const tuneBtn = tune?.querySelector(".text-button");
    setButtonLabel(tuneBtn, X.tuneCta);
    if (tuneBtn) tuneBtn.setAttribute("href", X.tuneUpHref);

    const em = document.querySelector(".feature-emergency");
    text(em?.querySelector(".feature-overline"), X.emOver);
    text(em?.querySelector(".price-lockup span"), X.emSpan);
    text(em?.querySelector(":scope > p"), X.emText);
    const emLis = em?.querySelectorAll("li") || [];
    text(emLis[0], X.emL1);
    text(emLis[1], X.emL2);
    text(emLis[2], X.emL3);
    setButtonLabel(em?.querySelector(".text-button"), X.emCta);

    const emSec = document.querySelector(".emergency-types .split-heading");
    text(emSec?.querySelector(".eyebrow"), X.emEye);
    text(emSec?.querySelector("h2"), X.emTitle);
    text(emSec?.querySelector(":scope > p"), X.emLead);
    const types = document.querySelectorAll(".emergency-type");
    const typeData = [
      ["tTree", "dTree", "treeHref"],
      ["tLeak", "dLeak", "leakHref"],
      ["tChim", "dChim", "chimneyHref"],
      ["tWind", "dWind", "windHref"],
      ["tHail", "dHail", "hailHref"],
      ["tHole", "dHole", "holeHref"],
    ];
    types.forEach((el, i) => {
      const [tk, dk, hk] = typeData[i];
      text(el.querySelector("strong"), X[tk]);
      text(el.querySelector(".emergency-description"), X[dk]);
      el.setAttribute("href", X[hk]);
    });

    const ins = document.querySelector(".insurance-section");
    text(ins?.querySelector(".insurance-label p"), A.insuranceEyebrow);
    text(ins?.querySelector(".insurance-label strong"), X.insLabel);
    text(ins?.querySelector("h2"), A.insuranceTitle);
    text(ins?.querySelector(".insurance-lede"), A.insuranceText);
    const steps = ins?.querySelectorAll(".insurance-steps article") || [];
    [
      ["st1t", "st1d"],
      ["st2t", "st2d"],
      ["st3t", "st3d"],
      ["st4t", "st4d"],
      ["st5t", "st5d"],
    ].forEach(([tk, dk], i) => {
      text(steps[i]?.querySelector("h3"), X[tk]);
      text(steps[i]?.querySelector("p"), X[dk]);
    });
    const claimBtn = ins?.querySelector(".storm-actions .button");
    setButtonLabel(claimBtn, X.claimCta);
    if (claimBtn) claimBtn.setAttribute("href", X.stormHref);
    text(ins?.querySelector(".official-link"), X.doi);

    const clt = document.querySelector(".charlotte-section");
    text(clt?.querySelector(".eyebrow"), X.cltEye);
    text(clt?.querySelector("h2"), X.cltTitle);
    text(clt?.querySelector(".charlotte-layout > div:last-child > p"), X.cltText);
    const resources = clt?.querySelectorAll(".local-resource-grid a") || [];
    text(resources[0]?.querySelector("small"), X.res1s);
    text(resources[0]?.querySelector("strong"), X.res1t);
    text(resources[1]?.querySelector("small"), X.res2s);
    text(resources[1]?.querySelector("strong"), X.res2t);

    const mem = document.querySelector(".membership-section");
    text(mem?.querySelector(".membership-price small"), X.perMonth);
    text(mem?.querySelector(".eyebrow"), A.membershipEyebrow);
    text(mem?.querySelector("h2"), A.membershipTitle);
    text(mem?.querySelector(".membership-layout > div:last-child > p:not(.eyebrow)"), A.membershipText);
    setButtonLabel(mem?.querySelector(".button"), A.membershipCta);

    const cov = document.querySelector(".coverage-section");
    text(cov?.querySelector(".eyebrow"), A.areasEyebrow);
    text(cov?.querySelector("h2"), A.areasTitle);
    const areaLinks = cov?.querySelectorAll(".area-page-links a") || [];
    if (areaLinks[0]) areaLinks[0].setAttribute("href", X.charlotteHref);
    if (areaLinks[1]) areaLinks[1].setAttribute("href", X.rockHillHref);
    text(cov?.querySelector(".coverage-layout > div:last-child > p"), A.areasText);
    setButtonLabel(cov?.querySelector(".text-button"), X.confirmAddr);

    const proof = document.querySelector(".proof-section");
    text(proof?.querySelector(".eyebrow"), A.proofEyebrow);
    text(proof?.querySelector("h2"), A.proofTitle);
    text(proof?.querySelector(".project-heading > div:last-child > p"), A.proofText);
    const note = proof?.querySelector(".proof-note");
    if (note) {
      const t = [...note.childNodes].find((n) => n.nodeType === Node.TEXT_NODE && n.textContent.trim());
      if (t) t.textContent = X.proofNote;
    }
    text(proof?.querySelector(".project-social-link"), X.insta);
    proof?.querySelectorAll("figcaption span").forEach((el) => text(el, X.projectCap));

    const faq = document.querySelector(".faq-section");
    text(faq?.querySelector(".eyebrow"), A.faqEyebrow);
    text(faq?.querySelector("h2"), A.faqTitle);
    faq?.querySelectorAll("details").forEach((item, i) => {
      const row = faqs[i];
      if (!row) return;
      const summary = item.querySelector("summary");
      const plus = summary?.querySelector("span");
      if (summary) summary.innerHTML = `${row[0]}${plus ? plus.outerHTML : ""}`;
      text(item.querySelector("p"), row[1]);
    });

    const contact = document.querySelector(".contact-section");
    text(contact?.querySelector(".eyebrow"), A.contactEyebrow);
    text(contact?.querySelector("h2"), A.contactTitle);
    text(contact?.querySelector(".contact-copy > p:not(.eyebrow)"), A.contactText);
    const directs = contact?.querySelectorAll(".direct-contact small") || [];
    text(directs[0], X.call);
    text(directs[1], X.emailLbl);
    const form = contact?.querySelector(".contact-form");
    const formLabels = form?.querySelectorAll("label > span") || [];
    text(formLabels[0], X.yourName);
    text(formLabels[1], X.phone);
    text(formLabels[3], X.address);
    text(formLabels[4], X.need);
    text(formLabels[5], X.happened);
    text(formLabels[6], X.details);
    const nameInput = form?.querySelector('input[name="name"]');
    if (nameInput) nameInput.placeholder = X.namePh;
    const addrInput = form?.querySelector('input[name="address"]');
    if (addrInput) addrInput.placeholder = X.addrPh;
    const detailsInput = form?.querySelector("textarea");
    if (detailsInput) detailsInput.placeholder = X.detailsPh;
    const svcSel = form?.querySelector('select[name="service"]');
    const svcMap = [X.optInsp, X.optRepl, X.optRep, X.optStorm, X.optEm, X.optTune, X.optGutter, X.optComm];
    svcSel?.querySelectorAll("option").forEach((opt, i) => text(opt, svcMap[i]));
    const issSel = form?.querySelector('select[name="issue"]');
    const issMap = [X.issNs, X.issLeak, X.issTree, X.issChim, X.issWind, X.issHail, X.issHole, X.issOld];
    issSel?.querySelectorAll("option").forEach((opt, i) => text(opt, issMap[i]));
    setButtonLabel(form?.querySelector("button[type='submit']"), A.formCta);
    text(form?.querySelector(".form-note"), A.formNote);

    const footerP = document.querySelector(".footer-top p");
    if (footerP) footerP.textContent = `${A.footerText} · ${X.footerClaim}`;
    const bottoms = document.querySelectorAll(".footer-bottom span");
    text(bottoms[1], X.footerBottom);
  }

  function calcRange() {
    const card = document.querySelector(".calculator-card");
    if (!card) return null;
    const home = Math.min(12000, Math.max(500, Number(card.querySelector('input[type="text"]')?.value) || 2000));
    const stories = Number(card.querySelectorAll("select")[0]?.value || 1);
    const material = card.querySelectorAll("select")[1]?.value || "architectural";
    const shape = card.querySelectorAll("select")[2]?.value || "standard";
    const tearOff = card.querySelector(".checkbox-field input")?.checked ?? true;
    const pitchBtn = card.querySelector(".pitch-option.active");
    const pitch = pitchBtn?.querySelector(".pitch-low")
      ? "low"
      : pitchBtn?.querySelector(".pitch-steep")
        ? "steep"
        : "standard";
    const storyFactor = stories === 1 ? 1.06 : stories === 2 ? 1.25 : 1.42;
    const shapeFactor = { simple: 1.05, standard: 1.2, complex: 1.4 }[shape];
    const pitchFactor = { low: 1.04, standard: 1.13, steep: 1.34 }[pitch];
    const rates = { architectural: [515, 650], premium: [650, 840], metal: [860, 1210] }[material];
    const squares = Math.max(8, (home / stories / 100) * storyFactor * shapeFactor * pitchFactor);
    const extra = Math.max(0, stories - 1) * 725;
    const tear = tearOff ? [75, 105] : [0, 0];
    return {
      squares,
      low: squares * (rates[0] + tear[0]) + extra,
      high: squares * (rates[1] + tear[1]) + extra * 1.45,
    };
  }

  function updateCalculator() {
    const range = calcRange();
    if (!range) return;
    lastRange = range;
    const strong = document.querySelector(".calculator-header strong");
    const small = document.querySelector(".calculator-header small");
    const A = i18nPack?.u?.[lang] || { square: "Estimated roof area", squares: "squares" };
    if (strong) strong.innerHTML = `${money(range.low)} <i>—</i> ${money(range.high)}`;
    if (small)
      small.textContent = `${A.square}: ${Math.round(range.squares * 100)} sq ft · ${range.squares.toFixed(1)} ${A.squares}`;
  }

  function initCalculator() {
    const card = document.querySelector(".calculator-card");
    if (!card) return;
    card.querySelectorAll("input, select").forEach((el) => {
      el.addEventListener("input", updateCalculator);
      el.addEventListener("change", updateCalculator);
    });
    card.querySelectorAll(".pitch-option").forEach((btn) => {
      btn.addEventListener("click", () => {
        card.querySelectorAll(".pitch-option").forEach((b) => {
          b.classList.toggle("active", b === btn);
          b.setAttribute("aria-pressed", b === btn ? "true" : "false");
        });
        updateCalculator();
      });
    });
    updateCalculator();
  }

  const switcherHtml = `<div class="lang-switcher" role="group" aria-label="Language"><span class="lang-globe" aria-hidden="true"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><circle cx="12" cy="12" r="9"/><path d="M3 12h18M12 3c3 3.2 3 14.8 0 18M12 3c-3 3.2-3 14.8 0 18"/></svg></span><div class="lang-track"><button class="lang-option" type="button" data-lang="en" aria-pressed="false"><span class="lang-code">EN</span><span class="lang-name">English</span></button><button class="lang-option" type="button" data-lang="es" aria-pressed="false"><span class="lang-code">ES</span><span class="lang-name">Español</span></button></div></div>`;

  function currentLang() {
    return window.MajesticLang?.get() || lang || "en";
  }

  function pathIs(value) {
    const path = window.MajesticLang?.cleanPath(location.pathname) || location.pathname;
    return path === value;
  }

  function isHome() {
    return pathIs("/") || pathIs("/es");
  }

  function syncSwitcher(next) {
    document.querySelectorAll(".lang-option").forEach((btn) => {
      const on = btn.getAttribute("data-lang") === next;
      btn.classList.toggle("active", on);
      btn.setAttribute("aria-pressed", on ? "true" : "false");
    });
    document.querySelectorAll(".lang-switcher").forEach((el) => {
      el.setAttribute("aria-label", next === "es" ? "Idioma" : "Language");
    });
  }

  function mountSwitcher() {
    if (!document.querySelector(".lang-switcher")) {
      const old = document.querySelector(".language-toggle, .language-link");
      if (old) {
        old.insertAdjacentHTML("afterend", switcherHtml);
        old.remove();
      } else {
        const header = document.querySelector(".claim-header .site-shell, .site-header .header-inner, .header-actions");
        if (header) header.insertAdjacentHTML("beforeend", switcherHtml);
      }
    }

    document.querySelectorAll(".language-toggle:not(.lang-option), a.language-link").forEach((el) => el.remove());

    document.querySelectorAll(".lang-option").forEach((btn) => {
      btn.addEventListener("click", async () => {
        const next = btn.getAttribute("data-lang");
        if (!next || next === currentLang()) return;
        window.MajesticLang?.set(next);
        lang = next;
        if (isHome()) {
          const dest = next === "es" ? "/es" : "/";
          if (!pathIs(dest)) {
            location.href = dest + location.hash;
            return;
          }
        }
        const dest = window.MajesticLang?.localize(location.pathname, next) || location.pathname;
        location.href = dest + location.search + location.hash;
      });
    });

    syncSwitcher(currentLang());
  }

  function interceptLinks() {
    document.addEventListener("click", (event) => {
      const link = event.target.closest("a[href]");
      if (!link || event.metaKey || event.ctrlKey || event.shiftKey || event.altKey) return;
      if (link.target === "_blank" || link.hasAttribute("download")) return;
      const href = link.getAttribute("href");
      if (!href || href.startsWith("#") || href.startsWith("mailto:") || href.startsWith("tel:")) return;
      let url;
      try {
        url = new URL(href, location.origin);
      } catch {
        return;
      }
      if (url.origin !== location.origin) return;
      const nextPath = window.MajesticLang?.localize(url.pathname, currentLang());
      if (nextPath && nextPath !== url.pathname) {
        event.preventDefault();
        location.href = nextPath + url.search + url.hash;
      }
    });
  }

  function initForms() {
    document.querySelectorAll("form").forEach((form) => {
      form.addEventListener("submit", (event) => {
        event.preventDefault();
        const data = new FormData(form);
        const isEs = (document.querySelector("main")?.lang || "en") === "es";
        const subject = isEs
          ? extras.es.mailSubject
          : extras.en.mailSubject;
        const lines = [];
        for (const [key, value] of data.entries()) {
          if (typeof value === "string") lines.push(`${key}: ${value}`);
        }
        if (document.querySelector(".calculator-card")) {
          lines.push("", `Planning range / Rango de planificación: ${money(lastRange.low)}-${money(lastRange.high)}`);
        }
        const note = form.querySelector(".form-note");
        if (note) note.textContent = isEs ? extras.es.mailNote : extras.en.mailNote;
        window.location.href = `mailto:${EMAIL}?subject=${encodeURIComponent(subject)}&body=${encodeURIComponent(lines.join("\n"))}`;
      });
    });
  }

  document.addEventListener("DOMContentLoaded", async () => {
    mountSwitcher();
    interceptLinks();
    initCalculator();
    initForms();

    const stored = currentLang();
    lang = stored || "en";
    document.documentElement.classList.remove("i18n-boot");
    document.documentElement.lang = document.querySelector("html")?.lang || lang;
    syncSwitcher(document.documentElement.lang === "es" || pathIs("/es") ? "es" : "en");
  });
})();
