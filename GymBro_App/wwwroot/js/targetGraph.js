
const ctx = document.getElementById('targetGraph');
var chart;

async function renderGraph() {
    let stats = [1,1,1,1];
    stats = await getFoodStats();

    chart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ['Calories', 'Protein', 'Carbs', 'Fats'],
        datasets: [{
        label: 'Target',
        data: [document.getElementById('Calories').innerText, 
                document.getElementById('Protein').innerText, 
                document.getElementById('Carbs').innerText, 
                document.getElementById('Fats').innerText],
        borderWidth: 1
        }, 
        {
        label: 'Actual',
        data: stats,
        borderWidth: 1
        }]
    },
    options: {
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

    document.querySelectorAll(".food-from-api").forEach(async function (p) {
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
    });
    return stats;
}

renderGraph();

setTimeout(() => {
  chart.update(); // Wait for load, will not show stats otherwise
}, 1500);