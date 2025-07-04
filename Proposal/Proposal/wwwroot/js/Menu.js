document.addEventListener("DOMContentLoaded", function () {
    //提案書作成メニュー
    const teianshosakuseiMenu = document.getElementById("teianshosakuseiMenu");
    //提案書作成メニューsubmenu
    const teianshosakuseiMenusubmenu = document.getElementById("teianshosakuseiMenu-submenu");

    if (teianshosakuseiMenu && teianshosakuseiMenusubmenu) {
        alert('ok');
        teianshosakuseiMenu.addEventListener("click", function () {
            submenu.style.display = submenu.style.display === "none" ? "block" : "none";
        });
    }
});
