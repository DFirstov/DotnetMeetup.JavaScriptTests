const axios = require('axios')
const knex = require("knex")
const uuid = require("uuid")

test('GET FallingTime without GA returns default GA', async () => {
    // Act
    const url = 'http://localhost:5204/FallingTime'
    const response = await axios.get(url)

    // Assert
    expect(response.data['gravityAcceleration']).toEqual({
        name: 'default',
        value: 9.81
    })
})

describe.each([
    [0, 0.00],
    [1, 0.45],
    [5, 1.01]
])('startHeight = %s', (startHeight, expectedTime) => {
    test('GET FallingTime without GA returns correct falling time', async () => {
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

describe.each([
    [0, 1, 0.00],
    [1, 1, 1.41],
    [5, 6, 1.29]
])('startHeight = %s, ga = %s', (startHeight, ga, expectedTime) => {
    test('GET FallingTime with GA returns correct falling time', async () => {
        // Arrange
        const gaName = uuid.v4()
        await configureGravityAccelerationMockOK(gaName, ga)

        // Act
        const url = `http://localhost:5204/FallingTime?startHeight=${startHeight}&gaName=${gaName}`
        const response = await axios.get(url)

        // Assert
        expect(response.data['value']).toBeCloseTo(expectedTime, 2)
    })
})

describe.each([
    [0, 0.00],
    [1, 0.45],
    [5, 1.01]
])('startHeight = %s', (startHeight, expectedTime) => {
    test('GET FallingTime uses default GA when service is not accessible', async () => {
        // Arrange
        const gaName = uuid.v4()
        await configureGravityAccelerationMock503(gaName)

        // Act
        const url = `http://localhost:5204/FallingTime?startHeight=${startHeight}&gaName=${gaName}`
        const response = await axios.get(url)

        // Assert
        expect(response.data['value']).toBeCloseTo(expectedTime, 2)
    })
})

describe.each([
    [0, 1, 0.00],
    [1, 1, 1.41],
    [5, 6, 1.29]
])('startHeight = %s, ga = %s', (startHeight, ga, expectedTime) => {
    const dbClient = createDbClient()

    test(`GET FallingTime uses GA from DB if possible`, async () => {
        // Arrange
        const gaName = uuid.v4()
        await dbClient
            .insert({
                Name: gaName,
                Value: ga
            })
            .into('GravityAcceleration')

        // Act
        const url = `http://localhost:5204/FallingTime?startHeight=${startHeight}&gaName=${gaName}`
        const response = await axios.get(url)

        // Assert
        expect(response.data['value']).toBeCloseTo(expectedTime, 2)
    })
})

async function configureGravityAccelerationMockOK(gaName, ga) {
    const mapping = {
        request: {
            method: 'GET',
            urlPattern: `/gravityAcceleration/${gaName}`
        },
        response: {
            jsonBody: {
                name: gaName,
                value: ga
            }
        }
    }

    await axios.post('http://localhost:5433/__admin/mappings', mapping)
}

async function configureGravityAccelerationMock503(gaName) {
    const mapping = {
        request: {
            method: 'GET',
            urlPattern: `/gravityAcceleration/${gaName}`
        },
        response: {
            status: 503
        }
    }

    await axios.post('http://localhost:5433/__admin/mappings', mapping)
}

function createDbClient() {
    const dbClient = knex({
        client: 'pg',
        connection: {
            host: 'localhost',
            port: '5432',
            user: 'postgres',
            password: 'js-tests'
        }
    })

    afterAll(async () => {
        await dbClient.destroy()
    })

    return dbClient
}
