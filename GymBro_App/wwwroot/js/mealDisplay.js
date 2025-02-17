
document.querySelectorAll("ol p").forEach(async function (p) {
    let response =  await fetch("/api/food/" + p.getAttribute("data-api-id"));
    if(response.ok){
        let result = await response.json();
        console.log(result);
        p.textContent = result.foodName;
    } else {
        console.log("Error: " + response.status);
    }
});