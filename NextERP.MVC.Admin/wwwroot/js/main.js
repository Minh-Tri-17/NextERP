//& Cuộn bảng: Sticky thead
function tableScrollHandler() {
    const tableContainer = document.querySelector(".table-container");
    if (tableContainer) {
        $(tableContainer).off("scroll").on("scroll", function () {
            $(".table thead").toggleClass("sticky-active", $(this).scrollTop() > 0);
        });
    }
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

//& Phần dưới chưa dùng

//& Check toàn nhóm
function InitCheckedGroup() {
    $(".group-toggle").on("change", function () {
        const isChecked = $(this).prop("checked");
        $(this).closest("tbody").find(".form-check-input").prop("checked", isChecked);
    });
}

//& Hiển thị dữ liệu thật sau delay (đây là hàm tạm thời, sau này những code bên trong sẽ để vào các func sau khi call API)
function InitDataWithDelay() {
    setTimeout(() => {
        $(".real-data").removeClass("d-none");
        $(".loading-data").addClass("d-none");
        AnimateNumber();

        setTimeout(() => {
            RenderChartColumn();
            RenderChartDonut();
            RenderChartRadar();
            RenderChartLine();
        }, 200);
    }, 3000);
}

//& Gán màu ngẫu nhiên cho các item
function InitRandomColors() {
    const colors = ["#d4f8c4", "#ffe5a8", "#f8d4d4", "#c4e3f8", "#b0a695", "#b4b4b8", "#e0c3fc"];
    const items = $(".new-user-item").toArray();
    const shuffledColors = [...colors].sort(() => Math.random() - 0.5);

    items.forEach((item, index) => {
        const colorIndex = index < shuffledColors.length ? index : Math.floor(Math.random() * shuffledColors.length);
        $(item).css("background-color", shuffledColors[colorIndex]);
    });
}

//& Hiệu ứng tăng số
function AnimateNumber() {
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
}

//& Biểu đồ Line
function RenderChartLine() {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector("#chart-line");
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: [{ name: "Desktops", data: [10, 41, 35, 51, 49, 62, 69, 91, 148] }],
        chart: {
            height: 200,
            type: "line",
            zoom: { enabled: false },
            toolbar: { show: true, tools: { download: true } },
        },
        title: {
            text: "Line chart",
            style: { fontSize: "16px", fontWeight: "bold" },
        },
        dataLabels: { enabled: false },
        stroke: { curve: "straight" },
        colors: ["#b0a695"],
        grid: {
            row: {
                colors: ["#f3f3f3", "transparent"],
                opacity: 0.5,
            },
        },
        xaxis: {
            categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep"],
        },
    });

    chart.render();
}

//& Biểu đồ Radar
function RenderChartRadar() {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector("#chart-radar");
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: [
            { name: "Series 1", data: [80, 50, 30, 40, 100, 20] },
            { name: "Series 2", data: [20, 30, 40, 80, 20, 80] },
            { name: "Series 3", data: [44, 76, 78, 13, 43, 10] },
        ],
        chart: {
            height: 400,
            type: "radar",
            toolbar: { show: true, tools: { download: true } },
        },
        title: {
            text: "Radar Chart",
            style: { fontSize: "16px", fontWeight: "bold" },
        },
        stroke: { width: 2 },
        colors: ["#d4f8c4", "#c4e3f8", "#f8d4d4"],
        fill: { opacity: 0.1 },
        markers: { size: 0 },
        yaxis: { stepSize: 20 },
        xaxis: {
            categories: ["2011", "2012", "2013", "2014", "2015", "2016"],
        },
    });

    chart.render();
}

//& Biểu đồ Donut
function RenderChartDonut() {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector("#chart-donut");
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: [44, 55, 41, 17, 15],
        chart: {
            type: "donut",
            height: 400,
            toolbar: { show: true, tools: { download: true } },
        },
        title: {
            text: "Donut Chart",
            style: { fontSize: "16px", fontWeight: "bold" },
        },
        colors: ["#d4f8c4", "#ffe5a8", "#f8d4d4", "#c4e3f8", "#b0a695", "#b4b4b8", "#e0c3fc"],
        states: {
            hover: {
                filter: { type: "darken" },
            },
        },
        responsive: [
            {
                breakpoint: 1540,
                options: {
                    chart: { width: "100%", height: 400 },
                    legend: { position: "bottom" },
                },
            },
        ],
    });

    chart.render();
}

//& Biểu đồ Column
function RenderChartColumn() {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector("#chart-column");
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: [
            { name: "Net Profit", data: [44, 55, 57, 56, 61, 58, 63, 60, 66] },
            { name: "Revenue", data: [76, 85, 101, 98, 87, 105, 91, 114, 94] },
            { name: "Free Cash Flow", data: [35, 41, 36, 26, 45, 48, 52, 53, 41] },
        ],
        chart: {
            type: "bar",
            height: 400,
            toolbar: { show: true, tools: { download: true } },
        },
        title: {
            text: "Column Chart",
            style: { fontSize: "16px", fontWeight: "bold" },
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: "55%",
                borderRadius: 5,
                borderRadiusApplication: "end",
            },
        },
        dataLabels: { enabled: false },
        stroke: {
            show: true,
            width: 2,
            colors: ["transparent"],
        },
        colors: ["#d4f8c4", "#c4e3f8", "#e0c3fc"],
        states: {
            hover: {
                filter: { type: "darken" },
            },
        },
        xaxis: {
            categories: ["Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct"],
        },
        yaxis: {
            title: {
                text: "$ (thousands)",
            },
        },
        fill: { opacity: 1 },
        tooltip: {
            y: {
                formatter: (val) => "$ " + val + " thousands",
            },
        },
    });

    chart.render();
}
