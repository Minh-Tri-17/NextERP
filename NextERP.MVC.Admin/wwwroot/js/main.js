//& Cuộn bảng: Sticky thead
function tableScrollHandler() {
    $(".table-container")
        .off("scroll")// Gỡ bỏ sự kiện scroll cũ (nếu có) để tránh bị bind nhiều lần
        .on("scroll", // Gắn lại sự kiện scroll cho mỗi table-container
            function () {
                $(this).find(".table thead").toggleClass("sticky-active", $(this).scrollTop() > 0);
            });
}

//& Kéo thả nhóm
function sortableHandler() {
    $(".sortable-group").each(function () {
        new Sortable(this, {
            animation: 150,
            swap: true,
            swapClass: 'highlight',
            fallbackOnBody: true,
            swapThreshold: 0.65,
            handle: "td",
        });
    });
}

//& Check toàn nhóm
function checkedGroup() {
    $(".group-toggle").on("change", function () {
        const isChecked = $(this).prop("checked");
        $(this).closest("tbody").find(".form-check-input").prop("checked", isChecked);
    });
}

//& Gán màu ngẫu nhiên cho các item
function randomColors() {
    const colors = ["#d4f8c4", "#ffe5a8", "#f8d4d4", "#c4e3f8", "#b0a695", "#b4b4b8", "#e0c3fc"];
    const items = $(".service-item").toArray();
    const shuffledColors = [...colors].sort(() => Math.random() - 0.5);

    items.forEach((item, index) => {
        const colorIndex = index < shuffledColors.length ? index : Math.floor(Math.random() * shuffledColors.length);
        $(item).css("background-color", shuffledColors[colorIndex]);
    });
}

//& Hiệu ứng tăng số
function animateNumber() {
    $(".value").each(function () {
        const $this = $(this);
        const value = $this.data("value");

        $({ count: 0 }).animate(
            { count: value },
            {
                duration: 3000,
                step: function () {
                    $this.text("$" + Math.floor(this.count).toLocaleString());
                },
                complete: function () {
                    $this.text("$" + Math.floor(value).toLocaleString());
                },
            }
        );
    });

    $(".change-up").each(function () {
        const $this = $(this);
        const value = $this.data("value");

        $({ count: 0 }).animate(
            { count: value },
            {
                duration: 3000,
                step: function () {
                    $this.text("+" + Math.floor(this.count).toLocaleString() + "%");
                },
                complete: function () {
                    $this.text("+" + Math.floor(value).toLocaleString() + "%");
                },
            }
        );
    });

    $(".change-down").each(function () {
        const $this = $(this);
        const value = $this.data("value");

        $({ count: 0 }).animate(
            { count: value },
            {
                duration: 3000,
                step: function () {
                    $this.text("-" + Math.floor(this.count).toLocaleString() + "%");
                },
                complete: function () {
                    $this.text("-" + Math.floor(value).toLocaleString() + "%");
                },
            }
        );
    });
}
