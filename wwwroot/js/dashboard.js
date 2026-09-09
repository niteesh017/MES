const connection =
        new signalR.HubConnectionBuilder()
            .withUrl("/mesHub")
            .withAutomaticReconnect()
            .build();

    const trendLabels = [];
    const productionTrendData = [];

    const gradeTrendLabels = [];
    const grade1TrendData = [];
    const grade2TrendData = [];
    const grade3TrendData = [];
    const grade4TrendData = [];

    let productionTrendChart;
    let gradeChart;
    let gradeTrendChart;

    let lastGradeValues = [null, null, null, null];

    function createCharts() {

        const trendContext =
            document.getElementById("productionTrendChart");

        if (trendContext) {

            productionTrendChart =
                new Chart(trendContext, {

                    type: "line",

                    data: {
                        labels: trendLabels,

                        datasets: [{
                            label: "Output",
                            data: productionTrendData,
                            borderColor: "#f26f24",
                            backgroundColor: "rgba(43,59,73,0.5)",
                            borderWidth: 2,
                            tension: 0.35,
                            fill: true,
                            pointRadius: 0,
                            pointHoverRadius: 5
                        }]
                    },

                    options: {

                        responsive: true,
                        maintainAspectRatio: false,
                        animation: false,

                        interaction: {
                            intersect: false,
                            mode: "index"
                        },

                        plugins: {

                            legend: {
                                display: true,
                                position: "top",
                                align: "end"
                            },

                            tooltip: {
                                callbacks: {

                                    label: function(context) {

                                        return "Output: " +
                                            Number(
                                                context.parsed.y
                                            ).toFixed(2) +
                                            " KG";
                                    }
                                }
                            }
                        },

                        scales: {

                            x: {

                                grid: {
                                    display: false
                                },

                                ticks: {
                                    maxTicksLimit: 10,
                                    autoSkip: true,
                                    maxRotation: 0,
                                    minRotation: 0
                                }
                            },

                            y: {

                                beginAtZero: true,

                                ticks: {

                                    callback: function(value) {

                                        return value + " KG";
                                    }
                                }
                            }
                        }
                    }
                });
        }

        const gradeContext =
            document.getElementById("gradeChart");

        if (gradeContext) {

            gradeChart =
                new Chart(gradeContext, {

                    type: "doughnut",

                    data: {

                        labels: [
                            "Grade 1",
                            "Grade 2",
                            "Grade 3",
                            "Grade 4 / Reject"
                        ],

                        datasets: [{

                            data: [
                                0,
                                0,
                                0,
                                0
                            ],

                            backgroundColor: [
                                "#10B981",
                                "#3B82F6",
                                "#F59E0B",
                                "#EF4444"
                            ],

                            borderColor: "#FFFFFF",
                            borderWidth: 3,
                            hoverOffset: 0
                        }]
                    },

                    options: {

                        responsive: true,
                        maintainAspectRatio: false,

                        cutout: "50%",

                        animation: false,

                        resizeDelay: 100,

                        plugins: {

                            legend: {
                                display: false
                            },

                            tooltip: {

                                callbacks: {

                                    label: function(context) {

                                        return context.label +
                                            ": " +
                                            Number(
                                                context.raw
                                            ).toFixed(2) +
                                            " KG";
                                    }
                                }
                            }
                        }
                    }
                });
        }

        const gradeTrendContext =
            document.getElementById("gradeTrendChart");

        if (gradeTrendContext) {

            gradeTrendChart =
                new Chart(gradeTrendContext, {

                    type: "line",

                    data: {

                        labels: gradeTrendLabels,

                        datasets: [

                            {
                                label: "Grade 1",
                                data: grade1TrendData,
                                borderColor: "#10B981",
                                backgroundColor: "transparent",
                                borderWidth: 2,
                                tension: 0.35,
                                pointRadius: 0,
                                pointHoverRadius: 4
                            },

                            {
                                label: "Grade 2",
                                data: grade2TrendData,
                                borderColor: "#3B82F6",
                                backgroundColor: "transparent",
                                borderWidth: 2,
                                tension: 0.35,
                                pointRadius: 0,
                                pointHoverRadius: 4
                            },

                            {
                                label: "Grade 3",
                                data: grade3TrendData,
                                borderColor: "#F59E0B",
                                backgroundColor: "transparent",
                                borderWidth: 2,
                                tension: 0.35,
                                pointRadius: 0,
                                pointHoverRadius: 4
                            },

                            {
                                label: "Grade 4 / Reject",
                                data: grade4TrendData,
                                borderColor: "#EF4444",
                                backgroundColor: "transparent",
                                borderWidth: 2,
                                tension: 0.35,
                                pointRadius: 0,
                                pointHoverRadius: 4
                            }

                        ]
                    },

                    options: {

                        responsive: true,
                        maintainAspectRatio: false,
                        animation: false,

                        interaction: {
                            intersect: false,
                            mode: "index"
                        },

                        plugins: {

                            legend: {
                                display: true,
                                position: "top",
                                align: "end",

                                labels: {
                                    boxWidth: 8,
                                    boxHeight: 8,
                                    padding: 6,
                                    font: {
                                        size: 9
                                    }
                                }
                            },

                            tooltip: {

                                callbacks: {

                                    label: function(context) {

                                        return context.dataset.label +
                                            ": " +
                                            Number(
                                                context.parsed.y
                                            ).toFixed(2) +
                                            " KG";
                                    }
                                }
                            }
                        },

                        scales: {

                            x: {

                                grid: {
                                    display: false
                                },

                                ticks: {

                                    maxTicksLimit: 5,
                                    autoSkip: true,
                                    maxRotation: 0,
                                    minRotation: 0,

                                    font: {
                                        size: 8
                                    }
                                }
                            },

                            y: {

                                beginAtZero: true,

                                ticks: {

                                    maxTicksLimit: 4,

                                    font: {
                                        size: 8
                                    },

                                    callback: function(value) {

                                        return value + " KG";
                                    }
                                }
                            }
                        }
                    }
                });
        }
    }

    function updateTrendChart(data) {

        if (!productionTrendChart || !data) {
            return;
        }

        const date =
            new Date(data.recordedAt);

        const time =
            date.toLocaleTimeString(
                [],
                {
                    hour: "2-digit",
                    minute: "2-digit",
                    second: "2-digit"
                }
            );

        trendLabels.push(time);

        productionTrendData.push(
            Number(data.outputWeight) || 0
        );

        const maxPoints = 20;

        if (trendLabels.length > maxPoints) {

            trendLabels.splice(
                0,
                trendLabels.length - maxPoints
            );
        }

        if (productionTrendData.length > maxPoints) {

            productionTrendData.splice(
                0,
                productionTrendData.length - maxPoints
            );
        }

        productionTrendChart.update("none");
    }

    function updateGradeChart(data) {

        if (!gradeChart || !data) {
            return;
        }

        const values = [

            Number(data.grade1Weight) || 0,

            Number(data.grade2Weight) || 0,

            Number(data.grade3Weight) || 0,

            Number(data.grade4Weight) || 0

        ];

        const changed =
            values.some(
                function(value, index) {

                    return value !==
                        lastGradeValues[index];
                }
            );

        if (!changed) {
            return;
        }

        lastGradeValues =
            [...values];

        gradeChart.data.datasets[0].data =
            values;

        gradeChart.update("none");

        const total =
            values.reduce(
                function(sum, value) {

                    return sum + value;

                },
                0
            );

        const gradeTotal =
            document.getElementById(
                "gradeTotal"
            );

        if (gradeTotal) {

            gradeTotal.innerText =
                total.toFixed(2) +
                " KG";
        }

        function percentage(value) {

            if (total <= 0) {
                return "0.0%";
            }

            return (
                (value / total) *
                100
            ).toFixed(1) + "%";
        }

        const legendIds = [

            "grade1Legend",
            "grade2Legend",
            "grade3Legend",
            "grade4Legend"

        ];

        values.forEach(
            function(value, index) {

                const element =
                    document.getElementById(
                        legendIds[index]
                    );

                if (!element) {
                    return;
                }

                element.innerText =
                    value.toFixed(2) +
                    " KG (" +
                    percentage(value) +
                    ")";
            }
        );
    }

    function updateGradeTrendChart(data) {

        if (!gradeTrendChart || !data) {
            return;
        }

        const date =
            new Date(data.recordedAt);

        const time =
            date.toLocaleTimeString(
                [],
                {
                    hour: "2-digit",
                    minute: "2-digit",
                    second: "2-digit"
                }
            );

        const grade1 =
            Number(data.grade1Weight) || 0;

        const grade2 =
            Number(data.grade2Weight) || 0;

        const grade3 =
            Number(data.grade3Weight) || 0;

        const grade4 =
            Number(data.grade4Weight) || 0;

        gradeTrendLabels.push(time);

        grade1TrendData.push(grade1);

        grade2TrendData.push(grade2);

        grade3TrendData.push(grade3);

        grade4TrendData.push(grade4);

        const maxPoints = 20;

        if (gradeTrendLabels.length > maxPoints) {

            gradeTrendLabels.splice(
                0,
                gradeTrendLabels.length - maxPoints
            );
        }

        if (grade1TrendData.length > maxPoints) {

            grade1TrendData.splice(
                0,
                grade1TrendData.length - maxPoints
            );
        }

        if (grade2TrendData.length > maxPoints) {

            grade2TrendData.splice(
                0,
                grade2TrendData.length - maxPoints
            );
        }

        if (grade3TrendData.length > maxPoints) {

            grade3TrendData.splice(
                0,
                grade3TrendData.length - maxPoints
            );
        }

        if (grade4TrendData.length > maxPoints) {

            grade4TrendData.splice(
                0,
                grade4TrendData.length - maxPoints
            );
        }

        gradeTrendChart.update("none");
    }

    function updateMachineStatus(status) {

        const element =
            document.getElementById(
                "machineStatus"
            );

        if (!element) {
            return;
        }

        element.classList.remove(
            "status-running",
            "status-stopped",
            "status-fault"
        );

        const currentStatus =
            status || "--";

        element.innerText =
            currentStatus;

        const normalizedStatus =
            currentStatus.toLowerCase();

        if (normalizedStatus === "running") {

            element.classList.add(
                "status-running"
            );

        } else if (
            normalizedStatus === "fault"
        ) {

            element.classList.add(
                "status-fault"
            );

        } else {

            element.classList.add(
                "status-stopped"
            );
        }
    }

    connection.on(
        "MachineDataUpdated",
        function(data) {

            if (!data) {
                return;
            }

            const machineCode =
                document.getElementById(
                    "machineCode"
                );

            if (machineCode) {

                machineCode.innerText =
                    data.machineCode ||
                    "Machine";
            }

            const machineSpeed =
                document.getElementById(
                    "machineSpeed"
                );

            if (machineSpeed) {

                machineSpeed.innerText =
                    Number(
                        data.machineSpeed
                    ).toFixed(0);
            }

            const inputWeight =
                document.getElementById(
                    "inputWeight"
                );

            if (inputWeight) {

                inputWeight.innerText =
                    Number(
                        data.inputWeight
                    ).toFixed(2);
            }

            const outputWeight =
                document.getElementById(
                    "outputWeight"
                );

            if (outputWeight) {

                outputWeight.innerText =
                    Number(
                        data.outputWeight
                    ).toFixed(2);
            }

            const totalProduction =
                document.getElementById(
                    "totalProduction"
                );

            if (totalProduction) {

                totalProduction.innerText =
                    Number(
                        data.totalProduction
                    ).toFixed(2);
            }

            const lastUpdate =
                document.getElementById(
                    "lastUpdate"
                );

            if (lastUpdate) {

                lastUpdate.innerText =
                    new Date(
                        data.recordedAt
                    ).toLocaleTimeString();
            }

            const plcStatus =
                document.getElementById(
                    "plcStatus"
                );

            if (plcStatus) {

                plcStatus.innerText =
                    "Connected";
            }

            updateMachineStatus(
                data.machineStatus
            );

            updateTrendChart(data);

            updateGradeChart(data);

            updateGradeTrendChart(data);
        }
    );

    async function loadCurrentProduction() {

        try {

            const response =
                await fetch(
                    "/Dashboard/GetCurrentProduction"
                );

            if (!response.ok) {
                return;
            }

            const data =
                await response.json();

            const productionOrderContent =
                document.getElementById(
                    "productionOrderContent"
                );

            const noBatchMessage =
                document.getElementById(
                    "noBatchMessage"
                );

            const batchStatus =
                document.getElementById(
                    "batchStatus"
                );

            if (!data) {

                if (productionOrderContent) {

                    productionOrderContent.style.display =
                        "none";
                }

                if (noBatchMessage) {

                    noBatchMessage.style.display =
                        "block";
                }

                if (batchStatus) {

                    batchStatus.innerText =
                        "No Active Batch";

                    batchStatus.className =
                        "badge bg-secondary";
                }

                return;
            }

            if (productionOrderContent) {

                productionOrderContent.style.display =
                    "block";
            }

            if (noBatchMessage) {

                noBatchMessage.style.display =
                    "none";
            }

            const orderNumber =
                document.getElementById(
                    "orderNumber"
                );

            if (orderNumber) {

                orderNumber.innerText =
                    data.orderNumber || "--";
            }

            const productName =
                document.getElementById(
                    "productName"
                );

            if (productName) {

                productName.innerText =
                    data.productName || "--";
            }

            const batchNumber =
                document.getElementById(
                    "batchNumber"
                );

            if (batchNumber) {

                batchNumber.innerText =
                    data.batchNumber || "--";
            }

            const targetQuantity =
                document.getElementById(
                    "targetQuantity"
                );

            if (targetQuantity) {

                targetQuantity.innerText =
                    Number(
                        data.targetQuantity
                    ).toFixed(2);
            }

            const batchProduced =
                document.getElementById(
                    "batchProduced"
                );

            if (batchProduced) {

                batchProduced.innerText =
                    Number(
                        data.producedQuantity
                    ).toFixed(2);
            }

            const batchGood =
                document.getElementById(
                    "batchGood"
                );

            if (batchGood) {

                batchGood.innerText =
                    Number(
                        data.goodQuantity
                    ).toFixed(2);
            }

            const batchReject =
                document.getElementById(
                    "batchReject"
                );

            if (batchReject) {

                batchReject.innerText =
                    Number(
                        data.rejectQuantity
                    ).toFixed(2);
            }

            if (batchStatus) {

                const currentBatchStatus =
                    data.batchStatus ||
                    "Unknown";

                batchStatus.innerText =
                    currentBatchStatus;

                batchStatus.className =
                    currentBatchStatus.toLowerCase() ===
                        "running"
                        ? "badge bg-success"
                        : "badge bg-secondary";
            }

            const progress =
                Math.min(
                    Math.max(
                        Number(
                            data.progress
                        ) || 0,
                        0
                    ),
                    100
                );

            const productionPercentage =
                document.getElementById(
                    "productionPercentage"
                );

            if (productionPercentage) {

                productionPercentage.innerText =
                    progress.toFixed(2) +
                    "%";
            }

            const progressBar =
                document.getElementById(
                    "productionProgress"
                );

            if (progressBar) {

                progressBar.style.width =
                    progress + "%";

                progressBar.innerText =
                    progress.toFixed(2) +
                    "%";
            }

        } catch (error) {

            console.error(
                "Failed to load current production:",
                error
            );
        }
    }

    connection.onreconnected(
        function() {

            loadCurrentProduction();
        }
    );

    async function startConnection() {

        try {

            await connection.start();

        } catch (error) {

            console.error(
                "SignalR connection failed:",
                error
            );

            setTimeout(
                startConnection,
                5000
            );
        }
    }

    createCharts();

    loadCurrentProduction();

    setInterval(
        loadCurrentProduction,
        1000
    );

    startConnection();