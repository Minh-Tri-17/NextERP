$(window).on("load", function () {
    $("body").addClass("loaded");

    setTimeout(() => {
        $("#login-form").addClass("active");
    }, 800);
});

const $loginForm = $("#login-form");
const $forgotForm = $("#forgot-form");
const $otpForm = $("#otp-form");

// Hàm chuyển form với animation
function switchForm($hideForm, $showForm) {
    $hideForm.removeClass("active");

    setTimeout(() => {
        $hideForm.addClass("hidden");
        $showForm.removeClass("hidden");

        setTimeout(() => {
            $showForm.addClass("active");
        }, 20);
    }, 300);
}

function showForgotPassword() {
    switchForm($loginForm, $forgotForm);
}


function backToLogin() {
    const $currentForm = $forgotForm.hasClass("hidden") ? $otpForm : $forgotForm;
    switchForm($currentForm, $loginForm);
}