const axios = require('axios');
const knex = require('knex');
const uuid = require('uuid');
const kafkajs = require('kafkajs');

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

test('POST GravityAcceleration produces message to Kafka', async () => {
    const kafka = new kafkajs.Kafka({
        brokers: ['127.0.0.1:9093']
    });
    
    const consumer = kafka.consumer({groupId: 'js-tests'});
    
    await consumer.connect();
    await consumer.subscribe({topic: 'ga-created'});
    
    const messages = [];
    
    await consumer.run({
        eachMessage: async ({message}) => {
            messages.push(message);
        }
    });

    const expectedGa = {
        name: uuid.v4(),
        value: 123
    }

    await axios.post('http://localhost:5204/GravityAcceleration', expectedGa);
    
    let message;
    
    while (!message) {
        message = messages.find(m => m.key.toString() === expectedGa.name);
        await new Promise(resolve => setTimeout(resolve, 100));
    }
    
    expect(message.value.toString()).toBe('123');
}, 60_000);
