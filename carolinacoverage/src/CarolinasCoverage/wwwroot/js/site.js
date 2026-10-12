(() => {
  const PRODUCTS = [
    { id: "auto", icon: "🚗", es: "Seguro de auto", en: "Auto insurance" },
    { id: "home", icon: "🏠", es: "Seguro de hogar", en: "Home insurance" },
    { id: "renters", icon: "🔑", es: "Seguro para inquilinos", en: "Renters insurance" },
    { id: "pet", icon: "🐾", es: "Seguro para mascotas", en: "Pet insurance" },
    { id: "business", icon: "🏢", es: "Seguro para negocios", en: "Business insurance" },
    { id: "commercial-auto", icon: "🚚", es: "Auto comercial", en: "Commercial auto" },
    { id: "life", icon: "🤍", es: "Seguro de vida", en: "Life insurance" }
  ];

  const LANG_KEY = "cc-lang";

  const escapeHtml = (value) => String(value ?? "")
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;");

  const rememberLang = (lang) => {
    try { localStorage.setItem(LANG_KEY, lang); } catch { /* ignore */ }
  };

  const withHash = (url) => {
    if (!window.location.hash || url.includes("#")) return url;
    return url.split("#")[0] + window.location.hash;
  };

  const langSwitchHtml = (en, esUrl, enUrl, theme = "light") => `
    <nav class="langSwitch langSwitch--${theme}" aria-label="Language / Idioma">
      <a class="langSwitch-opt ${en ? "" : "is-on"}" href="${esUrl}" hreflang="es" lang="es" data-lang-choice="es">
        <span class="langSwitch-code">ES</span>
        <span class="langSwitch-name">Español</span>
      </a>
      <a class="langSwitch-opt ${en ? "is-on" : ""}" href="${enUrl}" hreflang="en" lang="en" data-lang-choice="en">
        <span class="langSwitch-code">EN</span>
        <span class="langSwitch-name">English</span>
      </a>
    </nav>`;

  const productName = (id, lang) => {
    const item = PRODUCTS.find((p) => p.id === id) || PRODUCTS[0];
    return lang === "en" ? item.en : item.es;
  };

  const field = (id, label, type = "text", required = true, extra = "") => `
    <label class="qField">${escapeHtml(label)}
      <input ${required ? "required" : ""} type="${type}" data-field="${id}" ${extra}>
    </label>`;

  const select = (id, label, options) => `
    <label class="qField">${escapeHtml(label)}
      <select required data-field="${id}">
        <option value="">—</option>
        ${options.map((o) => `<option>${escapeHtml(o)}</option>`).join("")}
      </select>
    </label>`;

  const yesNo = (lang) => lang === "en" ? ["Yes", "No"] : ["Sí", "No"];
  const unsure = (lang) => lang === "en" ? ["No", "Yes", "Not sure"] : ["No", "Sí", "No estoy seguro/a"];
  const yesUnsure = (lang) => lang === "en" ? ["Yes", "No", "Not sure"] : ["Sí", "No", "No estoy seguro/a"];

  const worksheet = (id, lang, state) => {
    const h = lang === "es";
    if (id === "auto") {
      return [
        field("fullName", h ? "Nombre completo" : "Full name"),
        field("dob", h ? "Fecha de nacimiento" : "Date of birth", "date"),
        field("address", h ? "Dirección" : "Address"),
        field("vin1", "VIN #1"),
        field("vin2", "VIN #2 (si aplica)", "text", false),
        field("vin3", "VIN #3 (si aplica)", "text", false),
        select("autoCoverage", h ? "¿Cobertura completa o responsabilidad civil?" : "Full coverage or liability?",
          h ? ["Cobertura completa", "Responsabilidad civil"] : ["Full coverage", "Liability"]),
        field("licenseState", h ? "Estado de licencia" : "Driver license state"),
        field("license", h ? "Número de licencia" : "Driver license number"),
        field("currentCarrier", h ? "Aseguradora actual" : "Current insurer"),
        field("phone", h ? "Teléfono" : "Phone", "tel"),
        field("email", "Email", "email")
      ].join("");
    }
    if (id === "home") {
      return [
        field("fullName", h ? "Nombre del propietario" : "Homeowner name"),
        field("dob", h ? "Fecha de nacimiento" : "Date of birth", "date"),
        field("propertyAddress", h ? "Dirección de la propiedad" : "Property address"),
        field("currentCarrier", h ? "Aseguradora actual" : "Current insurer"),
        select("claims3", h ? "¿Reclamos en la propiedad durante los últimos 3 años?" : "Claims on the property during the past 3 years?", unsure(lang)),
        field("phone", h ? "Teléfono para devolverte la llamada" : "Callback phone", "tel"),
        field("email", "Email", "email")
      ].join("");
    }
    if (id === "pet") {
      const plan = state.petPlan || "Accident & illness";
      return `
        <div class="petPlans">
          <p>${h ? "Elige una dirección de cobertura antes de cotizar" : "Choose a coverage direction before quoting"}</p>
          <button type="button" data-pet-plan="Accident & illness" class="${plan === "Accident & illness" ? "picked" : ""}">
            <b>${h ? "Accidentes y enfermedades" : "Accident & illness"}</b>
            <span>${h ? "Puede considerar lesiones accidentales y enfermedades elegibles." : "May consider eligible accidental injuries and illnesses."}</span>
          </button>
          <button type="button" data-pet-plan="Accident, illness & wellness" class="${plan === "Accident, illness & wellness" ? "picked" : ""}">
            <b>${h ? "Accidentes, enfermedades y bienestar" : "Accident, illness & wellness"}</b>
            <span>${h ? "Incluye la conversación de bienestar preventivo, según el plan final." : "Includes a preventive-wellness conversation, subject to the final plan."}</span>
          </button>
        </div>
        ${field("fullName", h ? "Nombre completo" : "Full name")}
        ${field("dob", h ? "Fecha de nacimiento" : "Date of birth", "date")}
        ${field("address", h ? "Dirección" : "Address")}
        ${field("email", "Email", "email")}
        ${field("phone", h ? "Teléfono" : "Phone", "tel")}
        ${field("petName", h ? "Nombre de la mascota" : "Pet name")}
        ${select("species", h ? "¿Perro o gato?" : "Dog or cat?", h ? ["Perro", "Gato"] : ["Dog", "Cat"])}
        ${field("breed", h ? "Raza" : "Breed")}
        ${select("gender", h ? "Sexo" : "Gender", h ? ["Macho", "Hembra"] : ["Male", "Female"])}
        ${field("petAge", h ? "Edad de la mascota" : "Pet age")}
        ${select("vetExam", h ? "¿Examen veterinario completo en los últimos 12 meses?" : "Comprehensive veterinarian exam in the past 12 months?", yesUnsure(lang))}
        ${select("spayed", h ? "¿Esterilizado/a?" : "Spayed or neutered?", yesUnsure(lang))}
        ${select("reimbursement", h ? "Preferencia de reembolso" : "Reimbursement preference", ["70%", "80%", "90%"])}
        ${select("deductible", h ? "Preferencia de deducible" : "Deductible preference", ["$250", "$500", "$1,000"])}
        ${select("annualMax", h ? "Límite anual deseado" : "Desired annual maximum", ["$5,000", "$10,000", "$15,000"])}
      `;
    }
    if (id === "business") {
      return [
        field("businessName", h ? "Nombre legal del negocio" : "Legal business name"),
        field("businessAddress", h ? "Dirección del negocio" : "Business address"),
        field("taxId", h ? "Tax ID del negocio" : "Business Tax ID"),
        field("yearCreated", h ? "Año de creación" : "Year created"),
        select("workersComp", h ? "¿Compensación laboral?" : "Workers’ compensation?", yesNo(lang)),
        select("generalLiability", h ? "¿Responsabilidad civil general?" : "General liability?", yesNo(lang)),
        field("owner", h ? "Nombre del propietario" : "Owner’s name"),
        field("ownerDob", h ? "Fecha de nacimiento del propietario" : "Owner date of birth", "date"),
        field("phone", h ? "Teléfono" : "Phone", "tel"),
        field("email", "Email", "email"),
        field("businessType", h ? "Tipo de negocio" : "Type of business"),
        field("employees", h ? "Cantidad de empleados" : "Employees"),
        field("payroll", h ? "Nómina estimada" : "Estimated payroll"),
        field("gross", h ? "Ingresos brutos estimados" : "Estimated gross revenue"),
        field("currentCarrier", h ? "Aseguradora actual" : "Current insurer")
      ].join("");
    }
    if (id === "commercial-auto") {
      return [
        field("businessName", h ? "Nombre del negocio" : "Business name"),
        field("businessAddress", h ? "Dirección del negocio" : "Business address"),
        field("businessType", h ? "Tipo de negocio" : "Type of business"),
        field("taxId", h ? "Tax ID del negocio" : "Business Tax ID"),
        field("dot", "DOT # (si aplica)", "text", false),
        field("owner", h ? "Nombre del propietario" : "Owner’s name"),
        field("ownerDob", h ? "Fecha de nacimiento del propietario" : "Owner date of birth", "date"),
        field("truckVin", h ? "VIN del camión" : "Truck VIN"),
        field("truckValue", h ? "Valor del camión" : "Truck value"),
        field("cargo", h ? "Monto de carga requerido" : "Cargo amount needed"),
        select("commercialCoverage", h ? "¿Cobertura completa o responsabilidad?" : "Full coverage or liability?",
          h ? ["Cobertura completa", "Responsabilidad civil"] : ["Full coverage", "Liability"]),
        field("currentCarrier", h ? "Aseguradora actual" : "Current insurer"),
        field("driver", h ? "Nombre del conductor" : "Driver’s name"),
        field("driverDob", h ? "Fecha de nacimiento del conductor" : "Driver date of birth", "date"),
        field("license", h ? "Número de licencia del conductor" : "Driver license number"),
        field("phone", h ? "Teléfono" : "Phone", "tel"),
        field("email", "Email", "email")
      ].join("");
    }
    if (id === "life") {
      return [
        field("fullName", h ? "Nombre completo" : "Full name"),
        field("dob", h ? "Fecha de nacimiento" : "Date of birth", "date"),
        field("address", h ? "Dirección" : "Address"),
        select("lifeType", h ? "Tipo de seguro de vida" : "Life insurance type",
          h ? ["Término", "Vida entera", "Gastos finales", "No estoy seguro/a"] : ["Term", "Whole life", "Final expense", "Not sure"]),
        field("phone", h ? "Teléfono" : "Phone", "tel"),
        field("email", "Email", "email"),
        `<p class="formHint">${h
          ? "No se cotiza salud, Medicare ni disability. Sin examen no significa aprobación garantizada."
          : "Health, Medicare and disability are not quoted. No-exam does not mean guaranteed acceptance."}</p>`
      ].join("");
    }
    return [
      field("fullName", h ? "Nombre completo" : "Full name"),
      field("dob", h ? "Fecha de nacimiento" : "Date of birth", "date"),
      field("rentalAddress", h ? "Dirección de renta" : "Rental address"),
      field("moveDate", h ? "Fecha de mudanza" : "Move-in date", "date"),
      field("contents", h ? "Valor aproximado de pertenencias" : "Approximate belongings value"),
      field("phone", h ? "Teléfono" : "Phone", "tel"),
      field("email", "Email", "email"),
      `<p class="formHint">${h
        ? "La agencia aún no ha enviado una hoja específica de inquilinos; el agente confirmará los datos adicionales necesarios antes de cotizar."
        : "The agency has not yet supplied a renters-specific worksheet; the agent will confirm any additional details needed before quoting."}</p>`
    ].join("");
  };

  const initQuote = (root) => {
    const lang = root.dataset.lang || "es";
    const h = lang === "es";
    const notice = h
      ? "No ingreses Seguro Social, información bancaria ni contraseñas. La forma de pago se entrega únicamente por el canal oficial y seguro de la aseguradora."
      : "Do not enter Social Security, banking information, or passwords. Payment instructions are provided only through the insurer’s official secure channel.";
    const state = { product: "auto", step: 1, done: false, petPlan: "Accident & illness", values: {} };

    const readFields = () => {
      root.querySelectorAll("[data-field]").forEach((el) => {
        state.values[el.dataset.field] = el.value;
      });
    };

    const fillFields = () => {
      root.querySelectorAll("[data-field]").forEach((el) => {
        if (state.values[el.dataset.field]) el.value = state.values[el.dataset.field];
      });
    };

    const render = () => {
      const p = PRODUCTS.find((x) => x.id === state.product) || PRODUCTS[0];
      if (state.done) {
        root.innerHTML = `
          <div class="success">
            <span>✓</span>
            <h3>${h ? "Solicitud lista para revisión." : "Request ready for review."}</h3>
            <p>${h
              ? "Este prototipo verifica que completes la hoja correcta. Antes de recibir solicitudes reales, Carolinas Coverage conectará el canal seguro de entrega; el agente confirmará disponibilidad, opciones y el enlace de pago oficial. No se cobra ni se emite una póliza en esta página."
              : "This prototype verifies that you complete the right worksheet. Before it receives live requests, Carolinas Coverage will connect the secure delivery channel; an agent will confirm availability, options, and the official payment link. No payment is collected and no policy is issued on this page."}</p>
            <a class="button" href="tel:+17045953429">704-595-3429</a>
            <button class="back" type="button" data-quote-reset>${h ? "Iniciar otra solicitud" : "Start another request"}</button>
          </div>`;
        return;
      }

      const progress = `
        <div class="progress">
          <b class="${state.step >= 1 ? "on" : ""}">1</b><i></i>
          <b class="${state.step >= 2 ? "on" : ""}">2</b><i></i>
          <b class="${state.step >= 3 ? "on" : ""}">3</b>
        </div>`;

      if (state.step === 1) {
        root.innerHTML = `
          <form>
            ${progress}
            <h3>${h ? "¿Qué quieres cotizar?" : "What would you like to quote?"}</h3>
            <div class="types typesSix">
              ${PRODUCTS.map((item) => `
                <button type="button" class="${state.product === item.id ? "selected" : ""}" data-quote-type="${item.id}">
                  ${item.icon}<span>${h ? item.es : item.en}</span>
                </button>`).join("")}
            </div>
            <label class="qField">${h ? "Código postal" : "ZIP code"}
              <input required inputmode="numeric" pattern="[0-9]{5}" placeholder="28273" data-field="zip">
            </label>
            <button class="button wide" type="submit">${h ? "Continuar con mi formulario" : "Continue to my form"} →</button>
          </form>`;
      } else if (state.step === 2) {
        root.innerHTML = `
          <form>
            ${progress}
            <h3>${p.icon} ${productName(p.id, lang)}</h3>
            <p class="formHint">${h
              ? "Completa esta hoja para que el agente pueda preparar una cotización real. Los campos corresponden a la hoja de cotización de Carolinas Coverage."
              : "Complete this worksheet so the agent can prepare a real quote. The fields follow Carolinas Coverage’s quote worksheet."}</p>
            <div class="quoteFields">
              ${worksheet(p.id, lang, state)}
              ${field("source", h ? "¿Cómo se enteró de nosotros? (opcional)" : "How did you hear about us? (optional)", "text", false)}
            </div>
            <label class="consent">
              <input required type="checkbox">
              <span>${h
                ? "Autorizo a Carolinas Coverage a contactarme sobre esta solicitud. Entiendo que la cotización, cobertura y emisión final requieren revisión y aprobación de la aseguradora."
                : "I authorize Carolinas Coverage to contact me about this request. I understand final quote, coverage, and issuance require insurer review and approval."}</span>
            </label>
            <div class="formactions">
              <button class="back" type="button" data-quote-back>← ${h ? "Atrás" : "Back"}</button>
              <button class="button" type="submit">${h ? "Revisar mi solicitud" : "Review my request"} →</button>
            </div>
          </form>`;
      } else {
        root.innerHTML = `
          <form>
            ${progress}
            <h3>${h ? "Confirma el siguiente paso." : "Confirm the next step."}</h3>
            <div class="reviewBox">
              <b>${p.icon} ${productName(p.id, lang)}</b>
              <p>${h
                ? "Tu formulario está completo. Elegirás cómo quieres recibir la cotización y, solo si la apruebas, el enlace de pago oficial."
                : "Your worksheet is complete. Choose how you want to receive the quote and, only if you approve it, the official payment link."}</p>
            </div>
            ${select("contactPreference",
              h ? "¿Cómo prefieres recibir opciones y el enlace de pago?" : "How would you prefer to receive options and a payment link?",
              h ? ["Correo electrónico", "Mensaje de texto", "Llamada"] : ["Email", "Text message", "Phone call"])}
            <label class="consent">
              <input required type="checkbox">
              <span>${notice}</span>
            </label>
            <div class="formactions">
              <button class="back" type="button" data-quote-back>← ${h ? "Editar datos" : "Edit details"}</button>
              <button class="button" type="submit">${h ? "Enviar para revisión" : "Send for review"} →</button>
            </div>
          </form>`;
      }
      fillFields();
    };

    root.addEventListener("click", (event) => {
      const typeBtn = event.target.closest("[data-quote-type]");
      if (typeBtn) {
        readFields();
        state.product = typeBtn.dataset.quoteType;
        render();
        return;
      }
      const petBtn = event.target.closest("[data-pet-plan]");
      if (petBtn) {
        readFields();
        state.petPlan = petBtn.dataset.petPlan;
        render();
        return;
      }
      if (event.target.closest("[data-quote-back]")) {
        readFields();
        state.step = Math.max(1, state.step - 1);
        render();
        return;
      }
      if (event.target.closest("[data-quote-reset]")) {
        state.step = 1;
        state.done = false;
        render();
      }
    });

    root.addEventListener("submit", (event) => {
      event.preventDefault();
      if (!event.target.checkValidity()) return;
      readFields();
      if (state.step === 1) state.step = 2;
      else if (state.step === 2) state.step = 3;
      else state.done = true;
      render();
    });

    document.querySelectorAll("[data-quote-product]").forEach((btn) => {
      btn.addEventListener("click", () => {
        state.product = btn.dataset.quoteProduct;
        state.step = 1;
        state.done = false;
        render();
        document.getElementById("quote")?.scrollIntoView({ behavior: "smooth" });
      });
    });

    render();
    window.CCQuote = { setProduct(id) { state.product = id; state.step = 1; state.done = false; render(); } };
  };

  const initService = (root) => {
    const dataEl = document.getElementById("service-data");
    if (!dataEl) return;
    const data = JSON.parse(dataEl.textContent);
    const copy = data.copy;
    const lang = data.lang;
    const h = lang === "es";
    const state = { step: 0, choice: "", done: false };

    const fieldLabel = (label, textarea = false) =>
      `<label>${escapeHtml(label)}${textarea
        ? `<textarea rows="3" placeholder="Demo"></textarea>`
        : `<input placeholder="Demo">`}</label>`;

    const render = () => {
      if (state.done) {
        root.innerHTML = `
          <div class="innerTop">
            <a class="serviceBack" href="${data.home}">← Carolinas Coverage</a>
            ${langSwitchHtml(lang === "en", data.esUrl || "/es", data.enUrl || "/")}
          </div>
          <section class="serviceShell">
            <p class="eyebrow">${h ? "VISTA PREVIA DEL FLUJO" : "WORKFLOW PREVIEW"}</p>
            <div class="serviceSuccess">
              <span>✓</span>
              <h1>${h ? "Solicitud lista para revisión" : "Request ready for review"}</h1>
              <p>${escapeHtml(copy.complete)}</p>
              <div>
                <b>${h ? "Próximo paso simulado" : "Simulated next step"}</b>
                <p>${h
                  ? "1. El formulario se valida. 2. Se asigna al equipo. 3. El cliente recibe confirmación por email o SMS. 4. La aseguradora o el agente confirma cualquier cambio real."
                  : "1. The form is validated. 2. It is assigned to the team. 3. The client receives confirmation by email or SMS. 4. The insurer or agent confirms any real change."}</p>
              </div>
              <button type="button" data-service-reset>${h ? "Reiniciar demostración" : "Restart demonstration"}</button>
            </div>
          </section>`;
        return;
      }

      const emergency = data.type === "claim"
        ? `<p class="serviceEmergency">${h
          ? "Emergencia: llama al 911. Este formulario no reemplaza atención de emergencia ni un reporte oficial de reclamo."
          : "Emergency: call 911. This form does not replace emergency care or an official claim report."}</p>`
        : "";

      let body = "";
      if (state.step === 0) {
        body = `
          <h2>${escapeHtml(copy.steps[0])}</h2>
          <p>${h
            ? "Primero identificamos la póliza. En producción, esta información se validará contra la fuente autorizada."
            : "First we identify the policy. In production, this information will be validated against the authorized source."}</p>
          <div class="serviceFields">
            ${fieldLabel(copy.fields[0])}
            ${fieldLabel(copy.fields[1])}
            ${fieldLabel(copy.fields[2])}
          </div>`;
      } else if (state.step === 1) {
        body = `
          <h2>${escapeHtml(copy.steps[1])}</h2>
          <p>${h ? "Selecciona la opción que mejor describe tu solicitud." : "Select the option that best describes your request."}</p>
          <div class="serviceChoices">
            ${copy.choices.map((choice) =>
              `<button type="button" class="${state.choice === choice ? "chosen" : ""}" data-choice="${escapeHtml(choice)}">${state.choice === choice ? "✓ " : ""}${escapeHtml(choice)}</button>`
            ).join("")}
          </div>`;
      } else if (state.step === 2) {
        const hint = data.type === "claim"
          ? (h
            ? "Las fotos o documentos se cargarán mediante un enlace seguro cuando se conecte el sistema. No adjuntes documentos reales en esta demostración."
            : "Photos or documents will upload through a secure link when the system is connected. Do not attach real documents in this demonstration.")
          : (h
            ? "Completa los detalles que permiten al equipo preparar la respuesta correcta."
            : "Complete the details that let the team prepare the right response.");
        body = `
          <h2>${escapeHtml(copy.steps[2])}</h2>
          <p>${hint}</p>
          <div class="serviceFields">
            ${copy.fields.slice(3).map((label, index) => fieldLabel(label, index === 1)).join("")}
          </div>
          ${data.type === "claim" ? `
            <div class="demoUpload">
              <b>＋ ${h ? "Agregar fotos o documentos (demo)" : "Add photos or documents (demo)"}</b>
              <small>${h ? "La conexión segura se activará antes de aceptar archivos reales." : "The secure connection will activate before real files are accepted."}</small>
            </div>` : ""}`;
      } else {
        body = `
          <h2>${escapeHtml(copy.steps[3])}</h2>
          <div class="serviceReview">
            <b>${escapeHtml(state.choice || (h ? "Solicitud seleccionada" : "Selected request"))}</b>
            <p>${escapeHtml(copy.summary)}</p>
            <p>${escapeHtml(copy.safety)}</p>
          </div>
          <label class="serviceConsent">
            <input type="checkbox">
            ${h
              ? "Entiendo que esta es una demostración y autorizo un contacto cuando se habilite el flujo real."
              : "I understand this is a demonstration and authorize contact when the real workflow is enabled."}
          </label>`;
      }

      root.innerHTML = `
        <div class="innerTop">
          <a class="serviceBack" href="${data.home}">← Carolinas Coverage</a>
          ${langSwitchHtml(lang === "en", data.esUrl || "/es", data.enUrl || "/")}
        </div>
        <section class="serviceShell">
          <div class="serviceIntro">
            <span>${copy.icon}</span>
            <div>
              <p class="eyebrow">${h ? "CENTRO DE SERVICIO - DEMOSTRACIÓN" : "SERVICE CENTER - DEMONSTRATION"}</p>
              <h1>${escapeHtml(copy.title)}</h1>
              <p>${escapeHtml(copy.intro)}</p>
            </div>
          </div>
          ${emergency}
          <ol class="serviceProgress">
            ${copy.steps.map((step, index) =>
              `<li class="${index <= state.step ? "active" : ""}"><b>${index + 1}</b><span>${escapeHtml(step)}</span></li>`
            ).join("")}
          </ol>
          <section class="serviceForm">
            ${body}
            <div class="serviceActions">
              ${state.step > 0
                ? `<button type="button" class="serviceSecondary" data-service-back>← ${h ? "Atrás" : "Back"}</button>`
                : "<span></span>"}
              ${state.step < 3
                ? `<button type="button" data-service-next>${h ? "Continuar →" : "Continue →"}</button>`
                : `<button type="button" data-service-finish>${escapeHtml(copy.review)}</button>`}
            </div>
          </section>
          <p class="serviceFineprint">${h
            ? "Demostración solamente: no se guarda ni transmite información, no se emite cobertura y no se procesa un pago. La integración con EzLynx, aseguradoras, email/SMS y pago seguro se activará después de la autorización y configuración oficial."
            : "Demonstration only: no information is stored or sent, no coverage is issued, and no payment is processed. The EzLynx, carrier, email/SMS, and secure-payment integrations will activate after official authorization and setup."}</p>
        </section>`;
    };

    root.addEventListener("click", (event) => {
      const choice = event.target.closest("[data-choice]");
      if (choice) {
        state.choice = choice.dataset.choice;
        render();
        return;
      }
      if (event.target.closest("[data-service-back]")) {
        state.step = Math.max(0, state.step - 1);
        render();
        return;
      }
      if (event.target.closest("[data-service-next]")) {
        state.step = Math.min(3, state.step + 1);
        render();
        return;
      }
      if (event.target.closest("[data-service-finish]")) {
        state.done = true;
        render();
        return;
      }
      if (event.target.closest("[data-service-reset]")) {
        state.done = false;
        state.step = 0;
        state.choice = "";
        render();
      }
    });

    render();
  };

  const initPortal = () => {
    const buttons = [...document.querySelectorAll("[data-portal-item]")];
    if (!buttons.length) return;
    buttons.forEach((button) => {
      button.addEventListener("click", () => {
        buttons.forEach((item) => item.classList.remove("open"));
        button.classList.add("open");
      });
    });
  };

  const initLanguage = () => {
    document.addEventListener("click", (event) => {
      const link = event.target.closest("a[data-lang-choice]");
      if (!link) return;
      rememberLang(link.dataset.langChoice);
    });
  };

  document.addEventListener("DOMContentLoaded", () => {
    initLanguage();
    const quote = document.getElementById("quote-root");
    if (quote) initQuote(quote);
    const serviceData = document.getElementById("service-data");
    const service = serviceData ? document.querySelector("main.servicePage") : null;
    if (service) initService(service);
    initPortal();
  });
})();
