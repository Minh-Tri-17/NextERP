//* Khởi tạo tất cả sự kiện
function initEvents() {
    handleForgotPasswordClick();
    handleBackToLoginClick();
}

initEvents();

//& Xử lý khi nhấn "Quên mật khẩu"
function handleForgotPasswordClick() {
    $("#forgot-password").on("click", function () {
        $("#loginWrapper").addClass("show-forgot");
        $(".login-container")[0].reset();
        $(".login-container").removeClass("was-validated");
    });
}

//& Xử lý khi quay lại từ form "Quên mật khẩu"
function handleBackToLoginClick() {
    $("#back-login").on("click", function () {
        $("#loginWrapper").removeClass("show-forgot");

        // Reset tất cả form trong forgot-container
        $(".forgot-container form").each(function () {
            this.reset();
            $(this).removeClass("was-validated");
        });
    });
}
