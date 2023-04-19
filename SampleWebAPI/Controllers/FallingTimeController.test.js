const axios = require('axios');
const knex = require("knex");
const uuid = require("uuid");

test('GET FallingTime without GA name returns default value', async () => {
    const response = await axios.get('http://localhost:5204/FallingTime');
    expect(response.data['gravityAcceleration']).toEqual({
        name: 'default',
        value: 9.81
    });
});

describe.each([
    [0, 0.00],
    [1, 0.45],
    [5, 1.01]
])('startHeight = %s', (startHeight, expectedFallingTime) => {
    test(`GET FallingTime for startHeight = ${startHeight} returns fallingTime = ${expectedFallingTime}`, async () => {
        const response = await axios.get(`http://localhost:5204/FallingTime?startHeight=${startHeight}`);
        expect(response.data['value']).toBeCloseTo(expectedFallingTime, 2);
    });
});

test('GET FallingTime for negative startHeight returns 400', async () => {
    const response = await axios.get('http://localhost:5204/FallingTime?startHeight=-1', {validateStatus: () => true});
    expect(response.status).toBe(400);
});

describe.each([
    [0, 1, 0.00],
    [1, 1, 1.41],
    [5, 6, 1.29]
])('startHeight = %s, ga = %s', (startHeight, ga, expectedFallingTime) => {
    test(`GET FallingTime for startHeight = ${startHeight} and GA = ${ga} in DB returns fallingTime = ${expectedFallingTime}`, async () => {
        const dbClient = knex({
            client: 'pg',
            connection: {
                host: 'localhost',
                port: '5432',
                user: 'postgres',
                password: 'js-tests'
            }
        });

        const gaData = {
            Name: uuid.v4(),
            Value: ga
        };

        await dbClient
            .insert(gaData)
            .into('GravityAcceleration');

        const response = await axios.get(`http://localhost:5204/FallingTime?startHeight=${startHeight}&gaName=${gaData.Name}`);

        expect(response.data['value']).toBeCloseTo(expectedFallingTime, 2);

        await dbClient.destroy();
    });
});

describe.each([
    [0, 1, 0.00],
    [1, 1, 1.41],
    [5, 6, 1.29]
])('startHeight = %s, ga = %s', (startHeight, ga, expectedFallingTime) => {
    test(`GET FallingTime for startHeight = ${startHeight} and GA = ${ga} in service returns fallingTime = ${expectedFallingTime}`, async () => {
        const gaData = {
            name: uuid.v4(),
            value: ga
        };

        const mapping = {
            request: {
                method: 'GET',
                urlPattern: `/gravityAcceleration/${gaData.name}`
            },
            response: {
                jsonBody: gaData
            }
        }

        await axios.post('http://localhost:5433/__admin/mappings', mapping);

        const response = await axios.get(`http://localhost:5204/FallingTime?startHeight=${startHeight}&gaName=${gaData.name}`);

        expect(response.data['value']).toBeCloseTo(expectedFallingTime, 2);
    });
});

describe.each([
    [0, 0.00],
    [1, 0.45],
    [5, 1.01]
])('startHeight = %s', (startHeight, expectedFallingTime) => {
    test('GET FallingTime uses default GA when service is not accessible', async () => {
        const gaName = uuid.v4();
        
        const mapping = {
            request: {
                method: 'GET',
                urlPattern: `/gravityAcceleration/${gaName}`
            },
            response: {
                status: 503
            }
        };
        
        await axios.post('http://localhost:5433/__admin/mappings', mapping);

        const response = await axios.get(`http://localhost:5204/FallingTime?startHeight=${startHeight}&gaName=${gaName}`);

        expect(response.data['value']).toBeCloseTo(expectedFallingTime, 2);
    });
});
