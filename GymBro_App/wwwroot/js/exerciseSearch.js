let exerciseSearchButton = document.getElementById("exerciseSearchButton");

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
    }
});

async function displayExerciseSearchResults(result) {
    console.log("displaySearchResults called with:", result);

    let resultList = document.getElementById("exerciseSearchResults");
    if (!resultList) {
        console.error("Elements not found");
        return;
    }
    console.log("✅ Found #exerciseSearchResults in the DOM.");

        let selectedWorkouts = document.getElementById("selectedWorkouts");
    if (!selectedWorkouts) {
        selectedWorkouts = document.createElement("div");
        selectedWorkouts.id = "selectedWorkouts";
        document.body.appendChild(selectedWorkouts);
    }
    
    resultList.innerHTML = "";

    let heading = document.createElement("h2");
    heading.textContent = "Here are our top 10 results";
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
            console.log("🆕 Add button clicked for:", exercise.name);
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
    
    let exerciseEntry = document.createElement("div");
    exerciseEntry.className = "selected-exercise entry";
    exerciseEntry.style.border = "1px solid #ccc";
    exerciseEntry.style.padding = "8px";
    exerciseEntry.style.marginBottom = "4px";

    exerciseEntry.innerHTML = `<strong>${exercise.name}</strong>`;
    
    let removeButton = document.createElement("button");
    removeButton.innerText = "Remove";
    removeButton.className = "btn btn-danger btn-sm";
    removeButton.style.marginLeft = "8px";
    removeButton.addEventListener("click", function() {
         exerciseEntry.remove();
    });
    
    exerciseEntry.appendChild(removeButton);
    exerciseCart.appendChild(exerciseEntry);
}
