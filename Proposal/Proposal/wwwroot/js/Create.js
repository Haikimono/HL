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

    // 編集権限の制御
    // statusがnullまたは1以外の場合は編集不可
    var statusElement = document.getElementById('proposalStatus');
    if (statusElement) {
        var status = parseInt(statusElement.textContent.trim());
        var isEditable = isNaN(status) || status === 1;
        
        if (!isEditable) {
            // 基本情報区域のすべてのコントロールを無効化
            var baseDiv = document.getElementById('baseDiv');
            if (baseDiv) {
                var controls = baseDiv.querySelectorAll('input, select, textarea, button[type="button"]');
                controls.forEach(function(control) {
                    if (control.type !== 'submit' && control.type !== 'button') {
                        control.disabled = true;
                    }
                });
            }
            
            // 提案内容区域のすべてのコントロールを無効化
            var teianDiv = document.getElementById('teianDiv');
            if (teianDiv) {
                var controls = teianDiv.querySelectorAll('input, select, textarea, button[type="button"]');
                controls.forEach(function(control) {
                    if (control.type !== 'submit' && control.type !== 'button') {
                        control.disabled = true;
                    }
                });
            }
            
            // 提出ボタンを無効化（戻るボタンは除く）
            var submitButtons = document.querySelectorAll('button[type="submit"]');
            submitButtons.forEach(function(button) {
                if (button.value !== 'Menu') {
                    button.disabled = true;
                }
            });
        }
    }

    // 提案の区分によるグループ情報表示切替
    function toggleGroupSection() {
        var teianKbn = document.getElementById('teianKbn');
        var groupSection = document.getElementById('groupSection');
        if (!teianKbn || !groupSection) return;
        // "グループ"の値（enum値2）で表示、それ以外で非表示
        if (teianKbn.value == "2") {
            groupSection.style.display = '';
        } else {
            groupSection.style.display = 'none';
        }
    }
    var teianKbn = document.getElementById('teianKbn');
    if (teianKbn) {
        teianKbn.addEventListener('change', toggleGroupSection);
        toggleGroupSection(); // 初期表示時も実行
    }

    // 「第一次審査者を経ずに提出する」チェック時、下の入力欄を無効化
    function toggleDaiichishinsashaInputs() {
        var checkbox = document.getElementById('check');
        var target = document.getElementById('daiichishinsashaInputs');
        if (!checkbox || !target) return;
        var disabled = checkbox.checked;
        var controls = target.querySelectorAll('input, select, textarea');
        controls.forEach(function(ctrl) {
            ctrl.disabled = disabled;
        });
    }
    var daiichiCheckbox = document.getElementById('check');
    if (daiichiCheckbox) {
        daiichiCheckbox.addEventListener('change', toggleDaiichishinsashaInputs);
        toggleDaiichishinsashaInputs(); // 初期表示時も実行
    }
});

//基本情報と提案内容
function showDiv(target) {
    const baseDiv = document.getElementById('baseDiv');
    const teianDiv = document.getElementById('teianDiv');
    const btnBase = document.getElementById('btnBase');
    const btnTeian = document.getElementById('btnTeian');

    // 新しいcssクラスを使用してコンテンツ表示を制御
    if (target === 'base') {
        baseDiv.classList.add('active');
        teianDiv.classList.remove('active');
    } else {
        baseDiv.classList.remove('active');
        teianDiv.classList.add('active');
    }

    // 新しいnavタブのactiveクラス制御
    btnBase.classList.toggle('active', target === 'base');
    btnTeian.classList.toggle('active', target === 'teian');

    // 古いBootstrapボタンクラスも維持（互換性のため）
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


