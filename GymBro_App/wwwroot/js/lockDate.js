document.getElementById("MealPlanId").addEventListener("change", function () {
    const selectedOption = this.options[this.selectedIndex];
    const start = selectedOption.getAttribute("data-start");
    const end = selectedOption.getAttribute("data-end");

    const dateInput = document.getElementById("Date");
    dateInput.min = start;
    dateInput.max = end;

    // Reset the current value if it's out of bounds
    if (dateInput.value && (dateInput.value < start || dateInput.value > end)) {
        dateInput.value = "";
    }
});

window.addEventListener("DOMContentLoaded", () => {
    document.getElementById("MealPlanId").dispatchEvent(new Event("change"));
});