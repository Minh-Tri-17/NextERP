//* Khởi tạo tất cả sự kiện
function initEvents() {
    handleSidebarClick();
    handleDarkModeToggle();
}

initEvents();

//& Xử lý sự kiện click sidebar menu
function handleSidebarClick() {
    $(".sidebar-menu-item > a").click(function () {
        if ($(this).closest(".submenu").length) return;

        const currentIcon = $(this).find("i.fa-plus, i.fa-minus");
        toggleIconAnimation(currentIcon, !currentIcon.hasClass("fa-minus"));

        $(".sidebar-menu-item > a")
            .not(this)
            .each(function () {
                const otherIcon = $(this).find("i.fa-minus");
                if (otherIcon.length) toggleIconAnimation(otherIcon, false);
            });
    });
}

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
function handleDarkModeToggle() {
    $(".dark-mode input").change(function () {
        if ($(this).is(":checked")) {
            $(".header ").css("background-color", "#2f2f2f");
            $(".layout-wrapper").css("background-color", "#2f2f2f");
            $(".sidebar-menu-item a").css("color", "#b0a695");
            $(".breadcrumb-item").css("color", "#b0a695");

            const style = document.createElement('style');
            style.innerHTML = `
              .breadcrumb-item::before {
                color: #b0a695 !important;
              }
            `;
            document.head.appendChild(style);

        } else {
            $(".header").css("background-color", "#f8f5f2");
            $(".layout-wrapper").css("background-color", "#f8f5f2");
            $(".sidebar-menu-item a").css("color", "#6a6a6a");
            $(".breadcrumb-item").css("color", "#6a6a6a");

            const style = document.createElement('style');
            style.innerHTML = `
              .breadcrumb-item::before {
                color: #6a6a6a !important;
              }
            `;
            document.head.appendChild(style);
        }
    });
}
