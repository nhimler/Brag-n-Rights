
const ctx = document.getElementById('targetGraph');
Chart.register(ChartDataLabels);
Chart.register(window['chartjs-plugin-annotation']);

var chart;

async function renderGraph() {
    let stats = [1,1,1,1];
    stats = await getFoodStats();

    let targets = [Number(document.getElementById('Calories').innerText), 
                   Number(document.getElementById('Protein').innerText), 
                   Number(document.getElementById('Carbs').innerText), 
                   Number(document.getElementById('Fats').innerText)];
    for(let i in targets){
        if(!targets[i]) {
            targets[i] = 0;
        }
    }

    chart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ['Calories', 'Protein', 'Carbs', 'Fats'],
        datasets: [{
        label: 'Percentage of Target',//'Target',
        data: [100 * stats[0] / targets[0], 
                100 * stats[1] / targets[1], 
                100 * stats[2] / targets[2], 
                100 * stats[3] / targets[3]],
        borderWidth: 1,
        datalabels: {
          anchor: 'end',
          align: 'start',
          formatter: function(value, context) {
            // Show actual value from stats
            return stats[context.dataIndex].toFixed(1) + ' / ' + targets[context.dataIndex].toFixed(1);
          }
        }
        // }, 
        // {
        // label: 'Actual',
        // data: stats,
        // borderWidth: 1
        }]
    },
    options: {
        plugins: {
            annotation: {
                annotations: {
                    line100: {
                    type: 'line',
                    yMin: 100,
                    yMax: 100,
                    borderColor: 'red',
                    borderWidth: 2,
                    borderDash: [6, 6],
                    label: {
                        content: '100%',
                        enabled: true,
                        yAdjust: -10,
                        position: 'end',
                        backgroundColor: 'rgba(0, 0, 0, 0)',
                        color: 'red'
                    }
                    }
                }
            },
            datalabels: {
                anchor: 'end',
                align: 'top',
                color: 'black',
                font: {
                    weight: 'bold'
                },
                offset: -20 // make sure it appears above other elements
            }
        },
        scales: {
        y: {
            beginAtZero: true
        }
        }
    }
    });

}

async function getFoodStats() {
    let stats = [0, 0, 0, 0];

    for (const p of document.querySelectorAll(".food-from-api")) {
    let response =  await fetch("/api/food/" + p.getAttribute("data-api-id"));
        if(response.ok){
            let result = await response.json();
            console.log(result);
            let description = result.foodDescription;
            const matchCalories = description.match(/Calories:\s*(\d+\.?\d*)/);
            const matchFat = description.match(/Fat:\s*(\d+\.?\d*)/);
            const matchProtein = description.match(/Protein:\s*(\d+\.?\d*)/);
            const matchCarbs = description.match(/Carbs:\s*(\d+\.?\d*)/);

            const calories = matchCalories ? parseFloat(matchCalories[1]) : null;
            const fat = matchFat ? parseFloat(matchFat[1]) : null;
            const protein = matchProtein ? parseFloat(matchProtein[1]) : null;
            const carbs = matchCarbs ? parseFloat(matchCarbs[1]) : null;

            stats[0] += calories;
            stats[1] += protein;
            stats[2] += carbs;
            stats[3] += fat;
        } else {
            console.log("Error: " + response.status);
        }
    }
    console.log("Stats: " + stats);

    return stats;
}

renderGraph();

// setTimeout(() => {
//   chart.update(); // Wait for load, will not show stats otherwise
// }, 2000);