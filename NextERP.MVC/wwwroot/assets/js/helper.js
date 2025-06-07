//* Khởi tạo tất cả sự kiện
function InitEvents() {
  InitCloseValidation();
}

InitEvents();

//& Close validation form
function InitCloseValidation() {
  $(document).on("hidden.bs.modal", ".modal", function () {
    const modal = $(this);
    setTimeout(() => {
      modal.find("form.needs-validation").removeClass("was-validated").trigger("reset").find(".is-invalid, .is-valid").removeClass("is-invalid is-valid");
    }, 300);
  });
}
