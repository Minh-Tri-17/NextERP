//* Khởi tạo tất cả sự kiện
function InitEvents() {
  HandleSidebarClick();
  HandleDarkModeToggle();
}

InitEvents();

//& Xử lý sự kiện click sidebar menu
function HandleSidebarClick() {
  $(".sidebar-menu-item > a").click(function () {
    if ($(this).closest(".submenu").length) return;

    const currentIcon = $(this).find("i.fa-plus, i.fa-minus");
    ToggleIconAnimation(currentIcon, !currentIcon.hasClass("fa-minus"));

    $(".sidebar-menu-item > a")
      .not(this)
      .each(function () {
        const otherIcon = $(this).find("i.fa-minus");
        if (otherIcon.length) ToggleIconAnimation(otherIcon, false);
      });
  });
}

//& Xử lý toggle biểu tượng +/-
function ToggleIconAnimation($icon, toMinus) {
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
function HandleDarkModeToggle() {
  $(".dark-mode input").change(function () {
    if ($(this).is(":checked")) {
      $(".layout-wrapper").css("background-color", "#2f2f2f");
      $(".sidebar-menu-item a").css("color", "#b0a695");
    } else {
      $(".layout-wrapper").css("background-color", "#f8f5f2");
      $(".sidebar-menu-item a").css("color", "#6a6a6a");
    }
  });
}
