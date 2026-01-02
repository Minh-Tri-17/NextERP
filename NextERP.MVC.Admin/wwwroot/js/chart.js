//& Biểu đồ Line
function renderChartLine(id, title, data) {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector(id);
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: [{ name: "Desktops", data: data.values }],
        chart: {
            height: 200,
            type: "line",
            zoom: { enabled: false },
            toolbar: { show: true, tools: { download: true } },
        },
        title: {
            text: title,
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
            categories: data.labels,
        },
    });

    chart.render();
}

//& Biểu đồ Radar
function renderChartRadar(id, title, data) {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector(id);
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: data.values,
        chart: {
            height: 400,
            type: "radar",
            toolbar: { show: true, tools: { download: true } },
        },
        title: {
            text: title,
            style: { fontSize: "16px", fontWeight: "bold" },
        },
        stroke: { width: 2 },
        colors: ["#d4f8c4", "#c4e3f8", "#f8d4d4"],
        fill: { opacity: 0.1 },
        markers: { size: 0 },
        yaxis: { stepSize: 20 },
        xaxis: {
            categories: data.labels,
        },
    });

    chart.render();
}

//& Biểu đồ Donut
function renderChartDonut(id, title, data) {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector(id);
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: data.values,
        labels: data.labels,
        chart: {
            type: "donut",
            height: 400,
            toolbar: { show: true, tools: { download: true } },
        },
        title: {
            text: title,
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
function renderChartColumn(id, title, data) {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector(id);
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: data.values,
        chart: {
            type: "bar",
            height: 400,
            toolbar: { show: true, tools: { download: true } },
        },
        title: {
            text: title,
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
            categories: data.labels,
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

//& Biểu đồ Slope
function renderChartSlope(id, title, data) {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector(id);
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: data.values,
        chart: {
            height: 400,
            width: "100%",
            type: "line",
            toolbar: { show: true, tools: { download: true } },
        },
        title: {
            text: title,
            style: { fontSize: "16px", fontWeight: "bold" },
        },
        colors: ["#d4f8c4", "#ffe5a8", "#f8d4d4", "#c4e3f8", "#b0a695", "#b4b4b8", "#e0c3fc"],
        plotOptions: {
            line: {
                isSlopeChart: true,
            },
        },
        tooltip: {
            followCursor: true,
            intersect: false,
            shared: true,
        },
        dataLabels: {
            background: {
                enabled: true,
            },
            formatter(val, opts) {
                const seriesName = opts.w.config.series[opts.seriesIndex].name;
                return val !== null ? seriesName : "";
            },
            offsetY: 5,
        },
        yaxis: {
            show: true,
            labels: {
                show: true,
            },
        },
        xaxis: {
            position: "bottom",
        },
        legend: {
            show: true,
            position: "top",
            horizontalAlign: "left",
        },
        stroke: {
            width: [2, 3, 4, 2],
            dashArray: [0, 0, 5, 2],
            curve: "smooth",
        },
    });

    chart.render();
}

//& Biểu đồ Funnel
function renderChartFunnel(id, title, data) {
    //! ApexCharts yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector(id);
    if (!chartElement) return;

    const chart = new ApexCharts(chartElement, {
        series: [
            {
                name: "",
                data: data.values,
            },
        ],
        chart: {
            type: "bar",
            height: 400,
            dropShadow: {
                enabled: true,
            },
        },
        title: {
            text: title,
            style: { fontSize: "16px", fontWeight: "bold" },
        },
        plotOptions: {
            bar: {
                borderRadius: 0,
                horizontal: true,
                distributed: true,
                barHeight: "80%",
                isFunnel: true,
            },
        },
        colors: ["#d4f8c4", "#ffe5a8", "#f8d4d4", "#c4e3f8", "#b0a695", "#b4b4b8", "#e0c3fc", "#9ea5ee"],
        dataLabels: {
            enabled: true,
            formatter: function (val, opt) {
                return opt.w.globals.labels[opt.dataPointIndex];
            },
            dropShadow: {
                enabled: true,
            },
        },
        xaxis: {
            categories: data.labels,
        },
        legend: {
            show: false,
        },
    });

    chart.render();
}

//& Biểu đồ Tree
function renderChartTree(id, title, data) {
    //! ApexTree yêu cầu bạn phải truyền vào một DOM element thật sự, không phải một jQuery object
    const chartElement = document.querySelector(id);
    if (!chartElement) return;

    const chart = new ApexTree(chartElement, {
        contentKey: "data",
        width: "100%",
        height: "100%",
        nodeWidth: 150,
        nodeHeight: 70,
        childrenSpacing: 70,
        siblingSpacing: 30,
        direction: "top",
        nodeTemplate: (content) => {
            return `<div style="display: flex; flex-direction: column; height: 100%;">
                    <div style='display: flex;flex-direction: row;justify-content: space-between;align-items: center;height: 100%; box-shadow: 1px 2px 4px #ccc; padding: 0 7px;'>
                      <img style='width: 50px;height: 50px;border-radius: 50%;' src='${content.imageURL}' alt='' />
                      <div style="font-weight: bold; font-family: Arial; font-size: 14px">${content.name}</div>
                    </div>
                    <div style='margin-top: auto; border-bottom: 10px solid ${content.borderColor}'></div>
                  </div>`;
        },
        nodeStyle: "box-shadow: -3px -6px 8px -5px rgba(0,0,0,0.31);",
        enableExpandCollapse: true,
        enableToolbar: true,
    });

    chart.render(data);
}