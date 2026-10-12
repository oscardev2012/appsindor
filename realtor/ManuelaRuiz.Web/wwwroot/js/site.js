(() => {
  const money = (n) => "$" + Math.round(Number(n) || 0).toLocaleString("en-US");
  const moneyExact = (n) => "$" + Number(n || 0).toLocaleString("en-US", { maximumFractionDigits: 0 });

  const payment = (principal, annualRate, years) => {
    const r = (annualRate / 100) / 12;
    const n = years * 12;
    if (r === 0) return principal / n;
    return (principal * r * Math.pow(1 + r, n)) / (Math.pow(1 + r, n) - 1);
  };

  const maxPrincipal = (monthlyPI, annualRate, years) => {
    const r = (annualRate / 100) / 12;
    const n = years * 12;
    if (r === 0) return monthlyPI * n;
    return monthlyPI * (Math.pow(1 + r, n) - 1) / (r * Math.pow(1 + r, n));
  };

  // Home search summary
  document.querySelectorAll("[data-home-search]").forEach((root) => {
    const form = root.querySelector(".searchForm");
    const summary = root.querySelector(".searchSummary strong");
    if (!form || !summary) return;
    const sync = () => {
      const loc = form.querySelector('[name="location"]')?.value || "Charlotte, NC";
      const min = form.querySelector('[name="minPrice"]')?.value || "0";
      const max = form.querySelector('[name="maxPrice"]')?.value || "0";
      const beds = form.querySelector('[name="beds"]')?.value || "3";
      const baths = form.querySelector('[name="baths"]')?.value || "2";
      summary.textContent = `${loc} · ${moneyExact(min)}–${moneyExact(max)} · ${beds}+ beds · ${baths}+ baths`;
    };
    form.addEventListener("input", sync);
    form.addEventListener("change", sync);
    sync();
  });

  // Mortgage calculator on home
  document.querySelectorAll("[data-mortgage-calc]").forEach((root) => {
    const numberInputs = [...root.querySelectorAll(".calcControls input[type='number']")];
    const priceInput = numberInputs[0];
    const hoaInput = numberInputs[1];
    const priceRange = root.querySelector(".calcControls input[type='range']");
    const selects = root.querySelectorAll(".calcControls select");
    const downSelect = selects[0];
    const creditSelect = selects[1];
    const termSelect = selects[2];
    const totalStrong = root.querySelector(".calcTotal strong");
    const totalSpan = root.querySelector(".calcTotal span");
    const downSmall = downSelect?.parentElement?.querySelector("small");
    const rateSmall = creditSelect?.parentElement?.querySelector("small");
    const breakdown = root.querySelector(".breakdown");
    if (!priceInput || !totalStrong || !breakdown) return;

    const rateForCredit = () => {
      const c = creditSelect?.value || "";
      if (c.startsWith("760")) return 6.15;
      if (c.startsWith("740")) return 6.35;
      if (c.startsWith("700")) return 6.65;
      if (c.startsWith("660")) return 6.95;
      if (c.startsWith("620")) return 7.35;
      return 6.85;
    };

    const sync = () => {
      const price = Number(priceInput.value) || 0;
      if (priceRange && Number(priceRange.value) !== price) priceRange.value = String(price);
      const downPct = Number(downSelect?.value || 10);
      const down = price * downPct / 100;
      const loan = Math.max(price - down, 0);
      const rate = rateForCredit();
      const years = Number(termSelect?.value || 30);
      const hoa = Number(hoaInput?.value || 0);
      const pi = payment(loan, rate, years);
      const tax = (price * 0.01) / 12;
      const insurance = (price * 0.0045) / 12;
      const pmi = downPct < 20 ? (loan * 0.007) / 12 : 0;
      const total = pi + tax + insurance + pmi + hoa;
      const loanLabel = root.dataset.loanLabel || "estimated loan";
      const assumedLabel = root.dataset.assumedLabel || "assumed";
      totalStrong.innerHTML = `${money(total)}<i>/mo</i>`;
      if (totalSpan) totalSpan.textContent = `${money(loan)} ${loanLabel} · ${rate.toFixed(2)}%`;
      if (downSmall) downSmall.textContent = money(down);
      if (rateSmall) rateSmall.textContent = `${rate.toFixed(2)}% ${assumedLabel}`;
      const rows = breakdown.querySelectorAll("span");
      const vals = [pi, tax, insurance, pmi, hoa];
      rows.forEach((row, i) => {
        const b = row.querySelector("b");
        row.textContent = "";
        if (b) row.appendChild(b);
        row.append(money(vals[i] || 0));
      });
    };

    priceInput.addEventListener("input", () => {
      if (priceRange) priceRange.value = priceInput.value;
      sync();
    });
    priceRange?.addEventListener("input", () => {
      priceInput.value = priceRange.value;
      sync();
    });
    [downSelect, creditSelect, termSelect, hoaInput].forEach((el) => el?.addEventListener("input", sync));
    [downSelect, creditSelect, termSelect].forEach((el) => el?.addEventListener("change", sync));
    sync();
  });

  // Qualify / buying power calculator
  document.querySelectorAll("[data-qualify-calc]").forEach((root) => {
    const deck = root.querySelector(".inputDeck");
    const result = root.querySelector(".qualifyResult");
    if (!deck || !result) return;
    const inputs = deck.querySelectorAll("input, select");
    const rangeEl = result.querySelector("strong");
    const facts = result.querySelectorAll(".resultFacts b");
    const lowHidden = root.querySelector("[data-range-low]");
    const highHidden = root.querySelector("[data-range-high]");
    const limitNote = result.querySelector(".limitNote");

    const sync = () => {
      const nums = [...deck.querySelectorAll('input[type="number"]')].map((i) => Number(i.value) || 0);
      const income = nums[0] || 0;
      const debts = nums[1] || 0;
      const funds = nums[2] || 0;
      const rate = nums[3] || 6.5;
      const hoa = nums[4] || 0;
      const selects = deck.querySelectorAll("select");
      const downPct = Number(selects[0]?.value || 5);
      const dti = Number(selects[1]?.value || 36) / 100;
      // Budget for PITI+HOA. Do NOT subtract HOA here — it is included in monthly housing below.
      const maxHousing = Math.max(income * dti - debts, 0);

      let home = 0;
      let down = 0;
      let limitedBy = "income";
      const tryGuess = (guess) => {
        const d = guess * downPct / 100;
        if (d > funds) return "funds";
        const loan = guess - d;
        const p = payment(loan, rate, 30);
        const tax = (guess * 0.01) / 12;
        const ins = (guess * 0.0045) / 12;
        const pmi = downPct < 20 ? (loan * 0.007) / 12 : 0;
        return (p + tax + ins + pmi + hoa) <= maxHousing ? "ok" : "income";
      };

      for (let guess = 50000; guess <= 2000000; guess += 1000) {
        const status = tryGuess(guess);
        if (status === "ok") {
          home = guess;
          down = guess * downPct / 100;
        } else {
          limitedBy = status;
          break;
        }
      }
      if (home > 0) {
        for (let guess = home + 100; guess <= home + 900; guess += 100) {
          if (tryGuess(guess) === "ok") {
            home = guess;
            down = guess * downPct / 100;
          } else break;
        }
      }
      if (home === 0 && tryGuess(50000) === "funds") limitedBy = "funds";

      const low = Math.round(home * 0.85);
      const high = Math.round(home);
      const closing = Math.round(high * 0.03);
      if (rangeEl) rangeEl.textContent = `${money(low)} – ${money(high)}`;
      if (facts[0]) facts[0].textContent = money(maxHousing);
      if (facts[1]) facts[1].textContent = money(down || high * downPct / 100);
      if (facts[2]) facts[2].textContent = money(closing);
      if (lowHidden) lowHidden.value = String(low);
      if (highHidden) highHidden.value = String(high);

      if (limitNote) {
        const isEs = document.documentElement.lang?.startsWith("es");
        const title = limitNote.querySelector("b");
        const body = limitNote.querySelector("p");
        if (limitedBy === "funds") {
          if (title) title.textContent = isEs
            ? "Hoy limitan más los fondos disponibles para el enganche."
            : "Available down-payment funds limit more today.";
          if (body) body.textContent = isEs
            ? "Eso indica qué pregunta llevar primero al lender. Programas, crédito, reservas, impuestos, seguro, HOA y documentación pueden cambiar el resultado."
            : "That shows which question to take to the lender first. Programs, credit, reserves, taxes, insurance, HOA and documents can change the result.";
          limitNote.classList.remove("income");
          limitNote.classList.add("funds");
        } else {
          if (title) title.textContent = isEs
            ? "Hoy limita más la relación ingreso/deuda."
            : "Today the income/debt ratio limits more.";
          if (body) body.textContent = isEs
            ? "Eso indica qué pregunta llevar primero al lender. Programas, crédito, reservas, impuestos, seguro, HOA y documentación pueden cambiar el resultado."
            : "That shows which question to take to the lender first. Programs, credit, reserves, taxes, insurance, HOA and documents can change the result.";
          limitNote.classList.remove("funds");
          limitNote.classList.add("income");
        }
      }
    };

    inputs.forEach((el) => {
      el.addEventListener("input", sync);
      el.addEventListener("change", sync);
    });
    sync();
  });

  // Sold portfolio filters + horizontal rail
  document.querySelectorAll("[data-sold-portfolio]").forEach((root) => {
    const rail = root.querySelector(".propertyRail");
    const cards = [...root.querySelectorAll(".propertyCard")];
    const filters = [...root.querySelectorAll(".filterBar [data-filter]")];
    const prev = root.querySelector("[data-rail-prev]");
    const next = root.querySelector("[data-rail-next]");
    if (!rail || !cards.length) return;

    const applyFilter = (key) => {
      filters.forEach((btn) => btn.classList.toggle("active", btn.dataset.filter === key));
      cards.forEach((card) => {
        const show = key === "all" || card.dataset.category === key;
        card.hidden = !show;
      });
      rail.scrollTo({ left: 0, behavior: "smooth" });
    };

    filters.forEach((btn) => {
      btn.addEventListener("click", () => applyFilter(btn.dataset.filter || "all"));
    });

    const step = () => Math.min(rail.clientWidth * 0.85, 420);
    prev?.addEventListener("click", () => rail.scrollBy({ left: -step(), behavior: "smooth" }));
    next?.addEventListener("click", () => rail.scrollBy({ left: step(), behavior: "smooth" }));
  });

  document.querySelectorAll("[data-form-switch]").forEach((root) => {
    const buyUrl = root.dataset.buyUrl;
    const sellUrl = root.dataset.sellUrl;
    const buyEmbed = root.dataset.buyEmbed;
    const sellEmbed = root.dataset.sellEmbed;
    const open = root.querySelector("[data-form-open]");
    const frame = root.querySelector("[data-form-frame]");
    const title = root.querySelector("[data-form-title]");
    const english = document.documentElement.lang === "en";
    const apply = (seller) => {
      if (open) {
        open.href = seller ? sellUrl : buyUrl;
        open.textContent = seller
          ? (english ? "Open seller questionnaire" : "Completar cuestionario de vendedor")
          : (english ? "Open buyer questionnaire" : "Completar cuestionario de comprador");
      }
      if (frame) {
        frame.src = seller ? sellEmbed : buyEmbed;
        frame.title = seller
          ? (english ? "Seller questionnaire" : "Cuestionario de vendedor")
          : (english ? "Buyer questionnaire" : "Cuestionario de comprador");
      }
      if (title) {
        title.textContent = seller
          ? (english ? "Seller questionnaire" : "Cuestionario de vendedor")
          : (english ? "Buyer questionnaire" : "Cuestionario de comprador");
      }
    };
    root.querySelectorAll('input[name="goal"]').forEach((input) => {
      input.addEventListener("change", () => apply(input.value === "sell"));
    });
  });
})();
