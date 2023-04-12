const axios = require('axios');

test('GET FallingTime returns correct GravityAcceleration', async () => {
    const response = await axios.get('http://localhost:5204/FallingTime');
    expect(response.data['gravityAcceleration']).toBe(9.81);
});

describe.each([
    [0, 0.00],
    [1, 0.45],
    [5, 1.01]
])('startHeight = %s', (startHeight, expectedFallingTime) => {
    test(`GET FallingTime for startHeight = ${startHeight} returns fallingTime = ${expectedFallingTime}`, async () => {
        const response = await axios.get(`http://localhost:5204/FallingTime?startHeight=${startHeight}`);
        expect(response.data['fallingTime']).toBeCloseTo(expectedFallingTime, 2);
    });
});

test('GET FallingTime for negative startHeight returns 400', async () => {
    const response = await axios.get('http://localhost:5204/FallingTime?startHeight=-1', {validateStatus: () => true});
    expect(response.status).toBe(400);
});
