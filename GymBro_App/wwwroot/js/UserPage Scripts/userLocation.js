function getPositionSuccess(pos) {
    const demoOutput = document.getElementById('for-demo-only');
    const coordinates = pos.coords;
    demoOutput.innerHTML = `Latitude: ${coordinates.latitude}, Longitude: ${coordinates.longitude}`;
}

function getPositionError(err) {
    console.log(`Error ${err.code}: couldn't get location. Issue: ${err.message}`);
}

function getUserPosition() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(getPositionSuccess, getPositionError);
    } else {
        console.log('navigator.geolocation not found.');
    }
}