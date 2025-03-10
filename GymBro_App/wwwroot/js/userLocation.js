document.addEventListener("DOMContentLoaded", () => {
    const mapElement = document.getElementById("nearby-gyms-map")
    if (mapElement) {
        embedDefaultMap()
        navigator.geolocation.getCurrentPosition(embedMapAtUserPosition, getPositionError)
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

        mapFrame.src = `https://www.google.com/maps/embed/v1/view?key=${result.apiKey}&center=${lat},${long}&zoom=18`
    }
}

async function getNearbyGyms(lat,long) {
    // TODO: Call this method in a better way (ex: "api/maps/nearby?latitude=lat&longitude=long")
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
            // let gymOpenStatus = gym.openNow ? "Open" : "Closed"
            
            gymListHTML += `
                <a target="_blank" rel="noopener noreferrer" href="#" class="card mb-3 text-decoration-none text-dark diplayed-gym-card">
                    <div class="card-body">
                        <h5 class="card-title">${gym.displayName.text}</h5>
                        <p class="card-text"><small class="text-body-secondary">${gym.formattedAddress}</small></p>
                        <p class="card-text">Time not implemented yet &centerdot; </p>
                    </div>
                </a>
            `
            gymList.innerHTML = gymListHTML
            // console.log(gym)
            console.log(gym.displayName.text)
            console.log(gym.formattedAddress)
            console.log("")
        }
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