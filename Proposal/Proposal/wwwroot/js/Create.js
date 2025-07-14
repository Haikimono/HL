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

    // 画面初期表示時にViewBagの値に基づいて画面を切り替え
    // ページ初期表示時、ViewBagの値によりタブを自動で切り替える
    var flagElem = document.getElementById('showProposalContentFlag'); // id='showProposalContentFlag'の要素（サーバーからのタブ表示フラグ）を取得
    var showProposalContent = flagElem && flagElem.textContent.trim() === 'true'; // flagElemが存在すれば、その内容をtrimして'true'かどうか判定
    setTimeout(function() {
        var btnTeian = document.getElementById('btnTeian'); // 'btnTeian'（提案内容タブボタン）の要素を取得
        var btnBase = document.getElementById('btnBase');   // 'btnBase'（基本情報タブボタン）の要素を取得
        if (showProposalContent) { // showProposalContentがtrueなら（提案内容タブを表示したい場合）
            if (btnTeian) btnTeian.click(); // btnTeianが存在すれば、クリックをシミュレート
        } else { // それ以外（基本情報タブを表示したい場合）
            if (btnBase) btnBase.click(); // btnBaseが存在すれば、クリックをシミュレート
        }
    }, 0); // 0ミリ秒遅延で実行し、ボタンが描画済みであることを保証
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
    document.querySelectorAll("input[type='text'], textarea").forEach(e => {
        if (e.id !== "teianYear") { 
            e.value = "";
        }
    });

    document.querySelectorAll("select").forEach(e => e.selectedIndex = 0);

    document.querySelectorAll("input[type='checkbox']").forEach(e => e.checked = false);

    for (let i = 1; i <= 5; i++) {
        const fileInput = document.getElementById(`file${i}`);
        const fileNameBox = document.getElementById(`fileName${i}`);
        if (fileInput) fileInput.value = "";
        if (fileNameBox) fileNameBox.value = "";
    }
}


