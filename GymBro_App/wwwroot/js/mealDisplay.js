
document.querySelectorAll(".food-from-api").forEach(async function (p) {
    let response =  await fetch("/api/food/" + p.getAttribute("data-api-id"));
    if(response.ok){
        let result = await response.json();
        console.log(result);
        p.textContent = result.foodName;
    } else {
        console.log("Error: " + response.status);
    }
});

async function getSchedule(user) {
    try{
        document.addEventListener('DOMContentLoaded', async function() {
            var calendarEl = document.getElementById('calendar');
            var calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridMonth',
            events: '/api/mealplans/schedule/' + user
            });
            calendar.render();

            var listEl = document.getElementById('list');

            document.getElementById('view-btn').addEventListener('click', function() {
                if (this.textContent === 'Calendar View') {
                    this.textContent = 'List View';
                } else {
                    this.textContent = 'Calendar View';
                }
                listEl.toggleAttribute('hidden');
                calendarEl.toggleAttribute('hidden');
                calendar.render();
            });
        });
    } catch (error) {
        console.error("No calendar found: ", error);
    }
}