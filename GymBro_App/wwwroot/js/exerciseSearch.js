let exerciseSearchButton = document.getElementById("exerciseSearchButtonAddon");
let exerciseIdList = [];

let isUserLoggedIn = false;

document.addEventListener('DOMContentLoaded', function() {
    if (typeof isLoggedIn !== 'undefined') {
        isUserLoggedIn = isLoggedIn;
        console.log("Login status detected:", isUserLoggedIn);
    } else {
        console.log("No login status detected, assuming not logged in");
    }
});


exerciseSearchButton.addEventListener("click", async function(){
    let name = document.getElementById("exerciseInput").value;
    console.log("We are looking for: " + name);

    let response = await fetch(`/api/exercises/${name}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    let result;
    if(response.ok){
        result = await response.json();
        console.log(result);
        await displayExerciseSearchResults(result);
    } else {
        console.log("Error: " + response.status);
        displayExerciseSearchFailure()
    }
});

function clearExerciseSearchResults() {
    let resultList = document.getElementById("exerciseSearchResults")
    if (!resultList) {
        console.error("Elements not found")
        return
    }

    console.log("Found #exerciseSearchResults in the DOM.")

    // Clear previous results
    resultList.innerHTML = ""
}

function displayExerciseSearchFailure() {
    clearExerciseSearchResults()
    let resultList = document.getElementById("exerciseSearchResults")
    let noResultsMessage = document.createElement("h4")
    noResultsMessage.className = "text-center mt-3 mb-3"
    noResultsMessage.id = "noResultsMessage"
    noResultsMessage.textContent = `No results found for "${document.getElementById("exerciseInput").value}"`
    resultList.appendChild(noResultsMessage)
    return
}


async function displayExerciseSearchResults(result) {
    console.log("displaySearchResults called with:", result);

    let resultList = document.getElementById("exerciseSearchResults");
    if (!resultList) {
        console.error("Elements not found");
        return;
    }
    console.log("Found #exerciseSearchResults in the DOM.");

    let selectedWorkouts = document.getElementById("selectedWorkouts");
    if (!selectedWorkouts) {
        selectedWorkouts = document.createElement("div");
        selectedWorkouts.id = "selectedWorkouts";
        document.body.appendChild(selectedWorkouts);
    }

    let heading = document.createElement("h2");
    if (exerciseInput.value === "") {
        heading.textContent = "Here are our first 10 exercises:";
    } else {
        heading.textContent = "Here are our top 10 results for " + exerciseInput.value + ":";
    }
    resultList.appendChild(heading);

    let table = document.createElement("table");
    table.className = "table table-bordered";
    let tbody = document.createElement("tbody");

    result.forEach(exercise => {
        let row = document.createElement("tr");

        let nameCell = document.createElement("td");
        nameCell.textContent = exercise.name;
        row.appendChild(nameCell);

        let bodyPartCell = document.createElement("td");
        bodyPartCell.textContent = exercise.bodyPart;
        row.appendChild(bodyPartCell);

        let equipmentCell = document.createElement("td");
        equipmentCell.textContent = exercise.equipment;
        row.appendChild(equipmentCell);

        let targetCell = document.createElement("td");
        targetCell.textContent = exercise.target;
        row.appendChild(targetCell);

        let gifCell = document.createElement("td");
        let gifLink = document.createElement("a");
        gifLink.href = exercise.gifUrl;
        gifLink.textContent = "View GIF";
        gifLink.target = "_blank";
        gifCell.appendChild(gifLink);
        row.appendChild(gifCell);

        let addCell = document.createElement("td");
        let addButton = document.createElement("button");
        addButton.className = "btn btn-primary";
        addButton.textContent = "Add";
        addButton.type = "button";
        addButton.addEventListener("click", function(e){
            console.log("🆕 Add button clicked for:", exercise.name, "ID:", exercise.id);
            let row = e.target.closest("tr");
            if(row) row.remove();
            addExerciseToCart(exercise);
        });
        addCell.appendChild(addButton);
        row.appendChild(addCell);

        tbody.appendChild(row);
    });

    table.appendChild(tbody);
    resultList.appendChild(table);
}

function addExerciseToCart(exercise) {
    let exerciseCart = document.getElementById("exerciseCart");
    if (!exerciseCart) {
        console.error("Exercise cart container not found.");
        return;
    }
    
    exerciseIdList.push(exercise.id);
    console.log("Exercise ID added to list:", exercise.id);
    console.log("Current exercise ID list:", exerciseIdList);
    
    let exerciseEntry = document.createElement("div");
    exerciseEntry.className = "selected-exercise entry";
    exerciseEntry.style.border = "1px solid #ccc";
    exerciseEntry.style.padding = "8px";
    exerciseEntry.style.marginBottom = "4px";

    exerciseEntry.dataset.exerciseId = exercise.id;

    exerciseEntry.innerHTML = `<strong>${exercise.name}</strong>`;
    
    let removeButton = document.createElement("button");
    removeButton.innerText = "Remove";
    removeButton.className = "btn btn-danger btn-sm";
    removeButton.style.marginLeft = "8px";
    removeButton.addEventListener("click", function() {
        let exerciseId = exerciseEntry.dataset.exerciseId;
        let idIndex = exerciseIdList.indexOf(exerciseId);
        if (idIndex > -1) {
            exerciseIdList.splice(idIndex, 1);
            console.log("Exercise ID removed from list:", exerciseId);
            console.log("Updated exercise ID list:", exerciseIdList);
        }
         exerciseEntry.remove();
    });

    exerciseEntry.appendChild(removeButton);
    exerciseCart.appendChild(exerciseEntry);

    if (isUserLoggedIn) {
        let addExerciseToCartButton = document.getElementById("addExerciseToCartButton");
        if (!addExerciseToCartButton) {
            addExerciseToCartButton = document.createElement("button");
            addExerciseToCartButton.id = "addExerciseToCartButton";
            addExerciseToCartButton.textContent = "Add Exercises to Workout";
            addExerciseToCartButton.className = "btn btn-success";
            addExerciseToCartButton.style.marginTop = "12px";
            addExerciseToCartButton.addEventListener("click", async function() {
                let workoutPlanSelect = document.getElementById("workoutPlanSelectorList");
                if (!workoutPlanSelect) {
                    console.error("Workout plan selector not found.");
                    return;
                }
                let selectedWorkoutPlanId = workoutPlanSelect.value;
                if (!selectedWorkoutPlanId) {
                    console.error("No workout plan selected.");
                    return;
                }
                console.log("Adding exercises to workout plan ID:", selectedWorkoutPlanId);
                
                let payload = {
                    workoutPlanId: parseInt(selectedWorkoutPlanId),
                    exerciseApiIds: exerciseIdList
                };
                let response = await fetch('/api/Workouts/AddExercises', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(payload)
                });
                if(response.ok){
                    let updatedWorkout = await response.json();
                    console.log("Exercises added successfully:", updatedWorkout);
                    window.location.href = "/Workouts/Index";
                } else {
                    console.error("Error adding exercises to workout:", response.status);
                }
            });
            exerciseCart.appendChild(addExerciseToCartButton);
        }
    }
}
