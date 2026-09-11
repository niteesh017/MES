const connection = new signalR.HubConnectionBuilder()
    .withUrl("/mesHub")
    .withAutomaticReconnect()
    .build();

const trendLabels = [], productionTrendData = [];
const gradeTrendLabels = [], grade1TrendData = [], grade2TrendData = [], grade3TrendData = [], grade4TrendData = [];

let productionTrendChart, gradeChart, gradeTrendChart;
let lastGradeValues = [null, null, null, null];

const el = id => document.getElementById(id);
const num = (v, d = 2) => Number(v ?? 0).toFixed(d);

function createCharts() {
    const trend = el("productionTrendChart");

    if (trend) {
        productionTrendChart = new Chart(trend, {
            type: "line",
            data: {
                labels: trendLabels,
                datasets: [{
                    label: "Output",
                    data: productionTrendData,
                    borderColor: "#f26f24",
                    backgroundColor: "rgba(43,59,73,0.5)",
                    borderWidth: 2,
                    tension: .35,
                    fill: true,
                    pointRadius: 0,
                    pointHoverRadius: 5
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: false,
                interaction: { intersect: false, mode: "index" },
                plugins: {
                    legend: { display: true, position: "top", align: "end" },
                    tooltip: {
                        callbacks: {
                            label: c => `Output: ${num(c.parsed.y)} KG`
                        }
                    }
                },
                scales: {
                    x: {
                        grid: { display: false },
                        ticks: { maxTicksLimit: 10, autoSkip: true, maxRotation: 0 }
                    },
                    y: {
                        beginAtZero: true,
                        ticks: { callback: v => `${v} KG` }
                    }
                }
            }
        });
    }

    const grade = el("gradeChart");

    if (grade) {
        gradeChart = new Chart(grade, {
            type: "doughnut",
            data: {
                labels: ["Grade 1", "Grade 2", "Grade 3", "Grade 4 / Reject"],
                datasets: [{
                    data: [0, 0, 0, 0],
                    backgroundColor: ["#10B981", "#3B82F6", "#F59E0B", "#EF4444"],
                    borderColor: "#FFFFFF",
                    borderWidth: 3
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: "50%",
                animation: false,
                resizeDelay: 100,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        callbacks: {
                            label: c => `${c.label}: ${num(c.raw)} KG`
                        }
                    }
                }
            }
        });
    }

    const gradeTrend = el("gradeTrendChart");

    if (gradeTrend) {
        const colors = ["#10B981", "#3B82F6", "#F59E0B", "#EF4444"];
        const labels = ["Grade 1", "Grade 2", "Grade 3", "Grade 4 / Reject"];

        const datasets = [
            grade1TrendData,
            grade2TrendData,
            grade3TrendData,
            grade4TrendData
        ].map((data, i) => ({
            label: labels[i],
            data,
            borderColor: colors[i],
            backgroundColor: "transparent",
            borderWidth: 2,
            tension: .35,
            pointRadius: 0,
            pointHoverRadius: 4
        }));

        gradeTrendChart = new Chart(gradeTrend, {
            type: "line",
            data: {
                labels: gradeTrendLabels,
                datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: false,
                interaction: { intersect: false, mode: "index" },
                plugins: {
                    legend: {
                        position: "top",
                        align: "end",
                        labels: {
                            boxWidth: 8,
                            boxHeight: 8,
                            padding: 6,
                            font: { size: 9 }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: c => `${c.dataset.label}: ${num(c.parsed.y)} KG`
                        }
                    }
                },
                scales: {
                    x: {
                        grid: { display: false },
                        ticks: {
                            maxTicksLimit: 5,
                            autoSkip: true,
                            maxRotation: 0,
                            font: { size: 8 }
                        }
                    },
                    y: {
                        beginAtZero: true,
                        ticks: {
                            maxTicksLimit: 4,
                            font: { size: 8 },
                            callback: v => `${v} KG`
                        }
                    }
                }
            }
        });
    }
}

function time(value) {
    return new Date(value).toLocaleTimeString([], {
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit"
    });
}

function limit(arr, max = 20) {
    if (arr.length > max) {
        arr.splice(0, arr.length - max);
    }
}

function updateTrendChart(data) {
    if (!productionTrendChart || !data) return;

    trendLabels.push(time(data.recordedAt));
    productionTrendData.push(Number(data.outputWeight) || 0);

    limit(trendLabels);
    limit(productionTrendData);

    productionTrendChart.update("none");
}

function updateGradeChart(data) {
    if (!gradeChart || !data) return;

    const values = [
        Number(data.grade1Weight) || 0,
        Number(data.grade2Weight) || 0,
        Number(data.grade3Weight) || 0,
        Number(data.grade4Weight) || 0
    ];

    if (!values.some((v, i) => v !== lastGradeValues[i])) return;

    lastGradeValues = [...values];

    gradeChart.data.datasets[0].data = values;
    gradeChart.update("none");

    const total = values.reduce((a, b) => a + b, 0);
    const totalEl = el("gradeTotal");

    if (totalEl) {
        totalEl.innerText = `${num(total)} KG`;
    }

    ["grade1Legend", "grade2Legend", "grade3Legend", "grade4Legend"]
        .forEach((id, i) => {
            const e = el(id);

            if (e) {
                const percentage =
                    total > 0
                        ? ((values[i] / total) * 100).toFixed(1)
                        : "0.0";

                e.innerText = `${num(values[i])} KG (${percentage}%)`;
            }
        });
}

function updateGradeTrendChart(data) {
    if (!gradeTrendChart || !data) return;

    gradeTrendLabels.push(time(data.recordedAt));

    [
        grade1TrendData,
        grade2TrendData,
        grade3TrendData,
        grade4TrendData
    ].forEach((arr, i) => {
        arr.push(Number(data[`grade${i + 1}Weight`]) || 0);
        limit(arr);
    });

    limit(gradeTrendLabels);

    gradeTrendChart.update("none");
}

function updateMachineStatus(status) {
    const e = el("machineStatus");

    if (!e) return;

    e.classList.remove(
        "status-running",
        "status-stopped",
        "status-fault"
    );

    const value = status || "--";

    e.innerText = value;

    const s = value.toLowerCase();

    e.classList.add(
        s === "running"
            ? "status-running"
            : s === "fault"
                ? "status-fault"
                : "status-stopped"
    );
}

function updateMachineKpis(data) {
    if (!data) return;

    if (el("oeeValue")) {
        el("oeeValue").innerText = `${num(data.oee)}%`;
    }

    if (el("temperatureValue")) {
        el("temperatureValue").innerText =
            `${num(data.processTemperature, 1)} °C`;
    }

    const health = el("healthValue");

    if (health) {
        const value = data.machineHealth || "Unknown";

        health.innerText = value;

        health.classList.remove(
            "text-success",
            "text-warning",
            "text-danger"
        );

        health.classList.add(
            value.toLowerCase() === "healthy"
                ? "text-success"
                : value.toLowerCase() === "warning"
                    ? "text-warning"
                    : "text-danger"
        );
    }

    if (el("downtimeValue")) {
        const total = Math.max(
            0,
            Math.floor(Number(data.downtimeSeconds) || 0)
        );

        const h = Math.floor(total / 3600);
        const m = Math.floor((total % 3600) / 60);
        const s = total % 60;

        el("downtimeValue").innerText =
            `${String(h).padStart(2, "0")}:` +
            `${String(m).padStart(2, "0")}:` +
            `${String(s).padStart(2, "0")}`;
    }

    if (el("productionRateValue")) {
        el("productionRateValue").innerText =
            `${num(data.productionRate)}`;
    }
}

function updateMachineData(data) {
    if (!data) return;

    if (el("machineCode")) {
        el("machineCode").innerText =
            data.machineCode || "Machine";
    }

    if (el("machineSpeed")) {
        el("machineSpeed").innerText =
            num(data.machineSpeed, 0);
    }

    if (el("inputWeight")) {
        el("inputWeight").innerText =
            num(data.inputWeight);
    }

    if (el("outputWeight")) {
        el("outputWeight").innerText =
            num(data.outputWeight);
    }

    if (el("totalProduction")) {
        el("totalProduction").innerText =
            num(data.totalProduction);
    }

    if (el("lastUpdate")) {
        el("lastUpdate").innerText =
            new Date(data.recordedAt).toLocaleTimeString();
    }

    if (el("plcStatus")) {
        el("plcStatus").innerText = "Connected";
        el("plcStatus").className = "badge bg-success";
    }

    updateMachineStatus(data.machineStatus);
    updateMachineKpis(data);
    updateTrendChart(data);
    updateGradeChart(data);
    updateGradeTrendChart(data);
}

// connection.on("MachineDataUpdated", updateMachineData);
connection.on("MachineDataUpdated", data => {
    console.log("SIGNALR DATA:", data);
    updateMachineData(data);
});

async function loadCurrentProduction() {
    try {
        const response = await fetch(
            "/Dashboard/GetCurrentProduction",
            {
                method: "GET",
                headers: {
                    "Accept": "application/json"
                },
                cache: "no-store"
            }
        );

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const text = await response.text();

        const data = text.trim()
            ? JSON.parse(text)
            : null;

        const content = el("productionOrderContent");
        const noBatch = el("noBatchMessage");
        const status = el("batchStatus");

        if (!data) {
            if (content) {
                content.style.display = "none";
            }

            if (noBatch) {
                noBatch.style.display = "block";
            }

            if (status) {
                status.innerText = "No Active Batch";
                status.className = "badge bg-secondary";
            }

            if (el("productionPercentage")) {
                el("productionPercentage").innerText = "0.00%";
            }

            if (el("productionProgress")) {
                el("productionProgress").style.width = "0%";
                el("productionProgress").innerText = "0.00%";
            }

            return;
        }

        if (content) {
            content.style.display = "block";
        }

        if (noBatch) {
            noBatch.style.display = "none";
        }

        [
            ["orderNumber", data.orderNumber],
            ["productName", data.productName],
            ["batchNumber", data.batchNumber],
            ["targetQuantity", num(data.targetQuantity)],
            ["batchProduced", num(data.producedQuantity)],
            ["batchGood", num(data.goodQuantity)],
            ["batchReject", num(data.rejectQuantity)]
        ].forEach(([id, value]) => {
            if (el(id)) {
                el(id).innerText = value || "--";
            }
        });

        if (status) {
            const value = data.batchStatus || "Unknown";

            status.innerText = value;

            status.className =
                value.toLowerCase() === "running"
                    ? "badge bg-success"
                    : "badge bg-secondary";
        }

        const progress = Math.min(
            Math.max(Number(data.progress) || 0, 0),
            100
        );

        if (el("productionPercentage")) {
            el("productionPercentage").innerText =
                `${progress.toFixed(2)}%`;
        }

        if (el("productionProgress")) {
            el("productionProgress").style.width =
                `${progress}%`;

            el("productionProgress").innerText =
                `${progress.toFixed(2)}%`;
        }

    } catch (error) {
        console.error(
            "Failed to load current production:",
            error
        );
    }
}

connection.onreconnected(loadCurrentProduction);

async function startConnection() {
    try {
        await connection.start();
    } catch {
        setTimeout(startConnection, 5000);
    }
}

createCharts();
loadCurrentProduction();
setInterval(loadCurrentProduction, 1000);
startConnection();