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
    let resultList = document.getElementById("exerciseSearchResults")
    if (!resultList) {
        console.error("Elements not found")
        return
    }

    console.log("Found #exerciseSearchResults in the DOM.")

    // Clear previous results
    resultList.innerHTML = ""

    // Display heading
    let heading = document.createElement("h4")
    heading.className = "text-center mt-3 mb-3"
    heading.textContent = `Results for "${document.getElementById("exerciseInput").value}" (${result.length})`
    resultList.appendChild(heading)

    // Create a row container for the cards
    let row = document.createElement("div")
    row.className = "row mt-3 d-flex align-items-start"

    result.forEach(exercise => {
        splitExerciseName = exercise.name.split(" ")
        splitExerciseName.forEach((word, index) => {
            splitExerciseName[index] = word.charAt(0).toUpperCase() + word.slice(1).toLowerCase()
        })
        exerciseName = splitExerciseName.join(" ")
        console.log("Exercise name split:", exerciseName)

        // Create the card container
        let card = document.createElement("div")
        card.className = "col-lg-3 col-md-4 col-sm-6 mb-3" // Responsive column sizes

        // Create the card element
        let cardElement = document.createElement("div")
        cardElement.className = "card exercise-card"
        cardElement.style.width = "100%"

        // Add the exercise image
        let img = document.createElement("img")
        img.src = exercise.gifUrl
        img.className = "card-img-top"
        img.alt = `${exerciseName} Gif`
        cardElement.appendChild(img)

        // Add the card body
        let cardBody = document.createElement("div")
        cardBody.className = "card-body"

        // Add the exercise name
        let cardTitle = document.createElement("h5")
        cardTitle.className = "card-title"
        cardTitle.innerHTML = `<b>${exerciseName}</b>`
        cardBody.appendChild(cardTitle)

        // Add badges for exercise details
        let badgeContainer = document.createElement("div")
        badgeContainer.className = "d-flex justify-content-evenly align-items-center mb-3"
        let badgeClass = "badge rounded-pill bg-info mb-1"
        badgeContainer.innerHTML = `
            <span class="${badgeClass}">${capitalizeFirstLetterWord(exercise.equipment)}</span>
            <span class="${badgeClass}">${capitalizeFirstLetterWord(exercise.bodyPart)}</span>
            <span class="${badgeClass}">${capitalizeFirstLetterWord(exercise.target)}</span>
        `
        cardBody.appendChild(badgeContainer)


        let buttonContainer = document.createElement("div")
        buttonContainer.className = "d-flex justify-content-center align-items-center"
        // Add the "view details" button to open the modal
        let modalButton = document.createElement("button")
        modalButton.className = "btn btn-primary exercise-details-btn btn-sm rounded-pill"
        modalButton.textContent = "View Details"
        modalButton.type = "button"
        modalButton.setAttribute("data-bs-toggle", "modal")
        modalButton.setAttribute("data-bs-target", "#exerciseModal")
        modalButton.addEventListener("click", function () {
            populateExerciseModal(exercise)
        })
        buttonContainer.appendChild(modalButton)

        // Add the "Add to Cart" button if user is logged in
        if (isUserLoggedIn) {
            let addToCartButton = document.createElement("button")
            addToCartButton.className = "btn btn-success btn-sm rounded-pill ms-2"
            addToCartButton.textContent = "Add to Cart"
            addToCartButton.type = "button"
            addToCartButton.addEventListener("click", function () {
                addExerciseToCart(exercise)
            })
            buttonContainer.appendChild(addToCartButton)
        }
        cardBody.appendChild(buttonContainer)

        // Append the card body to the card
        cardElement.appendChild(cardBody)

        // Append the card to the card container
        card.appendChild(cardElement)

        // Append the card container to the row
        row.appendChild(card)
    })

    // Append the row to the results list
    resultList.appendChild(row)
}

function populateExerciseModal(exercise) {
    console.log("Populating modal with exercise:", exercise)

    // Set the modal title
    let splitExerciseName = exercise.name.split(" ")
    let exerciseName = capitalizeFirstLetterList(splitExerciseName).join(" ")

    let modalTitle = document.getElementById("modalExerciseName")
    if (modalTitle) {
        modalTitle.textContent = exerciseName
    }

    // Set the modal image
    let modalImage = document.querySelector("#modalExerciseBackdrop img")
    if (modalImage) {
        modalImage.src = exercise.gifUrl
        modalImage.alt = `${exercise.name} Gif`
    }
    else {
        modalImage.src = "https://placehold.co/360x360?text=No+Image+Found"
    }
    
    // Set the modal Secondary Target Muscle(s)
    let secondaryMusclesList = exercise.secondaryMuscles
    let secondaryMuscles = "None"
    if (exercise.secondaryMuscles.length > 0) {
        secondaryMuscles = capitalizeFirstLetterList(secondaryMusclesList).join(", ")
    }

    // Set the modal content
    let modalContent = document.getElementById("modalExerciseContent")
    if (modalContent) {
        modalContent.innerHTML = `
            <div class="mb-3">
                <p><b>Equipment: </b> ${capitalizeFirstLetterWord(exercise.equipment)}</p>
                <p><b>Body Part: </b> ${capitalizeFirstLetterWord(exercise.bodyPart)}</p>
                <p><b>Primary Target Muscle: </b> ${capitalizeFirstLetterWord(exercise.target)}</p>
                <p><b>Secondary Target Muscle(s): </b> ${secondaryMuscles}</p>
            </div>
            <h5>Instructions</h5>
            <ol class="list-group list-group-numbered mb-3">
                ${exercise.instructions
                    .map(
                        (instruction, index) =>
                            `<li class="mb-1">${instruction}</li>`
                    )
                    .join("")}
            </ol>
        `
    }
}

function capitalizeFirstLetterList(splitList) {
    splitList.forEach((word, index) => {
        splitList[index] = word.charAt(0).toUpperCase() + word.slice(1).toLowerCase()
    })
    return splitList
}

function capitalizeFirstLetterWord(word) {
    return word.charAt(0).toUpperCase() + word.slice(1).toLowerCase()
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
