document.addEventListener("DOMContentLoaded", function () {
    fetch('/Workouts/WorkoutCreationActionsPartial')
        .then(response => response.text())
        .then(html => {
            document.getElementById('workoutCreationActions').innerHTML = html;
        })
        .catch(error => console.error('Error loading partial view:', error));
});