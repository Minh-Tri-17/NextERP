//& Xử lý sự kiện click sidebar menu
$(".sidebar-menu-item > a").on("click", function () {
    if ($(this).closest(".submenu").length) return;

    const $currentIcon = $(this).find("i.fa-plus, i.fa-minus");
    toggleIconAnimation($currentIcon, !$currentIcon.hasClass("fa-minus"));

    $(".sidebar-menu-item > a")
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
if (localStorage.getItem("theme") === "dark") {
    $("body").addClass("dark-mode");
    $(".dark-mode .toggle-mode input").prop("checked", true);
} else {
    $("body").removeClass("dark-mode");
    $(".dark-mode .toggle-mode input").prop("checked", false);
}

// Khi user thay đổi checkbox
$(".toggle-mode input").on("change", function () {
    if ($(this).is(":checked")) {
        $("body").addClass("dark-mode");
        localStorage.setItem("theme", "dark");
    } else {
        $("body").removeClass("dark-mode");
        localStorage.setItem("theme", "light");
    }
});
