---
name: majestic-seo
description: >-
  Reviews and enriches Majestic Roofing SEO from the bilingual search-intent
  map (Excel + keyword Word). Use when the user asks for SEO, keywords, titles,
  H1, FAQ, hreflang, sitemap, landing pages, Search Console, Google Business
  Profile copy, or to rank roof repair/replacement/claims pages in Charlotte NC
  or Rock Hill SC.
---

# Majestic Roofing SEO agent

Source of truth: `Documentos/seo/seoroofingt/majestic_roofing_seo_search_scenarios.xlsx` and `Documentos/seo/majestic_roofing_palabras_clave_busqueda_EN_ES.docx`.

## Review then enrich

1. Match the URL to one intent family. Do not create a new page for a modifier (`near me`, `cost`, `same day`, `Raleigh`).
2. Put the primary keyword in `<title>` (catalog), one H1, first paragraph, and one H2. Add 4–7 FAQs using Word phrases.
3. Internal-link related P0/P1 pages. Keep EN/ES pairs in `Seo/SeoCatalog.cs` and `wwwroot/js/lang-boot.js`.
4. NAP: 5122 Belstone Lane, Charlotte, NC 28215 · (980) 800-5991 · `ericb.majestic@gmail.com`.
5. After HTML changes, restart `dotnet run` and check `/`, `/es`, the new URL, `/sitemap.xml`.

## Compliance (never contradict)

- Homeowner files the claim; carrier decides coverage. Majestic inspects, documents, estimates construction. Not a public adjuster.
- No promise of claim approval, deductible waiver, 24/7, or same-day **repair**. Same-**business-day response** is allowed.
- Raleigh is **outside** the service area. Do not publish Raleigh landing pages.
- English and Spanish must be equivalent and natural (Spanglish OK on `/es` latino queries).

## P0 URLs (built)

Keep and enrich: `/roof-repair-charlotte-nc`, `/roof-replacement-charlotte-nc`, `/roof-installation-charlotte-nc`, `/emergency-roof-repair-charlotte-nc`, `/storm-damage-roof-repair-charlotte-nc`, `/roof-claim-process`, `/roof-claim-denied-or-underpaid`, `/free-roof-inspection-charlotte-nc`, `/roof-replacement-cost-charlotte-nc`, `/es/reemplazo-techo-charlotte-nc` plus EN/ES pairs in `SeoCatalog`.

Keep existing slugs (301 from the Excel slug if needed): leak → `/active-roof-leak-repair-charlotte-nc`, hail → `/hail-damage-roof-inspection-charlotte-nc`, wind → `/wind-missing-shingles-repair-charlotte-nc`, claim ES → `/es/reclamos-de-seguro-de-techo-charlotte-nc`.

## Titles

Set unique copy in `SeoCatalog` (layout reads catalog, not ViewData Title). 50–60 characters when possible. Include Charlotte NC on local P0 pages.
