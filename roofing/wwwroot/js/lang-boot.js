(() => {
  const KEY = "majestic-lang";
  const pairs = [
    ["/roof-insurance-claim-charlotte-nc", "/es/reclamos-de-seguro-de-techo-charlotte-nc"],
    ["/roof-adjuster-visit-guide-charlotte-nc", "/es/guia-visita-adjuster-techo-charlotte-nc"],
    ["/roof-insurance-supplements-charlotte-nc", "/es/suplementos-de-reclamos-de-techo-charlotte-nc"],
    ["/roofing-charlotte-nc", "/es/roofing-charlotte-nc"],
    ["/roofing-rock-hill-sc", "/es/roofing-rock-hill-sc"],
    ["/tree-roof-damage-charlotte-nc", "/es/danos-de-arbol-en-techo-charlotte-nc"],
    ["/active-roof-leak-repair-charlotte-nc", "/es/reparacion-de-liqueo-de-techo-charlotte-nc"],
    ["/chimney-flashing-leak-repair-charlotte-nc", "/es/reparacion-de-flashing-de-chimenea-charlotte-nc"],
    ["/wind-missing-shingles-repair-charlotte-nc", "/es/reparacion-de-shingles-por-viento-charlotte-nc"],
    ["/hail-damage-roof-inspection-charlotte-nc", "/es/inspeccion-de-dano-de-granizo-charlotte-nc"],
    ["/open-roof-hole-decking-repair-charlotte-nc", "/es/reparacion-de-agujero-y-decking-charlotte-nc"],
    ["/roofing-systems-charlotte-nc", "/es/sistemas-de-techo-charlotte-nc"],
    ["/roof-warranty-transfer", "/es/transferencia-de-garantia-de-techo"],
    ["/roof-repair-charlotte-nc", "/es/reparacion-de-techo-charlotte-nc"],
    ["/roof-replacement-charlotte-nc", "/es/reemplazo-techo-charlotte-nc"],
    ["/roof-installation-charlotte-nc", "/es/instalacion-de-techo-charlotte-nc"],
    ["/emergency-roof-repair-charlotte-nc", "/es/reparacion-de-emergencia-techo-charlotte-nc"],
    ["/storm-damage-roof-repair-charlotte-nc", "/es/reparacion-techo-por-tormenta-charlotte-nc"],
    ["/roof-claim-process", "/es/proceso-de-reclamo-de-techo"],
    ["/roof-claim-denied-or-underpaid", "/es/reclamo-de-techo-denegado-o-pago-bajo"],
    ["/free-roof-inspection-charlotte-nc", "/es/inspeccion-gratis-de-techo-charlotte-nc"],
    ["/roof-replacement-cost-charlotte-nc", "/es/costo-de-reemplazo-de-techo-charlotte-nc"],
    ["/faqs", "/es/preguntas-frecuentes"],
    ["/repair-or-replace-roof", "/es/reparar-o-reemplazar-techo"],
    ["/roof-tune-up-charlotte-nc", "/es/mantenimiento-de-techo-charlotte-nc"],
  ];
  const enToEs = Object.fromEntries(pairs);
  const esToEn = Object.fromEntries(pairs.map(([en, es]) => [es, en]));

  function cleanPath(path) {
    const value = (path || "/").split("#")[0].split("?")[0];
    const trimmed = value.replace(/\/+$/, "");
    return trimmed === "" ? "/" : trimmed;
  }

  function pageLang(path) {
    const p = cleanPath(path);
    if (p === "/es" || p.startsWith("/es/")) return "es";
    return "en";
  }

  function localize(path, lang) {
    const p = cleanPath(path);
    if (p === "/" || p === "/es") return lang === "es" ? "/es" : "/";
    if (lang === "es") return enToEs[p] || (p.startsWith("/es/") ? p : p);
    return esToEn[p] || (p.startsWith("/es/") ? "/" : p);
  }

  function get() {
    try {
      const value = localStorage.getItem(KEY);
      return value === "es" || value === "en" ? value : null;
    } catch {
      return null;
    }
  }

  function set(lang) {
    try {
      localStorage.setItem(KEY, lang);
    } catch {
      /* ignore */
    }
  }

  const path = cleanPath(location.pathname);
  const urlLang = pageLang(path);
  let stored = get();

  if (!stored && urlLang) {
    set(urlLang);
    stored = urlLang;
  }

  if (stored && urlLang && stored !== urlLang) {
    const dest = localize(path, stored);
    if (dest !== path) {
      location.replace(dest + location.search + location.hash);
      return;
    }
  }

  window.MajesticLang = { KEY, cleanPath, pageLang, localize, get, set };
})();
