//* Khởi tạo tất cả sự kiện
function initEvents() {
    initCloseValidation();
}

initEvents();

//& Close validation form
function initCloseValidation() {
    $(document).on("hidden.bs.modal", ".modal", function () {
        const modal = $(this);

        setTimeout(() => {
            modal.find("form.needs-validation")
                .removeClass("was-validated")
                .trigger("reset")
                .find(".is-invalid, .is-valid")
                .removeClass("is-invalid is-valid");
        }, 300);
    });
}

function checkLengthCheckbox(length, editButton, createButton, deleteButton, idForm) {
    const selectedCheckboxCount = length == null || length == undefined
        ? $('.' + idForm + ':checked').length : length;

    if (selectedCheckboxCount == 1) {
        $('.' + editButton).removeClass(' disabled');
        $('.' + createButton).addClass(' disabled');
        $('.' + deleteButton).removeClass(' disabled');
    }
    else if (selectedCheckboxCount == 0) {
        $('.' + editButton).addClass(' disabled');
        $('.' + createButton).removeClass(' disabled');
        $('.' + deleteButton).addClass(' disabled');
    }
    else {
        $('.' + editButton).addClass(' disabled');
        $('.' + createButton).addClass(' disabled');
        $('.' + deleteButton).removeClass(' disabled');
    }
}
function showMessage(message) {
    // Xác định có phải thông báo thành công hay không
    const isSuccess = message.startsWith("S:");
    const isFailure = message.startsWith("F:");

    // Loại bỏ tiền tố "S:" hoặc "F:"
    const cleanMessage = message.replace(/^S:\s*|^F:\s*/, '');

    $("#success-msg")
        .text(cleanMessage)
        .removeClass("d-none")
        .addClass("toast show toast-body")
        .toggleClass("text-bg-success", isSuccess)
        .toggleClass("text-bg-danger", isFailure);

    setTimeout(() => {
        $("#success-msg")
            .text("")
            .removeClass("toast show toast-body text-bg-success text-bg-danger")
            .addClass("d-none");
    }, 10000);
}

function parseResponse(response) {
    const contentType = response.headers.get("content-type");

    if (contentType && contentType.includes("application/json")) {
        return response.json().then(data => ({ type: 'json', data }));
    } else {
        return response.text().then(data => ({ type: 'html', data }));
    }
}
