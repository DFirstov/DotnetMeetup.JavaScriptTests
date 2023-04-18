const axios = require('axios');
const knex = require('knex');
const uuid = require('uuid');

test('POST GravityAcceleration saves object to database', async () => {
    const expectedGa = {
        name: uuid.v4(),
        value: 123
    }

    await axios.post('http://localhost:5204/GravityAcceleration', expectedGa);

    const dbClient = knex({
        client: 'pg',
        connection: {
            host: 'localhost',
            port: '5432',
            user: 'postgres',
            password: 'js-tests'
        }
    });

    const actualGa = await dbClient
        .select('*')
        .from('GravityAcceleration')
        .where({Name: expectedGa.name});

    expect(actualGa).toEqual([{
        Name: expectedGa.name,
        Value: expectedGa.value
    }]);
    
    await dbClient.destroy();
});
