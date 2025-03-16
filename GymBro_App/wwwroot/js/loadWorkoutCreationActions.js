document.addEventListener("DOMContentLoaded", function () {
    fetch('/Workouts/WorkoutCreationActionsPartial')
        .then(response => response.text())
        .then(html => {
            document.getElementById('workoutCreationActions').innerHTML = html;
        })
        .catch(error => console.error('Error loading partial view:', error));
});

document.addEventListener("DOMContentLoaded", function () {
    const loginDropdown = document.getElementById("loginDropdown");
    if (loginDropdown) {
        const dropdown = new bootstrap.Dropdown(loginDropdown);
        dropdown.show();
    }
});