document.addEventListener("DOMContentLoaded", () => {
    navigator.geolocation.getCurrentPosition(embedDefaultMap, embedDefaultMap)
})


function getPositionError(err) {
    console.log(`Error ${err.code}: couldn't get location. Issue: ${err.message}`)
    return null
}

async function putUserPosition(position) {
    const coordinates = position.coords
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

function getUserPosition(successFunction) {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successFunction, getPositionError)
    }
    else {
        console.log('navigator.geolocation not found.')
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
        let lat = coordinates.latitude.toFixed(6)
        let long = coordinates.longitude.toFixed(6)

        mapFrame.src = `https://www.google.com/maps/embed/v1/view?key=${result.apiKey}&center=${lat},${long}&zoom=18`
    }
}
