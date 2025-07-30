document.addEventListener("DOMContentLoaded", function () {
    // ドロップダウンのoption HTML文字列をdata属性から取得し、グローバル変数に格納する
    // これにより、グループメンバー追加時などに各種セレクトボックスの選択肢を動的に生成できる
    // Razor側で生成したoptionリストをJSで再利用するため、必ず必要な処理
    var dropdownOptions = document.getElementById('dropdownOptions');
    if (dropdownOptions) {
        // 所属のoptionリスト
        window.affiliationOptions = dropdownOptions.getAttribute('data-affiliation-options') || '';
        // 部・署のoptionリスト
        window.departmentOptions = dropdownOptions.getAttribute('data-department-options') || '';
        // 課・部門のoptionリスト
        window.sectionOptions = dropdownOptions.getAttribute('data-section-options') || '';
        // 係・担当のoptionリスト
        window.subsectionOptions = dropdownOptions.getAttribute('data-subsection-options') || '';
    }

    //添付ファイル
    for (let i = 1; i <= 5; i++) {
        const btn = document.getElementById(`btn${i}`);
        const fileInput = document.getElementById(`file${i}`);
        const fileNameInput = document.getElementById(`fileName${i}`);

        if (!btn || !fileInput || !fileNameInput) {
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
    var flagElem = document.getElementById('showProposalContentFlag');
    var showProposalContent = flagElem && flagElem.textContent.trim() === 'true';
    if (showProposalContent) {
        showDiv('teian');
    } else {
        showDiv('base');
    }

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
        if (teianKbn.value.trim() === "2") {
            groupSection.style.display = 'block';
            groupSection.hidden = false;
        } else {
            groupSection.style.display = 'none';
            groupSection.hidden = true;
        }
    }
    var teianKbn = document.getElementById('teianKbn');
    if (teianKbn) {
        teianKbn.addEventListener('change', toggleGroupSection);
        setTimeout(toggleGroupSection, 0);
    }

    // 「第一次審査者を経ずに提出する」チェック時、下の入力欄を無効化
    function toggleDaiichishinsashaInputs() {
        var checkbox = document.querySelector('input[name="BasicInfo.SkipFirstReviewer"]');
        var target = document.getElementById('daiichishinsashaInputs');
        if (!checkbox || !target) return;
        var disabled = checkbox.checked;
        var controls = target.querySelectorAll('input, select, textarea');
        controls.forEach(function(ctrl) {
            ctrl.disabled = disabled;
            if (disabled) {
                if (ctrl.tagName === 'SELECT') {
                    ctrl.selectedIndex = 0;
                } else if (ctrl.type === 'checkbox' || ctrl.type === 'radio') {
                    ctrl.checked = false;
                } else {
                    ctrl.value = '';
                }
            }
        });
    }
    
    // 等待DOM完全加载后再绑定事件
    setTimeout(function() {
        var daiichiCheckbox = document.querySelector('input[name="BasicInfo.SkipFirstReviewer"]');
        if (daiichiCheckbox) {
            daiichiCheckbox.addEventListener('change', toggleDaiichishinsashaInputs);
            toggleDaiichishinsashaInputs(); // 初期表示時も実行
        }
    }, 100);

    // 「提案の区分」による「氏名/代表名」ラベル切替
    function toggleShimeiLabel() {
        var teianKbn = document.getElementById('teianKbn');
        var shimeiLabel = document.getElementById('shimeiLabel');
        if (!teianKbn || !shimeiLabel) return;
        // value="1"は個人、value="2"はグループ
        if (teianKbn.value.trim() === "1") {
            shimeiLabel.innerHTML = '氏名 <span class="required-badge">必須</span>';
        } else if (teianKbn.value.trim() === "2") {
            shimeiLabel.innerHTML = '代表名 <span class="required-badge">必須</span>';
        } else {
            shimeiLabel.innerHTML = '氏名又は代表名 <span class="required-badge">必須</span>';
        }
    }
    var teianKbn = document.getElementById('teianKbn');
    if (teianKbn) {
        teianKbn.addEventListener('change', toggleShimeiLabel);
        setTimeout(toggleShimeiLabel, 0);
    }

    //グループメンバー追加
    let memberIndex = 0;
    const MAX_MEMBERS = 10;
    function createMemberCard(index) {
        return `
    <div class="group-member-card mb-3" data-index="${index}">
    <div class="member-header d-flex align-items-center mb-2">
        <span class="member-number me-2">${index + 1}</span>
        <span class="member-title">メンバー ${index + 1}</span>
        ${index >= 3 ? '<button type="button" class="btn btn-danger btn-sm ms-auto remove-member">削除</button>' : ''}
    </div>
        <div class="form-row-modern">
            <div class="form-group-modern me-2">
                <label class="form-label-modern">所属</label>
                <select name="BasicInfo.GroupMembers[${index}].AffiliationId" class="form-control-modern">${window.affiliationOptions}</select>
            </div>
            <div class="form-group-modern">
                <label class="form-label-modern">部・署</label>
                <select name="BasicInfo.GroupMembers[${index}].DepartmentId" class="form-control-modern">${window.departmentOptions}</select>
                <span class="validation-message validation-department"></span>
            </div>
        </div>
        <div class="form-row-modern">
            <div class="form-group-modern me-2">
                <label class="form-label-modern">課・部門</label>
                <select name="BasicInfo.GroupMembers[${index}].SectionId" class="form-control-modern">${window.sectionOptions}</select>
                <span class="validation-message validation-section"></span>
            </div>
            <div class="form-group-modern">
                <label class="form-label-modern">係・担当</label>
                <select name="BasicInfo.GroupMembers[${index}].SubsectionId" class="form-control-modern">${window.subsectionOptions}</select>
                <span class="validation-message validation-subsection"></span>
            </div>
        </div>
        <div class="form-row-modern">
            <div class="form-group-modern">
                <label class="form-label-modern">氏名</label>
                <input type="text" name="BasicInfo.GroupMembers[${index}].Name" class="form-control-modern" placeholder="氏名を入力" />
                <span class="validation-message validation-name"></span>
            </div>
        </div>
    </div>`;
    }

    function createMemberCardWithValue(index, member) {
        // 下拉菜单选中项处理
        function getOptions(options, value) {
            if (!value) return options;
            // 只给匹配的option加selected
            return options.replace(new RegExp(`value=["']${value}["']`), `value="${value}" selected`);
        }
        return `
    <div class="group-member-card mb-3" data-index="${index}">
        <div class="member-header d-flex align-items-center mb-2">
            <span class="member-number me-2">${index + 1}</span>
            <span class="member-title">メンバー ${index + 1}</span>
            ${index >= 3 ? '<button type="button" class="btn btn-danger btn-sm ms-auto remove-member">削除</button>' : ''}
        </div>
        <div class="form-row-modern">
            <div class="form-group-modern me-2">
                <label class="form-label-modern">所属</label>
                <select name="BasicInfo.GroupMembers[${index}].AffiliationId" class="form-control-modern">${getOptions(window.affiliationOptions, member.AffiliationId)}</select>
            </div>
            <div class="form-group-modern">
                <label class="form-label-modern">部・署</label>
                <select name="BasicInfo.GroupMembers[${index}].DepartmentId" class="form-control-modern">${getOptions(window.departmentOptions, member.DepartmentId)}</select>
                <span class="validation-message validation-department"></span>
            </div>
        </div>
        <div class="form-row-modern">
            <div class="form-group-modern me-2">
                <label class="form-label-modern">課・部門</label>
                <select name="BasicInfo.GroupMembers[${index}].SectionId" class="form-control-modern">${getOptions(window.sectionOptions, member.SectionId)}</select>
                <span class="validation-message validation-section"></span>
            </div>
            <div class="form-group-modern">
                <label class="form-label-modern">係・担当</label>
                <select name="BasicInfo.GroupMembers[${index}].SubsectionId" class="form-control-modern">${getOptions(window.subsectionOptions, member.SubsectionId)}</select>
                <span class="validation-message validation-subsection"></span>
            </div>
        </div>
        <div class="form-row-modern">
            <div class="form-group-modern">
                <label class="form-label-modern">氏名</label>
                <input type="text" name="BasicInfo.GroupMembers[${index}].Name" class="form-control-modern" placeholder="氏名を入力" value="${member.Name || ''}" />
                <span class="validation-message validation-name"></span>
            </div>
        </div>
    </div>`;
    }

    //メンバー№と表示設定
    function renumberGroupMembers() {
        const cards = document.querySelectorAll('#groupMembersContainer .group-member-card');
        cards.forEach((card, idx) => {
            card.querySelector('.member-number').textContent = idx + 1;
            card.querySelector('.member-title').textContent = `メンバー ${idx + 1}`;
        });
    }
    // 初期3名メンバーを表示
    window.addEventListener('DOMContentLoaded', function () {
        const container = document.getElementById('groupMembersContainer');
        let members = window.initialGroupMembers || [];
        // members.length === 0 の場合は「新規作成」時や、サーバーからグループメンバー情報が渡されていない場合です。
        // この場合、空のメンバー入力欄を3つ自動生成します。
        if (members.length === 0) {
            for (let i = 0; i < 3; i++) {
                container.insertAdjacentHTML('beforeend', createMemberCard(memberIndex++));
            }
        } else {
            // 編集やバリデーションエラー時は、サーバーから渡されたメンバー情報で入力欄を生成・値をセットします。
            members.forEach((m, i) => {
                container.insertAdjacentHTML('beforeend', createMemberCardWithValue(i, m));
                memberIndex++;
            });
        }
        renumberGroupMembers();
    });

    //メンバー人数制御
    document.getElementById('addMemberBtn').addEventListener('click', function () {
        const container = document.getElementById('groupMembersContainer');
        const currentCount = container.querySelectorAll('.group-member-card').length;
        if (currentCount >= MAX_MEMBERS) {
            alert('グループメンバーは最大10人までです。');
            return;
        }
        container.insertAdjacentHTML('beforeend', createMemberCard(memberIndex++));
        renumberGroupMembers();
    });

    //メンバー削除
    document.getElementById('groupMembersContainer').addEventListener('click', function (e) {
        if (e.target.classList.contains('remove-member')) {
            e.target.closest('.group-member-card').remove();
            renumberGroupMembers();
        }
    });

    // フォーム送信時のグループメンバー入力チェック
    const form = document.querySelector('form');
    if (form) {
        form.addEventListener('submit', function (e) {
            var teianKbn = document.getElementById('teianKbn');
            if (!teianKbn || teianKbn.value.trim() !== "2") {
                // チーム以外はチェック不要
                return;
            }
            // 既存のエラーメッセージをクリア
            document.querySelectorAll('.group-member-card .validation-message').forEach(function(msg){ msg.textContent = ''; });
            let hasError = false;
            const groupCards = document.querySelectorAll('.group-member-card');
            groupCards.forEach(function(card, idx) {
                const n = idx + 1;
                const aff = card.querySelector(`[name='BasicInfo.GroupMembers[${idx}].AffiliationId']`);
                const dep = card.querySelector(`[name='BasicInfo.GroupMembers[${idx}].DepartmentId']`);
                const sec = card.querySelector(`[name='BasicInfo.GroupMembers[${idx}].SectionId']`);
                const sub = card.querySelector(`[name='BasicInfo.GroupMembers[${idx}].SubsectionId']`);
                const name = card.querySelector(`[name='BasicInfo.GroupMembers[${idx}].Name']`);
                // 各エラーメッセージ表示用要素
                const depMsg = card.querySelector('.validation-department');
                const secMsg = card.querySelector('.validation-section');
                const subMsg = card.querySelector('.validation-subsection');
                const nameMsg = card.querySelector('.validation-name');
                // affiliation_id_Xが選択されている場合のみチェック
                if (aff && aff.value) {
                    if (!dep || !dep.value) {
                        hasError = true;
                        if(depMsg) depMsg.textContent = '部・署は必須です。';
                    }
                    if (!sec || !sec.value) {
                        hasError = true;
                        if(secMsg) secMsg.textContent = '課・部門は必須です。';
                    }
                    if (!sub || !sub.value) {
                        hasError = true;
                        if(subMsg) subMsg.textContent = '係・担当は必須です。';
                    }
                    if (!name || !name.value) {
                        hasError = true;
                        if(nameMsg) nameMsg.textContent = '氏名は必須です。';
                    } else if (name.value.length > 20) {
                        hasError = true;
                        if(nameMsg) nameMsg.textContent = '氏名は最大20文字までです。';
                    }
                }
            });
            if (hasError) {
                e.preventDefault();
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

// 組織架构テスト機能
document.addEventListener("DOMContentLoaded", function () {
    // 組織架构テストの初期化
    initializeOrganizationTest();
});

function initializeOrganizationTest() {
    let allOrganizations = [];
    
    // 加载所有组织数据
    function loadAllOrganizations() {
        $.get('/Create/GetAllOrganizations', function(data) {
            allOrganizations = data;
            loadTopLevelOrganizations();
        }).fail(function() {
            console.log('組織架构データの取得に失敗しました');
        });
    }
    
    // 加载顶级组织
    function loadTopLevelOrganizations() {
        $.get('/Create/GetTopLevelOrganizations', function(data) {
            let html = '<option value="">選択してください</option>';
            data.forEach(function(item) {
                html += `<option value="${item.value}">${item.text}</option>`;
            });
            $('#testLevel1').html(html);
        }).fail(function() {
            console.log('トップレベル組織の取得に失敗しました');
        });
    }
    
    // Level 1 选择事件
    $('#testLevel1').change(function() {
        let selectedId = $(this).val();
        updateTestResults('Level 1', selectedId);
        
        // 清空所有下级下拉菜单
        $('#testLevel2').html('<option value="">選択してください</option>');
        $('#testLevel3').html('<option value="">選択してください</option>');
        $('#testLevel4').html('<option value="">選択してください</option>');
        
        if (selectedId) {
            loadChildOrganizations(selectedId, '#testLevel2');
        }
    });
    
    // Level 2 选择事件
    $('#testLevel2').change(function() {
        let selectedId = $(this).val();
        updateTestResults('Level 2', selectedId);
        
        // 清空下级下拉菜单
        $('#testLevel3').html('<option value="">選択してください</option>');
        $('#testLevel4').html('<option value="">選択してください</option>');
        
        if (selectedId) {
            loadChildOrganizations(selectedId, '#testLevel3');
        }
    });
    
    // Level 3 选择事件
    $('#testLevel3').change(function() {
        let selectedId = $(this).val();
        updateTestResults('Level 3', selectedId);
        
        // 清空下级下拉菜单
        $('#testLevel4').html('<option value="">選択してください</option>');
        
        if (selectedId) {
            loadChildOrganizations(selectedId, '#testLevel4');
        }
    });
    
    // Level 4 选择事件
    $('#testLevel4').change(function() {
        let selectedId = $(this).val();
        updateTestResults('Level 4', selectedId);
    });
    
    // 加载子组织
    function loadChildOrganizations(parentId, targetSelector) {
        $.get('/Create/GetOrganizationsByParentId', { parentId: parentId }, function(data) {
            let html = '<option value="">選択してください</option>';
            
            // 根据targetSelector确定应该显示哪个Level的数据
            let targetLevel = 0;
            if (targetSelector === '#testLevel2') targetLevel = 2;
            else if (targetSelector === '#testLevel3') targetLevel = 3;
            else if (targetSelector === '#testLevel4') targetLevel = 4;
            
            data.forEach(function(item) {
                // 只显示对应Level的数据
                if (item.level === targetLevel) {
                    html += `<option value="${item.id}">${item.name}</option>`;
                }
            });
            $(targetSelector).html(html);
        }).fail(function() {
            console.log('子組織の取得に失敗しました');
        });
    }
    
    // 更新测试结果
    function updateTestResults(level, selectedId) {
        let selectedName = '';
        let selectedLevel = '';
        if (selectedId) {
            let org = allOrganizations.find(x => x.id === selectedId);
            selectedName = org ? org.name : '';
            selectedLevel = org ? `(Level ${org.level})` : '';
        }
        
        let newResult = `<div><strong>${level}:</strong> ${selectedId || '未選択'} ${selectedName ? `(${selectedName})` : ''} ${selectedLevel}</div>`;
        
        // 清除之前的结果，只显示当前选择
        $('#testResults').html(newResult);
    }
    
    // 初始化
    loadAllOrganizations();
}


