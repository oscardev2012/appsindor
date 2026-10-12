let lang = localStorage.getItem("safeLanguage") || "en";
function applyLanguage() {
  document.documentElement.lang = lang;
  document.querySelectorAll("[data-en]").forEach((e) => {
    e.textContent = e.dataset[lang];
  });
  document.getElementById("realtorLanguage").textContent =
    lang === "en" ? "ES" : "EN";
}
document.getElementById("realtorLanguage").onclick = () => {
  lang = lang === "en" ? "es" : "en";
  localStorage.setItem("safeLanguage", lang);
  applyLanguage();
};
applyLanguage();
