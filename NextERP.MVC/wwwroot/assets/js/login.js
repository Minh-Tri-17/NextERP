//* Khởi tạo tất cả sự kiện
function InitEvents() {
    HandleForgotPasswordClick();
    HandleBackToLoginClick();
}

InitEvents();

//& Xử lý khi nhấn "Quên mật khẩu"
function HandleForgotPasswordClick() {
    $("#forgot-password").on("click", function () {
        $("#loginWrapper").addClass("show-forgot");
        $(".login-container")[0].reset();
        $(".login-container").removeClass("was-validated");
    });
}

//& Xử lý khi quay lại từ form "Quên mật khẩu"
function HandleBackToLoginClick() {
    $("#back-login").on("click", function () {
        $("#loginWrapper").removeClass("show-forgot");
        $(".forgot-container")[0].reset();
        $(".forgot-container").removeClass("was-validated");
    });
}
