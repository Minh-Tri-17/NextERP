//* Khởi tạo tất cả sự kiện
function initEvents() {
    initCloseValidation();
}

initEvents();

//& Đóng validation form
function initCloseValidation() {
    $(document).on("hidden.bs.modal", ".modal", function () {
        const $modal = $(this);

        setTimeout(() => {
            $modal.find("form.needs-validation")
                .removeClass("was-validated")
                .trigger("reset")
                .find(".is-invalid, .is-valid")
                .removeClass("is-invalid is-valid");
        }, 300);
    });
}

//& Kiểm tra độ dài checkbox để bật/tắt nút
function checkLengthCheckbox(length, editButton, createButton, deleteButton, deletePermanentlyButton, idForm) {
    const selectedCheckboxCount = length == null || length == undefined ? $('.' + idForm + ':checked').length : length;
    const $isDelete = $("#is-delete").is(":checked");

    const $editBtn = $('.' + editButton);
    const $createBtn = $('.' + createButton);
    const $deleteBtn = $('.' + deleteButton);
    const $deletePermanentlyBtn = $('.' + deletePermanentlyButton);

    if ($isDelete) {
        if (selectedCheckboxCount === 0) {
            $deletePermanentlyBtn.addClass('disabled');
        } else {
            $deletePermanentlyBtn.removeClass('disabled');
        }

        // Không cần thao tác với create/edit/delete khi đang ở trạng thái "is-delete"
        $createBtn.addClass('disabled');
        $editBtn.addClass('disabled');
        $deleteBtn.addClass('disabled');
    }
    else {
        if (selectedCheckboxCount === 0) {
            $editBtn.addClass('disabled');
            $createBtn.removeClass('disabled');
            $deleteBtn.addClass('disabled');
        } else if (selectedCheckboxCount === 1) {
            $editBtn.removeClass('disabled');
            $createBtn.addClass('disabled');
            $deleteBtn.removeClass('disabled');
        } else {
            $editBtn.addClass('disabled');
            $createBtn.addClass('disabled');
            $deleteBtn.removeClass('disabled');
        }

        // Không cần thao tác với delete permanently khi đang khác trạng thái "is-delete"
        $deletePermanentlyBtn.addClass('disabled');
    }
}

//& Hiển thị thông báo thành công hoặc thất bại
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

//& Chuyển đổi response thành JSON hoặc HTML dựa vào content type
function parseResponse(response) {
    const contentType = response.headers.get("content-type");

    if (contentType && contentType.includes("application/json")) {
        return response.json().then(data => ({ type: 'json', data }));
    } else {
        return response.text().then(data => ({ type: 'html', data }));
    }
}

//& Sau khi load dữ liệu thành công, ẩn loader và xử lý cuộn bảng
function affterCallAPISuccess() {
    $(".loader").css("display", "none");

    // Gọi hàm tableScrollHandler để xử lý cuộn bảng
    tableScrollHandler();

    // Gọi hàm sortableHandler để xử lý kéo thả row
    sortableHandler();
}

//& Format cho date
function formatDateToInput(dateStr) {
    if (!dateStr) return '';

    const d = new Date(dateStr);
    const yyyy = d.getFullYear();
    const mm = (d.getMonth() + 1).toString().padStart(2, '0');
    const dd = d.getDate().toString().padStart(2, '0');

    return `${yyyy}-${mm}-${dd}`;
}

//& Format cho time
function formatTimeToInput(dateStr) {
    if (!dateStr) return '';

    const d = new Date(dateStr);
    const hh = d.getHours().toString().padStart(2, '0');
    const min = d.getMinutes().toString().padStart(2, '0');

    return `${hh}:${min}`;
}
