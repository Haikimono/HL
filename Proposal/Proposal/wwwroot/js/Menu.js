document.addEventListener("DOMContentLoaded", function () {
    //提案書作成メニュー
    const teianshosakuseiMenu = document.getElementById("teianshosakuseiMenu");
    //提案書作成メニューsubmenu
    const teianshosakuseiMenusubmenu = document.getElementById("teianshosakuseiMenu-submenu");
    //一次審査・管理者メニュー
    const ichijishinnsakanrishaMenu = document.getElementById("ichijishinnsakanrishaMenu");
    //一次審査・管理者メニューsubmenu
    const ichijishinnsakanrishaMenusubmenu = document.getElementById("ichijishinnsakanrishaMenu-submenu");
    //二次審査メニュー
    const nichijishinnsatsukanrishaMenu = document.getElementById("nichijishinnsatsukanrishaMenu");
    //二次審査メニューsubmenu
    const nichijishinnsatsukanrishaMenusubmenu = document.getElementById("nichijishinnsatsukanrishaMenusubmenu-submenu");
    //局管理者メニュー
    const kyokukannrishaMenu = document.getElementById("kyokukannrishaMenu");
    //局管理者メニューsubmenu
    const kyokukannrishaMenusubmenu = document.getElementById("kyokukannrishaMenu-submenu");
    //三次審査メニュー
    const sanjishinnsatsukanrishaMenu = document.getElementById("sanjishinnsatsukanrishaMenu");
    //三次審査メニューsubmenu
    const sanjishinnsatsukanrishaMenusubmenu = document.getElementById("sanjishinnsatsukanrishaMenu-submenu");
    //庁管理者メニュー
    const chokannrishaMenu = document.getElementById("chokannrishaMenu");
    //庁管理者メニューsubmenu
    const chokannrishaMenusubmenu = document.getElementById("chokannrishaMenu-submenu");
    //検索メニュー
    const kensakuMenu = document.getElementById("kensakuMenu");
    //検索メニューsubmenu
    const kensakuMenusubmenu = document.getElementById("kensakuMenu-submenu");

    //提案書作成メニュー
    if (teianshosakuseiMenu && teianshosakuseiMenusubmenu) {
        teianshosakuseiMenu.addEventListener("click", function () {
            teianshosakuseiMenusubmenu.style.display = teianshosakuseiMenusubmenu.style.display === "none" ? "block" : "none";
        });
    }
    //一次審査・管理者メニュー
    if (ichijishinnsakanrishaMenu && ichijishinnsakanrishaMenusubmenu) {
        ichijishinnsakanrishaMenu.addEventListener("click", function () {
            ichijishinnsakanrishaMenusubmenu.style.display = ichijishinnsakanrishaMenusubmenu.style.display === "none" ? "block" : "none";
        });
    }
    //二次審査メニュー
    if (nichijishinnsatsukanrishaMenu && nichijishinnsatsukanrishaMenusubmenu) {
        nichijishinnsatsukanrishaMenu.addEventListener("click", function () {
            nichijishinnsatsukanrishaMenusubmenu.style.display = nichijishinnsatsukanrishaMenusubmenu.style.display === "none" ? "block" : "none";
        });
    }
    //局管理者メニュー
    if (kyokukannrishaMenu && kyokukannrishaMenusubmenu) {
        kyokukannrishaMenu.addEventListener("click", function () {
            kyokukannrishaMenusubmenu.style.display = kyokukannrishaMenusubmenu.style.display === "none" ? "block" : "none";
        });
    }
    //三次審査メニュー
    if (sanjishinnsatsukanrishaMenu && sanjishinnsatsukanrishaMenusubmenu) {
        sanjishinnsatsukanrishaMenu.addEventListener("click", function () {
            sanjishinnsatsukanrishaMenusubmenu.style.display = sanjishinnsatsukanrishaMenusubmenu.style.display === "none" ? "block" : "none";
        });
    }
    //庁管理者メニュー
    if (chokannrishaMenu && chokannrishaMenusubmenu) {
        chokannrishaMenu.addEventListener("click", function () {
            chokannrishaMenusubmenu.style.display = chokannrishaMenusubmenu.style.display === "none" ? "block" : "none";
        });
    }
    //検索メニュー
    if (kensakuMenu && kensakuMenusubmenu) {
        kensakuMenu.addEventListener("click", function () {
            kensakuMenusubmenu.style.display = kensakuMenusubmenu.style.display === "none" ? "block" : "none";
        });
    }
});
