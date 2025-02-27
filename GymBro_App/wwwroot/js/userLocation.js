
function getPositionError(err) {
    console.log(`Error ${err.code}: couldn't get location. Issue: ${err.message}`);
    return null;
}

async function putUserPosition(position) {
    const coordinates = position.coords;
    console.log(`${coordinates.latitude.toFixed(6)}, ${coordinates.longitude.toFixed(6)}`);

    const user = {
        latitude : coordinates.latitude,
        longitude : coordinates.longitude
    }

    const response = await fetch('/api/users', {
        method: 'PUT',
        headers: {
            'Accept': 'application/json; application/problem+json; charset=utf-8',
            'Content-Type': 'application/json; charset=UTF-8'
        },
        body: JSON.stringify(user)
    });

    if (!response.ok) {
        console.log('Something went wrong when updating user location.');
    }
}

function getUserPosition() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(putUserPosition);
    } else {
        console.log('navigator.geolocation not found.');
    }
}

