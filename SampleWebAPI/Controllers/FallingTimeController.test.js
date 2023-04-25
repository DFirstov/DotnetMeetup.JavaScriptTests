const axios = require('axios')

test('GET FallingTime returns default GA', async () => {
    // Act
    const url = 'http://localhost:5204/FallingTime'
    const response = await axios.get(url)

    // Assert
    expect(response.data['gravityAcceleration']).toBe(9.81)
})

describe.each([
    [0, 0.00],
    [1, 0.45],
    [5, 1.01]
])('startHeight = %s', (startHeight, expectedTime) => {
    test('GET FallingTime returns correct falling time', async () => {
        // Act
        const url = `http://localhost:5204/FallingTime?startHeight=${startHeight}`
        const response = await axios.get(url)

        // Assert
        expect(response.data['value']).toBeCloseTo(expectedTime, 2)
    })
})

test('GET FallingTime for negative startHeight returns 400', async () => {
    // Act
    const url = 'http://localhost:5204/FallingTime?startHeight=-1'
    const response = await axios.get(url, {validateStatus: () => true})

    // Assert
    expect(response.status).toBe(400)
})
