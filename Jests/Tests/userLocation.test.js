/**
 * @jest-environment jsdom
 */

const {
    getPositionError,
    putUserPosition,
    embedDefaultMap,
    embedMapAtUserPosition
} = require('../../GymBro_App/wwwroot/js/userLocation');

test('getPositionError logs the correct error message', () => {
    console.log = jest.fn();
    const error = { code: 1, message: 'Permission denied' };
    getPositionError(error);
    expect(console.log).toHaveBeenCalledWith("Error 1: couldn't get location. Issue: Permission denied");
});

test('putUserPosition logs the correct coordinates', async () => {
    console.log = jest.fn();
    const position = { coords: { latitude: 44.851419, longitude: -123.237022 } };
    global.fetch = jest.fn(() =>
        Promise.resolve({
            ok: true,
            json: () => Promise.resolve({})
        })
    );
    await putUserPosition(position);
    expect(console.log).toHaveBeenCalledWith("44.851419, -123.237022");
});

test('embedDefaultMap sets the map src to the correct coordinates', async () => {
    document.body.innerHTML = '<iframe id="nearby-gyms-map"></iframe>';
    global.fetch = jest.fn(() =>
        Promise.resolve({
            ok: true,
            json: () => Promise.resolve({ apiKey: 'test-api-key' })
        })
    );
    await embedDefaultMap();
    const mapFrame = document.getElementById('nearby-gyms-map');
    expect(mapFrame.src).toBe('https://www.google.com/maps/embed/v1/view?key=test-api-key&center=39.828175,-98.5795&zoom=4');
});

test('embedMapAtUserPosition sets the map src to the correct coordinates', async () => {
    document.body.innerHTML = '<iframe id="nearby-gyms-map"></iframe>';
    const position = { coords: { latitude: 12.345678, longitude: -123.987654 } };
    global.fetch = jest.fn(() =>
        Promise.resolve({
            ok: true,
            json: () => Promise.resolve({ apiKey: 'test-api-key' })
        })
    );
    await embedMapAtUserPosition(position);
    const mapFrame = document.getElementById('nearby-gyms-map');
    expect(mapFrame.src).toBe('https://www.google.com/maps/embed/v1/view?key=test-api-key&center=12.345678,-123.987654&zoom=18');
});