function bindCounter(input, countEl, previewEl, placeholder) {
    if (!input || !countEl) return;
    const max = Number(input.dataset.max);
    const update = () => {
        const value = (input.value || "").trim();
        const length = (input.value || "").length;
        countEl.textContent = length + " / " + max;
        countEl.classList.toggle("bad", length > max);
        countEl.classList.toggle("ok", length > 0 && length <= max);
        if (previewEl) {
            previewEl.textContent = value || placeholder;
        }
        refreshPublish();
    };
    input.addEventListener("input", update);
    update();
}

function refreshPublish() {
    const title = document.getElementById("title");
    const body = document.getElementById("body");
    const btn = document.getElementById("publish-btn");
    if (!title || !body || !btn) return;
    const t = (title.value || "").trim().length;
    const b = (body.value || "").length;
    btn.disabled = t === 0 || t > Number(title.dataset.max) || b > Number(body.dataset.max);
}

function bindImagePreview() {
    const input = document.getElementById("imageUrl");
    const img = document.getElementById("preview-image");
    const figure = document.getElementById("preview-figure");
    const card = document.getElementById("preview-card");
    if (!input || !img || !figure || !card) return;
    const update = () => {
        const url = (input.value || "").trim();
        const valid = url && /^https?:\/\//i.test(url);
        if (valid) {
            img.src = url;
        } else {
            img.removeAttribute("src");
        }
        figure.hidden = !valid;
        card.classList.toggle("has-image", !!valid);
    };
    input.addEventListener("input", update);
    update();
}

document.addEventListener("DOMContentLoaded", () => {
    bindCounter(
        document.getElementById("title"),
        document.getElementById("title-count"),
        document.getElementById("preview-title"),
        "El título aparece aquí");
    bindCounter(
        document.getElementById("body"),
        document.getElementById("body-count"),
        document.getElementById("preview-body"),
        "El texto de la noticia se muestra completo en el home.");
    bindImagePreview();
});
