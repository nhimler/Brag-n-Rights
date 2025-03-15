document.addEventListener("DOMContentLoaded", () => {
    const mapElement = document.getElementById("nearby-gyms-map")
    if (mapElement) {
        embedDefaultMap()
        navigator.geolocation.getCurrentPosition(embedMapAtUserPosition, getPositionError)
    }
    const setLocationButton = document.getElementById("set-location-button");
    if (setLocationButton) {
        setLocationButton.addEventListener("click", () => {
            // TODO: Find a way to update the map without having to call getCurrentPosition twice
            navigator.geolocation.getCurrentPosition(embedMapAtUserPosition, getPositionError);
            navigator.geolocation.getCurrentPosition(getNearbyGyms, getPositionError);
        });
    }

    const getUserLocation = document.getElementById("getUserLocation-btn")
    if (getUserLocation) {
        console.log("getUserLocation-btn found")
        getUserLocation.addEventListener("click", () => {
            navigator.geolocation.getCurrentPosition((position) => {
                reverseGeocode(position.coords.latitude, position.coords.longitude)
            }, getPositionError)
        })
    }
    else {
        console.log("getUserLocation-btn not found")
    }
})

function getPositionError(err) {
    console.log(`Error ${err.code}: couldn't get location. Issue: ${err.message}`)
    return null
}

async function putUserPosition(position) {
    const coordinates = position.coords
    console.log(`${coordinates.latitude.toFixed(6)}, ${coordinates.longitude.toFixed(6)}`)

    const user = {
        latitude : coordinates.latitude,
        longitude : coordinates.longitude
    }

    const response = await fetch('/api/users', {
        method: 'PUT',
        headers: {
            'Accept': 'application/json application/problem+json charset=utf-8',
            'Content-Type': 'application/json charset=UTF-8'
        },
        body: JSON.stringify(user)
    })

    if (!response.ok) {
        console.log('Something went wrong when updating user location.')
    }
}

async function embedDefaultMap() {
    const mapFrame = document.getElementById("nearby-gyms-map")
    let response = await fetch(`/api/maps`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if (response.ok) {
        let result = await response.json()
        mapFrame.src = `https://www.google.com/maps/embed/v1/view?key=${result.apiKey}&center=39.828175,-98.5795&zoom=4`
    }
}

async function embedMapAtUserPosition(position) {
    // console.log("Got to embedMap")
    const mapFrame = document.getElementById("nearby-gyms-map")
    let coordinates = position.coords

    let response = await fetch(`/api/maps`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if (response.ok) {
        let result = await response.json()
        // console.log("Got response")
        
        let lat = coordinates.latitude.toFixed(6)
        let long = coordinates.longitude.toFixed(6)

        mapFrame.src = `https://www.google.com/maps/embed/v1/view?key=${result.apiKey}&center=${lat},${long}&zoom=10`
    }
}

async function getNearbyGyms(pos) {
    // TODO: Call this method in a better way (ex: "api/maps/nearby?latitude=lat&longitude=long").
    // This would also involve changing how this function is called in the event listener.
    let lat = pos.coords.latitude
    let long = pos.coords.longitude
    let response = await fetch(`/api/maps/nearby/${lat}/${long}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if(response.ok){
        let result = await response.json()
        let gymList = document.getElementById("nearby-gym-search-list")
        let gymListHTML = ""

        for (let i = 0; i < result.length; i++) {
            let gym = result[i]
            // console.log(gym.regularOpeningHours)
            
            // Gets open/close status and the next time it opens/closes. TODO: Fix this so it adjusts for UTC time and 24/7 gyms
            // let gymOpenStatus = gym.regularOpeningHours.openNow ? "Open" : "Closed"
            // console.log(`${gym.regularOpeningHours.nextCloseTime} ${gym.regularOpeningHours.nextOpenTime}`)
            // let gymNextStatusHours = gymOpenStatus ? `Closes at ${gym.regularOpeningHours.nextCloseTime} ` : `Opens at ${gym.regularOpeningHours.nextOpenTime}`
            // gymNextStatusHours = gymNextStatusHours.split("T")[1].slice(0, -1)
            // console.log("Closes at: " + gym.regularOpeningHours.nextCloseTime)
            // console.log("Opens at: " + gym.regularOpeningHours.nextOpenTime)


            let gymHours = gym.regularOpeningHours.weekdayDescriptions
            let gymHoursParagraphs = ""
            gymHours.forEach(day => {
                gymHoursParagraphs += `<p class="card-text my-1">${day}</p>`
            });
            
            gymListHTML += `
                <a target="_blank" rel="noopener noreferrer" href="${gym.websiteUri}" class="card mb-3 text-decoration-none text-dark diplayed-gym-card">
                    <div class="card-body">
                        <h5 class="card-title">${gym.displayName.text}</h5>
                        <p class="card-text"><small class="text-body-secondary">${gym.formattedAddress}</small></p>
                        ${gymHoursParagraphs}
                    </div>
                </a>
            `
            gymList.innerHTML = gymListHTML
            // console.log(gym)
            // console.log(gym.displayName.text)
            // console.log(gym.formattedAddress)
            // console.log("")
        }
    }
    else {
        console.log("Error: " + response.status)
    }
}

async function reverseGeocode(lat, long) {
    let response = await fetch(`/api/maps/reversegeocode/${lat}/${long}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if(response.ok){
        let result = await response.json()
        console.log(result)
        let userLocation = document.getElementById("user-location")
        userLocation.innerHTML = `Location: ${result.address}`
        
    }
    else {
        console.log("Error: " + response.status)
    }
}

if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        getPositionError,
        putUserPosition,
        embedDefaultMap,
        embedMapAtUserPosition,
        getNearbyGyms
    }
}