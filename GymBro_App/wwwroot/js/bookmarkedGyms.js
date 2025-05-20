document.addEventListener("DOMContentLoaded", () => {
    console.log("Bookmarked gyms page loaded")
    if (document.getElementsByClassName('delete-gym-btn'))
    {
        console.log("Delete gym button found")
        for (const deleteGymButton of document.getElementsByClassName('delete-gym-btn')) {
            deleteGymButton.addEventListener('click', async (event) => {
                const gymPlace = event.target.getAttribute('id')
                console.log("Gym place ID: " + gymPlace)
                const gymPlaceID = gymPlace.replace("places/", "")
                console.log("Gym bookmark ID: " + gymPlaceID)
                const isDeleted = await deleteGymBookmark(gymPlaceID)

                if (isDeleted) {
                    console.log(`Attempting to remove gym card... ${gymPlaceID}`)
                    removeGymCard(gymPlace)
                }
                
            });
        }
    }
})

async function deleteGymBookmark(gymPlaceID) {
    let response = await fetch(`/api/gymUser/bookmark/delete/${gymPlaceID}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
    })
    if (response.ok) {
        console.log("Gym bookmark deleted successfully")
        return true
    }
    else {
        console.log("Something went wrong when deleting the bookmark: " + response.status)
        return false
    }
}

function removeGymCard(gymCardId) {
    const gymCard = document.getElementById(`gym-card-${gymCardId}`)
    if (gymCard) {
        gymCard.remove()
        console.log("Gym card removed")
    } else {
        console.log("Gym card not found")
    }
}