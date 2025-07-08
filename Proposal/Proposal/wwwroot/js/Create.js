document.addEventListener("DOMContentLoaded", function () {
    //添付ファイル
    for (let i = 1; i <= 5; i++) {
        const btn = document.getElementById(`btn${i}`);
        const fileInput = document.getElementById(`file${i}`);
        const fileNameInput = document.getElementById(`fileName${i}`);

        if (!btn || !fileInput || !fileNameInput) {
            console.warn(`Missing element for file@${i}`);
            continue;
        }

        btn.addEventListener("click", function () {
            fileInput.click();
        });

        fileInput.addEventListener("change", function () {
            if (fileInput.files.length > 0) {
                fileNameInput.value = fileInput.files[0].name;
            } else {
                fileNameInput.value = '';
            }
        });
    }
});

//基本情報と提案内容
function showDiv(target) {
    const baseDiv = document.getElementById('baseDiv');
    const teianDiv = document.getElementById('teianDiv');
    const btnBase = document.getElementById('btnBase');
    const btnTeian = document.getElementById('btnTeian');

    baseDiv.style.display = target === 'base' ? 'block' : 'none';
    teianDiv.style.display = target === 'teian' ? 'block' : 'none';

    btnBase.classList.toggle('btn-primary', target === 'base');
    btnBase.classList.toggle('btn-outline-primary', target !== 'base');

    btnTeian.classList.toggle('btn-primary', target === 'teian');
    btnTeian.classList.toggle('btn-outline-primary', target !== 'teian');
}

//初期化
function initializeForm() {
    document.querySelectorAll("input[type='text'], textarea").forEach(e => e.value = "");

    document.querySelectorAll("select").forEach(e => e.selectedIndex = 0);

    document.querySelectorAll("input[type='checkbox']").forEach(e => e.checked = false);

    for (let i = 1; i <= 5; i++) {
        const fileInput = document.getElementById(`file${i}`);
        const fileNameBox = document.getElementById(`fileName${i}`);
        if (fileInput) fileInput.value = "";
        if (fileNameBox) fileNameBox.value = "";
    }
}



