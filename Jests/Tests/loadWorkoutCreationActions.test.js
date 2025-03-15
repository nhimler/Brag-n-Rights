/**
 * @jest-environment jsdom
 */

// Stores callbacks for simulating DOM content loaded
const domContentLoadedCallbacks = [];

jest.mock('bootstrap', () => ({
    Dropdown: jest.fn().mockImplementation(() => ({
        show: jest.fn()
    }))
}));

global.bootstrap = {
    Dropdown: jest.fn().mockImplementation(() => ({
        show: jest.fn()
    }))
};

document.getElementById = jest.fn();
document.addEventListener = jest.fn((event, callback) => {
    if (event === 'DOMContentLoaded') {
        domContentLoadedCallbacks.push(callback);
    }
});

global.fetch = jest.fn();

beforeEach(() => {
    // Clears all mocks before each test
    jest.clearAllMocks();
    document.getElementById.mockClear();
    global.fetch.mockClear();
    
    domContentLoadedCallbacks.length = 0;
    
    jest.isolateModules(() => {
        require('../../GymBro_App/wwwroot/js/loadWorkoutCreationActions');
    });
});

test('correctly handles multiple listeners', () => {
    expect(domContentLoadedCallbacks.length).toBe(2);
});


test('shows login dropdown if element exists', () => {
    const mockDropdownElement = {};
    document.getElementById.mockReturnValue(mockDropdownElement);
    
    if (domContentLoadedCallbacks.length > 1) {
        domContentLoadedCallbacks[1]();
    }
    
    expect(document.getElementById).toHaveBeenCalledWith('loginDropdown');
    expect(bootstrap.Dropdown).toHaveBeenCalledWith(mockDropdownElement);
    expect(bootstrap.Dropdown.mock.results[0].value.show).toHaveBeenCalled();
});

test('does not show login dropdown if element does not exist', () => {
    document.getElementById.mockReturnValue(null);
    
    if (domContentLoadedCallbacks.length > 1) {
        domContentLoadedCallbacks[1]();
    }
    
    expect(document.getElementById).toHaveBeenCalledWith('loginDropdown');
    expect(bootstrap.Dropdown).not.toHaveBeenCalled();
});
