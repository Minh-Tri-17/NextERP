//& Xử lý sự kiện click sidebar menu
$(".nav-item > .nav-link").on("click", function () {
    if ($(this).closest(".submenu").length) return;

    const $currentIcon = $(this).find("i.fa-plus, i.fa-minus");
    toggleIconAnimation($currentIcon, !$currentIcon.hasClass("fa-minus"));

    $(".nav-item > .nav-link")
        .not(this)
        .each(function () {
            const $otherIcon = $(this).find("i.fa-minus");
            if ($otherIcon.length) toggleIconAnimation($otherIcon, false);
        });
});

//& Xử lý toggle biểu tượng +/-
function toggleIconAnimation($icon, toMinus) {
    $icon.css({ transform: "scale(0.3)", opacity: 0 });

    setTimeout(() => {
        if (toMinus) {
            $icon.removeClass("fa-plus").addClass("fa-minus");
        } else {
            $icon.removeClass("fa-minus").addClass("fa-plus");
        }

        $icon.css({ transform: "scale(1)", opacity: 1 });
    }, 200);
}

//& Xử lý chế độ sáng / tối
function themeManager() {
    const $toggleBtn = $("#darkModeToggle");

    if ($toggleBtn.length === 0) {
        return;
    }

    const $toggleIcon = $toggleBtn.find("i");
    const $htmlElement = $("html");
    const THEME_KEY = "admin_theme";

    //& Xử lý chế độ sáng / tối
    const setTheme = (theme) => {
        if (theme === "dark") {
            $htmlElement.attr("data-theme", "dark");
            $toggleIcon.removeClass("fa-moon dark-mode-icon").addClass("fa-sun light-mode-icon");
        } else {
            $htmlElement.removeAttr("data-theme");
            $toggleIcon.removeClass("fa-sun light-mode-icon").addClass("fa-moon dark-mode-icon");
        }

        localStorage.setItem(THEME_KEY, theme);
    };

    const savedTheme = localStorage.getItem(THEME_KEY);
    const systemPrefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;

    if (savedTheme) {
        setTheme(savedTheme);
    } else if (systemPrefersDark) {
        setTheme("dark");
    }

    //@ Khi user click nút toggle
    $toggleBtn.on("click", function () {
        const currentTheme = $htmlElement.attr("data-theme");
        const newTheme = currentTheme === "dark" ? "light" : "dark";

        setTheme(newTheme);
    });
}

themeManager();
